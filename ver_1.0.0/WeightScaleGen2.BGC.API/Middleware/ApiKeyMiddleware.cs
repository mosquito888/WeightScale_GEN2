using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.Middleware;

namespace WeightScaleGen2.BGC.API.Middleware
{
    public class ApiKeyMiddleware
    {
        private IConfiguration _config;
        public ApiKeyMiddleware(IConfiguration config)
        {
            _config = config;
        }

        private readonly RequestDelegate _next;
        private string privateKey = "";
        private string publicKey = "";

        //request header name
        private const string APIKEYNAME = "SecretKey";
        private const string USER = "User";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                this.publicKey = context.RequestServices.GetRequiredService<IConfiguration>().GetValue<string>("PublicKey");

                //string _x = this.Encrypt("DE4EEA5D75456A128A2A2BF6735EC"); 
                //Uth8Y9sIK5x3H5TvDVBQA2ryr12n/nqAtJtCoTNWz62lu3kuD17tFwINCUdbREkSd5+6+HfktmIDr5uyhgJcrg==

                if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    var json = JsonConvert.SerializeObject("Api Key was not provided. (Using ApiKeyMiddleware)");
                    await context.Response.WriteAsJsonAsync(json);
                    return;
                }

                if (context.Request.Headers.TryGetValue(USER, out var username))
                {
                    if (username[0] != null || username[0] != "")
                    {
                        context.Session.SetString(Constants.Session.User, username[0]);
                    }
                    else
                    {
                        context.Session.SetString(Constants.Session.User, Constants.Session.UnknownUser);
                    }
                }
                else
                {
                    context.Session.SetString(Constants.Session.User, Constants.Session.UnknownUser);
                }

                var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
                var apiKey = appSettings.GetValue<string>(APIKEYNAME);

                //decrypt by public key
                string _decryptKey = this.DecryptKey(extractedApiKey);

                if (!apiKey.Equals(_decryptKey))
                {
                    context.Response.StatusCode = 401;
                    var json = JsonConvert.SerializeObject("Unauthorized client. (Using ApiKeyMiddleware)");
                    await context.Response.WriteAsJsonAsync(json); return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 401;
                var json = JsonConvert.SerializeObject(ex.ToString());
                await context.Response.WriteAsJsonAsync(json); return;
            }
        }

        private string DecryptKey(string cipherText)
        {
            return KeyTools.DecryptKey(cipherText, publicKey);
        }

        public string EncryptText(string text) => KeyTools.EncryptKey(text, publicKey);

        public string DecryptText(string cipherText) => KeyTools.DecryptKey(cipherText, publicKey);
    }
}

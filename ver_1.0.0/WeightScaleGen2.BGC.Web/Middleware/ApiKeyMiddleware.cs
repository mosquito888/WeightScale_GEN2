using Microsoft.Extensions.Configuration;
using WeightScaleGen2.BGC.Models.Authen;
using WeightScaleGen2.BGC.Models.Middleware;

namespace WeightScaleGen2.BGC.Web.Middleware
{
    public class ApiKeyMiddleware
    {
        private IConfiguration _config;

        public ApiKeyMiddleware(IConfiguration config)
        {
            _config = config;
            privateKey = config.GetSection("SecretKey").Value;
            publicKey = config.GetSection("PublicKey").Value;
        }

        private string privateKey = "";
        private string publicKey = "";

        public string EncryptKey()
        {
            return KeyTools.EncryptKey(privateKey, publicKey);
        }

        public string EncryptText(string text) => KeyTools.EncryptKey(text, publicKey);

        public string DecryptText(string cipherText) => KeyTools.DecryptKey(cipherText, publicKey);

        public UserAuthen BasicAuthen()
        {
            return new UserAuthen() { username = "admin", password = "Pa$$WoRd" };
        }
    }
}

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.Globalization;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            // Add Microsoft Identity Platform sign-in
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                            .AddMicrosoftIdentityWebApp(options =>
                            {
                                Configuration.Bind("AzureAd", options);

                                options.Events = new OpenIdConnectEvents
                                {
                                    OnTokenValidated = async context =>
                                    {
                                        var userService = context.HttpContext.RequestServices.GetRequiredService<UserService>();
                                        var authService = context.HttpContext.RequestServices.GetRequiredService<AuthService>();

                                        var username = context.Principal.FindFirst("preferred_username")?.Value
                                                     ?? context.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

                                        if (!string.IsNullOrEmpty(username))
                                        {
                                            var token = await authService.GenerateToken(username);
                                            authService.SetTokenToCookie(token.token);
                                        }
                                    }
                                };
                            });

            // Add Microsoft Identity Platform token acquisition
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddMicrosoftIdentityUI();

            // Add authorization policy
            //services.AddControllersWithViews(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //});

            // Add services
            services.AddTransient<IExcelUtilitiesCommon>(e =>
            {
                return new ExcelUtilitiesCommon();
            });

            // Add services
            services.AddScoped(typeof(AuthService));
            services.AddScoped(typeof(UserService));
            services.AddScoped(typeof(LogService));
            services.AddScoped(typeof(EmployeeService));
            services.AddScoped(typeof(CompanyService));
            services.AddScoped(typeof(PlantService));
            services.AddScoped(typeof(MasterService));
            services.AddScoped(typeof(DepartmentService));
            services.AddScoped(typeof(SystemService));
            services.AddScoped(typeof(ItemMasterService));
            services.AddScoped(typeof(SupplierService));
            services.AddScoped(typeof(SenderService));
            services.AddScoped(typeof(ItemMasterRelationService));
            services.AddScoped(typeof(WeightMasterService));
            services.AddScoped(typeof(WeightHistoryService));
            services.AddScoped(typeof(WeightInService));
            services.AddScoped(typeof(WeightInHistoryService));
            services.AddScoped(typeof(WeightOutService));
            services.AddScoped(typeof(WeightOutHistoryService));
            services.AddScoped(typeof(WeightCompareService));
            services.AddScoped(typeof(WeightDailyService));
            services.AddScoped(typeof(WeightSummaryDayService));
            services.AddScoped(typeof(DocumentPOService));
            services.AddScoped(typeof(ReturnDataService));
            services.AddScoped(typeof(DashboardService));
            services.AddScoped(typeof(MMPOService));
            services.AddScoped(typeof(UOMConversionService));
            services.AddScoped(typeof(WeighingScaleService));

            // Add session
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRewriter(new RewriteOptions().Add(context =>
            {
                if (context.HttpContext.Request.Path == "/MicrosoftIdentity/Account/SignedOut")
                {
                    context.HttpContext.Response.Redirect("/Home/Logout");
                }
            }));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            app.Use(next => context =>
            {
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");

                return next(context);
            });
        }
    }
}

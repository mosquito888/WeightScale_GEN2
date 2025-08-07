using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Globalization;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.API.Common.Logger;
using WeightScaleGen2.BGC.API.Helper;
using WeightScaleGen2.BGC.API.Middleware;
using WeightScaleGen2.BGC.API.Middleware.BasicAuthen;
using WeightScaleGen2.BGC.Models.DBModelsEF;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDistributedMemoryCache();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // This EF Core
            services.AddDbContext<ApplicationContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            // This Connection SQL
            services.AddTransient<IDatabaseConnectionFactory>(e =>
            {
                return new SqlConnectionFactory(Configuration.GetConnectionString("DBConnection"));
            });

            services.AddTransient<IDatabaseConnectionFactoryPO>(e =>
            {
                return new SqlConnectionFactoryPO(Configuration.GetConnectionString("DBConnectionPO"));
            });

            services.AddTransient<ISecurityCommon>(e =>
            {
                return new SecurityCommon(Configuration);
            });
            services.AddTransient<ILogger, DatabaseLogger>();

            #region [Api Versioning]
            // Add API Versioning to the Project
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
                config.UseApiBehavior = false;
            });
            #endregion [Api Versioning]

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WeightScaleGen2.BGC.API v1",
                    Description = "WeightScaleGen2.BGC.API Use for internal user only",
                });
                options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });
                options.OperationFilter<RemoveVersionFromParameter>();
                options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();
            });

            services.AddHttpContextAccessor();

            // This Basic Authne Request
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            // This Service and Repository
            #region [Service and Repository]
            AddService(ref services);
            #endregion [Service and Repository]

            // authen
            services.AddScoped<IUserAuthen, UserAuthen>();
            services.AddScoped(typeof(UserInfoModel));

            // auto mappper
            services.AddAutoMapper(cfg => { });


            // session
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeightScaleGen2.BGC.API v1"));
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Use(next => context =>
            {
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");

                return next(context);
            });
        }

        // This method gets called by the service and repository.
        public void AddService(ref IServiceCollection services)
        {
            // services
            #region [services]
            services.AddScoped(typeof(BaseAPIService));
            services.AddScoped(typeof(AboutAPIService));
            services.AddScoped(typeof(MenuAPIService));
            services.AddScoped(typeof(UserAPIService));
            services.AddScoped(typeof(LogAPIService));
            services.AddScoped(typeof(PrefixAPIService));
            services.AddScoped(typeof(EmployeeAPIService));
            services.AddScoped(typeof(MasterAPIService));
            services.AddScoped(typeof(DepartmentAPIService));
            services.AddScoped(typeof(CompanyAPIService));
            services.AddScoped(typeof(PlantAPIService));
            services.AddScoped(typeof(SystemAPIService));
            services.AddScoped(typeof(ItemMasterAPIService));
            services.AddScoped(typeof(SupplierAPIService));
            services.AddScoped(typeof(SenderAPIService));
            services.AddScoped(typeof(GroupMasterAPIService));
            services.AddScoped(typeof(ItemMasterRelationAPIService));
            services.AddScoped(typeof(WeightMasterAPIService));
            services.AddScoped(typeof(WeightHistoryAPIService));
            services.AddScoped(typeof(WeightInAPIService));
            services.AddScoped(typeof(WeightInHistoryAPIService));
            services.AddScoped(typeof(WeightOutAPIService));
            services.AddScoped(typeof(WeightOutHistoryAPIService));
            services.AddScoped(typeof(WeightCompareAPIService));
            services.AddScoped(typeof(WeightDailyAPIService));
            services.AddScoped(typeof(WeightSummaryDayAPIService));
            services.AddScoped(typeof(DocumentPOAPIService));
            services.AddScoped(typeof(ReturnDataAPIService));
            services.AddScoped(typeof(DashboardAPIService));
            services.AddScoped(typeof(MMPOAPIService));
            services.AddScoped(typeof(UOMConversionAPIService));
            services.AddScoped(typeof(SapAPIService));
            #endregion [services]

            // repositorys
            #region [repository]
            services.AddScoped(typeof(PrefixDocRepository));
            services.AddScoped(typeof(CompanyRepository));
            services.AddScoped(typeof(PlantRepository));
            services.AddScoped(typeof(MasterRepository));
            services.AddScoped(typeof(UserRepository));
            services.AddScoped(typeof(DepartmentRepository));
            services.AddScoped(typeof(MenuRepository));
            services.AddScoped(typeof(EmployeeRepository));
            services.AddScoped(typeof(LogRepository));
            services.AddScoped(typeof(SystemRepository));
            services.AddScoped(typeof(BaseRepository));
            services.AddScoped(typeof(ItemMasterRepository));
            services.AddScoped(typeof(SupplierRepository));
            services.AddScoped(typeof(SenderRepository));
            services.AddScoped(typeof(GroupMasterRepository));
            services.AddScoped(typeof(ItemMasterRelationRepository));
            services.AddScoped(typeof(WeightMasterRepository));
            services.AddScoped(typeof(WeightHistoryRepository));
            services.AddScoped(typeof(WeightInRepository));
            services.AddScoped(typeof(WeightInHistoryRepository));
            services.AddScoped(typeof(WeightOutRepository));
            services.AddScoped(typeof(WeightOutHistoryRepository));
            services.AddScoped(typeof(WeightCompareRepository));
            services.AddScoped(typeof(WeightDailyRepository));
            services.AddScoped(typeof(WeightSummaryDayRepository));
            services.AddScoped(typeof(DocumentPORepository));
            services.AddScoped(typeof(ReturnDataRepository));
            services.AddScoped(typeof(DashboardRepository));
            services.AddScoped(typeof(MMPORepository));
            services.AddScoped(typeof(UOMConversionRepository));
            services.AddScoped(typeof(UOMConversionSapRepository));
            services.AddScoped(typeof(SenderMappingRepository));
            services.AddScoped(typeof(IdentNumberRepository));
            #endregion [repository]
        }
    }
}

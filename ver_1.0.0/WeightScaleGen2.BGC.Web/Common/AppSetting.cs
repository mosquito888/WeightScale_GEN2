using Microsoft.Extensions.Configuration;

namespace WeightScaleGen2.BGC.Web.Common
{
    public static class AppSetting
    {
#if DEBUG
        private static readonly string fileconfig = "appsettings.Development.json";
#else
        private static readonly string fileconfig = "appsettings.json";
#endif
        public static string SecretKey()
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileconfig).Build();
            var configuration = config["Api:SecretKey"];
            return configuration;
        }

        public static string BaseUrl()
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileconfig).Build();
            var configuration = config["Api:BaseUrl"];
            return configuration;
        }
    }
}

using Microsoft.Extensions.Configuration;

namespace WeightScaleGen2.BGC.API.Common
{
    public static class AppSetting
    {
#if DEBUG
        private static readonly string fileconfig = "appsettings.Development.json";
#else
        private static readonly string fileconfig = "appsettings.json";
#endif
        public static string Connection()
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileconfig).Build();
            var configuration = config["ConnectionStrings:DBConnection"];
            return configuration;
        }

        public static string SAPHost()
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileconfig).Build();
            var configuration = config["SAPNCO:SAPHost"];
            return configuration;
        }

        public static string SAPSystemID()
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileconfig).Build();
            var configuration = config["SAPNCO:SAPSystemID"];
            return configuration;
        }

        public static string SAPClient()
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileconfig).Build();
            var configuration = config["SAPNCO:SAPClient"];
            return configuration;
        }

        public static string SAPSystemNumber()
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileconfig).Build();
            var configuration = config["SAPNCO:SAPSystemNumber"];
            return configuration;
        }

        public static string SAPLanguage()
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileconfig).Build();
            var configuration = config["SAPNCO:SAPLanguage"];
            return configuration;
        }

        public static string SAPDestination()
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileconfig).Build();
            var configuration = config["SAPNCO:SAPDestination"];
            return configuration;
        }

        public static string SAPFunction()
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileconfig).Build();
            var configuration = config["SAPNCO:SAPFunction"];
            return configuration;
        }

        public static string SAPTable()
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileconfig).Build();
            var configuration = config["SAPNCO:SAPTable"];
            return configuration;
        }

        public static string SAPReturnTable()
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileconfig).Build();
            var configuration = config["SAPNCO:SAPReturnTable"];
            return configuration;
        }

        public static string SAPVersion()
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileconfig).Build();
            var configuration = config["SAPNCO:SAPHost"];
            return configuration;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Sample
{
    /// <summary>
    /// Utility class to use the settings and connection strings in the different environments
    /// All values are stored in Azure
    /// </summary>
    public static class Configuration
    {
        public static bool IsLocal
        {
            get
            {
                return Environment.GetEnvironmentVariable("APPSETTING_type") == null;
            }
        }

        public static bool IsDevelopment
        {
            get
            {
                return (Environment.GetEnvironmentVariable("APPSETTING_type") != null) && (Environment.GetEnvironmentVariable("APPSETTING_type") == "dev");
            }
        }

        public static bool IsProduction
        {
            get
            {
                return (Environment.GetEnvironmentVariable("APPSETTING_type") != null) && (Environment.GetEnvironmentVariable("APPSETTING_type") == "prod");
            }
        }

        /// <summary>
        /// Azure function that does reflection on dlls
        /// </summary>
        public static string NetFrameworkFunction
        {
            get
            {
                if (IsLocal)
                    return "http://localhost:7071";
                return Environment.GetEnvironmentVariable("APPSETTING_netframeworkapi");
            }
        }

      
    }
}

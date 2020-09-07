using System.Configuration;

namespace RegistrationApp.Helpers
{
    public class ConnectionHelper
    {
        public static string GetRdsConnectionString()
        {
            var appConfig = ConfigurationManager.AppSettings;

            string dbName = appConfig["RDS_DB_NAME"];

            if (string.IsNullOrEmpty(dbName)) return null;

            string username = appConfig["RDS_USERNAME"];
            string password = appConfig["RDS_PASSWORD"];
            string hostname = appConfig["RDS_HOSTNAME"];
            string port = appConfig["RDS_PORT"];

            return "Data Source=" + hostname + ";Initial Catalog=" + dbName + ";User ID=" + username + ";Password=" + password + ";";
        }
    }
}
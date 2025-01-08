namespace SmartCredit.FrontEnd.WebApp.Helpers
{
    public static class ConfigurationHelper
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetApiUrl()
        {
            return _configuration["ApiUrl"]; // Devuelve la URL de la API
        }
    }
}

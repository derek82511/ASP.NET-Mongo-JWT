using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace VieshowManager.Services.Helpers
{
    public static class JwtHelper
    {
        public static string audience { private set; get; }
        public static string issuer { private set; get; }
        public static byte[] secret { private set; get; }

        static JwtHelper()
        {
            audience = ConfigurationManager.AppSettings["Audience"];
            issuer = ConfigurationManager.AppSettings["Issuer"];
            secret = new SHA256CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Secret"]));
        }
    }
}
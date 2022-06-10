using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Vincall.OauthService.UserDB
{
    public static class Md5Helper
    {
        public static string Md5(string source)
        {
            if (string.IsNullOrEmpty(source)) return source;

            using (var md5 = new MD5CryptoServiceProvider())
            {
                byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(source + "abc"));
                return Convert.ToBase64String(result, Base64FormattingOptions.None);
            }
        }
    }
}

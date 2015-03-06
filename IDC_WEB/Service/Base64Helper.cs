using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IDC_WEB
{
    /// <summary>
    /// Base64编码类。
    /// </summary>
    public class Base64Helper
    {
        public static string StringToBase64String(String value)
        {
            // key = "MjYzY2Y3NGYtMzQ0Yy00NTUzLWJkM2UtZTQyMzMyY2NiMGEzOmRjZTA5M2YzLWIwYTktNDhmMi1iYWZlLWFjZDdiYWMyY2E3ZA==";
            byte[] binBuffer = (new ASCIIEncoding()).GetBytes(value);
            return Convert.ToBase64String(binBuffer);
        }
    }
}
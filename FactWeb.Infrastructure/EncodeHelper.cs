using System;
using System.Text;

namespace FactWeb.Infrastructure
{
    public static class EncodeHelper
    {
        public static string EncodeToBase64(string value)
        {
            var bytes = Encoding.ASCII.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }
    }
}

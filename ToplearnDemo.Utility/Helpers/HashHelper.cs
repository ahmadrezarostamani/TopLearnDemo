using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ToplearnDemo.Utility.Helpers
{
    public static class HashHelper
    {
        public static string ComputeHash(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = MD5.Create().ComputeHash(passwordBytes);
            string hash = Encoding.UTF8.GetString(hashBytes);
            return hash;
        }
    }
}

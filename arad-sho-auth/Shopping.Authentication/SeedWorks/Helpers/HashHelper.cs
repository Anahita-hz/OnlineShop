﻿using System;
using System.Security.Cryptography;

namespace Shopping.Authentication.SeedWorks.Helpers
{
    public class HashHelper
    {
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            var byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            var byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}
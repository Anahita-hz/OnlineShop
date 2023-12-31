﻿using System.Web.Security;
using Microsoft.Owin.Security.DataProtection;

namespace Shopping.Authentication.SeedWorks.Helpers
{
    public class MachineKeyDataProtector : IDataProtector
    {
        private readonly string[] _purposes;
        public MachineKeyDataProtector(params string[] purposes)
        {
            _purposes = purposes;
        }
        public byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, _purposes);
        }
        public byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, _purposes);
        }
    }
}
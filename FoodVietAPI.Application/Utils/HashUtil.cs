using CleanFoodVietAPI.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Utils
{
    public static class HashUtil
    {
        #region Password Hash
        private static readonly PasswordHasher<Account> _hasher = new PasswordHasher<Account>();

        public static string PasswordHash(string password)
        {
            return _hasher.HashPassword(null, password);
        }

        public static bool VerifyPassword(string password, string hashedPassword, out string? rehashedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null, hashedPassword, password);

            if (result == PasswordVerificationResult.Failed)
            {
                rehashedPassword = null;
                return false;
            }

            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                rehashedPassword = _hasher.HashPassword(null, password);
            }
            else
            {
                rehashedPassword = null;
            }

            return true;
        }
        #endregion
    }
}

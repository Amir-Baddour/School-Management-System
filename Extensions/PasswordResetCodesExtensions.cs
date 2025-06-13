using System;

namespace Testing1.Extensions
{
    public static class PasswordResetCodesExtensions
    {
        public static bool IsExpired(this PasswordResetCodes code, int expirationMinutes)
        {
            return (DateTime.Now - code.CreatedAt).TotalMinutes > expirationMinutes;
        }
    }
}
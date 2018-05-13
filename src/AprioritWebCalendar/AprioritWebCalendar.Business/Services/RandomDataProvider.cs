using System;
using System.Security.Cryptography;
using AprioritWebCalendar.Business.Interfaces;

namespace AprioritWebCalendar.Business.Services
{
    public class RandomDataProvider : IRandomDataProvider
    {
        public string GetRandomBase64String(int bytesSize)
        {
            var buffer = new byte[bytesSize];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(buffer);
            }

            return Convert.ToBase64String(buffer);
        }
    }
}

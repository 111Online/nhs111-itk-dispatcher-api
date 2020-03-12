using System;
using System.Text;

namespace NHS111.Domain.Itk.Dispatcher.Services
{
    using System.Security.Cryptography;

    public class HashService : IHashService
    {
        public string Compute(string source)
        {
            var sourceBytes = Encoding.ASCII.GetBytes(source);
            var hash = new MD5CryptoServiceProvider().ComputeHash(sourceBytes);
            return ByteArrayToString(hash);
        }

        public bool Compare(string hash, string newHash)
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hash, newHash) == 0;
        }

        private string ByteArrayToString(byte[] arrInput)
        {
            int i;
            var sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}

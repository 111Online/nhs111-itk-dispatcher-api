using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Utils.Cryptography
{
    public class ComputeHash : IComputeHash
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

    public interface IComputeHash
    {
        string Compute(string source);
        bool Compare(string source1, string source2);
    }
}

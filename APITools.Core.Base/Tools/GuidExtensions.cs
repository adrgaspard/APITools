using System;
using System.Security.Cryptography;

namespace APITools.Core.Base.Tools
{
    public class GuidExtensions
    {
        public static Guid HashIntoGuid(byte[] bytes, HashAlgorithm hashAlgorithm)
        {
            byte[] result = hashAlgorithm.ComputeHash(bytes);
            if (result is null || result.Length < 16)
            {
                throw new InvalidOperationException("The choosen hash algorithm don't provides a 128+ bit hash");
            }
            return new Guid(result);
        }
    }
}

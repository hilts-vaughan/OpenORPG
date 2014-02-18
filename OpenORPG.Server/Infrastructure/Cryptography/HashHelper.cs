using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Cryptography
{
    /// <summary>
    /// A static helper that can compute hashes while blocking, this takes up computational time and is used
    /// on the server to generate timesinks while checking passwords.
    /// 
    /// The hash to use is probably SHA512 for most needs as it's the most secure. MD5 can be used for things that need to be quicker.
    /// </summary>
    public class HashHelper
    {

        public enum HashType
        {
            SHA512,
            MD5
        }

        /// <summary>
        /// Gets the SHA512 of a mesage and returns it in all uppercase form
        /// </summary>
        /// <param name="message">The message to get</param>
        /// <returns></returns>
        public static string GetSha512(string message)
        {
            return ComputeHash(message, HashType.SHA512);
        }

        /// <summary>
        /// Gets the MD5 of a given string and returns it in all lowercase form.
        /// </summary>
        /// <param name="message">The message to get</param>
        /// <returns></returns>
        public static string GetMd5(string message)
        {
            return ComputeHash(message, HashType.MD5);
        }


        /// <summary>
        /// A small helper function that given a hash type will use the .NET libraries to compute and return the proper hash.
        /// </summary>
        /// <param name="message">The message to hash</param>
        /// <param name="type">The type of hash to compute</param>
        /// <returns></returns>
        private static string ComputeHash(string message, HashType type)
        {
            byte[] sourceBytes = Encoding.Default.GetBytes(message);
            byte[] hashBytes = null;

            switch (type)
            {
                case HashType.MD5:
                    hashBytes = MD5.Create().ComputeHash(sourceBytes);
                    break;
                case HashType.SHA512:
                    hashBytes = SHA512.Create().ComputeHash(sourceBytes);
                    break;
                default:
                    throw new NotImplementedException("The given hash type is not implemented");
            }
            
            var sb = new StringBuilder();
            
            foreach (byte t in hashBytes)
                sb.AppendFormat("{0:x2}", t);

            return sb.ToString().ToLower();
        }



    }
}

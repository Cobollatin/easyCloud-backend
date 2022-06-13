using System;
using System.Security.Cryptography;
using System.Text;

namespace easyCloud.Models {
    public class Security {
        public static string ComputeHash(string input, HashAlgorithm algorithm) {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }
    }
}

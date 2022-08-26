﻿using System.Security.Cryptography;
using System.Text;

namespace PHVN_WS_CORE.SHARED.Utils
{
    public static class CryptoHelper
    {
        /// <summary>
        /// This constant is used to determine the keysize of the encryption algorithm.
        /// Default value: 256.
        /// </summary>
        private const int Keysize = 256;

        /// <summary>
        /// Default password to encrypt/decrypt texts.
        /// It's recommended to set to another value for security.
        /// Default value: "KDYqndGd2WTx3CWx"
        /// </summary>
        private const string DefaultPassPhrase = @"KDYqndGd2WTx3CWx";

        /// <summary>
        /// This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        /// This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        /// 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        /// Default value: Encoding.ASCII.GetBytes("YFkCmLNyWuxY5XAV")
        /// </summary>
        private const string InitVector = @"YFkCmLNyWuxY5XAV";

        /// <summary>
        /// Default value: Encoding.ASCII.GetBytes("MH6v!jsu")
        /// </summary>
        private const string DefaultSalt = @"MH6v!jsu";

        public static string Encrypt(
            string plainText,
            string passPhrase = null,
            byte[] initVector = null,
            byte[] salt = null)
        {
            if (plainText == null)
                return null;

            if (passPhrase == null)
                passPhrase = DefaultPassPhrase;

            if (initVector == null)
                initVector = Encoding.ASCII.GetBytes(InitVector);

            if (salt == null)
                salt = Encoding.ASCII.GetBytes(DefaultSalt);

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, salt))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = Aes.Create())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVector))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                var cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(
            string cipherText,
            string passPhrase = null,
            byte[] initVector = null,
            byte[] salt = null)
        {
            if (string.IsNullOrEmpty(cipherText))
                return null;

            if (passPhrase == null)
                passPhrase = DefaultPassPhrase;

            if (initVector == null)
                initVector = Encoding.ASCII.GetBytes(InitVector);

            if (salt == null)
                salt = Encoding.UTF8.GetBytes(DefaultSalt);

            var cipherTextBytes = Convert.FromBase64String(cipherText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, salt))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = Aes.Create())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVector))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        public static string GenerateKey(int maxSize = 32)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % chars.Length]);
            }
            return result.ToString();
        }
    }
}

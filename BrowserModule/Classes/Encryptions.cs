using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace DF_WebModule.Classes
{
    public class Encryptions
    {
        //http://msdn.microsoft.com/en-us/library/system.security.cryptography.rijndaelmanaged(v=vs.110).aspx

        public static string passPhrase = "zaq1xsw2";    // pass in user password
        public static string saltValue = "c2c@emlv";              // can be any string
        public static string hashAlgorithm = "SHA1";              // can be "MD5"
        public static int passwordIterations = 2;                 // can be any number
        public static string initVector = "@1q2w3e4r5t6y7u8";     // must be 16 bytes
        public static int keySize = 256;                          // can be 192 or 128

        public static string EncryptDecrypt(string szPlainText, int szEncryptionKey = 6548569)
        {
            StringBuilder szInputStringBuild = new StringBuilder(szPlainText);
            StringBuilder szOutStringBuild = new StringBuilder(szPlainText.Length);
            char Textch;
            for (int iCount = 0; iCount < szPlainText.Length; iCount++)
            {
                Textch = szInputStringBuild[iCount];
                Textch = (char)(Textch ^ szEncryptionKey);
                szOutStringBuild.Append(Textch);
            }
            return szOutStringBuild.ToString();
        }

        // entry point for encryptions        
        public static void Encrypt(string enc, string filePath)
        {
            byte[] encrypted = null;
            try
            {
                // Encrypt the string to an array of bytes.
                encrypted = EncryptStringToBytes(enc);
            }
            catch (Exception ex)
            {
                clsCarePad.WriteLog(ex.ToString());
            }
            //return encrypted;

            BinaryWriter Writer = null;
            // Create a new stream to write to the file

            Writer = new BinaryWriter(File.Create(filePath));

            //if (!File.Exists(filePath))

            //    Writer = new BinaryWriter(File.Create(filePath));
            //else
            //    Writer = new BinaryWriter(File.OpenWrite(filePath));

            // Writer raw data                
            Writer.Write(encrypted);
            Writer.Flush();
            Writer.Close();
        }

        public static string EncryptString_(string str)
        {
            byte[] retBytes = EncryptStringToBytes(str);
            byte[] bb = Convert.FromBase64String(str);
            // return System.Text.Encoding.Default.GetString(retBytes);
            return Encoding.ASCII.GetString(bb);
        }

        static byte[] EncryptStringToBytes(string plainText)
        {
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);


            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (password == null)
                throw new ArgumentNullException("Key");
            if (initVectorBytes == null || initVectorBytes.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;


                // Generate encryptor from the existing key bytes and initialization 
                // vector. Key size will be defined based on the number of the key 
                // bytes.
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        // Start encrypting.
                        csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);

                        // Finish encrypting.
                        csEncrypt.FlushFinalBlock();

                        // Convert our encrypted data from a memory stream into a byte array.
                        byte[] cipherTextBytes = msEncrypt.ToArray();

                        // Convert our encrypted data from a memory stream into a byte array.
                        encrypted = cipherTextBytes;

                        // Close both streams.
                        msEncrypt.Close();
                        csEncrypt.Close();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        // entry popint for Decryptions
        public static string Decrypt(string filename)
        {
            string decrypted = "";
            byte[] cipherTextBytes = System.IO.File.ReadAllBytes(filename);

            try
            {
                // Decrypt the bytes to a string.
                decrypted = DecryptStringFromBytes(cipherTextBytes);
            }
            catch (Exception ex)
            {
                clsCarePad.WriteLog(ex.ToString());
            }
            return decrypted;
        }

        public static string Decryptstring_(string str)
        {
            // convert to byteArray
            byte[] retByte = Encoding.ASCII.GetBytes(str);
            return DecryptStringFromBytes(retByte);


        }

        static string DecryptStringFromBytes(byte[] cipherTextBytes)
        {

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

            // Check arguments.
            if (cipherTextBytes == null || cipherTextBytes.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (saltValueBytes == null || saltValueBytes.Length <= 0)
                throw new ArgumentNullException("Key");
            if (initVectorBytes == null || initVectorBytes.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                byte[] keyBytes = password.GetBytes(keySize / 8);

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate decryptor from the existing key bytes and initialization 
                // vector. Key size will be defined based on the number of the key 
                // bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                        // Start decrypting.
                        int decryptedByteCount = csDecrypt.Read(plainTextBytes, 0, plainTextBytes.Length);
                        // Close both streams.
                        msDecrypt.Close();
                        csDecrypt.Close();

                        // Convert decrypted data into a string. 
                        // Let us assume that the original plaintext string was UTF8-encoded.
                        plaintext = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                    }
                }

            }
            return plaintext;
        }
    }
}
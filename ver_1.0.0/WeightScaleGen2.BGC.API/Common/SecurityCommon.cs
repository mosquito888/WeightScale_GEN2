using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;
using System.Text;

namespace WeightScaleGen2.BGC.API.Common
{
    public interface ISecurityCommon
    {
        // Encrypt with key
        string EncryptData(string textData, string encryptionkey);
        string EncryptData(object textData, string encryptionkey);
        string DecryptData(string encryptedText, string encryptionkey);
        // Encrypt with key: Recommend for data id
        string EncryptDataUrlEncoder(string textData);
        string DecryptDataUrlEncoder(string encryptedText);
    }

    public partial class SecurityCommon : ISecurityCommon
    {
        private IConfiguration _config;
        private const int _Keysize = 128;
        private string _privateKey = "";
        private string _publicKey = "";

        public SecurityCommon(IConfiguration config)
        {
            _config = config;
        }

        #region [Encrypt Decrypt Data Url]
        public string EncryptDataUrlEncoder(string textData)
        {
            var encryptionkey = _config.GetSection("SecretKey").Value;
            return Base64UrlEncoder.Encode(this.EncryptData(textData: textData, encryptionkey: encryptionkey));
        }

        public string EncryptData(object textData, string encryptionkey)
        {
            string result = string.Empty;
            if (textData != null && textData?.ToString().Trim() != string.Empty)
            {
                result = this.EncryptData(textData: textData.ToString(), encryptionkey: encryptionkey);
            }
            return result;
        }

        public string EncryptData(string textData, string encryptionkey)
        {
            RijndaelManaged objrij = new RijndaelManaged
            {
                //set the mode for operation of the algorithm
                Mode = CipherMode.CBC,
                //set the padding mode used in the algorithm.
                Padding = PaddingMode.PKCS7,
                //set the size, in bits, for the secret key.
                KeySize = 0x80,
                //set the block size in bits for the cryptographic operation.
                BlockSize = 0x80
            };
            //set the symmetric key that is used for encryption & decryption.
            byte[] passBytes = Encoding.UTF8.GetBytes(encryptionkey);
            //set the initialization vector (IV) for the symmetric algorithm
            byte[] EncryptionkeyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            int len = passBytes.Length;
            if (len > EncryptionkeyBytes.Length)
            {
                len = EncryptionkeyBytes.Length;
            }
            Array.Copy(passBytes, EncryptionkeyBytes, len);
            objrij.Key = EncryptionkeyBytes;
            objrij.IV = EncryptionkeyBytes;
            //Creates symmetric AES object with the current key and initialization vector IV.
            ICryptoTransform objtransform = objrij.CreateEncryptor();
            byte[] textDataByte = Encoding.UTF8.GetBytes(textData);
            //Final transform the test string.
            return Convert.ToBase64String(objtransform.TransformFinalBlock(textDataByte, 0, textDataByte.Length));
        }

        public string DecryptDataUrlEncoder(string encryptedText)
        {
            var encryptionkey = _config.GetSection("SecretKey").Value;
            return this.DecryptData(encryptedText: Base64UrlEncoder.Decode(encryptedText), encryptionkey: encryptionkey);
        }

        public string DecryptData(string encryptedText, string encryptionkey)
        {
            RijndaelManaged objrij = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 0x80,
                BlockSize = 0x80
            };
            byte[] encryptedTextByte = Convert.FromBase64String(encryptedText);
            byte[] passBytes = Encoding.UTF8.GetBytes(encryptionkey);
            byte[] EncryptionkeyBytes = new byte[0x10];
            int len = passBytes.Length;
            if (len > EncryptionkeyBytes.Length)
            {
                len = EncryptionkeyBytes.Length;
            }
            Array.Copy(passBytes, EncryptionkeyBytes, len);
            objrij.Key = EncryptionkeyBytes;
            objrij.IV = EncryptionkeyBytes;
            byte[] TextByte = objrij.CreateDecryptor().TransformFinalBlock(encryptedTextByte, 0, encryptedTextByte.Length);
            return Encoding.UTF8.GetString(TextByte);  //it will return readable string
        }
        #endregion
    }
}

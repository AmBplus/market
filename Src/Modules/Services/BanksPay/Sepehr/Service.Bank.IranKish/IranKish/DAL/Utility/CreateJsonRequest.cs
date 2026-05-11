using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using IranKish.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace IranKish
{
    public static class CreateJsonRequest
    {

        /// static CreateJasonRequest method goes here 

        public static string CreateJasonRequest(string terminalId,string acceptorId, long amount,string revertUri, string passPhrase,
         string requestId ,string paymentId, string cmsPreservationId , string transactionType, BillInfo billInfo , List<MultiplexParameter> multiplexParameters )
        {

            try
            {


                RequestClass requestClass = new RequestClass();
                requestClass.Request.CmsPreservationId = cmsPreservationId;
                requestClass.Request.AcceptorId = acceptorId;
                requestClass.Request.amount = amount;
                requestClass.Request.BillInfo = billInfo;
                requestClass.Request.multiplexParameters = multiplexParameters;
                requestClass.Request.RequestId = requestId;
                requestClass.Request.RevertUri = revertUri;
                requestClass.Request.terminalId = terminalId;
                requestClass.Request.RequestTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                requestClass.Request.transactionType = transactionType;

                requestClass.Request.additionalParameters = new List<KeyValuePair<string, string>>
                 {
                    //new KeyValuePair<string, string>("nationalId", "0082806608")
                 };


            }
            catch (Exception ex)
            {

                throw ex;
            }


            return "";




        }


        public static string CreateJasonRequest(IPGDataModel iPGData)

        {


            RequestClass requestClass = new RequestClass();

            try
            {


                requestClass.Request.CmsPreservationId = iPGData.CmsPreservationId;

                requestClass.Request.AcceptorId = iPGData.AcceptorId;
                requestClass.Request.amount = iPGData.Amount;
                requestClass.Request.BillInfo = iPGData.BillInfo;
                requestClass.Request.multiplexParameters = iPGData.MultiplexParameters;
                requestClass.Request.PaymentId = iPGData.PaymentId;
                requestClass.Request.RequestId = iPGData.RequestId;
                requestClass.Request.RevertUri = iPGData.RevertURL;
                requestClass.Request.terminalId = iPGData.TerminalId;
                requestClass.Request.RequestTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                requestClass.Request.transactionType = iPGData.TransactionType;

                requestClass.Request.additionalParameters = new List<KeyValuePair<string, string>>
                {
                    //new KeyValuePair<string, string>("nationalId", "")
                };

                CreateAESCoding(requestClass, iPGData.RsaPublicKey, iPGData.PassPhrase, requestClass.Request.multiplexParameters == null ? false : true);

        }
            catch (Exception ex)
            {

                throw ex;
            }


            return JsonConvert.SerializeObject(requestClass);
        
        
        }


        private static void CreateAESCoding(RequestClass requestClass , string rsaPublicKey , string passPhrase, bool isMultiplex )
        {


            try
            {

                string baseString =
                requestClass.Request.terminalId +
                passPhrase +
                requestClass.Request.amount.ToString().PadLeft(12, '0') +
                (isMultiplex ? "01" : "00") +
                (isMultiplex ?
                requestClass.Request.multiplexParameters.Select(t => $"{t.Iban.Replace("IR", "2718")}{t.Amount.ToString().PadLeft(12, '0')}")
                .Aggregate((a, b) => $"{a}{b}")
                : string.Empty);
                using (Aes myAes = Aes.Create())
                {

                    myAes.KeySize = 128;
                    myAes.GenerateKey();
                    myAes.GenerateIV();
                    byte[] keyAes = myAes.Key;
                    byte[] ivAes = myAes.IV;

                    byte[] resultCoding = new byte[48];
                    byte[] baseStringbyte = baseString.HexStringToByteArray();

                    byte[] encrypted = EncryptStringToBytes_Aes(baseStringbyte, myAes.Key, myAes.IV);
                    //byte[] hsahash = new SHA256CryptoServiceProvider().ComputeHash(encrypted);
                    byte[] hsahash = SHA256.HashData(encrypted);
                    resultCoding = CombinArray(keyAes, hsahash);

                    requestClass.AuthenticationEnvelope.Data = RSAEncription(resultCoding, rsaPublicKey).ByteArrayToHexString();
                    //  string decripte = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);
                    requestClass.AuthenticationEnvelope.Iv = ivAes.ByteArrayToHexString();




                };

            }
            catch (Exception ex)
            {


           
            }
        
        
        
        
        }



        public static byte[] HexStringToByteArray(this string hexString)
        {

          return  Enumerable.Range(0, hexString.Length)
               .Where(x => x % 2 == 0)
               .Select(x => Convert.ToByte(value: hexString.Substring(startIndex: x, length: 2), fromBase: 16))
               .ToArray();

        }



        public static string ByteArrayToHexString(this byte[] bytes)
        {
        
            return( bytes.Select(t => t.ToString(format: "X2")).Aggregate((a, b) => $"{a}{b}"));
        }


        private static byte[] EncryptStringToBytes_Aes(byte[] plainText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = 128;
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    return encryptor.TransformFinalBlock(plainText, 0, plainText.Length);
                }
            }
        }

        private static byte[] CombinArray(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Array.Copy(first, 0, bytes, 0, first.Length);
            Array.Copy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }

        private static byte[] RSAEncription(byte[] aesCodingResult, string publicKey)
        {
            //var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                encryptEngine.Init(true, keyParameter);
            }

            return encryptEngine.ProcessBlock(aesCodingResult, 0, aesCodingResult.Length);
        }













    }
}

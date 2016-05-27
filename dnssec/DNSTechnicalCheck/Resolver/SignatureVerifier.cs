namespace Resolver
{
    using OpenSSL.Core;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using RSA = OpenSSL.Crypto.RSA;

    public class SignatureVerifier : ISignatureVerifier
    {
        public bool EqualArrays(byte[] arrayA, byte[] arrayB)
        {
            bool result = false;

            if (arrayA.Length == arrayB.Length)
            {
                bool arrayMatch = !arrayA.Where((t, i) => t != arrayB[i]).Any();

                result = arrayMatch;
            }

            return result;
        }

        public bool VerifyCryptographicallyOpenSsl(byte[] signatureData, byte[] signedData, byte[] keyData)
        {
            RSAParameters rsaParameters = GetRsaParameters(keyData);
            if (rsaParameters.Modulus.Length != signatureData.Length)
            {
                return false;
            }

            using (var rsa = new RSA())
            {
                rsa.PublicModulus = BigNumber.FromArray(rsaParameters.Modulus);
                rsa.PublicExponent = BigNumber.FromArray(rsaParameters.Exponent);

                byte[] locallyFormedSig = rsa.PublicDecrypt(signatureData, RSA.Padding.PKCS1);

                byte[] hashOfSignedData = GetHashOfSignedData(signedData);

                Console.WriteLine(hashOfSignedData.ToHexString());
                Console.WriteLine(locallyFormedSig.Skip(15).ToArray().ToHexString());

                //File.AppendAllText("C:\\combonsis.txt", "Hash of signed:" + System.Environment.NewLine);
                //File.AppendAllText("C:\\combonsis.txt", hashOfSignedData.ToHexString() + System.Environment.NewLine);
                //File.AppendAllText("C:\\combonsis.txt", "Locallyformed:" + System.Environment.NewLine);
                //File.AppendAllText("C:\\combonsis.txt", locallyFormedSig.Skip(19).ToArray().ToHexString() + System.Environment.NewLine);
                
                return true;
            }
        }

        private RSAParameters GetRsaParameters(byte[] wholeKey)
        {
            var parameters = new RSAParameters();
            parameters.Exponent = GetExponent(wholeKey);
            parameters.Modulus = GetModulus(parameters.Exponent.Length, wholeKey);
            return parameters;
        }

        private byte[] GetExponent(byte[] key)
        {
            int expLengthRepLength = GetExponentLengthLength(key);
            var exponent = new byte[GetExponentLength(key, expLengthRepLength)];
            Array.Copy(key, expLengthRepLength, exponent, 0, exponent.Length);
            return exponent;
        }

        private int GetExponentLengthLength(byte[] key)
        {
            // RFC 3110
            // if key[0] is 0, then exponent length is key[1],key[2] as unsigned int
            // if its >0, then its key[0] as unsigned int
            return key[0] == 0 ? 3 : 1;
        }

        private uint GetExponentLength(byte[] key, int exponentLengthLength)
        {
            return exponentLengthLength == 1
                       ? key[0]
                       : BitConverter.ToUInt32((new byte[] { 0x00, 0x00, key[1], key[2] }).Reverse(), 0);
        }

        private byte[] GetModulus(int exponentLength, byte[] key)
        {
            byte[] modulus = null;
            int modulusStartIndex = 0;

            if (exponentLength > 255)
            {
                modulusStartIndex = 3 + exponentLength;
            }
            else
            {
                modulusStartIndex = 1 + exponentLength;
            }

            modulus = new byte[key.Length - modulusStartIndex];
            Array.Copy(key, modulusStartIndex, modulus, 0, modulus.Length);
            return modulus;
        }
        
        private byte[] GetHashOfSignedData(byte[] signedData)
        {
            var algorithm = new SHA1Cng();
            return algorithm.ComputeHash(signedData);
        }

        public byte[] CombineSignedData(List<byte[]> signedDataArray, byte[] signature)
        {
            var overallLength = signature.Length;
            var dataArray = new List<byte[]>();
            dataArray.Add(signature);
            signedDataArray.Sort(CompareCanonical);
            foreach (var sig in signedDataArray)
            {
                dataArray.Add(sig);
                overallLength += sig.Length;
            }

            var result = CombineAllCanonicalDataArrays(overallLength, dataArray);
            return result;
        }

        private byte[] CombineAllCanonicalDataArrays(int overallLength, List<byte[]> allCanonicalDataFromRecords)
        {
            var allTheData = new byte[overallLength];
            int numOfCopiedBytes = 0;
            for (int i = 0; i < allCanonicalDataFromRecords.Count; i++)
            {
                Array.Copy(
                    allCanonicalDataFromRecords[i],
                    0,
                    allTheData,
                    numOfCopiedBytes,
                    allCanonicalDataFromRecords[i].Length);
                numOfCopiedBytes += allCanonicalDataFromRecords[i].Length;
            }

            return allTheData;
        }

        private int CompareCanonical(byte[] a, byte[] b)
        {
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            {
                int value = 0;
                if ((value = a[i].CompareTo(b[i])) == 0)
                {
                    continue;
                }

                return value;
            }

            return a.Length.CompareTo(b.Length);
        }
    }
}

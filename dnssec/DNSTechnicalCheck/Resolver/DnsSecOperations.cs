using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System;

namespace Resolver
{
    public class DnsSecOperations : IDnsSecOperations
    {
        /// <summary>
        /// Calculates ds records from input data.
        /// </summary>
        /// <param name="list">The input data.</param>
        /// <returns>
        /// list of ds records.
        /// </returns>
        public IList<DSRecord> CalculateRecords(IList<DsInputData> list)
        {
            var records = new List<DSRecord>();

            foreach (DsInputData data in list)
            {
                var recordDs = new DSRecord();
                byte[] inputData = GetInputData(data);
                var test = inputData.ToHexString();
                SetSha1Data(inputData, recordDs, data);
                SetAlgorithm(data, recordDs);
                SetSha256Data(inputData, recordDs);
                SetKey(data, recordDs);

                records.Add(recordDs);
            }

            return records;
        }

        /// <summary>
        /// Calculates ds records from input data.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <returns>
        /// list of ds records.
        /// </returns>
        public DSRecord CalculateRecord(DsInputData data)
        {
            var recordDs = new DSRecord();
            byte[] inputData = GetInputData(data);
            SetSha1Data(inputData, recordDs, data);
            SetAlgorithm(data, recordDs);
            SetSha256Data(inputData, recordDs);
            SetKey(data, recordDs);

            return recordDs;
        }

        /// <summary>
        /// Gets the input data.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <returns>input data</returns>
        private static byte[] GetInputData(DsInputData data)
        {
            byte[] ownerDataBytes = CanonicalFormatHelper.BuildUnpackedDomainInCanonicalForm(data.Domain);
            var test = ownerDataBytes.ToHexString();
            byte[] dnsKeyRRDataBytes = GeRRData(data);
            return ownerDataBytes.Concat(dnsKeyRRDataBytes).ToArray();
        }

        /// <summary>
        /// Ges the RR data.
        /// </summary>
        /// <param name="data">The ds data.</param>
        /// <returns>Converted rr data</returns>
        private static byte[] GeRRData(DsInputData data)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(BitConverter.GetBytes(data.Flags).Reverse().ToArray(), 0, 2);
                var test = BitConverter.GetBytes(data.Flags).Reverse().ToArray();
                ms.WriteByte(data.Protocol);
                ms.WriteByte(data.Algorithm);

                var keybytes = Convert.FromBase64String(data.DnsKey);
                ms.Write(keybytes, 0, keybytes.Length);

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Sets the sha1 data.
        /// </summary>
        /// <param name="inputData">
        /// The input data.
        /// </param>
        /// <param name="record">
        /// The record.
        /// </param>
        /// <param name="data">
        /// The input ds data.
        /// </param>
        private static void SetSha1Data(byte[] inputData, DSRecord record, DsInputData data)
        {
            byte[] sha1Hash;
            using (SHA1 sha1 = SHA1Cng.Create())
            {
                sha1Hash = sha1.ComputeHash(inputData);
            }

            record.DigestType = DigestType.Sha256;
            record.KeyTag = Calculate(GeRRData(data));
            record.DigestFieldText = sha1Hash.ToHexString();
            record.DigestField = sha1Hash;
        }

        /// <summary>
        /// Sets the algorithm.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <param name="record">The record.</param>
        private static void SetAlgorithm(DsInputData data, DSRecord record)
        {
            if (Enum.IsDefined(typeof(DSAlgorithm), Convert.ToInt32(data.Algorithm)))
            {
                record.Algorithm = (DSAlgorithm)data.Algorithm;
            }
            else
            {
                record.Algorithm = DSAlgorithm.None;
            }
        }

        /// <summary>
        /// Sets the sha256 data.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="record">The record.</param>
        private static void SetSha256Data(byte[] inputData, DSRecord record)
        {
            byte[] sha256Hash;
            using (SHA256 sha256 = SHA256Cng.Create())
            {
                sha256Hash = sha256.ComputeHash(inputData);
            }

            record.DigestField256Text = sha256Hash.ToHexString();
            record.DigestField256 = sha256Hash;
            record.DigestType256 = DigestType.Sha256;
        }

        /// <summary>
        /// Sets the key.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <param name="record">The record.</param>
        private static void SetKey(DsInputData data, DSRecord record)
        {
            record.DnsKey = data.DnsKey;
        }

        /// <summary>
        /// Calculates the key tag value for a given dns sec key rr data
        /// </summary>
        /// <param name="dnsKeyRRData">The DNS key RR data.</param>
        /// <returns>The Keytag</returns>
        private static int Calculate(byte[] dnsKeyRRData)
        {
            /*
             * The following port from ANSI C reference implementation calculates the value of
               a Key Tag.  This reference implementation applies to all algorithm
               types except algorithm 1 (see Appendix B.1).  The input is the wire
               format of the RDATA portion of the DNSKEY RR.  The code is written
               for clarity, not efficiency.

            
            * Assumes that int is at least 16 bits.
            * First octet of the key tag is the most significant 8 bits of the
            * return value;
            * Second octet of the key tag is the least significant 8 bits of the
            * return value.


           unsigned int
           keytag (
             unsigned char key[],  // the RDATA part of the DNSKEY RR 
             unsigned int keysize  // the RDLENGTH 
             )
            {
                    unsigned long ac;     // assumed to be 32 bits or larger 
                    int i;                // loop index 

                    for ( ac = 0, i = 0; i < keysize; ++i )
                            ac += (i & 1) ? key[i] : key[i] << 8;
                    ac += (ac >> 16) & 0xFFFF;
                    return ac & 0xFFFF;
            }

            => Yes we can do it more cryptically in C# 
             */

            int accumulator = dnsKeyRRData.Select((t, i) => (i & 1) > 0 ? t : t << 8).Sum();
            accumulator += (accumulator >> 16) & 0xFFFF;
            return accumulator & 0xFFFF;
        }
    }
}

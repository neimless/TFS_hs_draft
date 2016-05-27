namespace Resolver
{
    using ARSoft.Tools.Net.Dns;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// The canonical format helper.
    /// </summary>
    public static class CanonicalFormatHelper
    {
        public static byte[] GetCanonicalFormatData(int ttl, DnsKeyRecord rec)
        {
            var rrdata = GetCanonicalRRData(rec);
            using (var ms = new MemoryStream())
            {
                byte[] domainBytes = CanonicalFormatHelper.BuildUnpackedDomainInCanonicalForm(rec.Name);
                ms.Write(domainBytes, 0, domainBytes.Length);
                ms.Write(BitConverter.GetBytes((ushort)rec.RecordType).Reverse(), 0, 2);
                ms.Write(BitConverter.GetBytes((ushort)rec.RecordClass).Reverse(), 0, 2);
                ms.Write(BitConverter.GetBytes(ttl).Reverse(), 0, 4);
                ms.Write(BitConverter.GetBytes((ushort)rrdata.Length).Reverse(), 0, 2);
                ms.Write(rrdata, 0, rrdata.Length);
                return ms.ToArray();
            }
        }

        public static byte[] GetCanonicalFormatData(RrSigRecord rec)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(BitConverter.GetBytes((ushort)rec.TypeCovered).Reverse(), 0, 2);
                ms.Write(new[] { (byte)rec.Algorithm }, 0, 1);
                ms.Write(new[] { rec.Labels }, 0, 1);
                ms.Write(BitConverter.GetBytes(rec.OriginalTimeToLive).Reverse(), 0, 4);
                ms.Write(BitConverter.GetBytes(ConvertToSeconds(rec.SignatureExpiration)).Reverse(), 0, 4);
                ms.Write(BitConverter.GetBytes(ConvertToSeconds(rec.SignatureInception)).Reverse(), 0, 4);
                ms.Write(BitConverter.GetBytes(rec.KeyTag).Reverse(), 0, 2);
                byte[] domainBytes = BuildUnpackedDomainInCanonicalForm(rec.SignersName);
                ms.Write(domainBytes, 0, domainBytes.Length);
                return ms.ToArray();
            }
        }

        private static byte[] GetCanonicalRRData(DnsKeyRecord rec)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(BitConverter.GetBytes(rec.Flags).Reverse(), 0, 2);
                ms.Write(new[] { rec.Protocol }, 0, 1);
                ms.Write(new[] { (byte)rec.Algorithm }, 0, 1);
                ms.Write(rec.PublicKey, 0, rec.PublicKey.Length);
                return ms.ToArray();
            }
        }

        private static uint ConvertToSeconds(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return (uint)Math.Floor(diff.TotalSeconds);
        }

        /// <summary>
        /// The build unpacked domain in canonical form.
        /// </summary>
        /// <param name="domain">
        /// The domain.
        /// </param>
        /// <returns>
        /// Bytes of the unpacked domain
        /// </returns>
        /// <exception cref="ArgumentNullException">If domain is null
        /// </exception>
        public static byte[] BuildUnpackedDomainInCanonicalForm(string domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            domain = domain.ToLowerInvariant();
            return BuildExactWireFormatWithoutCompression(domain);
        }

        /// <summary>
        /// The build exact wire format without compression.
        /// </summary>
        /// <param name="domain">
        /// The domain.
        /// </param>
        /// <returns>
        /// Builds exact wire format of domain without any compression
        /// </returns>
        private static byte[] BuildExactWireFormatWithoutCompression(string domain)
        {
            var unpacked = new List<byte>();
            for (int i = 0; i < domain.Length; i++)
            {
                UnpackCurrent(domain, unpacked, i);
            }

            unpacked.Add(0);
            return unpacked.ToArray();
        }

        /// <summary>
        /// Gtes the length
        /// </summary>
        /// <param name="labelStartIndex">
        /// The label start index.
        /// </param>
        /// <param name="domain">
        /// The domain.
        /// </param>
        /// <returns>
        /// The length.
        /// </returns>
        private static int GetLength(int labelStartIndex, string domain)
        {
            int labelEndIndex = domain.IndexOf(".", labelStartIndex, StringComparison.OrdinalIgnoreCase);
            return labelEndIndex == -1 ? domain.Length - labelStartIndex : labelEndIndex - labelStartIndex;
        }

        /// <summary>
        /// The unpack current.
        /// </summary>
        /// <param name="domain">
        /// The domain.
        /// </param>
        /// <param name="unpacked">
        /// The unpacked.
        /// </param>
        /// <param name="i">
        /// Index of current character in domain
        /// </param>
        private static void UnpackCurrent(string domain, List<byte> unpacked, int i)
        {
            if (i == 0)
            {
                UnpackFirst(domain, unpacked);
            }
            else if (domain[i] == '.')
            {
                unpacked.Add((byte)GetLength(i + 1, domain));
            }
            else
            {
                unpacked.Add((byte)domain[i]);
            }
        }

        /// <summary>
        /// The unpack first.
        /// </summary>
        /// <param name="domain">
        /// The domain.
        /// </param>
        /// <param name="unpacked">
        /// The unpacked.
        /// </param>
        private static void UnpackFirst(string domain, List<byte> unpacked)
        {
            unpacked.Add((byte)GetLength(0, domain));
            unpacked.Add((byte)domain[0]);
        }
    }
}
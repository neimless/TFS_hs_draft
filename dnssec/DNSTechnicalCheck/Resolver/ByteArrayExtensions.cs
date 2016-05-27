namespace Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Byte array extensions methods
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Reverses byte ordering
        /// </summary>
        /// <param name="data">
        /// The data to reverse
        /// </param>
        /// <returns>
        /// reversed data
        /// </returns>
        public static byte[] Reverse(this byte[] data)
        {
            return Reverse<byte>(data);
        }

        /// <summary>
        /// Reverses ordering of any array
        /// </summary>
        /// <param name="data">
        /// The data to reverse
        /// </param>
        /// <typeparam name="T">Type of the array
        /// </typeparam>
        /// <returns>
        /// Reversed array
        /// </returns>
        /// <exception cref="ArgumentNullException">if the data is null
        /// </exception>
        public static T[] Reverse<T>(this T[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            var reversed = new T[data.Length];
            Array.Copy(data, reversed, data.Length);
            Array.Reverse(reversed);
            return reversed;
        }

        /// <summary>
        /// Extension that parses byte array to hex string
        /// </summary>
        /// <param name="array">Array to parse (this)</param>
        /// <returns>Hex string of the array</returns>
        public static string ToHexString(this byte[] array)
        {
            return ToHexWithCase(array, false);
        }

        /// <summary>
        /// Extension that parses byte array to upper case hex string
        /// </summary>
        /// <param name="array">The array to parse (this)</param>
        /// <returns>Upper case hex string</returns>
        public static string ToHexStringUpper(this byte[] array)
        {
            return ToHexWithCase(array, true);
        }

        /// <summary>
        /// Extensiont that parses hex string to byte array.
        /// </summary>
        /// <param name="hex">Hex data (this)</param>
        /// <exception cref="FormatException">When string has odd number of characters</exception>
        /// <returns>Byte array.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "We prefer lower case, and here it doesn't matter which it is")]
        public static byte[] HexToByteArray(this string hex)
        {
            if (hex == null)
            {
                throw new ArgumentNullException("hex");
            }

            hex = hex.Trim();
            if (string.IsNullOrEmpty(hex))
            {
                return new byte[0];
            }

            hex = hex.ToLowerInvariant();
            if (hex.Length % 2 != 0)
            {
                return new byte[0];
            }

            int numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        /// <summary>
        /// Removes the UTF8 identifier.
        /// </summary>
        /// <param name="data">The data containing the utf-8 identifier</param>
        /// <returns>New byte[] without utf-8 identifier</returns>
        public static byte[] RemoveUtf8Identifier(this byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            // 0xEF,0xBB,0xBF
            if (data.Length > 3 &&
                data[0] == 0xef &&
                data[1] == 0xbb &&
                data[2] == 0xbf)
            {
                var newData = new byte[data.Length - 3];
                Array.Copy(data, 3, newData, 0, newData.Length);
                return newData;
            }

            return data;
        }

        /// <summary>
        /// To hex with lower or upper case
        /// </summary>
        /// <param name="array">Array parameter</param>
        /// <param name="toUpper">Either lower or upper case</param>
        /// <returns>Result string</returns>
        private static string ToHexWithCase(IEnumerable<byte> array, bool toUpper)
        {
            var sb = new StringBuilder();
            if (array != null)
            {
                foreach (byte b in array)
                {
                    sb.Append(b.ToString(toUpper ? "X2" : "x2", CultureInfo.InvariantCulture));
                }
            }

            return sb.ToString();
        }
    }
}

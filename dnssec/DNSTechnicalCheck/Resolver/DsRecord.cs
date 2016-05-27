namespace Resolver
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// DS record class
    /// </summary>
    [Serializable]
    public class DSRecord
    {
        /// <summary>
        /// Gets or sets the key tag.
        /// </summary>
        /// <value>The key tag.</value>
        public int KeyTag { get; set; }

        /// <summary>
        /// Gets or sets IdentityType.
        /// </summary>
        public DSAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Gets or sets the type of the diggest.
        /// </summary>
        /// <value>The type of the diggest.</value>
        public DigestType DigestType { get; set; }

        /// <summary>
        /// Gets or sets the dns key field.
        /// </summary>
        /// <value>The dns key field.</value>
        public string DnsKey { get; set; }

        /// <summary>
        /// Gets or sets the diggest field
        /// </summary>
        /// <value>The diggest  field.</value>
        public byte[] DigestField { get; set; }

        /// <summary>
        /// Gets or sets the diggest field sha256 field.
        /// </summary>
        /// <value>The diggest 256 field.</value>
        public byte[] DigestField256 { get; set; }

        /// <summary>
        /// Gets or sets the type of the diggest.
        /// </summary>
        /// <value>The type of the diggest.</value>
        public DigestType DigestType256 { get; set; }

        /// <summary>
        /// Gets or sets the DnsSecKeyId
        /// </summary>
        /// <value>The DnsSecKeyId guid.</value>
        public Guid DnsSecKeyId { get; set; }

        /// <summary>
        /// Gets or sets the diggest field sha256 field binary.
        /// </summary>
        /// <value>The diggest 256 field.</value>
        public string DigestField256Text { get; set; }

        /// <summary>
        /// Gets or sets the diggest field binary
        /// </summary>
        /// <value>The diggest  field.</value>
        public string DigestFieldText { get; set; }

        /// <summary>
        /// Gets or sets the diggest flags
        /// </summary>
        /// <value>The flags field.</value>
        public int Flags { get; set; }

        /// <summary>
        /// Gets or sets DnsKeyAlgorithm.
        /// </summary>
        public DnsKeyAlgorithm DnsKeyAlgorithm { get; set; }
    }
}

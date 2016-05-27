namespace Resolver
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// DS record input data class
    /// </summary>
    [Serializable]
    public class DsInputData
    {
        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        /// <value>
        /// The flags.
        /// </value>
        public ushort Flags { get; set; }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>
        /// The protocol.
        /// </value>
        public byte Protocol { get; set; }

        /// <summary>
        /// Gets or sets the algorithm.
        /// </summary>
        /// <value>
        /// The algorithm.
        /// </value>
        public byte Algorithm { get; set; }

        /// <summary>
        /// Gets or sets the dns key field.
        /// </summary>
        /// <value>The dns key field.</value>
        public string DnsKey { get; set; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public string Domain { get; set; }
    }
}
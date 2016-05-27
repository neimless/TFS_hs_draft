namespace Resolver
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum definition for dns key Algorithm
    /// </summary>
    public enum DnsKeyAlgorithm : byte
    {
        /// <summary>
        /// Reserved for future use
        /// </summary>
        Reserved = 0,

        /// <summary>
        /// RSA with MD5 hash
        /// </summary>
        RsaMD5 = 1,

        /// <summary>
        /// Diffie-Helman algorithm
        /// </summary>
        DH = 2,

        /// <summary>
        /// DSA with SHA1 hash
        /// </summary>
        DsaSha1 = 3,

        /// <summary>
        /// Ecc algorithm
        /// </summary>
        Ecc = 4,

        /// <summary>
        /// RSA with SHA1 hash
        /// </summary>
        RsaSha1 = 5,

        /// <summary>
        /// DSA with SHA1 hash used with NSEC3
        /// </summary>
        DsaNsec3Sha1 = 6,

        /// <summary>
        /// RSA with SHA1 hash used with NSEC3 
        /// </summary>
        RsaNsec3Sha1 = 7,

        /// <summary>
        /// Indirect algorithm
        /// </summary>
        Indirect = 252,

        /// <summary>
        /// Private algorithm
        /// </summary>
        PrivateDns = 253,

        /// <summary>
        /// Private Oid
        /// </summary>
        PrivateOid = 254,

        /// <summary>
        /// Reserved for future use
        /// </summary>
        Reserved2 = 255
    }

    /// <summary>
    /// Enum definition for ds diggest type
    /// </summary>
    public enum DigestType
    {
        /// <summary>
        /// Invalid diggest type
        /// </summary>
        None = 0,

        /// <summary>
        /// enum definition for sha256
        /// </summary>
        Sha256 = 1,

        /// <summary>
        /// enum definition for sha512
        /// </summary>
        Sha512 = 2
    }

    /// <summary>
    /// Enum definition for ds Algorithm
    /// </summary>
    public enum DSAlgorithm
    {
        /// <summary>
        /// Invalid algorithm type
        /// </summary>
        None = 0,

        /// <summary>
        /// enum definition for RSA/SHA-1 [RSASHA1]
        /// </summary>
        RsaSha1 = 5,
        
        /// <summary>
        /// enum definition for RSA/SHA-1-NSEC3-SHA1
        /// </summary>
        RsaSha1Nsec3Sha1 = 7,

        /// <summary>
        /// enum definition for RSASHA256
        /// </summary>
        RsaSha256 = 8,

        /// <summary>
        /// enum definition for RSASHA512
        /// </summary>
        Rsasha512 = 10,

        /// <summary>
        /// enum definition for EcdsaSha256
        /// </summary>
        EcdsaSha256 = 13,

        /// <summary>
        /// enum definition for EcdsaSha384
        /// </summary>
        EcdsaSha384 = 14
    }
}
using System.Collections.Generic;

namespace Resolver
{
    public interface ISignatureVerifier
    {
        bool EqualArrays(byte[] arrayA, byte[] arrayB);
        bool VerifyCryptographicallyOpenSsl(byte[] signatureData, byte[] signedData, byte[] keyData);
        byte[] CombineSignedData(List<byte[]> signedDataArray, byte[] signature);
    }
}

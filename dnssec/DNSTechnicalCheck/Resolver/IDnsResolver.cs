using ARSoft.Tools.Net.Dns;
using System.Collections.Generic;

namespace Resolver
{
    public interface IDnsResolver
    {
        string ResolveA(string name);
        string ResolveAAAA(string name);
        List<SoaRecord> ResolveSOA(string name);
        List<string> ResolveNS(string name);
        List<DsRecord> ResolveDS(string name);
        List<DnsKeyRecord> ResolveDns(string name);
        List<RrSigRecord> ResolveRRSig(string name);

        ReturnCode GetRCode(string name);
    }
}

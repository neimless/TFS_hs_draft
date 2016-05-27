using ARSoft.Tools.Net.Dns;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Resolver
{
    public class DnsResolver : IDnsResolver
    {
        private IPAddress dnsaddress;
        private DnsClient dnsClient;

        public DnsResolver()
        {
            this.dnsaddress = IPAddress.Parse("8.8.8.8");
            this.dnsClient = new DnsClient(this.dnsaddress, 100000);
        }

        public DnsResolver(string dns)
        {
            var list = Dns.GetHostAddresses(dns);
            this.dnsaddress = Dns.GetHostAddresses(dns).First();
            if (string.IsNullOrEmpty(dns))
            {
                this.dnsaddress = IPAddress.Parse("8.8.8.8");
            }
            this.dnsClient = new DnsClient(this.dnsaddress, 5000);
        }

        public string ResolveA(string name)
        {
            var aresponse = this.dnsClient.Resolve(name, RecordType.A);
            var arecords = aresponse.AnswerRecords.OfType<ARecord>();

            return string.Join(", ", arecords.Select(x => x.Address).ToList());
        }

        public string ResolveAAAA(string name)
        {
            var aresponse = this.dnsClient.Resolve(name, RecordType.Aaaa);
            var arecords = aresponse.AnswerRecords.OfType<AaaaRecord>();

            return string.Join(", ", arecords.Select(x => x.Address).ToList());
        }

        public List<SoaRecord> ResolveSOA(string name)
        {
            var soaresponse = this.dnsClient.Resolve(name, RecordType.Soa);
            var nsrecords = soaresponse.AnswerRecords.OfType<SoaRecord>();
            return nsrecords.ToList();
        }

        public List<Heijden.DNS.RecordSOA> ResolveSOA2(string name)
        {
            var res = new Heijden.DNS.Resolver("8.8.8.8");
            var es = res.Query(name, Heijden.DNS.QType.SOA, Heijden.DNS.QClass.IN);
            var ans = es.Answers.OfType<Heijden.DNS.RecordSOA>();
            return ans.ToList();            
        }

        public List<string> ResolveNS(string name)
        {
            var nsresponse = this.dnsClient.Resolve(name, RecordType.Ns);
            var nsrecords = nsresponse.AnswerRecords.OfType<NsRecord>();
            return nsrecords.Select(x => x.NameServer).ToList();
        }

        public ReturnCode GetRCode(string name)
        {
            var nsresponse = this.dnsClient.Resolve(name);
            return nsresponse.ReturnCode;
        }

        public List<DsRecord> ResolveDS(string name)
        {
            var dsresponse = this.dnsClient.Resolve(name, RecordType.Ds);
            var dsrecords = dsresponse.AnswerRecords.OfType<DsRecord>();
            return dsrecords.ToList();
        }

        public List<DnsKeyRecord> ResolveDns(string name)
        {
            var dnskeyresponse = this.dnsClient.Resolve(name, RecordType.DnsKey);
            var dnskeyrecords = dnskeyresponse.AnswerRecords.OfType<DnsKeyRecord>();
            return dnskeyrecords.ToList();
        }

        public List<RrSigRecord> ResolveRRSig(string name)
        {
            var rrsigresponse = this.dnsClient.Resolve(name, RecordType.RrSig);
            var rrsigrecords = rrsigresponse.AnswerRecords.OfType<RrSigRecord>();
            return rrsigrecords.ToList();
        }
    }
}

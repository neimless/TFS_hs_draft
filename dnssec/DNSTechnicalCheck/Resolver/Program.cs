using ARSoft.Tools.Net.Dns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Resolver
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ISignatureVerifier verifier = new SignatureVerifier();
            IDnsSecOperations dnssec = new DnsSecOperations();

            while (true)
            {
                byte[] signeddata = new byte[0];
                byte[] keydata = new byte[0];
                var signeddataArray = new List<byte[]>();

                Console.WriteLine("Jätä kenttä tyhjäksi lopettaaksesi");
                Console.Write("Syötä domainnimi muodossa 'nimi.fi': ");
                var domainname = Console.ReadLine();
                if (string.IsNullOrEmpty(domainname))
                {
                    return;
                }

                Console.Write("Syötä kohdenimipalvelin muodossa 'ns.nimi.fi': ");
                var dns = Console.ReadLine();
                IDnsResolver resolver = new DnsResolver(dns);

                var rcode = resolver.GetRCode(domainname);
                if (rcode != ReturnCode.NoError)
                {
                    Console.WriteLine(rcode);
                }

                Console.WriteLine("A-tietue: ");
                Console.WriteLine(resolver.ResolveA(domainname));
                Console.WriteLine();

                Console.WriteLine("AAAA-tietue: ");
                Console.WriteLine(resolver.ResolveAAAA(domainname));
                Console.WriteLine();

                var nsrecords = resolver.ResolveNS(domainname);
                Console.WriteLine("NS-tietue: ");
                foreach (var rec in nsrecords)
                {
                    Console.Write(rec);
                    Console.Write(" : " + resolver.ResolveA(rec) + " || " + resolver.ResolveAAAA(rec));
                    Console.WriteLine();
                }
                Console.WriteLine();

                IDnsResolver resolver2 = new DnsResolver();
                var dsrecords = resolver2.ResolveDS(domainname);
                Console.WriteLine("DS-tietue: ");
                foreach (var rec in dsrecords)
                {
                    Console.WriteLine(rec.KeyTag + ", " + rec.Algorithm + ", " + rec.DigestType);
                    //Console.WriteLine(Convert.ToBase64String(rec.Digest));
                    //Console.WriteLine(rec.Digest.ToHexString());
                }
                Console.WriteLine();

                var soarecords = resolver.ResolveSOA(domainname);
                Console.WriteLine("SOA-tietue: ");
                foreach (var record in soarecords)
                {                    
                    Console.WriteLine(record.MasterName + ", " + record.ResponsibleName + ", " + record.SerialNumber);
                    Console.WriteLine(record.RefreshInterval + ", " + record.RetryInterval + ", " + record.ExpireInterval + ", " + record.NegativeCachingTTL);
                }
                Console.WriteLine();
                                
                Console.WriteLine("DNSKEY-tietue: ");
                var dnskeyrecords = resolver.ResolveDns(domainname);
                var dnskeys = new List<DsInputData>();
                foreach (var rec in dnskeyrecords)
                {
                    Console.Write(rec.Algorithm + ", " + rec.Flags + ", " + rec.Protocol + ", " + rec.RecordType + ", " + rec.TimeToLive + ", " + rec.IsZoneKey + ", ");
                    var newkey = new DsInputData();
                    newkey.Flags = rec.Flags;
                    newkey.Domain = domainname;
                    newkey.Protocol = rec.Protocol;
                    newkey.Algorithm = (byte)rec.Algorithm;
                    newkey.DnsKey = Convert.ToBase64String(rec.PublicKey);                    
                    dnskeys.Add(newkey);
                    var sec = dnssec.CalculateRecord(newkey);
                    Console.Write(sec.KeyTag);
                    if (sec.KeyTag == 56862)
                    {
                        keydata = rec.PublicKey;
                    }                    
                    foreach (var ds in dsrecords)
                    {
                        if (verifier.EqualArrays(ds.Digest, sec.DigestField256))
                        {
                            Console.Write("  :  Matching DS found (Sha256)");
                            break;
                        }
                        if (verifier.EqualArrays(ds.Digest, sec.DigestField))
                        {
                            Console.Write("  :  Matching DS found (Sha1)");
                        }
                    }
                    //Console.WriteLine(sec.DigestField256Text);
                    //Console.WriteLine(sec.DigestFieldText);
                    var data = CanonicalFormatHelper.GetCanonicalFormatData(300, rec);
                    signeddataArray.Add(data);
                    Console.WriteLine();
                }
                Console.WriteLine();

                Console.WriteLine("RRSIG-tietue: ");
                var rrsigrecords = resolver.ResolveRRSig(domainname);
                foreach (var rec in rrsigrecords)
                {
                    Console.WriteLine(rec.TypeCovered + ", " + rec.Labels + ", " + rec.KeyTag + ", " 
                        + rec.Algorithm + ", " + rec.SignersName + ", " + rec.SignatureExpiration + ", " 
                        + rec.OriginalTimeToLive + ", " + rec.Name);

                    var canSignature = CanonicalFormatHelper.GetCanonicalFormatData(rec);
                    if (rec.KeyTag == 56862 && rec.TypeCovered == RecordType.DnsKey)
                    {
                        var combo = verifier.CombineSignedData(signeddataArray, canSignature);
                        //File.AppendAllText("C:\\combonsis.txt", "Keydata:" + System.Environment.NewLine);
                        //File.AppendAllText("C:\\combonsis.txt", keydata.ToHexString() + System.Environment.NewLine);
                        //File.AppendAllText("C:\\combonsis.txt", "Signed data:" + System.Environment.NewLine);
                        //File.AppendAllText("C:\\combonsis.txt", combo.ToHexString() + System.Environment.NewLine);
                        //File.AppendAllText("C:\\combonsis.txt", "Signature:" + System.Environment.NewLine);
                        //File.AppendAllText("C:\\combonsis.txt", rec.Signature.ToHexString() + System.Environment.NewLine);
                        verifier.VerifyCryptographicallyOpenSsl(rec.Signature, combo, keydata);
                        //File.WriteAllText("C:\\combonsis.txt", combo.ToHexString());
                    }
                }
                Console.WriteLine();

                Console.WriteLine("Paina mitä tahansa näppäintä jatkaaksesi");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}

﻿using NUnit.Framework;
using System.Collections.Generic;

// ReSharper disable UnusedMethodReturnValue.Local

namespace DnsRip.Tests
{
    [TestFixture]
    public class ParserTests
    {
        public class ParseTest
        {
            public string Input { get; set; }
            public string Evaluated { get; set; }
            public string Parsed { get; set; }
            public DnsRip.InputType Type { get; set; }
        }

        private static IEnumerable<ParseTest> GetParseTests()
        {
            var tests = new List<ParseTest>
            {
                new ParseTest
                {
                    Input = "192.168.10.1",
                    Evaluated = "192.168.10.1",
                    Parsed = "192.168.10.1",
                    Type = DnsRip.InputType.Ip
                },
                new ParseTest
                {
                    Input = "http://192.168.10.1",
                    Evaluated = "http://192.168.10.1",
                    Parsed = "192.168.10.1",
                    Type = DnsRip.InputType.Ip
                },
                new ParseTest
                {
                    Input = "http://192.168.10.1/",
                    Evaluated = "http://192.168.10.1/",
                    Parsed = "192.168.10.1",
                    Type = DnsRip.InputType.Ip
                },
                new ParseTest
                {
                    Input = "http://192.168.10.1:80",
                    Evaluated = "http://192.168.10.1:80",
                    Parsed = "192.168.10.1",
                    Type = DnsRip.InputType.Ip
                },
                new ParseTest
                {
                    Input = "http://192.168.10.1:80/",
                    Evaluated = "http://192.168.10.1:80/",
                    Parsed = "192.168.10.1",
                    Type = DnsRip.InputType.Ip
                },
                new ParseTest
                {
                    Input = "2001:cdba:0000:0000:0000:0000:3257:9652",
                    Evaluated = "2001:cdba:0000:0000:0000:0000:3257:9652",
                    Parsed = "2001:cdba:0000:0000:0000:0000:3257:9652",
                    Type = DnsRip.InputType.Ip
                },
                new ParseTest
                {
                    Input = " 2001:cdba:0:0:0:0:3257:9652  ",
                    Evaluated = "2001:cdba:0:0:0:0:3257:9652",
                    Parsed = "2001:cdba:0:0:0:0:3257:9652",
                    Type = DnsRip.InputType.Ip
                },
                new ParseTest
                {
                    Input = "2001:cdba::3257:9652",
                    Evaluated = "2001:cdba::3257:9652",
                    Parsed = "2001:cdba::3257:9652",
                    Type = DnsRip.InputType.Ip
                },
                new ParseTest
                {
                    Input = "http://[2001:cdba:0000:0000:0000:0000:3257:9652]/",
                    Evaluated = "http://[2001:cdba:0000:0000:0000:0000:3257:9652]/",
                    Parsed = "2001:cdba:0000:0000:0000:0000:3257:9652",
                    Type = DnsRip.InputType.Ip
                },
                new ParseTest
                {
                    Input = "http://[2001:cdba:0000:0000:0000:0000:3257:9652]:80/",
                    Evaluated = "http://[2001:cdba:0000:0000:0000:0000:3257:9652]:80/",
                    Parsed = "2001:cdba:0000:0000:0000:0000:3257:9652",
                    Type = DnsRip.InputType.Ip
                },
                new ParseTest
                {
                    Input = "FE80:0000:0000:0000:0202:B3FF:FE1E:8329",
                    Evaluated = "fe80:0000:0000:0000:0202:b3ff:fe1e:8329",
                    Parsed = "fe80:0000:0000:0000:0202:b3ff:fe1e:8329",
                    Type = DnsRip.InputType.Ip
                },
                new ParseTest
                {
                    Input = "FE80::0202:B3FF:FE1E:8329",
                    Evaluated = "fe80::0202:b3ff:fe1e:8329",
                    Parsed = "fe80::0202:b3ff:fe1e:8329",
                    Type = DnsRip.InputType.Ip
                },
                new ParseTest
                {
                    Input = "random_string",
                    Evaluated = "random_string",
                    Parsed = null,
                    Type = DnsRip.InputType.Invalid
                },
                new ParseTest
                {
                    Input = " RANDOM_string ",
                    Evaluated = "random_string",
                    Parsed = null,
                    Type = DnsRip.InputType.Invalid
                },
                new ParseTest
                {
                    Input = "hostname.com",
                    Evaluated = "hostname.com",
                    Parsed = "hostname.com",
                    Type = DnsRip.InputType.Hostname
                },
                new ParseTest
                {
                    Input = "www.hostname.com",
                    Evaluated = "www.hostname.com",
                    Parsed = "www.hostname.com",
                    Type = DnsRip.InputType.Hostname
                },
                new ParseTest
                {
                    Input = "www.www.hostname.com",
                    Evaluated = "www.www.hostname.com",
                    Parsed = "www.www.hostname.com",
                    Type = DnsRip.InputType.Hostname
                },
                new ParseTest
                {
                    Input = "http://www.hostname.com",
                    Evaluated = "http://www.hostname.com",
                    Parsed = "www.hostname.com",
                    Type = DnsRip.InputType.Hostname
                },
                new ParseTest
                {
                    Input = "http://www.hostname.com:80",
                    Evaluated = "http://www.hostname.com:80",
                    Parsed = "www.hostname.com",
                    Type = DnsRip.InputType.Hostname
                },
                new ParseTest
                {
                    Input = "http://www.hostname.com/",
                    Evaluated = "http://www.hostname.com/",
                    Parsed = "www.hostname.com",
                    Type = DnsRip.InputType.Hostname
                },
                new ParseTest
                {
                    Input = "http://Hostname/  ",
                    Evaluated = "http://hostname/",
                    Parsed = null,
                    Type = DnsRip.InputType.Invalid
                }
            };

            foreach (var test in tests)
            {
                yield return test;
            }
        }

        [Test, TestCaseSource(nameof(GetParseTests))]
        public void ParseInput(ParseTest parseTest)
        {
            var result = DnsRip.Tools.Parse(parseTest.Input);
            Assert.That(result.Evaluated, Is.EqualTo(parseTest.Evaluated));
            Assert.That(result.Parsed, Is.EqualTo(parseTest.Parsed));
            Assert.That(result.Type, Is.EqualTo(parseTest.Type));
        }

        //TODO: Cleanup

        //[Test]
        //public void HaveSubDomainsIfHostname()
        //{
        //    var dnsRip = new DnsRip("domain.com");
        //    Assert.That(dnsRip.SubDomains, Is.All.EndsWith("."));
        //}

        //[Test]
        //public void NotHaveSubDomainsIfIp()
        //{
        //    var dnsRip = new DnsRip("192.168.10.1");
        //    Assert.That(dnsRip.SubDomains, Is.Null);
        //}
    }
}
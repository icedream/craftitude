using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Craftitude.ClientApi.Resolvers
{
    [ResolverName("mediafire")]
    public class MediafireResolver : ResolverBase
    {
        public override Stream ResolveToStream()
        {
            if (!Parameters.ContainsKey("id"))
                throw new ArgumentException("Needs MediaFire file ID");

            var wc = new WebClient();
            var originUrl = new Uri(string.Format("http://mediafire.com/?{0}", Parameters["id"]));
            var url = originUrl;
            Uri oldM = null;

            MemoryStream ms = new MemoryStream();

        retry1:
            Dictionary<string, string> headers = new Dictionary<string, string>();

            // Mediafire has buggy HTTP servers which tend to violate the protocol, crashing the whole download with a ProtocolViolationException.
            // We are going to do a manual request with manual parsing here. ._.
            var socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket1.Connect(new DnsEndPoint(url.Host, 80));
            var s = new NetworkStream(socket1);
            using (var sr = new StreamReader(s))
            using (var sw = new StreamWriter(s))
            {
                sw.WriteLine("GET {0} HTTP/1.0", url.PathAndQuery);
                sw.WriteLine("Host: {0}", url.Host);
                sw.WriteLine("User-Agent: ModernMinasLauncher/3.0 (U; compatible)");
                if (oldM != null)
                    sw.WriteLine("Referer: {0}", oldM.ToString());
                sw.WriteLine("Connection: close");
                sw.WriteLine();
                sw.Flush();
                var status = sr.ReadLine().Trim();
                Log.InfoFormat("Server returned status: {0}", status);
                if (!new[] { "200", "301", "302", "303" }.Contains(status.Split(' ')[1]))
                    throw new WebException("HTTP error: " + status);
                string line = "";
                while ((line = sr.ReadLine().Trim()).Any())
                {
                    var l = line.Split(':');
                    string n = l[0].ToLower();
                    string v = string.Join(":", l.Skip(1)).Substring(1);
                    headers.Add(n, v);
                }

                oldM = url;
                if (headers.ContainsKey("location"))
                {
                    url = new Uri(url, headers["location"]);
                    goto retry1;
                }

                long length;
                byte[] buffer;

                if (headers.ContainsKey("content-type") && headers["content-type"].Contains("text/html"))
                {
                    Uri m;

                    var c = sr.ReadToEnd();

                    // download_repair.php, fixes empty URI exception.
                    if (c.Contains("There was a problem with your download"))
                    {
                        m = originUrl;
                    }
                    else
                    {
                        // Find javascripted URI of direct download
                        m = new Uri(Regex.Match(c, "kNO = \"(.+)\"").Groups[1].Value);
                    }

                    url = m;
                    goto retry1;
                }

                if (!headers.ContainsKey("content-length"))
                {
                    throw new Exception("Mediafire server did not send back content-length header. Please contact the developer.");
                }

                length = long.Parse(headers["content-length"]);

                buffer = new byte[4096];

                // TODO: Use event locks here to make all async threads more efficient

                Task.Factory.StartNew(() =>
                {
                    var l0 = 0;
                    var l = 1;
                    while (l0 < 3) // ignore length for debugging purposes
                    {
                        l = sr.BaseStream.Read(buffer, 0, buffer.Length);
                        if (l == 0)
                        {
                            l0++;
                            continue;
                        }
                        else
                        {
                            l0 = 0;
                            ms.Write(buffer, 0, l);
                        }
                    }

                    length = ms.Position;
                });
                Task.Factory.StartNew(() =>
                {
                    while (ms.Position < length)
                    {
                        // TODO: Add download status monitoring
                        System.Threading.Thread.Sleep(50);
                    }
                });
                while (ms.Position < length)
                {
                    System.Threading.Thread.Sleep(50);
                }

                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);

                return ms;
            }
        }
    }
}

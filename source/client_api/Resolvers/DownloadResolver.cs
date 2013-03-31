using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Craftitude.ClientApi.Resolvers
{
    [ResolverName("download")]
    public class DownloadResolver : ResolverBase
    {
        public override string ResolveToString()
        {
            if (!Parameters.Contains("url") || string.IsNullOrEmpty(Parameters["url"].ToString()))
                throw new InvalidOperationException();

            string url = Parameters["url"].ToString();
            
            string resultString = null;

            using (var wc = new WebClient())
            {
                var tlock = new EventWaitHandle(false, EventResetMode.ManualReset);
                wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler((sender, e) =>
                {
                    // TODO: Add progress monitoring
                });
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler((sender, e) =>
                {
                    resultString = e.Result;
                    tlock.Set();
                });
                wc.DownloadStringAsync(new Uri(url));
                tlock.WaitOne();
                tlock.Dispose();
            }

            return resultString;
        }

        public override Stream ResolveToStream()
        {
            if (!Parameters.Contains("url") || string.IsNullOrEmpty(Parameters["url"].ToString()))
                throw new InvalidOperationException();

            string url = Parameters["url"].ToString();

            MemoryStream targetStream = new MemoryStream();

            using (var wc = new WebClient())
            {
                using (var input = wc.OpenRead(url))
                {
                    long length = long.Parse(wc.ResponseHeaders[HttpResponseHeader.ContentLength]);

                    // TODO: Use event locks here to make all async threads more efficient

                    // Transfer thread
                    Task.Factory.StartNew(() =>
                    {
                        while (targetStream.Position < length)
                        {
                            byte[] buffer = new byte[1024];
                            var read = input.Read(buffer, 0, buffer.Length);
                            targetStream.Write(buffer, 0, read);
                        }
                    });

                    // Status thread
                    Task.Factory.StartNew(() =>
                    {
                        while (targetStream.Position < length)
                        {
                            // TODO: Add progress monitoring
                            System.Threading.Thread.Sleep(50);
                        }
                    });

                    while (targetStream.Position < length)
                    {
                        System.Threading.Thread.Sleep(50);
                    }

                    targetStream.Flush();
                    targetStream.Seek(0, SeekOrigin.Begin);
                }
            }

            return targetStream;
        }
    }
}

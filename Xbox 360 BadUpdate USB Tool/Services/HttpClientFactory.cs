using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
namespace Xbox_360_BadStick.Services
{
    public static class HttpClientFactory
    {
        private static readonly ConcurrentDictionary<string, HttpClient> _clients = 
                new ConcurrentDictionary<string, HttpClient>();

        public static HttpClient GetClient(string name, Action<HttpClient> configure = null)
        {
            return _clients.GetOrAdd(name, _ =>
            {
                var client = new HttpClient();

                configure?.Invoke(client);

                if (client.BaseAddress != null)
                {
                    var sp = ServicePointManager.FindServicePoint(client.BaseAddress);
                    sp.ConnectionLeaseTimeout = (int)TimeSpan.FromMinutes(2).TotalMilliseconds;
                    sp.ConnectionLimit = 100;
                }
                
                client.DefaultRequestHeaders.UserAgent.ParseAdd(Shared.Settings.UserAgent);
                return client;
            });
        }
    }
}
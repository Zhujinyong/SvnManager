using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;

namespace Centa.SvnLog.WebApi.UnitTest
{
    public class TestClientProvider : IDisposable
    {
        public TestServer _server;

        public HttpClient Client { get; }


        public TestClientProvider()
        {
            var projectDir = Directory.GetCurrentDirectory() + @"..\..\..\..\..\Centa.SvnLog.WebApi";
            var config = new ConfigurationBuilder()
                 .SetBasePath(projectDir)
                 .AddJsonFile("appsettings.json", optional: false)
                 .Build();
            var webHost = WebHost.CreateDefaultBuilder()
                .UseConfiguration(config)
                .UseStartup<Startup>();
            _server = new TestServer(webHost);
            Client = _server.CreateClient();
          //  Client.BaseAddress = new Uri("http://10.1.32.13:8050");
        }

        public void Dispose()
        {
            _server?.Dispose();
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UserManagement.Tests
{
    //https://samueleresca.net/2017/03/unit-testing-asp-net-core-identity/
    //The to properly research to use this
    public class StartupTestFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly TestServer Server;
        public StartupTestFixture()
        {
            var builder = new WebHostBuilder()
                .UseStartup<TStartup>();

            Server = new TestServer(builder);
        }
        public void Dispose()
        {
            Server.Dispose();
        }
    }
}

using System;

namespace MangaDexSharp.Tests
{
    public class TestFixture : IDisposable
    {
        public MangaDexClient Client { get; }

        public TestFixture()
        {
            Client = new MangaDexClient();
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
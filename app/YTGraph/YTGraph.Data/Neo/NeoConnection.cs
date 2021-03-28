using System;
using Microsoft.Extensions.Options;
using Neo4j.Driver;
using YTGraph.Data.Neo.Configuration;

namespace YTGraph.Data.Neo
{
    public class NeoConnection : IDisposable
    {
        private readonly IDriver _database;
        public NeoConnection(IOptionsMonitor<NeoConfiguration> configMonitor)
        {
            var currentConfig = configMonitor.CurrentValue;
            _database = GraphDatabase.Driver(currentConfig.Uri, AuthTokens.Basic(currentConfig.User, currentConfig.Password));
        }

        public IAsyncSession GetSession() => _database.AsyncSession();

        public void Dispose()
        {
            _database?.Dispose();
        }
    }
}
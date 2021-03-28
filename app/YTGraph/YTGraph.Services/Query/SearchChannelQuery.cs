using System;
using System.Threading.Tasks;
using Neo4j.Driver;
using YTGraph.Data.Neo;

namespace YTGraph.Services.Query
{
    public class SearchChannelQuery : IDisposable
    {
        private readonly NeoConnection _connection;
        
        public SearchChannelQuery(NeoConnection connection)
        {
            _connection = connection;
        }
        
        public async Task DoSomething()
        {
            var session = _connection.GetSession();
            var result = await session.RunAsync("");
            var typedResult = (await result.SingleAsync()).As<string>();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _connection?.Dispose();
        }
    }
}
using StackExchange.Redis;

namespace Redis.Sentinel
{
    public class RedisService
    {
        static ConfigurationOptions sentinelOptions => new()
        {
            EndPoints =
            {
                {"localhost", 6383},
                {"localhost", 6384},
                {"localhost", 6385}
            },
            CommandMap = CommandMap.Sentinel,
            AbortOnConnectFail = false
        };

        static ConfigurationOptions masterOptions => new()
        {
            AbortOnConnectFail = false
        };

        static public async Task<IDatabase> RedisMasterDatabase()
        {
            ConnectionMultiplexer sentinelConnection = await ConnectionMultiplexer.SentinelConnectAsync(sentinelOptions);

            System.Net.EndPoint masterEndpoint = null;

            foreach (var endPoint in sentinelConnection.GetEndPoints())
            {
                IServer server = sentinelConnection.GetServer(endPoint);
                if (!server.IsConnected)
                    continue;
                masterEndpoint = await server.SentinelGetMasterAddressByNameAsync("mymaster"); //conf'daki ad
                break;
            }

            var localMasterIP = masterEndpoint.ToString() switch
            {
                "172.20.0.2:6379" => "localhost:6379",
                "172.20.0.3:6379" => "localhost:6380", 
                "172.20.0.4:6379" => "localhost:6381",
                "172.20.0.5:6379" => "localhost:6382"
            };

            ConnectionMultiplexer masterConnection = await ConnectionMultiplexer.ConnectAsync(localMasterIP);

            IDatabase db = masterConnection.GetDatabase();
            return db;
        }
    }
}

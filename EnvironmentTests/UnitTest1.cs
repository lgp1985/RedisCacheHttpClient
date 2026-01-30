using StackExchange.Redis;
using NRedisStack;
using NRedisStack.RedisStackCommands;

namespace EnvironmentTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

            ConfigurationOptions conf = new ConfigurationOptions
            {
                EndPoints = { "localhost:6379" },
                //User = "yourUsername",
                //Password = "yourPassword"
            };

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(conf);
            IDatabase db = redis.GetDatabase();

            db.StringSet("foo", "bar");
            Console.WriteLine(db.StringGet("foo")); // prints bar
        }
        [Fact]
        public async Task Test2()
        {
            var muxer = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
            var db = muxer.GetDatabase();
        }
    }
}

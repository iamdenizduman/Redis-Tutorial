using StackExchange.Redis;

ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:1453");
ISubscriber subs = connection.GetSubscriber();

await subs.SubscribeAsync("mychannel", (channel, message) =>
{
    Console.WriteLine();
    Console.WriteLine("Gelen: " + message);
});

while (true)
{
    Console.WriteLine("---");
    Console.Write("Mesaj: ");
    string msg = Console.ReadLine();
    await subs.PublishAsync("mychannel", msg);   
    Thread.Sleep(1500);
}


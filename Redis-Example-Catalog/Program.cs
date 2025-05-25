using Redis_Example_Catalog.Redis;
using Redis_Example_Catalog.Repositories;
using Redis_Example_Catalog.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RedisService>(sp =>
{
    return new RedisService("localhost", 1453);
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<IProductService>();
    await service.PrewarmCacheAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
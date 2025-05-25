using Redis_Example_Session;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<RedisService>(sp =>
{
    return new RedisService("localhost", 1453);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

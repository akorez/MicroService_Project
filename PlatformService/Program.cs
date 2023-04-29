using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataService;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

if (!builder.Environment.IsDevelopment())
{
	Console.WriteLine("--> Using Memory Db");
	builder.Services.AddDbContext<AppDbContext>(options =>
	{
		options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"));
	});
}
else
{
    Console.WriteLine("--> Using Memory Db");
    builder.Services.AddDbContext<AppDbContext>(options =>
	{
		options.UseInMemoryDatabase("InMemDb");
	});
}



builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddSingleton<IMessageBusClient,MessageBusClient>();

builder.Services.AddGrpc();

Console.WriteLine($"CommandService Endpoint {builder.Configuration["CommandService"]}");


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

PrepDb.PrepPopulation(app,builder.Environment.IsDevelopment());

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<GrpcPlatformService>();

app.MapGet("/protos/platforms.proto", async context => {
	await context.Response.WriteAsync(File.ReadAllText("Protos/plaforms.proto"));
});

app.Run();

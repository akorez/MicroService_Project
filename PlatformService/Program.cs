using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<AppDbContext>(options=> {
    options.UseInMemoryDatabase("InMemDb");
});

builder.Services.AddScoped<IPlatformRepository,PlatformRepository>();

builder.Services.AddHttpClient<ICommandDataClient,HttpCommandDataClient>();

Console.WriteLine($"CommandService Endpoint {builder.Configuration["CommandService"]}");


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

PrepDb.PrepPopulation(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
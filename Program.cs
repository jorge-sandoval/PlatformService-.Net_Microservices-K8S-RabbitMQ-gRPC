using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataService.Grpc;
using PlatformService.SyncDataService.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsProduction())
{
    Console.WriteLine($"--> Using SQL");
    var connectionString = builder.Configuration.GetConnectionString("PlatformsConnection");
    builder.Services.AddDbContext<AppDbContext>(
        options => options.UseSqlServer(connectionString)
    );
}
else
{
    Console.WriteLine($"--> Using InMemoryDB");
    builder.Services.AddDbContext<AppDbContext>(
        options => options.UseInMemoryDatabase("InMemoryDB")
    );
}

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

PrepDb.PrepPopulation(app, app.Environment);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();
app.MapGet(
    "/protos/platforms.proto",
    async context => {
        await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
    }
);

Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["CommandService"]}");

app.Run();


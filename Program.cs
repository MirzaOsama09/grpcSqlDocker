using GrpcGreeter.Models;
using GrpcGreeter.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
var provider = builder.Services.BuildServiceProvider();
var Configuration = provider.GetService<IConfiguration>();

// Add services to the container.
builder.Services.AddGrpc();
//builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDbContext<UserDbContext>(options =>
{
    var server = Configuration.GetValue<string>("ServerName");
    var port = "11433";
    var database = Configuration.GetValue<string>("Database");
    var user = Configuration.GetValue<string>("UserName");
    var password = Configuration.GetValue<string>("Password");

    options.UseSqlServer(
        $"Server={server},{port};Initial Catalog={database};User ID={user};Password={password}");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<UserService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

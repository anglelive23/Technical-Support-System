using TicketService.Entities.Config;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

// Data
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<TicketContext>();
builder.Services.AddDbContext<TicketContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Mapster Config
builder.Services.RegisterMapsterConfiguration();

// Serilog
builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = builder.Build();
app.MapGrpcService<TicketsService>();
app.Run();

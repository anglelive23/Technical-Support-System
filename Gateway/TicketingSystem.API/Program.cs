var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddOData(options =>
    {
        options.AddRouteComponents("api/odata", new TicketingEntityDataModel().GetEntityDataModel())
        .Select().Filter().Count().SetMaxTop(1000).Expand().OrderBy();
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TicketingSystem", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// gRPC
builder.Services.AddScoped<ITicketsGrpcDataClient, TicketsGrpcDataClient>();

// Mapster Config
builder.Services.RegisterMapsterConfiguration();

// Data
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<TicketingContext>();
builder.Services.AddDbContext<TicketingContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// JWT
builder.Services.Configure<JWT>(builder.Configuration.GetSection(nameof(JWT)));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    //opt.SaveToken = false;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? "OdkSeYWNl/ECZJaRsjzTqQ9rGb7jp0Ovp57idk1LeGs=")),
        ClockSkew = TimeSpan.Zero
    };
});

// Serilog
builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Cache
builder.Services.AddOutputCache(options =>
{
    // Collections
    options.AddPolicy("Tickets", policy => policy.Tag("Tickets").Expire(TimeSpan.FromHours(1)));
    options.AddPolicy("Projects", policy => policy.Tag("Projects").Expire(TimeSpan.FromHours(1)));
    options.AddPolicy("Clients", policy => policy.Tag("Clients").Expire(TimeSpan.FromHours(1)));
    options.AddPolicy("Employees", policy => policy.Tag("Employees").Expire(TimeSpan.FromHours(1)));
    // Single Item
    options.AddPolicy("Ticket", policy => policy.Tag("Tickets").SetVaryByQuery("Id").Expire(TimeSpan.FromHours(1)));
    options.AddPolicy("Project", policy => policy.Tag("Project").SetVaryByQuery("Id").Expire(TimeSpan.FromHours(1)));
    options.AddPolicy("Client", policy => policy.Tag("Client").SetVaryByQuery("Id").Expire(TimeSpan.FromHours(1)));
    options.AddPolicy("Employee", policy => policy.Tag("Employee").SetVaryByQuery("Id").Expire(TimeSpan.FromHours(1)));
});

// Cors
builder.Services.AddCors();

try
{
    Log.Information("Starting web server...");
    var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
    app.MapControllers();
    app.UseOutputCache();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal($"Starting web server failed {ex.Message}");
}


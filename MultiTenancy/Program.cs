var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddMultiTenantOptions<TenantSettings>(HandleEmptyConnectionString);
builder.Services.AddMultiTenantDbContxet();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IProductService, ProductService>();
var app = builder.Build();
await ApplyMultiTenantDbContext(app);
var tenantSettings = app.Services.GetRequiredService<IOptions<TenantSettings>>().Value;
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
static void HandleEmptyConnectionString(TenantSettings tenantOptions)
{
    foreach (var tenant in tenantOptions.Tenants)
    {
        if (string.IsNullOrWhiteSpace(tenant.Value.ConnectionString))
        {
            tenant.Value.ConnectionString = tenantOptions.Defaults?.ConnectionString;
        }
    }
}
static async Task ApplyMultiTenantDbContext(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var serviceProvider = scope.ServiceProvider;
    var tenantSettings = serviceProvider.GetRequiredService<IOptions<TenantSettings>>().Value;
    var dbcontex = serviceProvider.GetRequiredService<ApplicationDbContext>();
    foreach (var tenant in tenantSettings.Tenants)
    {
        dbcontex.Database.SetConnectionString(tenant.Value.ConnectionString);
        var pendingMigrations = await dbcontex.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            await dbcontex.Database.MigrateAsync();
        }
    }


}
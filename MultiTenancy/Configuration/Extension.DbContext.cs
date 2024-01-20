namespace MultiTenancy.Configuration;
public partial class Extension
{
    public static  IServiceCollection AddMultiTenantDbContxet(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var tenantSettings = serviceProvider
            .GetRequiredService<IOptions<TenantSettings>>()
            .Value;
            options.UseDatabaseProvider(tenantSettings.Defaults.DatabaseProvider);
        });
        return services;
    }
    public static void UseDatabaseProvider(this DbContextOptionsBuilder optionsBuilder, string databaseProvider, string connectionString = "")
    {
        var databaseProviderFactory = DataBasePrviderFactor.Create(databaseProvider);
        databaseProviderFactory.UseDatabaseProvider(optionsBuilder, connectionString);
    }

}

public static class DataBasePrviderFactor
{
    public static IDataBaseProvider Create(string databaseProvider)
    {
        return databaseProvider?.ToLowerInvariant() switch
        {
            "mssql" => new MsSqlServerProvider(),
            _ => throw new NotImplementedException()
        };
    }
}

namespace MultiTenancy.Configuration;
public static partial class Extension
{
    public static IServiceCollection AddMultiTenantOptions<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions)
        where TOptions : class
    {
        services
            .AddOptions<TOptions>()
            .Configure<IConfiguration>(
            (option, configuration) =>
             configuration.GetSection(typeof(TOptions).Name)
            .Bind(option))
            .PostConfigure(configureOptions);
        
        return services;
    }
}

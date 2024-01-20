namespace MultiTenancy.Services;
public class TenantService : ITenantService
{
    private readonly HttpContext? _httpContext;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly TenantSettings _tenantOptions;
    private readonly Tenant? _currentTenant;
    public TenantService(IHttpContextAccessor contextAccessor, IOptions<TenantSettings> options)
    {
        _tenantOptions = options.Value;
        _contextAccessor = contextAccessor;
        _httpContext = _contextAccessor?.HttpContext;
        if (_httpContext is not null)
        {
            _currentTenant = TryGetCurrentTenantFromHeader();
        }
    }
    public string? GetConnectionString()
    {
        return _currentTenant?.ConnectionString;
    }

    public Tenant GetCurrentTenant()
    {
        return _currentTenant;
    }

    public string? GetDatabaseProvider()
    {
        return _tenantOptions.Defaults.DatabaseProvider;
    }

    private Tenant TryGetCurrentTenantFromHeader()
    {
        if (!_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
        {
            throw new ArgumentException("No Tenent Provided");
        }

        if (!_tenantOptions.Tenants.TryGetValue(tenantId!, out var currentTenant))
        {
            throw new ArgumentException("Invalid TenantId");
        }
        return currentTenant;
    }
}

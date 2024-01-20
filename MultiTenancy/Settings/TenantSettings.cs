namespace MultiTenancy.Settings;
public class TenantSettings
{
    public Configuration Defaults { get; set; } = default!;
    public Dictionary<string, Tenant> Tenants { get; set; } = new();
}

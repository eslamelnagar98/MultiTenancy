﻿namespace MultiTenancy.Settings;
public class Tenant
{
    public string Name { get; set; } = null!;
    public string TenantId { get; set; } = null!;
    public string? ConnectionString { get; set; }
}

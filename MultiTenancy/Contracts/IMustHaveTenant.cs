namespace MultiTenancy.Contracts;
public interface IMustHaveTenant
{
    string TenantId { get; set; }
}

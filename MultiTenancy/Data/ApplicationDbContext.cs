namespace MultiTenancy.Data;
public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    private readonly ITenantService _tenantService;
    private readonly string _tenantId;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService) : base(options)
    {
        _tenantService = tenantService;
        _tenantId = _tenantService?.GetCurrentTenant()?.TenantId!;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _tenantService.GetConnectionString();
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }
        optionsBuilder.UseDatabaseProvider(_tenantService.GetDatabaseProvider()!, connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Product>()
            .HasQueryFilter(e => e.TenantId == _tenantId);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTenantFilter();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyTenantFilter()
    {
        var newTenatEntries = ChangeTracker
             .Entries<IMustHaveTenant>()
             .Where(e => e.State is EntityState.Added);
        foreach (var entry in newTenatEntries)
        {
            entry.Entity.TenantId = _tenantId;
        }
    }

}

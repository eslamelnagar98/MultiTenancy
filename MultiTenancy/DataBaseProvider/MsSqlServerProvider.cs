namespace MultiTenancy.DataBaseProvider;
public class MsSqlServerProvider : IDataBaseProvider
{
    public void UseDatabaseProvider(DbContextOptionsBuilder optionsBuilder, string connectionString = "")
    {
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            optionsBuilder.UseSqlServer(connectionString);
            return;
        }

        optionsBuilder.UseSqlServer();
    }
}

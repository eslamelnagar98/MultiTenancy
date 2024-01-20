namespace MultiTenancy.DataBaseProvider;
public interface IDataBaseProvider
{
    void UseDatabaseProvider(DbContextOptionsBuilder optionsBuilder, string connectionString = "");
}

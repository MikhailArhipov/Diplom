namespace TaskSheduler.DAL;

public interface IDomainObject
{
    [SQLite.AutoIncrement, SQLite.PrimaryKey]
    int Id { get; set; }
}

public interface IRepository<T> where T : new()
{
    int AddOrUpdate(T entity);
    int Delete(T entity);
    IEnumerable<T> GetAll();
}

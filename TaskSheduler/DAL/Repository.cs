using SQLite;

namespace TaskSheduler.DAL;

public class Repository<T> : IRepository<T> where T : IDomainObject, new()
{
    public Repository(string ConnString)
    {
        database = new SQLiteConnection(ConnString);
        database.CreateTable<T>(); 
    }

    SQLiteConnection database;

    public int AddOrUpdate(T entity) => entity.Id switch
    {
        0 => database.Insert(entity),
        _ => database.Update(entity)
    };
    //return entity.Id;

    public int Delete(T entity) => database.Delete(entity);

    public IEnumerable<T> GetAll() => database.Table<T>().ToList();
}

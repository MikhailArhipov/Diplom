using SQLite;

namespace TaskSheduler.DAL;

/// <summary> Интерфейс данных для SQL базы </summary>
public interface IDomainObject  //
{
    int Id { get; set; }
}

/// <summary> Интерфейс работы с SQL базой </summary>
public interface IRepository<T> where T : new()
{
    int AddOrUpdate(T entity);
    int Delete(T entity);
    IEnumerable<T> GetAll();
}

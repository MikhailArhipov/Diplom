using System.Collections.ObjectModel;
using System.Collections.Specialized;
using TaskSheduler.DAL;

namespace TaskSheduler.BL;

/// <summary> Слой бизнес логики, для обработки данных пользователя, обработки нет по условиям задачи, но вдруг будет</summary>
public class BL
{
    public event Action<object, ObservableCollection<TaskModel>> DomainReload;
    public event Action<object, TaskModel> ModelChanged;

    ObservableCollection<TaskModel> domain;
    IRepository<TaskModel> repository { get; set; }

    private void DomainCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        TaskModel item;
        if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
        {
            item = (TaskModel)e.NewItems[0];
            repository.AddOrUpdate(item);
            if (e.Action == NotifyCollectionChangedAction.Replace) ModelChanged?.Invoke(this, item);
        }

        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            item = (TaskModel)e.OldItems[0];
            repository.Delete(item);
        }
    }

    /// <summary> Инициализация переменных (репозитория, основной модели) </summary>
    public void InitDomain()
    {
        repository = new Repository<TaskModel>(
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tasks.db")
            );
        domain = new ObservableCollection<TaskModel>(repository.GetAll());
        domain.CollectionChanged += DomainCollectionChanged;
        DomainReload?.Invoke(this, domain);
    }

    /// <summary> Помещение задачи в основну модель </summary>
    public void AddOrUpdateTask(TaskModel model)
    {
        if (model.Id == 0) this.domain.Add(model);
        else this.domain[this.domain.ToList().FindIndex(el => el.Id == model.Id)] = model;
    }

    /// <summary> удаление задачи из основной модели </summary>
    public void DelTask(TaskModel model) => this.domain.RemoveAt(this.domain.ToList().FindIndex(el => el.Id == model.Id));

}

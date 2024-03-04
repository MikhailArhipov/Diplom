using System.Collections.ObjectModel;
using System.Collections.Specialized;
using TaskSheduler.DAL;
namespace TaskSheduler.BL;

public class BL
{
    public event Action<object, ObservableCollection<ModelTask>> DomainReload;

    ObservableCollection<ModelTask> domain;
    IRepository<ModelTask> repository { get; set; }

    private void DomainCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        ModelTask item;
        if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
        {
            item = (ModelTask)e.NewItems[0];
            repository.AddOrUpdate(item);
            //DomainReload?.Invoke(this, domain);
        }

        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            item = (ModelTask)e.OldItems[0];
            repository.Delete(item);
            //DomainReload?.Invoke(this, domain);
        }
    }

    public void InitDomain()
    {
        repository = new Repository<ModelTask>(
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tasks.db")
            );
        domain = new ObservableCollection<ModelTask>(repository.GetAll());
        domain.CollectionChanged += DomainCollectionChanged;
        DomainReload?.Invoke(this, domain);
    }

    public void AddOrUpdateTask(ModelTask model)
    {
        if (model.Id == 0) domain.Add(model);
        else domain[domain.ToList().FindIndex(el => el.Id == model.Id)] = model;
    }

    public void DelTask(ModelTask model)
    {
        domain.RemoveAt(domain.ToList().FindIndex(el => el.Id == model.Id));
    }

}

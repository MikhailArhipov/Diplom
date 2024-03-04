using System.Reflection;
using System.Windows.Input;

namespace TaskSheduler.ViewModel;

public class EditTaskViewModel : BaseViewModel
{
    public EditTaskViewModel(View.EditTask window, BL.BL bL, ModelTask model)
    {

        window.BindingContext = this;
        switch (model)
        {
            case null:
                Title = "Новая задача";
                Model = new();
                break;
            default:
                Title = "Редактирование / просмотр";
                Model = model.Clone();
                break;
        }
        StatusList = typeof(BL.TaskStatus_).GetFields().Select(el => (string)el.GetValue(null)).ToArray();

        DelCommand = new Command(async () =>
        {
            bL.DelTask(Model);
            await window.Navigation.PopAsync();
        });
        SaveCommand = new Command(async () =>
        {
            await window.Navigation.PopAsync();
            bL.AddOrUpdateTask(Model);
        });

        Model.PropertyChanged += (obj, e) =>
        {
            if (e.PropertyName == nameof(Model.TaskStatus))  //Завершение задачи
            {
                if (model.TaskStatus == BL.TaskStatus_.End)
                {
                    Model.FinishDate = DateTime.Now;
                    Model.IntensityReal = Math.Round(((DateTime)Model.FinishDate - (DateTime)model.CreationDate).TotalDays, 2);
                }
                else if (Model.FinishDate != null)
                {
                    Model.FinishDate = null;
                    Model.IntensityReal = null;
                }
            }
        };
    }

    public ModelTask Model { get; set; }

    public string Title
    {
        set => OnPropertyChanged(value);
        get => _title;
    }
    string _title;

    public string[] StatusList
    {
        set => OnPropertyChanged(value);
        get => Model.TaskStatus switch
        {
            BL.TaskStatus_.Start => _statusList.Where(el => el != BL.TaskStatus_.End).ToArray(),
            _ => _statusList.Where(el => el != BL.TaskStatus_.Start).ToArray()
        };
    }
    string[] _statusList;

    public ICommand DelCommand { get; set; }
    public ICommand SaveCommand { get; set; }

}

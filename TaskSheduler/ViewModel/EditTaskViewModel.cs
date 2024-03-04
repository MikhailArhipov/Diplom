using System.Globalization;
using System.Reflection;
using System.Windows.Input;

namespace TaskSheduler.ViewModel;

/// <summary> ViewModel редактирования задачи, группировка полей по фукнционалу (филда + бекфилда) </summary>
public class EditTaskViewModel : BaseViewModel
{
    public EditTaskViewModel(View.EditTask window, BL.BL bL, TaskModel model)
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
            await window.Navigation.PopAsync();
            bL.DelTask(Model);
        });
        SaveCommand = new Command(async () =>
        {
            await window.Navigation.PopAsync();
            bL.AddOrUpdateTask(Model);
        });
        CancelCommand = new Command(async () =>
        {
            await window.Navigation.PopAsync();
        });

        Model.PropertyChanged += (obj, e) =>
        {
            if (e.PropertyName == nameof(Model.TaskStatus))  //Завершение задачи
            {
                if (Model.TaskStatus == BL.TaskStatus_.End)
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

    public TaskModel Model { get; set; }    //Дочерняя ViewModel, содержащая поля для сохранения в базу, по сути это DTO блок класса, поэтому выделена в отдельный класс

    public string Title                     //Заголовок формы
    {
        set => OnPropertyChanged(value);
        get => _title;
    }
    string _title;

    public string[] StatusList              //Перечень статусов: для первого статуса "Назначена" недоступо изменение в последний "Выполнена" и наоборот
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
    public ICommand CancelCommand { get; set; }
}

public class DateConverter : IValueConverter    //Конвертор значений для поля Model.IntensityReal - реальная трудоемкость
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? null : $"{value} дн.";
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return double.TryParse(value.ToString(), out double d) ? d : null;
    }
}
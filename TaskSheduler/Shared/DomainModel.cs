using SQLite;
using System.ComponentModel;

namespace TaskSheduler;

/// <summary> Основная модель (анемичная): перечень полей, группировка полей по функционалу (филда + бекфилда), модель доступна из всех классов</summary>
public class TaskModel : ViewModel.BaseViewModel, DAL.IDomainObject
{
    public TaskModel()
    {
        IntensityPlan = 1;
    }

    [PrimaryKey, AutoIncrement, Column("_id")]
    public int Id { get; set; }

    public string TaskStatus    //Статус
    {
        set => OnPropertyChanged(value);
        get => _taskStatus;
    }
    string _taskStatus = BL.TaskStatus_.Start;

    public string Title             //Наименование
    {
        set => OnPropertyChanged(value);
        get => _title;
    }
    string _title;

    public string Description       //Описание
    {
        set => OnPropertyChanged(value);
        get => _description;
    }
    string _description;

    public string Workers           //Работники
    {
        set => OnPropertyChanged(value);
        get => _workers;
    }
    string _workers;

    public DateTime CreationDate    //Дата создания
    {
        set => OnPropertyChanged(value);
        get => _creationDate;
    }
    DateTime _creationDate = DateTime.Now;

    public int IntensityPlan        //Трудоемкость дней (меняет срок выполнения)
    {
        set
        {
            if (value != _intensityPlan)
            {
                _datePlan = CreationDate.AddDays(value);
                CallPropertyChanged(nameof(DatePlan));
            }
            OnPropertyChanged(value);
            CallPropertyChanged(nameof(IntensityVisible));
        }
        get => _intensityPlan;
    }
    int _intensityPlan;

    public DateTime DatePlan        //Планируемый срок выполнения (меняет трудоемкость)
    {
        set
        {
            if (value != _datePlan)
            {
                _intensityPlan = (int)(value.Date - CreationDate.Date).TotalDays;
                CallPropertyChanged(nameof(IntensityPlan));
            }
            OnPropertyChanged(value);
            CallPropertyChanged(nameof(DateVisible));
        }
        get => _datePlan;
    }
    DateTime _datePlan;

    public double? IntensityReal    //Реальная трудоемкость (рассчитывается при завершении задачи)
    {
        set
        {
            OnPropertyChanged(value);
            CallPropertyChanged(nameof(IntensityVisible));
        }
        get => _intensityReal;
    }
    double? _intensityReal;

    public DateTime? FinishDate     //реальный срок выполнения (рассчитывается при завершении задачи)
    {
        set
        {
            OnPropertyChanged(value);
            CallPropertyChanged(nameof(DateVisible));
        }
        get => _finishDate;
    }
    DateTime? _finishDate;

    public double IntensityVisible { get => FinishDate == null ? IntensityPlan : (double)IntensityReal; } //Расчетное поле трудоемкость в общем списке задач 

    public DateTime DateVisible { get => FinishDate == null ? DatePlan : (DateTime)FinishDate; }          //Расчетное поле срок выполненния в общем списке задач 


    public TaskModel Clone() => (TaskModel)this.MemberwiseClone();
}


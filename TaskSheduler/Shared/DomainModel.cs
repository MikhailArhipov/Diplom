using SQLite;
using System.ComponentModel;

namespace TaskSheduler;

[Table("ModelTask")]
public class ModelTask : ViewModel.BaseViewModel, DAL.IDomainObject
{
    public ModelTask()
    {
        IntensityPlan = 1;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [PrimaryKey, AutoIncrement, Column("_id")]
    public int Id { get; set; }

    public string TaskStatus
    {
        set => OnPropertyChanged(value);
        get => _taskStatus;
    }
    string _taskStatus = BL.TaskStatus_.Start;

    public string Title
    {
        set => OnPropertyChanged(value);
        get => _title;
    }
    string _title;

    public string Description
    {
        set => OnPropertyChanged(value);
        get => _description;
    }
    string _description;

    public string Workers
    {
        set => OnPropertyChanged(value);
        get => _worker;
    }
    string _worker;

    public DateTime CreationDate
    {
        set => OnPropertyChanged(value);
        get => _creationDate;
    }
    DateTime _creationDate = DateTime.Now;

    public int IntensityPlan
    {
        set
        {
            if (value != _intensityPlan)
            {
                _datePlan = CreationDate.AddDays(value);
                CallPropertyChanged(nameof(DatePlan));
            }
            OnPropertyChanged(value);
        }
        get => _intensityPlan;
    }
    int _intensityPlan;

    public DateTime DatePlan
    {
        set
        {
            if (value != _datePlan)
            {
                _intensityPlan = (int)(value.Date - CreationDate.Date).TotalDays;
                CallPropertyChanged(nameof(IntensityPlan));
            }
            OnPropertyChanged(value);
        }
        get => _datePlan;
    }
    DateTime _datePlan;

    public double? IntensityReal
    {
        set => OnPropertyChanged(value);
        get => _intensityReal;
    }
    double? _intensityReal;

    public DateTime? FinishDate
    {
        set => OnPropertyChanged(value);
        get => _finishDate;
    }
    DateTime? _finishDate;

    public ModelTask Clone() => (ModelTask)this.MemberwiseClone();
}


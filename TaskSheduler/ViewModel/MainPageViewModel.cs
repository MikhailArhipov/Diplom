using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TaskSheduler.ViewModel;

/// <summary> ViewModel просмотра задач, группировка полей по фукнционалу (филда + бекфилда) </summary>
/// MainPage по сути передает управление в этот класс, он является почти главным, как в модели View Molel First
/// Данные базы хранятся в SQL базе (в слое DAL), для большей гибкости между данный классом сделана прослойка - бизнес логика.
/// 
/// Таким образом архитектура программы View Model -> BL -> DAL (Model видна из View Model, BL и хранится в DAL )
///                                         ↘ View ........↙
class MainPageViewModel : BaseViewModel
{
    public MainPageViewModel(MainPage window)
    {
        this.window = window;
        window.BindingContext = this;
        bL = new();
        //Поскольку правильно инициализовать модель в бизнес-логике, то это передано в тот слой, а в ViewModel приходит ссылка при ее инициализации,
        bL.DomainReload += (obj, domain) => Domain = domain;
        //Хотелось сделать так, чтобы задачу можно было отредактировать не только по тапу на ней, но и по кнопке "Редактировать", так нагляднее.
        //В этом случае надо как то передать отредактированную модель в поле TaskSelected, чтобы нельзя было тапнуть по задаче, отредактировать ее,
        //а потом нажав на кнопку Редактировать провалиться в задачу до ее редактирования. Контрол ListView сделан кривовато, и при переключении на форму
        //не меняет фокус. Поэтому передал измененую задачу через событие обратно.
        bL.ModelChanged += (obj, model) => _taskSelected = model;
        bL.InitDomain();

        AddCommand = new Command(async () =>
        {
            await window.Navigation.PushAsync(new View.EditTask(null, bL));
        });
        EditCommand = new Command(async () =>
        {
            await window.Navigation.PushAsync(new View.EditTask(TaskSelected, bL));
        });
        InfoCommand = new Command(() =>
        {
            window.DisplayAlert(
                "О программе",
                "Информационная система «Управление задачами» \r\n" +
                "Подготовлена для диплома МФТИ по курсу «Разработчик C#»\r\n" +
                "Автор - Архипов М.М., поток № 2\r\n",
                "ОК");
        });
    }

    BL.BL bL;                                       //Ссылка на бизнес логику
    MainPage window;                                //Ссылка на страницу

    public TaskModel TaskSelected
    {
        set
        {
            OnPropertyChanged(value);
            if (_taskSelected != null) window.Navigation.PushAsync(new View.EditTask(TaskSelected, bL));
        }
        get => _taskSelected;
    }
    TaskModel _taskSelected;

    public ICommand AddCommand { get; set; }
    public ICommand EditCommand { get; set; }
    public ICommand InfoCommand { get; set; }

    public ObservableCollection<TaskModel> Domain { get; set; }     //Дочерняя ViewModel, содержащая массив полей для сохранения в базу, по сути это DTO блок, поэтому выделена в отдельный класс
}

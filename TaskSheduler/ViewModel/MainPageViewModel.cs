using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace TaskSheduler.ViewModel;

class MainPageViewModel : BaseViewModel
{
    public MainPageViewModel(MainPage window)
    {
        this.window = window;
        window.BindingContext = this;
        bL = new();
        bL.DomainReload += DomainReloadEvent;
        bL.InitDomain();

        AddCommand = new Command(async () =>
        {
            await window.Navigation.PushAsync(new View.EditTask(null, bL));
        });

        InfoCommand = new Command(() =>
        {
            window.DisplayAlert(
                "О программе",
                "Информационная система «Управление задачами» \r\n" +
                "Программа подготовлена для диплома МФТИ по курсу «Разработчик C#»\r\n" +
                "Автор - Архипов М.М., поток № 2\r\n" +
                "Иконки использованы с ресурса https://www.flaticon.com\r\n",
                "ОК");
        });
    }

    BL.BL bL;
    MainPage window;

    public ModelTask TaskSelected
    {
        set
        {
            OnPropertyChanged(value);
            if (_taskSelected != null) window.Navigation.PushAsync(new View.EditTask(TaskSelected, bL));
        }
        get => _taskSelected;
    }
    ModelTask _taskSelected;

    public double ScrollPosition
    {
        set => OnPropertyChanged(value);
        get => _scrollPosition;
    }
    double _scrollPosition;

    public ICommand AddCommand { get; set; }
    public ICommand EditCommand { get; set; }
    public ICommand InfoCommand { get; set; }

    public ObservableCollection<ModelTask> Domain { get; set; }

    void DomainReloadEvent(object obj, ObservableCollection<ModelTask> model) => Domain = model;
}

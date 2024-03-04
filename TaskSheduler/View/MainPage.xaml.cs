using System.Collections.ObjectModel;

namespace TaskSheduler;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        new ViewModel.MainPageViewModel(this);
        InitializeComponent();
    }



   
    // переходим на страницу PersonPage для добавления нового элемента
    //async void AddButton_Click(object sender, EventArgs e)
    //{
    //    await Navigation.PushAsync(new PersonPage(null));
    //}
    //// вспомогательный метод для добавления элемента в список
    //protected internal void AddPerson(Model phone)
    //{
    //    tasks.Add(phone);
    //}
}
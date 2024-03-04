using System.Collections.ObjectModel;

namespace TaskSheduler;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        new ViewModel.MainPageViewModel(this);
        InitializeComponent();
    }
}
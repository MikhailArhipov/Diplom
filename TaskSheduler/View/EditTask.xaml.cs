namespace TaskSheduler.View;

public partial class EditTask : ContentPage
{
    public EditTask(ModelTask model, BL.BL bl)
    {
        new ViewModel.EditTaskViewModel(this, bl, model);
        InitializeComponent();
    }
}
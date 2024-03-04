namespace TaskSheduler.View;

public partial class EditTask : ContentPage
{
    public EditTask(TaskModel model, BL.BL bl)
    {
        new ViewModel.EditTaskViewModel(this, bl, model);
        InitializeComponent();
    }
}
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TaskSheduler.ViewModel;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary> Измение бекфилды с уведомлением о событии</summary>
    protected void OnPropertyChanged<T>(T value, [CallerMemberName] string prop = null)
    {
        string prop_ = ("_" + prop).ToUpper();
        foreach (var field in this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (field.Name.ToUpper() != prop_) continue;
            var backValue = field.GetValue(this);
            if (value == null)
            {
                if (backValue != null)
                {
                    field.SetValue(this, null);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
                }
            }
            else if (!value.Equals((T)backValue))
            {
                field.SetValue(this, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }
    }

    /// <summary> Уведомление об измении поля по его имени </summary>
    protected void CallPropertyChanged([CallerMemberName] string prop = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}

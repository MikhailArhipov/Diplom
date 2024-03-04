using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TaskSheduler.ViewModel;

/// <summary> базовая ViewModel, от которой наследуются остальные </summary>
public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary> Измение бекфилды с уведомлением о событии</summary>
    protected void OnPropertyChanged<T>(T value, [CallerMemberName] string prop = null)
    {
        string prop_ = ("_" + prop).ToUpper();
        bool check = false;
        foreach (var field in this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (field.Name.ToUpper() != prop_) continue;
            check = true;
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
        if (!check) throw new Exception($"Ошибка, поле _{prop} не найдено");
    }

    /// <summary> Уведомление об измении поля prop - имя поля </summary>
    protected void CallPropertyChanged([CallerMemberName] string prop = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}

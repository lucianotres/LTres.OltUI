using System.ComponentModel;

namespace LTres.Olt.UI.Client.Services;

public class AppDataService : INotifyPropertyChanged
{
    public bool Loading
    {
        get => _Loading;
        set
        {
            _Loading = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Loading)));
        }
    }
    private bool _Loading = false;

    public event PropertyChangedEventHandler? PropertyChanged;
}
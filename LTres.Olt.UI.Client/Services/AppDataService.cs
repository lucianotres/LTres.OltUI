using System.ComponentModel;
using Microsoft.JSInterop;

namespace LTres.Olt.UI.Client.Services;

public class AppDataService(IJSRuntime js) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

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

    public string? Title { get; private set; }
    public string? Description { get; private set; }
 
    private void SetTitleInternal(string? title, string? description = null)
    {
        Title = title;
        Description = description;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
    }

    private async void SetAppTitle(string? appTitle)
    {
        await js.InvokeVoidAsync("eval", $"document.title = '{appTitle}'");        
    }

    public void SetTitle(string? title, string? description = null)
    {
        SetTitleInternal(title, description);
        SetAppTitle(title);
    }

    public void SetTitle<T>(AppPageLocalizer<T> localizer)
    {
        var localizedDesc = localizer["Description"];
        SetTitleInternal(localizer["Title"], localizedDesc.ResourceNotFound ? null : localizedDesc.Value);

        SetAppTitle(localizer.GetTitle());
    }

}
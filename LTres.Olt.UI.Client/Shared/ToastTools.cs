using MudBlazor;

namespace LTres.Olt.UI.Client.Shared;

public class ToastTools(ISnackbar snackbar)
{
    public void Success(string message)
    {
        snackbar.Add(message, severity: Severity.Success);
    }
}
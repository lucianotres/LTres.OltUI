using LTres.Olt.UI.Client.Services;
using MudBlazor;

namespace LTres.Olt.UI.Client.Shared;

public class DialogTools(
    IDialogService dialogService,
    AppPageLocalizer<DialogTools> localizer)
{
    public Task<IDialogReference> OkAsync(
        string content,
        string? title = null)
    {
        var parameters = new DialogParameters<QuestionDialog>
        {
            { x => x.ContentText, content },
            { x => x.ButtonText, "Ok" },
            { x => x.Color, Color.Default }
        };

        var options = new DialogOptions()
        {
            Position = DialogPosition.TopCenter,
            BackgroundClass = "dialog-background",
            MaxWidth = MaxWidth.ExtraSmall,
            FullWidth = true,
            CloseOnEscapeKey = true
        };

        return dialogService.ShowAsync<QuestionDialog>(title ?? localizer["AppTitle"], parameters, options);
    }

    public Task<IDialogReference> ConfirmAsync(
        string content,
        string? title = null,
        string? yesButtonText = null,
        string? noButtonText = null)
    {
        var parameters = new DialogParameters<QuestionDialog>
        {
            { x => x.ContentText, content },
            { x => x.ButtonText, yesButtonText ?? localizer["Yes"] },
            { x => x.CancelText, noButtonText ?? localizer["No"] },
            { x => x.Color, Color.Success },
            { x => x.CancelColor, Color.Default }
        };

        var options = new DialogOptions()
        {
            Position = DialogPosition.TopCenter,
            BackgroundClass = "dialog-background",
            MaxWidth = MaxWidth.ExtraSmall,
            FullWidth = true,
            CloseOnEscapeKey = true
        };

        return dialogService.ShowAsync<QuestionDialog>(title ?? localizer["AppTitle"], parameters, options);
    }

    public Task<IDialogReference> DeleteAsync(
        string content,
        string? title = null,
        string? yesButtonText = null,
        string? noButtonText = null)
    {
        var parameters = new DialogParameters<QuestionDialog>
        {
            { x => x.ContentText, content },
            { x => x.ButtonText, yesButtonText ?? localizer["Yes"] },
            { x => x.CancelText, noButtonText ?? localizer["No"] },
            { x => x.Color, Color.Error },
            { x => x.CancelColor, Color.Default }
        };

        var options = new DialogOptions()
        {
            Position = DialogPosition.TopCenter,
            BackgroundClass = "dialog-background",
            MaxWidth = MaxWidth.ExtraSmall,
            FullWidth = true,
            CloseOnEscapeKey = true
        };

        return dialogService.ShowAsync<QuestionDialog>(title ?? localizer["AppTitle"], parameters, options);
    }
}
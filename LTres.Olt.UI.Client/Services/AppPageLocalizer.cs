using LTres.Olt.UI.Client.Layout;
using Microsoft.Extensions.Localization;

namespace LTres.Olt.UI.Client.Services;

public class AppPageLocalizer<T> : IStringLocalizer<T>
{
    private readonly IStringLocalizer _appLocalizer;
    private readonly IStringLocalizer _pageLocalizer;

    public AppPageLocalizer(IStringLocalizer<MainLayout> appLocalizer, IStringLocalizer<T> pageLocalizer)
    {
        _appLocalizer = appLocalizer;
        _pageLocalizer = pageLocalizer;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var localizedStr = _pageLocalizer[name];
            return localizedStr.ResourceNotFound ? _appLocalizer[name] : localizedStr;
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var localizedStr = _pageLocalizer[name, arguments];
            return localizedStr.ResourceNotFound ? _appLocalizer[name, arguments] : localizedStr;
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
        _pageLocalizer.GetAllStrings(includeParentCultures);

    public string GetTitle(string key = "Title")
        => $"{this["AppTitle"]} | {this[key]}";
}

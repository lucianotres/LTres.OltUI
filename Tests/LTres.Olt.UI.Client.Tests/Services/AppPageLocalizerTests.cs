using Bunit;
using LTres.Olt.UI.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace LTres.Olt.UI.Client.Tests.Services;

public class AppPageLocalizerTests : TestContext
{
    [Fact]
    public void AppPageLocalizer_ShouldReturnLocalizedStrings()
    {
        Services.AddSingleton<IStringLocalizerFactory, TestStringLocalizerFactory>();
        Services.AddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
        Services.AddTransient(typeof(AppPageLocalizer<>), typeof(AppPageLocalizer<>));

        var appPageLocalizer = Services.GetRequiredService<AppPageLocalizer<AppPageLocalizerTests>>();

        Assert.NotNull(appPageLocalizer);
        Assert.Equal("Test Value", appPageLocalizer["TestKey"]);
        Assert.Equal("Test Value with args", appPageLocalizer["TestKey", "arg1"]);
    }


    private class TestStringLocalizerFactory : IStringLocalizerFactory
    {
        public IStringLocalizer Create(Type resourceSource) => new TestStringLocalizer();

        public IStringLocalizer Create(string baseName, string location) => new TestStringLocalizer();
    }

    private class TestStringLocalizer : IStringLocalizer
    {
        public LocalizedString this[string name] => new("TestKey", "Test Value");

        public LocalizedString this[string name, params object[] arguments] => new("TestKey", "Test Value with args");

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }
    }
}
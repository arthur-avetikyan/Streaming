using ReactiveUI;

using Streaming.BusinessLogic.Interfaces;
using Streaming.BusinessLogic.Models;
using Streaming.Composer;
using Streaming.ViewModels.Interfaces;
using System.Reactive;

namespace Streaming.ViewModels
{
    class SettingsDialogViewModel : ViewModelBase, /*DialogViewModelBase,*/ ISaver
    {
        public readonly IConfigurationSetting _configurationSetting;
        private readonly ApplicationSettings _settings;
        private string _url;

        public string Url { get => _url; private set => this.RaiseAndSetIfChanged(ref _url, value); }
        public ReactiveCommand<Unit, Unit> Save { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }

        public SettingsDialogViewModel(IConfigurationSetting configurationSetting, ApplicationSettings settings)
        {
            _configurationSetting = configurationSetting;
            _settings = settings;
            Url = _settings.ApiBaseUrl;

            var enabled = this.WhenAnyValue(
                f => f.Url,
                url => !string.IsNullOrWhiteSpace(url));

            Save = ReactiveCommand.Create(SaveUrl, enabled);
            Cancel = ReactiveCommand.Create(() => { });
        }

        public void SaveUrl()
        {
            _configurationSetting.AddOrUpdateAppSetting("ApplicationSettings:ApiBaseUrl", Url);
            _settings.ApiBaseUrl = Url;
        }
    }
}

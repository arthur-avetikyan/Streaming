
using Newtonsoft.Json;

using ReactiveUI;
using ReactiveUI.Validation.Contexts;

using Streaming.BusinessLogic;
using Streaming.BusinessLogic.Interfaces;
using Streaming.BusinessLogic.Models;
using Streaming.Composer;
using Streaming.ViewModels.Interfaces;

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive;
using System.Threading.Tasks;

namespace Streaming.ViewModels
{
    class RegisterViewModel : ViewModelBase, IRegister
    {
        private readonly ApplicationSettings _settings;
        public readonly IConfigurationSetting _configurationSetting;
        private string _name;
        private string _location;
        private int _timeZone;

        public string Name { get => _name; private set => this.RaiseAndSetIfChanged(ref _name, value); }
        public string Location { get => _location; private set => this.RaiseAndSetIfChanged(ref _location, value); }
        public int TimeZone { get => _timeZone; private set => this.RaiseAndSetIfChanged(ref _timeZone, value); }

        public RegisterRequest RegisterFrom { get; set; }

        public ReactiveCommand<Unit, Unit> Save { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }

        public ValidationContext ValidationContext { get; } = new ValidationContext();

        public RegisterViewModel(ApplicationSettings settings, IConfigurationSetting configurationSetting)
        {
            _settings = settings;
            _configurationSetting = configurationSetting;

            RegisterFrom = new RegisterRequest();

            var enabled = this.WhenAnyValue(
                vm => vm.Name,
                vm => vm.Location,
                (name, location) => !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(location));

            Save = ReactiveCommand.CreateFromTask(RegisterApp, enabled);
            Cancel = ReactiveCommand.Create(() => { });
            _configurationSetting = configurationSetting;
        }

        public async Task RegisterApp()
        {
            try
            {
                var kioskIdentifier = Guid.NewGuid();
                RegisterFrom.KioskIdentifier = kioskIdentifier;
                RegisterFrom.Name = Name;
                RegisterFrom.Location = Location;
                RegisterFrom.TimeZone = TimeZone;

                using HttpClient client = new HttpClient { BaseAddress = new Uri(_settings.ApiBaseUrl) };
                var response = await client.PostAsJsonAsync("api/kiosks", RegisterFrom);
                var result = JsonConvert.DeserializeObject<ApiResponseDetails<KioskResponse>>(await response.Content.ReadAsStringAsync());
                FileLogger.Instance.Log(LogTypes.Event, "Register API Called");

                if (result.IsSuccessStatusCode)
                {
                    _configurationSetting.AddOrUpdateAppSetting("ApplicationSettings:KioskIdentifier", kioskIdentifier);
                    _settings.KioskIdentifier = kioskIdentifier;
                    FileLogger.Instance.Log(LogTypes.Event, "App Registerd");
                }
            }
            catch (Exception ex)
            {
                FileLogger.Instance.Log(LogTypes.Error, "RegisterApp Method", ex.Message, ex.StackTrace);
            }
        }
    }
}

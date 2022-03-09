
using ReactiveUI;

using Streaming.BusinessLogic.Interfaces;
using Streaming.BusinessLogic.Models;
using Streaming.Composer;
using Streaming.ViewModels.Interfaces;

using System;
using System.Reactive.Linq;

namespace Streaming.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase content;

        private readonly IRecurringTask _pluginGetterService;
        private readonly ApplicationSettings _settings;
        public PluginListViewModel PluginListViewModel { get; }
        public IRegister RegisterViewModel { get; }

        public MainWindowViewModel(ApplicationSettings settings,
                                   IRecurringTask pluginGetterService,
                                   IRegister registerViewModel,
                                   PluginListViewModel pluginListViewModel)
        {
            _settings = settings;
            _pluginGetterService = pluginGetterService;
            _pluginGetterService.Start();

            RegisterViewModel = registerViewModel;
            Content = PluginListViewModel = pluginListViewModel;

            if (_settings.KioskIdentifier == Guid.Empty)
            {
                OpenRegisterForm();
            }
        }

        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public void OpenRegisterForm()
        {
            var viewModel = RegisterViewModel;
            Observable.Merge(viewModel.Save, viewModel.Cancel)
                .Take(1)
                .Subscribe(_ =>
                {
                    Content = PluginListViewModel;
                });

            Content = (ViewModelBase)viewModel;
        }
    }
}

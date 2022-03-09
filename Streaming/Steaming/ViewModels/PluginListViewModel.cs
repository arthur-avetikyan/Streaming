
using ReactiveUI;

using Streaming.Composer;
using Streaming.Composer.Base;
using Streaming.Composer.UI;
using Streaming.ViewModels.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Streaming.ViewModels
{
    class PluginListViewModel : ViewModelBase
    {
        private readonly ISaver _settingsDialogViewModel;

        private ViewModelBase _content;
        private bool _pluginSelected;
        private string _currentPlugin;
        private bool disposedValue;

        public ViewModelBase Content { get => _content; private set => this.RaiseAndSetIfChanged(ref _content, value); }
        public bool PluginSelected { get => _pluginSelected; private set => this.RaiseAndSetIfChanged(ref _pluginSelected, value); }

        public ObservableCollection<IUIViewProviderBase> Plugins { get => PluginProvider.Instance.Plugins; }

        public ReactiveCommand<string, Unit> Select { get; }
        public ReactiveCommand<Unit, Unit> Home { get; }
        public ReactiveCommand<Unit, Unit> Settings { get; }

        public PluginListViewModel(ISaver settingsDialogViewModel)
        {
            _settingsDialogViewModel = settingsDialogViewModel;

            Select = ReactiveCommand.Create<string>(key => { OnSelectPlugin(key); });
            Settings = ReactiveCommand.Create(OnSettingsOpen);
            Home = ReactiveCommand.Create(OnHomeScreen);

            OnSelectPlugin(Plugins.FirstOrDefault()?.EntryKey);
            Plugins.CollectionChanged += plugins_CollectionChanged;
        }

        private void OnSelectPlugin(string key)
        {
            DisablePlugin();

            var plugin = PluginProvider.Instance.Plugins.FirstOrDefault(p => p.Key.Equals(key));
            if (plugin != null)
            {
                PluginSelected = true;
                _currentPlugin = plugin.EntryKey;
                Content = (ViewModelBase)plugin.ViewModel.Value;
                if (Content is IManualControlable controlable)
                    controlable.StartProcess();
            }
        }

        private void OnHomeScreen() => DisablePlugin();

        private void OnSettingsOpen()
        {
            var dialog = _settingsDialogViewModel;
            Observable.Merge(dialog.Save, dialog.Cancel)
                .Take(1)
                .Subscribe(_ =>
                {
                    OnSelectPlugin(_currentPlugin);
                });

            Content = (ViewModelBase)dialog;
            PluginSelected = true;
        }

        private void plugins_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => GetNextPlugin();

        private void DisablePlugin()
        {
            if (Content != null)
            {
                PluginSelected = false;
                if (Content is IManualControlable controlable)
                    controlable.StopProcess();
                Content = null;
            }
        }

        private void GetNextPlugin()
        {
            if (Plugins.Count == 0)
                return;
            var currentIndex = Plugins.IndexOf(Plugins.FirstOrDefault(p => p.Key.Equals(_currentPlugin))) + 1;
            if (currentIndex >= Plugins.Count)
                currentIndex = 0;
            OnSelectPlugin(Plugins[currentIndex].EntryKey);
        }

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Content?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;

                base.Dispose(disposing);
            }
        }

        #endregion
    }
}
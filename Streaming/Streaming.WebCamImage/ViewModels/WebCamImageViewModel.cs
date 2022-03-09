
using ReactiveUI;

using Streaming.Composer;
using Streaming.Composer.Base;
using Streaming.WebCamImage.Services;

using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Streaming.WebCamImage.ViewModels
{
    [ExportViewModel("WebCamImageViewModel")]
    internal class WebCamImageViewModel : ViewModelBase, IViewModel, IManualControlable
    {
        private IWebCamImageProvider _webCamImageProvider;
        private IWebCamStreamProvider _webCamStreamProvider;
        private System.Timers.Timer _timer;

        private string _imagePath;
        private string _streamPath;
        private bool disposedValue;
        private int _executionCount;

        public string ImagePath { get => _imagePath; private set => this.RaiseAndSetIfChanged(ref _imagePath, value); }
        public string StreamPath { get => _streamPath; private set => this.RaiseAndSetIfChanged(ref _streamPath, value); }

        public ReactiveCommand<Unit, Unit> Capture { get; }
        public ReactiveCommand<Unit, Unit> Stop { get; }
        public ReactiveCommand<Unit, Unit> Start { get; }

        public WebCamImageViewModel()
        {
            _webCamImageProvider = new WebCamImageProvider();
            _webCamStreamProvider = new WebCamStreamProvider();

            _timer = new System.Timers.Timer();
            SetupTimer(_timer);

            Capture = ReactiveCommand.Create(CaptureImage);
            Stop = ReactiveCommand.Create(_timer.Stop);
            Start = ReactiveCommand.Create(_timer.Start);
        }

        public void SetupTimer(System.Timers.Timer timer)
        {
            timer.Interval = 80;
            timer.Elapsed += GetWebCampFrame;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void CaptureImage()
        {
            var imagePath = StreamPath;
            ImagePath = _webCamImageProvider.TakeImage(imagePath);
        }

        private void GetWebCampFrame(object sender, ElapsedEventArgs e)
        {
            var count = Interlocked.Increment(ref _executionCount);
            if (_executionCount % 100 == 0)
            {
                Task.Run(() => _webCamStreamProvider.ClearOldFrames());
            }
            StreamPath = _webCamStreamProvider.GetFrame();
        }

        public void StartProcess() => _timer.Start();

        public void StopProcess()
        {
            _timer.Stop();
            _webCamStreamProvider.ClearOldFrames();
        }

        #region IDisposable pattern

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _timer.Stop();
                    _timer.Close();
                    _webCamStreamProvider?.Dispose();
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
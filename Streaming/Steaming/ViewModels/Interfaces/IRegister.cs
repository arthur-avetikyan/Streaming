using ReactiveUI;

using System.Reactive;

namespace Streaming.ViewModels.Interfaces
{
    public interface IRegister
    {
        public ReactiveCommand<Unit, Unit> Save { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }
    }
}

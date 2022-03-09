using Streaming.Composer;
using Streaming.Services;

using System;
using System.Threading;

namespace Streaming.ViewModels.Dialog
{
    public class DialogViewModelBase<TResult> : ViewModelBase
        where TResult : DialogResultBase
    {
        public event EventHandler<DialogResultEventArgs<TResult>> CloseRequested;

        protected void Close() => Close(default);

        protected void Close(TResult result)
        {
            var args = new DialogResultEventArgs<TResult>(result);

            var handler = Volatile.Read(ref CloseRequested);
            handler?.Invoke(this, args);
        }
    }

    public class DialogViewModelBase : DialogViewModelBase<DialogResultBase>
    {

    }
}
using System;

namespace Streaming.ViewModels.Dialog
{
    public class DialogResultEventArgs<TResult> : EventArgs
    {
        public TResult Result { get; }

        public DialogResultEventArgs(TResult result)
        {
            Result = result;
        }
    }
}
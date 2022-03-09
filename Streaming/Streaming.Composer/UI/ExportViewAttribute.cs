using Streaming.Composer.Base;

using System;
using System.ComponentModel.Composition;

namespace Streaming.Composer.UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportViewAttribute : ExportAttribute
    {
        public ExportViewAttribute(string contractName)
            : base(contractName, typeof(IView))
        {
        }
    }
}

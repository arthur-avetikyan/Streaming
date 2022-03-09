using Streaming.Composer.Base;

using System;
using System.ComponentModel.Composition;

namespace Streaming.Composer.UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportViewModelAttribute : ExportAttribute
    {
        public ExportViewModelAttribute(string contractName)
            : base(contractName, typeof(IViewModel))
        {
        }
    }
}

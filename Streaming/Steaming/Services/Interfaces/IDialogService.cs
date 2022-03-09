using System.Threading.Tasks;

namespace Streaming.Services.Interfaces
{
    public interface IDialogService
    {
        Task<TResult> ShowDialogAsync<TResult>(string viewModelName)
            where TResult : DialogResultBase;

        Task ShowDialogAsync(string viewModelName);
    }
}
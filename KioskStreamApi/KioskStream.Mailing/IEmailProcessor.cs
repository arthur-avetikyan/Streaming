
using System.Threading.Tasks;

namespace KioskStream.Mailing
{
    public interface IEmailProcessor
    {
        Task SendAsync(EmailConfigurationParameters parameters);
    }
}

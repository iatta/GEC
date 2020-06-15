using System.Threading.Tasks;
using Pixel.Attendance.Views;
using Xamarin.Forms;

namespace Pixel.Attendance.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}

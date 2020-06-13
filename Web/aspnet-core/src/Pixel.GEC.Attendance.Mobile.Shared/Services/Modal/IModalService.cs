using System.Threading.Tasks;
using Pixel.GEC.Attendance.Views;
using Xamarin.Forms;

namespace Pixel.GEC.Attendance.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}

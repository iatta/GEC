using System.Threading.Tasks;
using GEC.Attendance.Views;
using Xamarin.Forms;

namespace GEC.Attendance.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}

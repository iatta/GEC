using Xamarin.Forms.Internals;

namespace Pixel.Attendance.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}
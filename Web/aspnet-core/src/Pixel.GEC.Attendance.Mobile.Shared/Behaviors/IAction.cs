using Xamarin.Forms.Internals;

namespace Pixel.GEC.Attendance.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}
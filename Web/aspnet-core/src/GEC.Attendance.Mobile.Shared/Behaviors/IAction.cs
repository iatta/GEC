using Xamarin.Forms.Internals;

namespace GEC.Attendance.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}
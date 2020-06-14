using Xamarin.Forms;

namespace GEC.Attendance.Views
{
    public partial class MainView : MasterDetailPage, IXamarinView
    {
        public MainView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}

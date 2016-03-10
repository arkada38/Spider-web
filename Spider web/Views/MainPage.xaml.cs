using Windows.UI.Xaml;

namespace Spider_web.Views
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

#if DEBUG
            //Editor.Visibility = Visibility.Visible;
#endif
        }
    }
}

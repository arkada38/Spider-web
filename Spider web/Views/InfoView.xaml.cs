using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Navigation;
using Spider_web.Utils;
using Spider_web.ViewModels;

namespace Spider_web.Views
{
    public sealed partial class InfoView
    {
        private MainPageViewModel _main;

        private string Score => Settings.Score.ToString("N0");
        private string PassedLvls => $"{_main.LvlsList.Count(lvl => lvl.IsFinished)} {new ResourceLoader().GetString("From")} {_main.LvlsList.Count}";

        public InfoView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _main = (MainPageViewModel)e.Parameter;
            base.OnNavigatedTo(e);
        }
    }
}

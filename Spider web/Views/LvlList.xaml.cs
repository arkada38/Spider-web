using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Spider_web.Models;
using Spider_web.ViewModels;

namespace Spider_web.Views
{
    public sealed partial class LvlList
    {
        private MainPageViewModel _main;
        private List<UserLvl> _lvlsList;

        public LvlList()
        {
            InitializeComponent();
        }

        private void OnLvlClick(object sender, ItemClickEventArgs e)
        {
            _main.OnLvlSelected((UserLvl)e.ClickedItem);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _main = (MainPageViewModel) e.Parameter;
            _lvlsList = _main.LvlsList;
            base.OnNavigatedTo(e);
        }
    }
}

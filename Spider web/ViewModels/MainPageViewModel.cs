using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Spider_web.Models;
using Spider_web.Utils;
using Spider_web.Views;
using static System.Math;

namespace Spider_web.ViewModels
{
    internal class MainPageViewModel : BaseViewModel
    {
        #region Fields

        private bool _isPaneOpen;
        private Visibility _gamePanelVisibility;
        private Visibility _backButtonVisibility;
        private int _menuIndex;
        private Frame _contentFrame;
        private List<UserLvl> _lvlsList;
        private string _header;
        private double _spiderScale;

        #endregion

        #region Properties

        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set
            {
                if (_isPaneOpen == value) return;
                _isPaneOpen = value;
                RaisePropertyChanged("IsPaneOpen");
            }
        }

        public Visibility GamePanelVisibility
        {
            get { return _gamePanelVisibility; }
            set
            {
                if (_gamePanelVisibility == value) return;
                _gamePanelVisibility = value;
                RaisePropertyChanged("GamePanelVisibility");
            }
        }

        public Visibility BackButtonVisibility
        {
            get { return _backButtonVisibility; }
            set
            {
                if (_backButtonVisibility == value) return;
                _backButtonVisibility = value;
                RaisePropertyChanged("BackButtonVisibility");
            }
        }

        public int MenuIndex
        {
            get { return _menuIndex; }
            set
            {
                if (_menuIndex == value) return;
                _menuIndex = value;
                ClickOnMenu();
                RaisePropertyChanged("MenuIndex");
            }
        }

        public Frame ContentFrame
        {
            get { return _contentFrame; }
            set
            {
                if (_contentFrame == value) return;
                _contentFrame = value;
                RaisePropertyChanged("ContentFrame");
            }
        }

        public List<UserLvl> LvlsList
        {
            get { return _lvlsList; }
            set
            {
                if (_lvlsList == value) return;
                _lvlsList = value;
                RaisePropertyChanged("LvlsList");
            }
        }

        public string Header
        {
            get { return _header; }
            set
            {
                if (_header == value) return;
                _header = value;
                RaisePropertyChanged("Header");
            }
        }

        public int ActiveLvl { get; private set; }

        public double SpiderScale
        {
            get { return _spiderScale; }
            set
            {
                if (Abs(_spiderScale - value) < .01) return;
                _spiderScale = Min(Max(.3, value), 1.7);

                Square?.RescaleSpiders();
                Editor?.RescaleSpiders();

                RaisePropertyChanged("SpiderScale");
            }
        }

        public SquareView Square { get; set; }
        public EditorView Editor { get; set; }

        #endregion

        #region Commands

        public ICommand TogglePaneCommand { get; set; }
        public ICommand ReloadLvlCommand { get; set; }
        public ICommand IncreaseScaleCommand { get; set; }
        public ICommand DecreaseScaleCommand { get; set; }
        public ICommand BackCommand { get; set; }

        #endregion

        public MainPageViewModel()
        {
            #region Init Commands
            TogglePaneCommand = new CommandHandler(o => IsPaneOpen = !IsPaneOpen);
            ReloadLvlCommand = new CommandHandler(o => OnLvlSelected(LvlsList[ActiveLvl - 1]));
            IncreaseScaleCommand = new CommandHandler(o => SpiderScale += .1);
            DecreaseScaleCommand = new CommandHandler(o => SpiderScale -= .1);
            BackCommand = new CommandHandler(o => MenuIndex = 0);
            #endregion

            Settings.InitSettings();
            BackButtonVisibility = Visibility.Collapsed;
            GamePanelVisibility = Visibility.Collapsed;

            ContentFrame = new Frame();
            LvlsList = new LvlFactory().UserLvls;
            ClickOnMenu();
            //MenuIndex = 1;
            ActiveLvl = 1;

            SpiderScale = 1;
        }

        #region Methods

        private void ClickOnMenu()
        {
            IsPaneOpen = false;
            GamePanelVisibility = Visibility.Collapsed;

            var loader = new ResourceLoader();

            switch (MenuIndex)
            {
                case 0:
                    Header = loader.GetString("LevelSelectionHeader");
                    ContentFrame.Navigate(typeof(LvlList), this);
                    BackButtonVisibility = Visibility.Collapsed;
                    break;
                case 1:
                    Header = loader.GetString("InformationHeader");
                    ContentFrame.Navigate(typeof(InfoView), this);
                    BackButtonVisibility = Visibility.Visible;
                    break;
                case 2:
                    Header = loader.GetString("EditorHeader");
                    ContentFrame.Navigate(typeof(EditorView), this);
                    BackButtonVisibility = Visibility.Visible;
                    GamePanelVisibility = Visibility.Visible;
                    break;
            }
        }

        public void OnLvlSelected(UserLvl userLvl)
        {
            LvlFactory.RefreshUserLvlS(LvlsList);

            if (userLvl.LvlNumber > 0)
            {
                ActiveLvl = userLvl.LvlNumber;
                Header = userLvl.LvlNumber.ToString();
                MenuIndex = -1;

                GamePanelVisibility = Visibility.Visible;
                BackButtonVisibility = Visibility.Visible;

                ContentFrame.Navigate(typeof(SquareView), this);
            }
        }

        public async Task OnLvlComplete()
        {
            LvlsList[ActiveLvl - 1].IsFinished = true;
            Settings.Score += LvlsList[ActiveLvl - 1].Difficult;

            var passedLvls = await Settings.GetPassedLvls();
            passedLvls.Add(ActiveLvl - 1);
            Settings.SetPassedLvls(passedLvls);

            var loader = new ResourceLoader();

                if (LvlsList.Count > ActiveLvl)
            {
                var dialog = new Windows.UI.Popups.MessageDialog($"{loader.GetString("Level")} {ActiveLvl} {loader.GetString("IsPassed")}", loader.GetString("Congratulations"));

                dialog.Commands.Add(new Windows.UI.Popups.UICommand(loader.GetString("Next")) {Id = 0});
                dialog.Commands.Add(new Windows.UI.Popups.UICommand(loader.GetString("Retry")) {Id = 1});

                var result = await dialog.ShowAsync();

                if (result != null && (int) result.Id == 0)
                {
                    OnLvlSelected(LvlsList[ActiveLvl]);
                }
                if (result != null && (int) result.Id == 1)
                {
                    OnLvlSelected(LvlsList[ActiveLvl - 1]);
                }
            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog($"{loader.GetString("Level")} {ActiveLvl} {loader.GetString("IsPassed")}, {loader.GetString("LevelsInDeveloping")}", loader.GetString("Congratulations"));

                dialog.Commands.Add(new Windows.UI.Popups.UICommand(loader.GetString("MainMenu")) {Id = 0});

                var result = await dialog.ShowAsync();

                if (result != null && (int) result.Id == 0)
                    MenuIndex = 0;
            }
        }

        #endregion
    }
}

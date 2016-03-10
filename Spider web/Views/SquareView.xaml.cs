using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Spider_web.ViewModels;
using static System.Math;

namespace Spider_web.Views
{
    //MainCanvas для отображения прямоугольника, заголовка, связей и выделений
    //SpiderCanvas для отображения пауков
    public sealed partial class SquareView
    {
        private MainPageViewModel _main;
        private bool _isSpiderSelected, _isLvlComplete, _isLvlCompleteDone;
        private Image _selectedSpider;
        private double _widthOfSpider, _heightOfSpider;

        public SquareView()
        {
            InitializeComponent();

            _selectedSpider = new Image();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _main = (MainPageViewModel)e.Parameter;
            _main.Square = this;
            base.OnNavigatedTo(e);
        }

        private void OnSquareSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawingSpiders();
            DrawingContacts();
            DrawingRectangle();
        }

        public void DrawingSpiders()
        {
            SpiderCanvas.Children.Clear();

            var r = new Random(DateTime.Now.Millisecond);
            var i = 0;
            foreach (var coordinates in _main.LvlsList[_main.ActiveLvl - 1].Coordinatese)
            {
                _widthOfSpider = 90 * _main.SpiderScale;//200
                _heightOfSpider = 46 * _main.SpiderScale;//102

                var image = new Image
                {
                    Width = _widthOfSpider,
                    Height = _heightOfSpider,
                    Source = new BitmapImage(new Uri($"ms-appx:///Assets/Spiders/{r.Next(1, 7)}.png")),
                    Name = i++.ToString()
                };
                
                SpiderCanvas.Children.Add(image);
                Canvas.SetLeft(image, ConvertWidthPercentToPoint(coordinates.X) - image.Width / 2);
                Canvas.SetTop(image, ConvertHeightPercentToPoint(coordinates.Y) - image.Height / 2);
            }
        }

        public void DrawingContacts()
        {
            ContactsCanvas.Children.Clear();

            #region Проверка на пересечение, отрисовка линий
            _isLvlComplete = true;

            foreach (var contact in _main.LvlsList[_main.ActiveLvl - 1].Contacts)
            {
                try
                {
                    var spider = SpiderCanvas.Children[contact.ContactStart] as Image;

                    var ax1 = Canvas.GetLeft(SpiderCanvas.Children[contact.ContactStart]) + _widthOfSpider / 2;
                    var ay1 = Canvas.GetTop(SpiderCanvas.Children[contact.ContactStart]) + _heightOfSpider / 2;

                    var ax2 = Canvas.GetLeft(SpiderCanvas.Children[contact.ContactStop]) + _widthOfSpider / 2;
                    var ay2 = Canvas.GetTop(SpiderCanvas.Children[contact.ContactStop]) + _heightOfSpider / 2;
                    var isCross = false;

                    foreach (var contactJ in _main.LvlsList[_main.ActiveLvl - 1].Contacts)
                    {
                        if (contact.ContactStart != contactJ.ContactStart && contact.ContactStop != contactJ.ContactStop)
                        {
                            var bx1 = Canvas.GetLeft(SpiderCanvas.Children[contactJ.ContactStart]) + _widthOfSpider / 2;
                            var by1 = Canvas.GetTop(SpiderCanvas.Children[contactJ.ContactStart]) + _heightOfSpider / 2;

                            var bx2 = Canvas.GetLeft(SpiderCanvas.Children[contactJ.ContactStop]) + _widthOfSpider / 2;
                            var by2 = Canvas.GetTop(SpiderCanvas.Children[contactJ.ContactStop]) + _heightOfSpider / 2;

                            var v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
                            var v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
                            var v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
                            var v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);

                            if (v1 * v2 < 0 && v3 * v4 < 0 || (Abs(ax1 - ax2) < 1 && Abs(ay1 - ay2) < 1))
                            {
                                isCross = true;
                                _isLvlComplete = false;
                                break;
                            }
                            // По оси x
                            if (Abs(ax1 - ax2) < 1 && Abs(bx1 - bx2) < 1 && Abs(ax1 - bx1) < 1 &&
                                ((Min(ay1, ay2) < Min(by1, by2) && Max(ay1, ay2) > Min(by1, by2)) ||
                                 (Min(by1, by2) < Min(ay1, ay2) && Max(by1, by2) > Min(ay1, ay2))))
                            {
                                isCross = true;
                                _isLvlComplete = false;
                                break;
                            }
                            // По оси y
                            if (Abs(ay1 - ay2) < 1 && Abs(by1 - by2) < 1 && Abs(ay1 - by1) < 1 &&
                                ((Min(ax1, ax2) < Min(bx1, bx2) && Max(ax1, ax2) > Min(bx1, bx2)) ||
                                 (Min(bx1, bx2) < Min(ax1, ax2) && Max(bx1, bx2) > Min(ax1, ax2))))
                            {
                                isCross = true;
                                _isLvlComplete = false;
                                break;
                            }
                        }
                    }

                    var line = new Line
                    {
                        X1 = ax1,
                        X2 = ax2,
                        Y1 = ay1,
                        Y2 = ay2,
                        Stroke = isCross ? new SolidColorBrush(Colors.Crimson) : new SolidColorBrush(Colors.LightSeaGreen)
                    };
                    ContactsCanvas.Children.Add(line);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Exception");
                }
            }
            #endregion

            #region Подсветка связанных пауков
            if (_isSpiderSelected)
            {
                var selectedSpiderIndex = 0;

                foreach (var contact in _main.LvlsList[_main.ActiveLvl - 1].Contacts)
                {
                    if (SpiderCanvas.Children[contact.ContactStart] == _selectedSpider)
                    {
                        selectedSpiderIndex = contact.ContactStart;
                        break;
                    }
                    if (SpiderCanvas.Children[contact.ContactStop] == _selectedSpider)
                    {
                        selectedSpiderIndex = contact.ContactStop;
                        break;
                    }
                }

                foreach (var contact in _main.LvlsList[_main.ActiveLvl - 1].Contacts)
                {
                    if (contact.ContactStart == selectedSpiderIndex || contact.ContactStop == selectedSpiderIndex)
                    {
                        var ellipse = new Ellipse
                        {
                            Width = _widthOfSpider + 20 * _main.SpiderScale,
                            Height = _heightOfSpider + 20 * _main.SpiderScale,
                            Stroke = new SolidColorBrush(Colors.Green),
                            Fill = new SolidColorBrush(Colors.MediumSeaGreen),
                            Opacity = .2
                        };
                        ContactsCanvas.Children.Add(ellipse);

                        if (contact.ContactStart == selectedSpiderIndex)
                        {
                            Canvas.SetLeft(ellipse, Canvas.GetLeft(SpiderCanvas.Children[contact.ContactStop]) - 10 * _main.SpiderScale);
                            Canvas.SetTop(ellipse, Canvas.GetTop(SpiderCanvas.Children[contact.ContactStop]) - 10 * _main.SpiderScale);
                        }
                        else if (contact.ContactStop == selectedSpiderIndex)
                        {
                            Canvas.SetLeft(ellipse, Canvas.GetLeft(SpiderCanvas.Children[contact.ContactStart]) - 10 * _main.SpiderScale);
                            Canvas.SetTop(ellipse, Canvas.GetTop(SpiderCanvas.Children[contact.ContactStart]) - 10 * _main.SpiderScale);
                        }
                    }
                }
            }
            #endregion
        }

        public void DrawingRectangle()
        {
            MainCanvas.Children.Clear();

            #region Вывод прямоугольника и номера уровня
            var r = new Rectangle
            {
                Height = Min(MainCanvas.ActualWidth, MainCanvas.ActualHeight),
                Width = Min(MainCanvas.ActualWidth, MainCanvas.ActualHeight),
                Stroke = new SolidColorBrush(Colors.LightSeaGreen),
                Opacity = .3
            };
            MainCanvas.Children.Add(r);
            Canvas.SetLeft(r, Max(0, (MainCanvas.ActualWidth - MainCanvas.ActualHeight) / 2));
            Canvas.SetTop(r, Max(0, (MainCanvas.ActualHeight - MainCanvas.ActualWidth) / 2));

            var l = new TextBlock
            {
                Text = _main.ActiveLvl.ToString(),
                FontSize = 250,
                Foreground = new SolidColorBrush(Colors.DarkSalmon),
                Opacity = .15,
                Width = MainCanvas.ActualWidth,
                TextAlignment = TextAlignment.Center
            };
            MainCanvas.Children.Add(l);

            var scaleFactor = DisplayInformation.GetForCurrentView().LogicalDpi;
            Canvas.SetTop(l, MainCanvas.ActualHeight / 2 - 250 * 72 / scaleFactor);
            #endregion
        }

        public void RescaleSpiders()
        {
            var oldWidthOfSpider = _widthOfSpider;
            var oldHeightOfSpider = _heightOfSpider;

            foreach (var spider in SpiderCanvas.Children)
            {
                var img = spider as Image;
                _widthOfSpider = 100 * _main.SpiderScale;
                _heightOfSpider = 59 * _main.SpiderScale;

                img.Width = _widthOfSpider;
                img.Height = _heightOfSpider;

            Canvas.SetLeft(spider, Canvas.GetLeft(spider) + (oldWidthOfSpider - _widthOfSpider) / 2);
            Canvas.SetTop(spider, Canvas.GetTop(spider) + (oldHeightOfSpider - _heightOfSpider) / 2);
            }

            DrawingContacts();
        }

        private async Task OnLvlComplete()
        {
            _isLvlComplete = false;
            if (!_isLvlCompleteDone)
            {
                _isLvlCompleteDone = true;
                await _main.OnLvlComplete();
            }
        }

        #region Обработка Pointer actions
        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var x = e.GetCurrentPoint(SpiderCanvas).Position.X;
            var y = e.GetCurrentPoint(SpiderCanvas).Position.Y;

            //Определяем нажатие на паука
            foreach (var image in SpiderCanvas.Children.OfType<Image>())
            {
                var x1 = Canvas.GetLeft(image);
                var x2 = Canvas.GetLeft(image) + image.ActualWidth;

                var y1 = Canvas.GetTop(image);
                var y2 = Canvas.GetTop(image) + image.ActualHeight;

                //Если указатель попал по паучку
                if (x >= x1 && x <= x2 && y >= y1 && y <= y2)
                {
                    _isSpiderSelected = true;
                    _selectedSpider = image;
                    DrawingContacts();
                    break;
                }
            }
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var x = e.GetCurrentPoint(SpiderCanvas).Position.X;
            var y = e.GetCurrentPoint(SpiderCanvas).Position.Y;

            //Если перемещается паук - меняется только его расположение и перерисовываются связи
            if (_isSpiderSelected)
            {
                var spider = _main.LvlsList[_main.ActiveLvl - 1].Coordinatese[Convert.ToInt32(_selectedSpider.Name)];
                x = Min(Max(0, x - _widthOfSpider / 2), SpiderCanvas.ActualWidth - _widthOfSpider);
                y = Min(Max(0, y - _heightOfSpider / 2), SpiderCanvas.ActualHeight - _heightOfSpider);

                spider.X = ConvertWidthPointToPercent(x + _widthOfSpider / 2);
                spider.Y = ConvertHeightPointToPercent(y + _heightOfSpider / 2);

                Canvas.SetLeft(_selectedSpider, x);
                Canvas.SetTop(_selectedSpider, y);

                DrawingContacts();
            }
        }

        private async void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _isSpiderSelected = false;
            DrawingContacts();

            if (_isLvlComplete)
                await OnLvlComplete();
        }

        private async void OnPointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            _isSpiderSelected = false;
            DrawingContacts();

            if (_isLvlComplete)
                await OnLvlComplete();
        }
        #endregion

        #region Конвертер координат
        private double ConvertWidthPercentToPoint(double x)
        {
            var a = Min(MainCanvas.ActualWidth, MainCanvas.ActualHeight);
            return Min((MainCanvas.ActualWidth - a) / 2 + x * a / 100, MainCanvas.ActualWidth - _widthOfSpider / 2);
        }

        private double ConvertHeightPercentToPoint(double y)
        {
            var a = Min(MainCanvas.ActualWidth, MainCanvas.ActualHeight);
            return Min((MainCanvas.ActualHeight - a) / 2 + y * a / 100, MainCanvas.ActualHeight - _heightOfSpider / 2);
        }

        private double ConvertWidthPointToPercent(double x)
        {
            var a = Min(MainCanvas.ActualWidth, MainCanvas.ActualHeight);
            return Min(Max(0, (x - (MainCanvas.ActualWidth - a) / 2) / a * 100), 100);
        }

        private double ConvertHeightPointToPercent(double y)
        {
            var a = Min(MainCanvas.ActualWidth, MainCanvas.ActualHeight);
            return Min(Max(0, (y - (MainCanvas.ActualHeight - a) / 2) / a * 100), 100);
        }
        #endregion
    }
}

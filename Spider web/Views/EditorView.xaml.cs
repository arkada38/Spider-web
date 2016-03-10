using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Spider_web.Models;
using Spider_web.Utils;
using Spider_web.ViewModels;
using static System.Math;

namespace Spider_web.Views
{
    public sealed partial class EditorView
    {
        private MainPageViewModel _main;
        private bool _isSpiderSelected, _isLvlComplete, _isSpiderAdding, _isLineAdding, _isLineAddingInProcess;
        private Image _selectedSpider;
        private double _widthOfSpider, _heightOfSpider;
        private readonly UserLvl _newLvl;
        private int _startContact, _stopContact;
        private readonly Line _line;

        public EditorView()
        {
            InitializeComponent();

            _newLvl = new UserLvl
            {
                Coordinatese = new List<SpidersCoordinates>(),
                Contacts = new List<SpidersContacts>()
            };
            _selectedSpider = new Image();
            _line = new Line
            {
                Stroke = new SolidColorBrush(Colors.Crimson)
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _main = (MainPageViewModel) e.Parameter;
            _main.Editor = this;
            base.OnNavigatedTo(e);
        }

        private void OnSquareSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CanvasSize.Text = $"{(int) MainCanvas.ActualWidth}x{(int) MainCanvas.ActualHeight}";

            DrawingSpiders();
            DrawingContacts();
        }

        public void DrawingSpiders()
        {
            SpiderCanvas.Children.Clear();

            var i = 0;
            foreach (var coordinates in _newLvl.Coordinatese)
            {
                _widthOfSpider = 100 * _main.SpiderScale;
                _heightOfSpider = 59 * _main.SpiderScale;

                var image = new Image
                {
                    Width = _widthOfSpider,
                    Height = _heightOfSpider,
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/Spiders/1.png")),
                    Name = i++.ToString()
                };

                SpiderCanvas.Children.Add(image);
                Canvas.SetLeft(image, ConvertWidthPercentToPoint(coordinates.X) - image.Width / 2);
                Canvas.SetTop(image, ConvertHeightPercentToPoint(coordinates.Y) - image.Height / 2);
            }
        }

        public void DrawingContacts()
        {
            MainCanvas.Children.Clear();

            #region Вывод прямоугольника
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
            #endregion

            #region Проверка на пересечение, отрисовка линий
            _isLvlComplete = true;

            foreach (var contact in _newLvl.Contacts)
            {
                try
                {
                    var ax1 = Canvas.GetLeft(SpiderCanvas.Children[contact.ContactStart]) + _widthOfSpider / 2;
                    var ay1 = Canvas.GetTop(SpiderCanvas.Children[contact.ContactStart]) + _heightOfSpider / 2;

                    var ax2 = Canvas.GetLeft(SpiderCanvas.Children[contact.ContactStop]) + _widthOfSpider / 2;
                    var ay2 = Canvas.GetTop(SpiderCanvas.Children[contact.ContactStop]) + _heightOfSpider / 2;
                    var isCross = false;

                    foreach (var contactJ in _newLvl.Contacts)
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
                                LvlComplete.Fill = _isLvlComplete ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                                break;
                            }
                            // По оси x
                            if (Abs(ax1 - ax2) < 1 && Abs(bx1 - bx2) < 1 && Abs(ax1 - bx1) < 1 &&
                                ((Min(ay1, ay2) < Min(by1, by2) && Max(ay1, ay2) > Min(by1, by2)) ||
                                 (Min(by1, by2) < Min(ay1, ay2) && Max(by1, by2) > Min(ay1, ay2))))
                            {
                                isCross = true;
                                _isLvlComplete = false;
                                LvlComplete.Fill = _isLvlComplete ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                                break;
                            }
                            // По оси y
                            if (Abs(ay1 - ay2) < 1 && Abs(by1 - by2) < 1 && Abs(ay1 - by1) < 1 &&
                                ((Min(ax1, ax2) < Min(bx1, bx2) && Max(ax1, ax2) > Min(bx1, bx2)) ||
                                 (Min(bx1, bx2) < Min(ax1, ax2) && Max(bx1, bx2) > Min(ax1, ax2))))
                            {
                                isCross = true;
                                _isLvlComplete = false;
                                LvlComplete.Fill = _isLvlComplete ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
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
                    MainCanvas.Children.Add(line);
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

                foreach (var contact in _newLvl.Contacts)
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

                foreach (var contact in _newLvl.Contacts)
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
                        MainCanvas.Children.Add(ellipse);

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

        private void AddSpider_Click(object sender, RoutedEventArgs e)
        {
            if (AddSpider.IsChecked != null)
                _isSpiderAdding = (bool) AddSpider.IsChecked;

            if (_isSpiderAdding)
            {
                _isLineAdding = false;
                AddLine.IsChecked = false;
            }
        }

        private void AddLine_Click(object sender, RoutedEventArgs e)
        {
            if (AddLine.IsChecked != null)
                _isLineAdding = (bool)AddLine.IsChecked;

            if (_isLineAdding)
            {
                _isSpiderAdding = false;
                AddSpider.IsChecked = false;
            }
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_newLvl.Coordinatese != null && _newLvl.Contacts != null && _newLvl.Coordinatese.Count > 0 && _newLvl.Contacts.Count > 0)
            {
                var spiderCoordinates = _newLvl.Coordinatese.Aggregate(string.Empty, (current, coordinates) =>
                current + $"{Round(coordinates.X)} {Round(coordinates.Y)} ");

                var spiderContacts = _newLvl.Contacts.Aggregate(string.Empty, (current, contacts) => current + $"{contacts.ContactStart} {contacts.ContactStop} ");

                var difficult = _newLvl.Coordinatese.Count * _newLvl.Contacts.Count;

                var lvl = @",
                new DefaultLvl
                {
                    CoordinatesString = """ + spiderCoordinates.Remove(spiderCoordinates.Length - 1) + @""",//" + difficult + @"
                    ContactsString = """ + spiderContacts.Remove(spiderContacts.Length - 1) + @"""
                }";

                var dataPackage = new DataPackage { RequestedOperation = DataPackageOperation.Copy };
                dataPackage.SetText(lvl);
                Clipboard.SetContent(dataPackage);

                TextToastManager.Instance.ShowToast("Код уровня скопирован в буфер обмена", $"Сложность {difficult}");
            }
            else TextToastManager.Instance.ShowToast("Уровень не может быть пустым");
        }

        private void MixButton_OnClickButton_Click(object sender, RoutedEventArgs e)
        {
            var r = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < 100; i++)
            {
                var r1 = r.Next(0, _newLvl.Coordinatese.Count);
                var r2 = r.Next(0, _newLvl.Coordinatese.Count);
                if (r1 == r2) continue;

                var c = _newLvl.Coordinatese;
                var coord = c[r1];

                c[r1] = c[r2];
                c[r2] = coord;
            }

            DrawingSpiders();
            DrawingContacts();
        }

        #region Обработка Pointer actions
        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var x = e.GetCurrentPoint(SpiderCanvas).Position.X;
            var y = e.GetCurrentPoint(SpiderCanvas).Position.Y;

            PressPos.Text = $"{(int)ConvertWidthPointToPercent(x)}x{(int)ConvertHeightPointToPercent(y)}";

            if (_isSpiderAdding)
            {
                _newLvl.Coordinatese.Add(new SpidersCoordinates { X = ConvertWidthPointToPercent(x), Y = ConvertHeightPointToPercent(y) });

                _widthOfSpider = 100 * _main.SpiderScale;
                _heightOfSpider = 59 * _main.SpiderScale;

                var image = new Image
                {
                    Width = _widthOfSpider,
                    Height = _heightOfSpider,
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/Spiders/1.png")),
                    Name = (_newLvl.Coordinatese.Count - 1).ToString()
                };

                SpiderCanvas.Children.Add(image);
                Canvas.SetLeft(image, x - image.Width / 2);
                Canvas.SetTop(image, y - image.Height / 2);

                _isSpiderSelected = true;
                _selectedSpider = image;
            }

            //Определяем нажатие на паука
            else foreach (var image in SpiderCanvas.Children.OfType<Image>())
            {
                var x1 = Canvas.GetLeft(image);
                var x2 = Canvas.GetLeft(image) + image.ActualWidth;

                var y1 = Canvas.GetTop(image);
                var y2 = Canvas.GetTop(image) + image.ActualHeight;

                //Если указатель попал по паучку
                if (x >= x1 && x <= x2 && y >= y1 && y <= y2)
                {
                    if (_isLineAdding)
                    {
                        _isLineAddingInProcess = true;
                        _startContact = Convert.ToInt32(image.Name);

                        _line.X1 = x;
                        _line.Y1 = y;
                        _line.X2 = x;
                        _line.Y2 = y;
                        MainCanvas.Children.Add(_line);
                    }
                    else
                    {
                        _isSpiderSelected = true;
                        _selectedSpider = image;
                        DrawingContacts();
                        break;
                    }
                }
            }
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var x = e.GetCurrentPoint(MainCanvas).Position.X;
            var y = e.GetCurrentPoint(MainCanvas).Position.Y;
            PointerPos.Text = $"{(int)x}x{(int)y}";

            //Если перемещается паук - меняется только его расположение и перерисовываются связи
            if (_isSpiderSelected)
            {
                var spider = _newLvl.Coordinatese[Convert.ToInt32(_selectedSpider.Name)];
                x = Min(Max(0, x - _widthOfSpider / 2), SpiderCanvas.ActualWidth - _widthOfSpider);
                y = Min(Max(0, y - _heightOfSpider / 2), SpiderCanvas.ActualHeight - _heightOfSpider);

                spider.X = ConvertWidthPointToPercent(x + _widthOfSpider / 2);
                spider.Y = ConvertHeightPointToPercent(y + _heightOfSpider / 2);

                Canvas.SetLeft(_selectedSpider, x);
                Canvas.SetTop(_selectedSpider, y);

                DrawingContacts();
            }
            else if (_isLineAddingInProcess)
            {
                _line.X2 = x;
                _line.Y2 = y;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var userLvl = (UserLvl) LvlBox.SelectedItem;
            if (userLvl == null) return;

            _newLvl.Contacts = userLvl.Contacts;
            _newLvl.Coordinatese = userLvl.Coordinatese;

            DrawingSpiders();
            DrawingContacts();
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _isSpiderSelected = false;
            DrawingContacts();

            LvlComplete.Fill = _isLvlComplete ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

            var a = Min(SpiderCanvas.ActualWidth, SpiderCanvas.ActualHeight);
            var x = e.GetCurrentPoint(SpiderCanvas).Position.X;
            var y = e.GetCurrentPoint(SpiderCanvas).Position.Y;

            //x = ConvertWidthPointToPercent(x);
            x = (x - (SpiderCanvas.ActualWidth - a) / 2) / a * 100;
            //y = ConvertHeightPercentToPoint(y);
            y = (y - (SpiderCanvas.ActualHeight - a) / 2) / a * 100;

            PressPos.Text = $"{(int)x}x{(int)y}";

            if (_isLineAddingInProcess)
            {
                x = e.GetCurrentPoint(SpiderCanvas).Position.X;
                y = e.GetCurrentPoint(SpiderCanvas).Position.Y;

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
                        _stopContact = Convert.ToInt32(image.Name);

                        var unical = _newLvl.Contacts.All(c => c.ContactStart != _startContact || c.ContactStop != _stopContact);

                        if (unical)
                        {
                            _newLvl.Contacts.Add(new SpidersContacts
                            {
                                ContactStart = _startContact,
                                ContactStop = _stopContact
                            });
                            DrawingContacts();
                        }
                    }
                }
                _isLineAddingInProcess = false;
            }
        }

        private void OnPointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            _isLineAddingInProcess = false;
            _isSpiderSelected = false;
            DrawingContacts();

            LvlComplete.Fill = _isLvlComplete ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
        }
        #endregion

        #region Конвертер координат
        private double ConvertWidthPercentToPoint(double x)
        {
            var a = Min(SpiderCanvas.ActualWidth, SpiderCanvas.ActualHeight);
            return Min((SpiderCanvas.ActualWidth - a) / 2 + x * a / 100, SpiderCanvas.ActualWidth - _widthOfSpider);
        }

        private double ConvertHeightPercentToPoint(double y)
        {
            var a = Min(SpiderCanvas.ActualWidth, SpiderCanvas.ActualHeight);
            return Min((SpiderCanvas.ActualHeight - a) / 2 + y * a / 100, SpiderCanvas.ActualHeight - _heightOfSpider);
        }

        private double ConvertWidthPointToPercent(double x)
        {
            var a = Min(SpiderCanvas.ActualWidth, SpiderCanvas.ActualHeight);
            return Min(Max(0, (x - (SpiderCanvas.ActualWidth - a) / 2) / a * 100), 100);
        }

        private double ConvertHeightPointToPercent(double y)
        {
            var a = Min(SpiderCanvas.ActualWidth, SpiderCanvas.ActualHeight);
            return Min(Max(0, (y - (SpiderCanvas.ActualHeight - a) / 2) / a * 100), 100);
        }
        #endregion
    }
}

using Candlestick_Patterns;
using ScottPlot;
using ScottPlot.Colormaps;
using ScottPlot.Palettes;
using ScottPlot.WPF;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace WPFGraphMaker
{
    public class PatternInfo
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string NormalizedName { get; set; }
        public string Category { get; set; } // e.g., "Patterns", "Formations", "Fibonacci"
    }

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly IDataFromJson _data = new DataFromJson();
        private readonly IMethodsDictionary _dict = new MethodsDictionary();
        private readonly ICandlesticAmountDictionary _candle = new CandlesticAmountDictionary();
        private List<ZigZagObject> _points = new List<ZigZagObject>();
        private List<OhlcvObject> _pointsOhlcv = new List<OhlcvObject>();
        private readonly int _startPoints = 100;
        private readonly int _scrollStep = 10;
        private int _lastPosition = 100;
        private List<int> _foundPatternIndexList = new();
        int counter = 0;
        private bool HasOhlcvData => _pointsOhlcv != null && _pointsOhlcv.Count > 0 && _startPoints > 0;

        public int CurrentPatternNumber => _foundPatternIndexList.Count == 0 ? 0 : counter + 1;
        public int TotalPatterns => _foundPatternIndexList.Count;
        private readonly string _sampleFile = Path.Combine(AppContext.BaseDirectory, @"..\..\..\sample_data.json");

        private List<PatternInfo> _patterns;
        private Window _patternPopup;
        private string _lastSearchText = "";

        private readonly string _searchPatternBasicPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\docs"));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            WpfPlot1 = new WpfPlot
            {
                Width = mainWin.Width - 30,
                Height = mainWin.Height - 80,
            };

            mainWin.SizeChanged += OnSizeChangedEvent;
            WpfPlot1.MouseWheel += OnMouseWheelEvent;
            InitializePatternSearch();
        }

        private void InitializePatternSearch()
        {
            _patterns = new List<PatternInfo>();

            // Load patterns from multiple markdown files
            string[] markdownFiles = new[]
            {
                "Fibonacci_list.md",
                "Formations_list.md",
                "Patterns_list.md",
            };

            foreach (var markdownPath in markdownFiles)
            {
                var path = Path.Combine(_searchPatternBasicPath, markdownPath);
                if (File.Exists(path))
                {
                    LoadPatternsFromMarkdown(path);
                }
            }

            patternNameTextBox.TextChanged += PatternNameTextBox_TextChanged;
        }

        private void LoadPatternsFromMarkdown(string filePath)
        {
            try
            {
                string content = File.ReadAllText(Path.Combine(_searchPatternBasicPath, filePath));

                // Extract category from filename (e.g., "patterns_example.md" -> "Patterns")
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string category = fileName.Replace("_list", "").Replace("_", " ");
                category = char.ToUpper(category[0]) + category.Substring(1);

                var pattern = @"<img[^>]*alt\s*=\s*[""']([^""']+)[""'][^>]*src\s*=\s*[""']([^""']+)[""']";
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                var matches = regex.Matches(content);

                foreach (Match match in matches)
                {
                    if (match.Groups.Count >= 3)
                    {
                        string patternName = match.Groups[1].Value.Trim();
                        string imageUrl = match.Groups[2].Value.Trim();

                        if (!string.IsNullOrEmpty(patternName) && !string.IsNullOrEmpty(imageUrl))
                        {
                            _patterns.Add(new PatternInfo
                            {
                                Name = patternName,
                                ImageUrl = imageUrl,
                                NormalizedName = NormalizePatternName(patternName),
                                Category = category
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading patterns from {filePath}: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string NormalizePatternName(string name)
        {
            return name.Replace(" ", "").ToLower();
        }

        private void PatternNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_suppressTextChanged) return;

            string searchText = patternNameTextBox.Text.Trim();
            if (searchText == _lastSearchText) return;

            _lastSearchText = searchText;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                ClosePatternPopup();
                return;
            }

            string normalizedSearch = NormalizePatternName(searchText);
            var matchingPatterns = _patterns.Where(p => p.NormalizedName.Contains(normalizedSearch)).ToList();

            if (matchingPatterns.Count == 0)
            {
                ClosePatternPopup();
                return;
            }

            if (matchingPatterns.Count == 1)
            {
                SetPatternText(matchingPatterns[0].Name);
                ShowPatternPopup(matchingPatterns[0]);
            }

            else
            {
                ShowMultiplePatternPopup(matchingPatterns, searchText);
            }

        }

        private bool _suppressTextChanged;
        private void SetPatternText(string text)
        {
            _suppressTextChanged = true;

            patternNameTextBox.Text = text;
            patternNameTextBox.CaretIndex = text.Length;

            _lastSearchText = text;

            _suppressTextChanged = false;
        }

        private void ShowPatternPopup(PatternInfo pattern)
        {
            ClosePatternPopup();
            _patternPopup = new Window
            {
                Title = $"Pattern Found: {pattern.Name}",
                Width = 450,
                Height = 380,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.SingleBorderWindow
            };

            var grid = new Grid { Margin = new Thickness(20) };
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var foundText = new TextBlock
            {
                Text = pattern.Name,
                FontSize = 10,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 5)
            };
            Grid.SetRow(foundText, 0);
            grid.Children.Add(foundText);

            // Category text
            var categoryText = new TextBlock
            {
                Text = $"Category: {pattern.Category}",
                FontSize = 12,
                FontStyle = FontStyles.Italic,
                Foreground = System.Windows.Media.Brushes.Gray,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 15)
            };
            Grid.SetRow(categoryText, 1);
            grid.Children.Add(categoryText);

            // Image
            var imageBorder = new Border
            {
                BorderBrush = System.Windows.Media.Brushes.LightGray,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, 0, 15),
                MaxWidth = 400,
                MaxHeight = 250
            };

            var image = new Image
            {
                Stretch = System.Windows.Media.Stretch.Uniform,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            imageBorder.Child = image;
            Grid.SetRow(imageBorder, 2);
            grid.Children.Add(imageBorder);

            // OK Button
            var okButton = new Button
            {
                Content = "OK",
                Width = 100,
                Height = 35,
                FontSize = 14,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                IsDefault = true
            };
            okButton.Click += (s, e) => ClosePatternPopup();
            Grid.SetRow(okButton, 3);
            grid.Children.Add(okButton);

            _patternPopup.Content = grid;
            LoadImageFromUrl(image, pattern.ImageUrl);

            _patternPopup.Show();
        }

        private void ShowMultiplePatternPopup(List<PatternInfo> patterns, string searchText)
        {
            ClosePatternPopup();

            _patternPopup = new Window
            {
                Title = $"Multiple Patterns Found ({patterns.Count} matches)",
                Width = 600,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                WindowStyle = WindowStyle.SingleBorderWindow
            };

            var grid = new Grid { Margin = new Thickness(20) };
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Header
            var header = new TextBlock
            {
                Text = "Multiple Patterns Found",
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };
            Grid.SetRow(header, 0);
            grid.Children.Add(header);

            // Refine search box
            var refineBorder = new Border
            {
                BorderBrush = System.Windows.Media.Brushes.LightGray,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 15),
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(248, 248, 248))
            };

            var refineGrid = new Grid();
            refineGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            refineGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var refineLabel = new TextBlock
            {
                Text = "Refine search:",
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0),
                FontWeight = FontWeights.SemiBold
            };
            Grid.SetColumn(refineLabel, 0);
            refineGrid.Children.Add(refineLabel);

            var refineTextBox = new TextBox
            {
                FontSize = 14,
                Padding = new Thickness(8),
                Text = searchText
            };
            refineTextBox.CaretIndex = refineTextBox.Text.Length;
            Grid.SetColumn(refineTextBox, 1);
            refineGrid.Children.Add(refineTextBox);

            refineBorder.Child = refineGrid;
            Grid.SetRow(refineBorder, 1);
            grid.Children.Add(refineBorder);

            // ListBox for patterns
            var listBox = new ListBox
            {
                BorderBrush = System.Windows.Media.Brushes.LightGray,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, 0, 15)
            };

            // Group patterns by category
            var groupedPatterns = patterns.OrderBy(p => p.Category).ThenBy(p => p.Name);

            foreach (var pattern in groupedPatterns)
            {
                // Create a custom display item
                var itemPanel = new StackPanel { Orientation = System.Windows.Controls.Orientation.Horizontal, Margin = new Thickness(5) };

                var nameText = new TextBlock
                {
                    Text = pattern.Name,
                    FontWeight = FontWeights.SemiBold,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Width = 300
                };

                var categoryText = new TextBlock
                {
                    Text = $"[{pattern.Category}]",
                    FontSize = 11,
                    FontStyle = FontStyles.Italic,
                    Foreground = System.Windows.Media.Brushes.Gray,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0)
                };

                itemPanel.Children.Add(nameText);
                itemPanel.Children.Add(categoryText);

                var listItem = new ListBoxItem { Content = itemPanel, Tag = pattern };
                listBox.Items.Add(listItem);
            }

            listBox.MouseDoubleClick += (s, e) =>
            {
                if (listBox.SelectedItem is ListBoxItem selectedItem && selectedItem.Tag is PatternInfo selected)
                {
                    SetPatternText(selected.NormalizedName);
                    ShowPatternPopup(selected);
                }
            };

            Grid.SetRow(listBox, 2);
            grid.Children.Add(listBox);

            // Refine search logic
            refineTextBox.TextChanged += (s, e) =>
            {
                string refineSearch = refineTextBox.Text.Trim();
                listBox.Items.Clear();

                List<PatternInfo> filteredPatterns;

                if (string.IsNullOrWhiteSpace(refineSearch))
                {
                    filteredPatterns = patterns;
                }
                else
                {
                    string normalized = NormalizePatternName(refineSearch);
                    filteredPatterns = patterns.Where(p => p.NormalizedName.Contains(normalized)).ToList();
                }

                // Re-populate listbox with filtered patterns
                var groupedFiltered = filteredPatterns.OrderBy(p => p.Category).ThenBy(p => p.Name);

                foreach (var pattern in groupedFiltered)
                {
                    var itemPanel = new StackPanel { Orientation = System.Windows.Controls.Orientation.Horizontal, Margin = new Thickness(5) };

                    var nameText = new TextBlock
                    {
                        Text = pattern.Name,
                        FontWeight = FontWeights.SemiBold,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        Width = 300
                    };

                    var categoryText = new TextBlock
                    {
                        Text = $"[{pattern.Category}]",
                        FontSize = 11,
                        FontStyle = FontStyles.Italic,
                        Foreground = System.Windows.Media.Brushes.Gray,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        Margin = new Thickness(10, 0, 0, 0)
                    };

                    itemPanel.Children.Add(nameText);
                    itemPanel.Children.Add(categoryText);

                    var listItem = new ListBoxItem { Content = itemPanel, Tag = pattern };
                    listBox.Items.Add(listItem);
                }

                if (filteredPatterns.Count == 1)
                    listBox.SelectedIndex = 0;
            };

            // Buttons
            var buttonPanel = new StackPanel
            {
                Orientation = System.Windows.Controls.Orientation.Horizontal,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };

            var viewButton = new Button
            {
                Content = "View Selected",
                Width = 120,
                Height = 35,
                FontSize = 14,
                Margin = new Thickness(0, 0, 10, 0),
                IsDefault = true,
                IsEnabled = false
            };
            viewButton.Click += (s, e) =>
            {
                if (listBox.SelectedItem is ListBoxItem selectedItem && selectedItem.Tag is PatternInfo selected)
                {
                    ShowPatternPopup(selected);
                }
            };

            listBox.SelectionChanged += (s, e) =>
            {
                viewButton.IsEnabled = listBox.SelectedItem != null;
            };

            var cancelButton = new Button
            {
                Content = "Cancel",
                Width = 100,
                Height = 35,
                FontSize = 14,
                IsCancel = true
            };
            cancelButton.Click += (s, e) => ClosePatternPopup();

            buttonPanel.Children.Add(viewButton);
            buttonPanel.Children.Add(cancelButton);

            Grid.SetRow(buttonPanel, 3);
            grid.Children.Add(buttonPanel);

            _patternPopup.Content = grid;
            _patternPopup.Show();

            refineTextBox.Focus();
        }

        private async void LoadImageFromUrl(Image imageControl, string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var imageBytes = await client.GetByteArrayAsync(url);

                    var bitmap = new BitmapImage();
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();
                        bitmap.Freeze();
                    }

                    imageControl.Source = bitmap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ClosePatternPopup()
        {
            if (_patternPopup != null)
            {
                _patternPopup.Close();
                _patternPopup = null;
            }
        }

        private void OnMouseWheelEvent(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                DeltaPlus();
            }
            else if (e.Delta < 0)
            {
                DeltaMinus();
            }
        }

        private void DeltaPlus()
        {
            if (!HasOhlcvData) return;
            if (_lastPosition - _startPoints < 0)
            {
                _lastPosition = _startPoints;
            }

            _lastPosition = _lastPosition + _scrollStep;
            if (_points.Count != 0)
            {
                DeltaPlusForZigZagPoints();
            }
            else
            {
                DeltaPlusForCandlestickOhlcvPoints();
            }
        }

        private void DeltaMinus()
        {
            if (!HasOhlcvData) return;
            _lastPosition = _lastPosition - _scrollStep;
            if (_points.Count != 0)
            {
                DeltaMinusForZigZagPoints();
            }
            else
            {
                DeltaMinusForCandlestickOhlcvPoints();
            }
        }

        private void DeltaMinusForZigZagPoints()
        {
            if (!HasOhlcvData) return;
            if (_lastPosition - _startPoints <= 0)
            {
                var yMinStart = GetYMinStartForZigZagPoints();
                var yMaxStart = GetYMaxStartForZigZagPoints();
                _lastPosition = 0;
                OnDataLoadedScale(yMinStart, yMaxStart);
            }
            else
            {
                DoMouseWheelAction();
            }
        }

        private void DeltaMinusForCandlestickOhlcvPoints()
        {
            if (!HasOhlcvData) return;
            if (_lastPosition - _startPoints <= 0)
            {
                var yMinStart = GetYMinStartForCandlesticGraph();
                var yMaxStart = GetYMaxStartForCandlesticGraph();
                _lastPosition = 0;
                OnDataLoadedScale(yMinStart, yMaxStart);
            }
            else
            {
                DoMouseWheelActionForCandlestick();
            }
        }

        private void DeltaPlusForCandlestickOhlcvPoints()
        {
            if (!HasOhlcvData) return;
            if (_lastPosition + _startPoints > _pointsOhlcv.Count)
            {
                var yMinStart = _pointsOhlcv.Select(x => x.Low).TakeLast(_startPoints).Min();
                var yMaxStart = _pointsOhlcv.Select(x => x.High).TakeLast(_startPoints).Max();
                WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                WpfPlot1.Plot.Axes.SetLimitsX(_pointsOhlcv.Count - _startPoints, _pointsOhlcv.Count);
                _lastPosition = _pointsOhlcv.Count;
            }
            else
            {
                DoMouseWheelActionForCandlestick();
            }
        }

        private void DeltaPlusForZigZagPoints()
        {
            if (!HasOhlcvData) return;
            if (_lastPosition + _startPoints > _points.Count)
            {
                var yMinStart = _points.Select(x => x.Close).TakeLast(_startPoints).Min();
                var yMaxStart = _points.Select(x => x.Close).TakeLast(_startPoints).Max();
                WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                WpfPlot1.Plot.Axes.SetLimitsX(_points.Count - _startPoints, _points.Count);
                _lastPosition = _points.Count;
            }
            else
            {
                DoMouseWheelAction();
            }
        }

        private void DoMouseWheelAction()
        {
            if (!HasOhlcvData) return;
            var yMinStart = _points.Select(x => x.Close).Skip(_lastPosition - _startPoints).Take(_startPoints).Min();
            var yMaxStart = _points.Select(x => x.Close).Skip(_lastPosition - _startPoints).Take(_startPoints).Max();

            OnMouseWheelScale(yMinStart, yMaxStart);
        }

        private void DoMouseWheelActionForCandlestick()
        {
            if (!HasOhlcvData) return;
            var yMinStart = _pointsOhlcv.Select(x => x.Low).Skip(_lastPosition - _startPoints).Take(_startPoints).Min();
            var yMaxStart = _pointsOhlcv.Select(x => x.High).Skip(_lastPosition - _startPoints).Take(_startPoints).Max();
            OnMouseWheelScale(yMinStart, yMaxStart);
        }

        private void OnMouseWheelScale(decimal yMinStart, decimal yMaxStart)
        {
            WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
            WpfPlot1.Plot.Axes.SetLimitsX(_lastPosition - _startPoints, _lastPosition);

            WpfPlot1.Refresh();
        }

        private void OnDataLoadedScale(decimal yMinStart, decimal yMaxStart)
        {
            WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
            WpfPlot1.Plot.Axes.SetLimitsX(-1, _startPoints);

            WpfPlot1.Refresh();
        }

        private void OnSizeChangedEvent(object sender, SizeChangedEventArgs e)
        {
            WpfPlot1.Width = e.NewSize.Width - 30;
            WpfPlot1.Height = e.NewSize.Height - 80;

            WpfPlot1.Refresh();
        }

        public WpfPlot WpfPlot1 { get; set; }

        private string _patternName;

        public string PatternName
        {
            get { return _patternName; }
            set
            {
                if (value != _patternName)
                {
                    _patternName = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _foundXTimes;

        public int FoundXTimes
        {
            get { return _foundXTimes; }
            set
            {
                if (value != _foundXTimes)
                {
                    _foundXTimes = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isTesting;
        public bool IsTesting
        {
            get => _isTesting;
            set
            {
                _isTesting = value;
                OnPropertyChanged();
            }
        }

        private string GetSuitableGroupByPatternName(string methodName)
        {
            var groupName = _dict.GetCategory().Where(x => x.Key.ToLower() == methodName).ToDictionary().Values.First();
            return groupName;
        }

        private async void OnFindNext(object sender, RoutedEventArgs e)
        {
            if (_points.Count > 0)
            {
                FindNext(_points);
            }
            if (_pointsOhlcv.Count > 0)
            {
                FindNext(_pointsOhlcv);
            }
        }

        private void FindNext(List<OhlcvObject> pointsOhlcv)
        {
            var number = 49;
            if (counter < _foundPatternIndexList.Count && _foundXTimes > 0 && _pointsOhlcv.Count > 0)
            {
                var currentIndex = _foundPatternIndexList[counter];
                if (_foundPatternIndexList[counter] - number <= 0)
                {
                    var yMinStart = GetYMinStartForCandlesticGraph();
                    var yMaxStart = GetYMaxStartForCandlesticGraph();
                    _lastPosition = 0;
                    OnDataLoadedScale(yMinStart, yMaxStart);
                }
                else if (_foundPatternIndexList[counter] + number >= _pointsOhlcv.Count)
                {
                    var yMinStart = _pointsOhlcv.Select(x => x.Close).TakeLast(_startPoints).Min();
                    var yMaxStart = _pointsOhlcv.Select(x => x.Close).TakeLast(_startPoints).Max();
                    WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                    WpfPlot1.Plot.Axes.SetLimitsX(_pointsOhlcv.Count - _startPoints, _pointsOhlcv.Count);
                    _lastPosition = _pointsOhlcv.Count;
                }
                else
                {
                    var yMinStart = _pointsOhlcv.Select(x => x.Close).Skip(currentIndex - number).Take(_startPoints).Min();
                    var yMaxStart = _pointsOhlcv.Select(x => x.Close).Skip(currentIndex - number).Take(_startPoints).Max();
                    WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                    WpfPlot1.Plot.Axes.SetLimitsX(currentIndex - number, currentIndex + number);
                    _lastPosition = currentIndex;
                }
                var patternIndex = _foundPatternIndexList[counter];
                var markerY = pointsOhlcv[patternIndex].Close;

                OnPropertyChanged(nameof(CurrentPatternNumber));
                WpfPlot1.Refresh();
            }
            counter += 1;
        }

        private void FindNext(List<ZigZagObject> _points)
        {
            var number = 49;
            if (counter < _foundPatternIndexList.Count && _foundXTimes > 0 && _points.Count > 0)
            {
                var currentIndex = _foundPatternIndexList[counter];
                if (_foundPatternIndexList[counter] - number <= 0)
                {
                    var yMinStart = GetYMinStartForZigZagPoints();
                    var yMaxStart = GetYMaxStartForZigZagPoints();
                    _lastPosition = 0;
                    OnDataLoadedScale(yMinStart, yMaxStart);
                }
                else if (_foundPatternIndexList[counter] + number >= _points.Count)
                {
                    var yMinStart = _points.Select(x => x.Close).TakeLast(_startPoints).Min();
                    var yMaxStart = _points.Select(x => x.Close).TakeLast(_startPoints).Max();
                    WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                    WpfPlot1.Plot.Axes.SetLimitsX(_points.Count - _startPoints, _points.Count);
                    _lastPosition = _points.Count;
                }
                else
                {
                    var yMinStart = _points.Select(x => x.Close).Skip(currentIndex - number).Take(_startPoints).Min();
                    var yMaxStart = _points.Select(x => x.Close).Skip(currentIndex - number).Take(_startPoints).Max();
                    WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                    WpfPlot1.Plot.Axes.SetLimitsX(currentIndex - number, currentIndex + number);
                    _lastPosition = currentIndex;
                }

                OnPropertyChanged(nameof(CurrentPatternNumber));
                WpfPlot1.Refresh();
            }
            counter += 1;
        }

        private async void OnStartClick(object sender, RoutedEventArgs e)
        {
            IsTesting = true;
            //var watch = Stopwatch.StartNew();
            //GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

            _foundPatternIndexList = new();
            FoundXTimes = 0;
            counter = 0;
            WpfPlot1.Plot.Clear();

            OnPropertyChanged(nameof(TotalPatterns));
            OnPropertyChanged(nameof(CurrentPatternNumber));

            //const string url = "https://gist.github.com/przemyslawbak/92c3d4bba27cfd2b88d0dd916bbdad14/raw/AAL_1min.json";
            //using var client = new HttpClient();
            //string json = await client.GetStringAsync(url);

            var json = string.Empty;
            using var fileStream = new FileStream(
                _sampleFile,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 1048576,
                useAsync: true);

            using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
            {
                json = await reader.ReadToEndAsync();
            }

            var patternName = string.IsNullOrWhiteSpace(patternNameTextBox.Text)
                ? "BearishBlackClosingMarubozu"
                : patternNameTextBox.Text.Trim().Replace(" ", "").ToLower();

            var getSuitableMethodByGivenName = GetSuitableGroupByPatternName(patternName);

            if (getSuitableMethodByGivenName == "patterns")
            {
                _points = new();
                _pointsOhlcv = GetCandlestickData(patternName, json);
                ViewCandlestickGraph(_pointsOhlcv);
                MarkCandlestickOnGraph(_pointsOhlcv, patternName);

                _foundPatternIndexList.Clear();
                _foundPatternIndexList.Capacity = _pointsOhlcv.Count / 10;

                for (int i = 0; i < _pointsOhlcv.Count; i++)
                {
                    if (_pointsOhlcv[i].Signal)
                    {
                        _foundPatternIndexList.Add(i);
                    }
                }
                FoundXTimes = _foundPatternIndexList.Count;
            }
            else
            {
                _pointsOhlcv = new();
                _points = GetGraphData(patternName, json, getSuitableMethodByGivenName);
                ViewGraph(_points);

                _foundPatternIndexList.Clear();
                _foundPatternIndexList.Capacity = _points.Count / 10;

                for (int i = 0; i < _points.Count; i++)
                {
                    if (_points[i].Signal)
                    {
                        _foundPatternIndexList.Add(i);
                    }
                }
                FoundXTimes = _foundPatternIndexList.Count;
            }

            bool hasEnoughData = getSuitableMethodByGivenName == "patterns"
                ? _pointsOhlcv.Count > _startPoints
                : _points.Count > _startPoints;

            if (hasEnoughData)
            {
                if (getSuitableMethodByGivenName == "patterns")
                {
                    var yMinStart = GetYMinStartForCandlesticGraph();
                    var yMaxStart = GetYMaxStartForCandlesticGraph();
                    WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                    WpfPlot1.Plot.Axes.SetLimitsX(-1, _startPoints);
                }
                else
                {
                    var yMinStart = GetYMinStartForZigZagPoints();
                    var yMaxStart = GetYMaxStartForZigZagPoints();
                    OnDataLoadedScale(yMinStart, yMaxStart);
                }
                WpfPlot1.Refresh();
            }
            else
            {
                MessageBox.Show("Not enough data loaded");
            }

            WpfPlot1.Refresh();
            //watch.Stop();
            //MessageBox.Show($"{watch.Elapsed.TotalMilliseconds:F2} ms");
            IsTesting = false;
            OnPropertyChanged(nameof(TotalPatterns));
            OnPropertyChanged(nameof(CurrentPatternNumber));
        }

        private decimal GetYMaxStartForZigZagPoints()
        {
           return _points.Select(x => x.Close).Take(_startPoints).Max();
        }

        private decimal GetYMinStartForZigZagPoints()
        {
            return _points.Select(x => x.Close).Take(_startPoints).Min();
        }

        private decimal GetYMaxStartForCandlesticGraph()
        {
            return _pointsOhlcv.Select(x => x.High).Take(_startPoints).Max();
        }

        private decimal GetYMinStartForCandlesticGraph()
        {
            return _pointsOhlcv.Select(x => x.Low).Take(_startPoints).Min();
        }

        private List<ZigZagObject> GetGraphData(string patternName, string json, string group)
        {
            return _data.GetDataFromJson(patternName, json, group);
        }

        private List<OhlcvObject> GetCandlestickData(string patternName, string json)
        {
            return _data.GetCandlestickDataFromJson(patternName, json);
        }

        private void ViewGraph(List<ZigZagObject> points)
        {
            var xAxesNumbers = new List<double>();
            for (int i = 0; i < points.Count; i++)
            {
                xAxesNumbers.Add((double)i);
            }

            ScottPlot.Palettes.Category20 palette = new();
            var numbersArray = xAxesNumbers.ToArray();
            var pointsPlot = points.Select(x => x.Close).ToArray();
            var myScatter = WpfPlot1.Plot.Add.Scatter(numbersArray, pointsPlot);

            for (int i = 0; i < points.Count; i++)
            {
                var item = points[i];
                if (item.Signal == true)
                {
                    MarkSingalOnChart(i, item, palette, 2);
                }
                if (item.Initiation == true)
                {
                    MarkSingalOnChart(i, item, palette, 8);
                }
            }

            myScatter.Color = Colors.Green;
            myScatter.LineWidth = 1;
            myScatter.MarkerSize = 1.2F;
            myScatter.MarkerShape = MarkerShape.OpenSquare;
            myScatter.LinePattern = LinePattern.Solid;
            WpfPlot1.Refresh();
        }

        private void MarkSingalOnChart(int i, ZigZagObject item, Category20 palette, int color)
        {
            var mp = WpfPlot1.Plot.Add.Marker(i, (double)item.Close);
            mp.MarkerShape = MarkerShape.OpenDiamond;
            mp.MarkerStyle.Size = 15F;
            mp.MarkerStyle.OutlineWidth = 2;
            mp.MarkerStyle.LineWidth = 3f;
            mp.MarkerStyle.LineColor = palette.GetColor(color);
        }

        private void ViewCandlestickGraph(List<OhlcvObject> points)
        {
            var ohlcvList = new List<ScottPlot.OHLC>();
            ohlcvList = points.Select(x => new OHLC() { Open = (double)x.Open, High = (double)x.High, Low = (double)x.Low, Close = (double)x.Close,}).ToList();
            var myScatter = WpfPlot1.Plot.Add.Candlestick(ohlcvList);
            myScatter.RisingColor = Colors.Green; 
            myScatter.FallingColor = Colors.Red;
            myScatter.Sequential = true;

            WpfPlot1.Refresh();
        }

        private void MarkCandlestickOnGraph(List<OhlcvObject> _pointsOhlcv, string patternName)
        {
            var candlesNumbers = _candle.GetCandlestickAmount();
            if (!candlesNumbers.TryGetValue(patternName, out var singleCandleAmount))
            {
                WpfPlot1.Refresh();
                return;
            }

            var palette = new ScottPlot.Palettes.Normal();

            var closeValues = new List<decimal>(_pointsOhlcv.Count);
            for (int i = 0; i < _pointsOhlcv.Count; i++)
            {
                closeValues.Add(_pointsOhlcv[i].Close);
            }

            var myScatter = WpfPlot1.Plot.Add.Signal(closeValues);
            myScatter.Color = Colors.Orange;

            for (int i = 0; i < _pointsOhlcv.Count; i++)
            {
                if (_pointsOhlcv[i].Signal == true)
                {
                    int startIdx = Math.Max(0, i - singleCandleAmount + 1);
                    for (int j = startIdx; j <= i; j++)
                    {
                        MarkCandleOnChart(j, _pointsOhlcv[j], palette, 0);
                    }
                }
            }

            WpfPlot1.Refresh();
        }

        private void MarkCandleOnChart(int i, OhlcvObject item, Normal palette, int color)
        {
            var mp = WpfPlot1.Plot.Add.Marker(i, (double)item.Close);
            mp.MarkerShape = MarkerShape.OpenTriangleDown;
            mp.MarkerStyle.Size = 20F;
            mp.MarkerStyle.OutlineWidth = 2;
            mp.MarkerStyle.LineWidth = 3f;
            mp.MarkerStyle.MarkerColor = palette.GetColor(color);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
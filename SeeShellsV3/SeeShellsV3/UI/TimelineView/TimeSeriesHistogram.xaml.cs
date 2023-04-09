using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using SeeShellsV3.Services;
using Unity;

namespace SeeShellsV3.UI
{
    /// <summary>
    /// Interaction logic for TimeSeriesHistogram.xaml
    /// </summary>
    public partial class TimeSeriesHistogram : UserControl
    {
        public IEnumerable ItemsSource { get => GetValue(ItemsSourceProp) as IEnumerable; set => SetValue(ItemsSourceProp, value); }

        public DateTime? MinimumDate { get => GetValue(MinimumDateProp) as DateTime?; set => SetValue(MinimumDateProp, value); }
        public DateTime? MaximumDate { get => GetValue(MaximumDateProp) as DateTime?; set => SetValue(MaximumDateProp, value); }

        public string ColorProperty { get => GetValue(ColorPropertyProp) as string; set => SetValue(ColorPropertyProp, value); }
        public string DateTimeProperty { get => GetValue(DateTimePropertyProp) as string; set => SetValue(DateTimePropertyProp, value); }

        public Color PlotAreaBorderColor { get => (Color) GetValue(PlotAreaBorderColorProp); set => SetValue(PlotAreaBorderColorProp, value); }
        public Color TextColor { get => (Color) GetValue(TextColorProp); set => SetValue(TextColorProp, value); }
        public Color TicklineColor { get => (Color)GetValue(TicklineColorProp); set => SetValue(TicklineColorProp, value); }

        public string YAxisTitle { get; set; }

        public ICollectionView Selected { get => CollectionViewSource.GetDefaultView(_selected); }

        private ICollectionView Items { get; set; }

        private readonly PlotModel _histPlotModel = new PlotModel();
        private readonly DateTimeAxis _dateAxis = new DateTimeAxis { Position = AxisPosition.Bottom };
        private readonly LinearAxis _freqAxis = new LinearAxis { Position = AxisPosition.Left, IsZoomEnabled = false, IsPanEnabled = false };
        private readonly PlotController _histPlotController = new PlotController();
        private readonly Legend _histLegend = new Legend();

        private readonly ObservableCollection<object> _selected = new ObservableCollection<object>();

        public TimeSeriesHistogram()
        {
            InitializeComponent();
            
            _histPlotModel.Axes.Add(_dateAxis);
            _histPlotModel.Axes.Add(_freqAxis);
            _histPlotModel.Legends.Add(_histLegend);

            _histPlotModel.MouseDown += _histLegend_MouseDown;
            _histPlotModel.MouseMove += _histPlotModel_MouseMove;

            _histPlotController.UnbindMouseDown(OxyMouseButton.Right);
            _histPlotController.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);

            List<Array> palettes = new List<Array>((IEnumerable<Array>)Application.Current.Resources["palettes"]);
            List<OxyColor> oxyPalette = new();
            List<Color> colors = new((IEnumerable<Color>)palettes[0]);
            foreach (Color color in colors)
            {
                oxyPalette.Add(
                    OxyColor.FromRgb(
                        color.R,
                        color.G,
                        color.B
                        )
                    );
            }
            HistPlotModel_setColors(new OxyPalette(oxyPalette));

            HistogramPlot.Model = _histPlotModel;
            HistogramPlot.Controller = _histPlotController;

            ResetHistSeries();
            UpdateAxes();
        }

        internal void HistPlotModel_setColors(OxyPalette palette)
        {
            _histPlotModel.DefaultColors = palette.Colors;
            Update();
        }

        private void _histPlotModel_MouseMove(object sender, OxyMouseEventArgs e)
        {
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                var results = _histPlotModel.HitTest(new HitTestArguments(e.Position, 10));

                var result = results
                    .Where(r => r.Element is Series)
                    .Select(r => (r, r.Element as Series))
                    .OrderBy(r => !r.Item2.IsSelected())
                    .ThenBy(r => (r.Item2.Tag as (object, int)?)?.Item2 ?? 0)
                    .Select(r => r.Item2.GetNearestPoint(e.Position, true))
                    .FirstOrDefault();

                if (result != null)
                {
                    HistogramPlot.ShowTracker(result);
                    e.Handled = true;
                    return;
                }
            }

            HistogramPlot.HideTracker();
        }

        private void _histLegend_MouseDown(object sender, OxyMouseDownEventArgs e)
        {
            if (e.ChangedButton != OxyMouseButton.Left)
                return;

            if (_histLegend.LegendArea.Contains(e.Position))
            {

                if (_histPlotModel.Series.OfType<HistogramSeries>().Where(s => s.IsVisible == false).Any())
                {
                    int count = _histPlotModel.Series.OfType<HistogramSeries>().Where(s => s.IsVisible == false).Count();
                    HistogramSeries[] series = new HistogramSeries[count];

                    for ( int i = 0; i < count; i++)
                    {
                        series[i] = _histPlotModel.Series.OfType<HistogramSeries>().Where(s => s.IsVisible == false).Distinct().ElementAt(i);
                    }

                    for ( int i = 0; i < series.Length; i++)
                    {
                        if (series[i].IsSelected())
                        {
                            series[i].Unselect();
                            _selected.Remove((series[i].Tag as (object, int)?)?.Item1);
                            series[i].IsVisible = true;
                        }
                        else
                        {
                            series[i].Select();
                            _selected.Add((series[i].Tag as (object, int)?)?.Item1);
                            series[i].IsVisible = true; 
                        }
                    }
                }
                UpdateColors();
                HistogramPlot.InvalidatePlot();
            }
            else
            {
                _histPlotModel_MouseMove(sender, e);
            }
        }

        protected void OnItemsChange(object sender, NotifyCollectionChangedEventArgs args) => Update();

        private CancellationTokenSource _tokenSource = null;
        protected void Update()
        {
            if (_tokenSource != null)
                _tokenSource.Cancel();

            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;

            Task.Run(() =>
            {
                Thread.Sleep(200);

                if (token.IsCancellationRequested)
                    return;

                Dispatcher.Invoke(() =>
                {
                    ResetHistSeries();
                    UpdateAxes();
                    UpdateColors();
                    HistogramPlot.InvalidatePlot();
                });
            }, token);
        }

        protected void UpdateColors()
        {
            HistogramPlot.InvalidatePlot();

            if (!_histPlotModel.Series.OfType<HistogramSeries>().Where(s => s.IsSelected()).Any())
            {

                _histPlotModel.Series.OfType<HistogramSeries>().ForEach(s => s.FillColor = OxyColor.FromAColor((byte)255, s.ActualFillColor));
            }
            else
                _histPlotModel.Series.OfType<HistogramSeries>().ForEach(s =>
                 {
                 if (!s.RenderInLegend)
                 {
                     s.FillColor = OxyColor.FromAColor((byte)(0), s.ActualFillColor);

                 }
                 else
                     s.FillColor = OxyColor.FromAColor((byte)(s.IsSelected() ? 255 : 20), s.ActualFillColor);
                 });   
        }

        protected void UpdateAxes()
        {
            _histPlotModel.IsLegendVisible = true;
            _histLegend.LegendTextColor = OxyColor.FromArgb(TextColor.A, TextColor.R, TextColor.G, TextColor.B);
            _histLegend.LegendTitleColor = OxyColor.FromArgb(TextColor.A, TextColor.R, TextColor.G, TextColor.B);
            _histLegend.LegendPlacement = LegendPlacement.Outside;
            _histLegend.LegendPosition = LegendPosition.TopCenter;
            _histLegend.LegendOrientation = LegendOrientation.Horizontal;
            _histLegend.LegendSymbolLength = 15.0;
            _histPlotModel.PlotAreaBorderColor = OxyColor.FromArgb(PlotAreaBorderColor.A, PlotAreaBorderColor.R, PlotAreaBorderColor.G, PlotAreaBorderColor.B);

            _dateAxis.TextColor = OxyColor.FromArgb(TextColor.A, TextColor.R, TextColor.G, TextColor.B);
            _dateAxis.TicklineColor = OxyColor.FromArgb(TicklineColor.A, TicklineColor.R, TicklineColor.G, TicklineColor.B);

            _freqAxis.Title = YAxisTitle;

            _freqAxis.TitleColor = OxyColor.FromArgb(TextColor.A, TextColor.R, TextColor.G, TextColor.B);
            _freqAxis.TextColor = OxyColors.Transparent;
            _freqAxis.TicklineColor = OxyColor.FromArgb(TicklineColor.A, TicklineColor.R, TicklineColor.G, TicklineColor.B);
        }

        protected void ResetHistSeries()
        {
            _histPlotModel.Series.Clear();

            if (Items == null || !Items.OfType<object>().Any())
                return;

            IOrderedEnumerable<(DateTime date, object item)> items = Items
                .OfType<object>()
                .Select(x => ((DateTime)x.GetDeepPropertyValue(DateTimeProperty), x))
                .OrderBy(x => x.Item1);

            BinningOptions options = new BinningOptions(
                BinningOutlierMode.CountOutliers,
                BinningIntervalType.InclusiveLowerBound,
                BinningExtremeValueMode.IncludeExtremeValues
            );

            DateTime begin = MinimumDate ?? items.First().date;
            DateTime end = MaximumDate ?? items.Last().date;

            if (end == begin)
                end = end.AddMinutes(15);

            var binBreaks = HistogramHelpers.CreateUniformBins(
                DateTimeAxis.ToDouble(begin),
                DateTimeAxis.ToDouble(end),
                50
            );

            IOrderedEnumerable<IGrouping<object, (DateTime date, object item)>> groups;
            if (ColorProperty == null)
                groups = items.GroupBy(x => (object) null).OrderByDescending(x => x.Count());
            else
                groups = items
                    .GroupBy(x => x.item.GetDeepPropertyValue(ColorProperty))
                    .OrderByDescending(x => x.Count());

            PriorityQueue<HistogramItem, int> bins = new PriorityQueue<HistogramItem, int>();
            Dictionary<string, OxyColor> colors = new Dictionary<string, OxyColor>();
            Dictionary<string, OxyColor> binColors = new Dictionary<string, OxyColor>();
            Dictionary<string, string> titles = new Dictionary<string, string>();
            Dictionary<string, string> binTitles = new Dictionary<string, string>();


            int count = 0;

            foreach (var group in groups)
            {
                OxyColor color = _histPlotModel.DefaultColors[count++ % _histPlotModel.DefaultColors.Count];
                colors[group.Key?.ToString()] = color;
                HistogramSeries s = new HistogramSeries();

                var dates = group
                .Select(x => x.date)
                .OrderBy(x => x);

                s.ItemsSource = HistogramHelpers.Collect(
                    dates.Select(x => DateTimeAxis.ToDouble(x)),
                    binBreaks,
                    options
                );

                s.ItemsSource
                    .OfType<HistogramItem>()
                    .ForEach(i => { i.Area = i.Area * dates.Count() / items.Count(); });

                s.Title = group.Key?.ToString() ?? string.Empty;
                s.ToolTip = s.Title;
                s.Tag = (group.Key, dates.Count());
                s.FillColor = color;

                titles[group.Key?.ToString()] = s.Title;

                if (_selected.Contains(group.Key))
                    s.Select();

                _histPlotModel.Series.Add(s);
            }

            foreach (var group in groups)
            {
                var dates = group
               .Select(x => x.date)
               .OrderBy(x => x);

                var realBins = HistogramHelpers.Collect(
                    dates.Select(x => DateTimeAxis.ToDouble(x)),
                    binBreaks,
                    options
                );

                OxyColor color = colors[group.Key?.ToString()];
                string title = titles[group.Key?.ToString()];

                foreach (HistogramItem bin in realBins)
                {

                    if (bin.Count == 0)
                        continue;

                    bin.Area = bin.Area * dates.Count() / items.Count();

                    binColors[bin.ToString()] = color;
                    binTitles[bin.ToString()] = title;

                    bins.Enqueue(bin, -bin.Count);

                }

            }

            int size = bins.Count;

            for (int i = 0; i < size; i++)
            {
                HistogramSeries s = new HistogramSeries();
                IList newBin = new List<HistogramItem>();
                HistogramItem curr = bins.Dequeue();
                newBin.Add(curr);
                s.ItemsSource = newBin;
                s.Title = binTitles[curr.ToString()];
                s.FillColor = binColors[curr.ToString()];
                s.RenderInLegend = false;
                _histPlotModel.Series.Add(s);
            }
        }

        public static readonly DependencyProperty ItemsSourceProp =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    null, FrameworkPropertyMetadataOptions.AffectsRender,
                    (o, v) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            if (t.Items != null)
                                t.Items.CollectionChanged -= t.OnItemsChange;

                            t.Items = CollectionViewSource.GetDefaultView(v.NewValue);

                            if (t.Items != null)
                                t.Items.CollectionChanged += t.OnItemsChange;

                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty MinimumDateProp =
            DependencyProperty.Register(
                nameof(MinimumDate),
                typeof(DateTime?),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    null,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            if (a.NewValue is DateTime d)
                            {
                                t._dateAxis.Reset();
                                t._dateAxis.Minimum = DateTimeAxis.ToDouble(d);
                            }
                            else
                            {
                                t._dateAxis.Minimum = double.NaN;
                            }

                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty MaximumDateProp =
            DependencyProperty.Register(
                nameof(MaximumDate),
                typeof(DateTime?),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    null,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            if (a.NewValue is DateTime d)
                            {
                                t._dateAxis.Maximum = DateTimeAxis.ToDouble(d);
                            }
                            else
                            {
                                t._dateAxis.Maximum = double.NaN;
                            }

                            t._dateAxis.Reset();
                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty DateTimePropertyProp =
            DependencyProperty.Register(
                nameof(DateTimeProperty),
                typeof(string),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    null,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty ColorPropertyProp =
            DependencyProperty.Register(
                nameof(ColorProperty),
                typeof(string),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    null,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            t._selected.Clear();
                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty TextColorProp =
            DependencyProperty.Register(
                nameof(TextColor),
                typeof(Color),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    Colors.Black,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty PlotAreaBorderColorProp =
            DependencyProperty.Register(
                nameof(PlotAreaBorderColor),
                typeof(Color),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    Colors.Black,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty TicklineColorProp =
            DependencyProperty.Register(
                nameof(TicklineColor),
                typeof(Color),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    Colors.Black,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            t.Update();
                        }
                    }
                )
            );
    }

    internal class TimeSeriesHistogramTrackerTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is TrackerHitResult result && values[1] is TimeSeriesHistogram t)
            {
                IList<(string, string)> trakerText = result.Text
                    .Split('\n')
                    .Select(s => s.Split(' '))
                    .Select(s => (s[0], s[1]))
                    .Where(s => s.Item1 != "Area:" && s.Item1 != "Value:")
                    .ToList();

                trakerText[0] = (trakerText[0].Item1, DateTimeAxis.ToDateTime(double.Parse(trakerText[0].Item2)).ToString());
                trakerText[1] = (trakerText[1].Item1, DateTimeAxis.ToDateTime(double.Parse(trakerText[1].Item2)).ToString());

                return result.Series.Title + "\n" + trakerText
                    .Select(i => i.Item1 + ' ' + i.Item2)
                    .Aggregate(string.Empty, (string a, string i) => a + '\n' + i)
                    .Trim();
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

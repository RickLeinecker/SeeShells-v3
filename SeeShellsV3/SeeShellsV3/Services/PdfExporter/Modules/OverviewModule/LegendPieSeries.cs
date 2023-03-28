using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;

namespace SeeShellsV3.Services
{
    public class LegendPieSeries : OxyPlot.Series.PieSeries
    {
        private int SliceIndex { get; set; }

        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            var xmid = (legendBox.Left + legendBox.Right) / 2;
            var ymid = (legendBox.Top + legendBox.Bottom) / 2;
            var height = (legendBox.Bottom - legendBox.Top) * 0.8;
            var width = height;
            rc.DrawRectangle(
                new OxyRect(xmid - (0.5 * width), ymid - (0.5 * height), width, height),
                Slices[SliceIndex].ActualFillColor,
                OxyColors.Transparent,
                this.StrokeThickness,
                this.EdgeRenderingMode);
        }

        public static void SetSeries(PieSeries pieSeries, PlotModel model)
        {
            model.Series.Clear();
            var total = pieSeries.Slices.Sum(s => s.Value);
            for (int i = 0; i < pieSeries.Slices.Count; i++)
            {
                PieSlice slice = pieSeries.Slices[i];
                LegendPieSeries sliceSeries = new LegendPieSeries()
                {
                    AngleIncrement = pieSeries.AngleIncrement,
                    AngleSpan = pieSeries.AngleSpan,
                    AreInsideLabelsAngled = pieSeries.AreInsideLabelsAngled,
                    Background = pieSeries.Background,
                    ColorField = pieSeries.ColorField,
                    Diameter = pieSeries.Diameter,
                    EdgeRenderingMode = pieSeries.EdgeRenderingMode,
                    ExplodedDistance = pieSeries.ExplodedDistance,
                    Font = pieSeries.Font,
                    FontSize = pieSeries.FontSize,
                    FontWeight = pieSeries.FontWeight,
                    SliceIndex = i,
                    InnerDiameter = pieSeries.InnerDiameter,
                    InsideLabelColor = pieSeries.InsideLabelColor,
                    InsideLabelFormat = pieSeries.InsideLabelFormat,
                    InsideLabelPosition = pieSeries.InsideLabelPosition,
                    IsExplodedField = pieSeries.IsExplodedField,
                    IsVisible = pieSeries.IsVisible,
                    ItemsSource = pieSeries.ItemsSource,
                    LabelField = pieSeries.LabelField,
                    LegendFormat = pieSeries.LegendFormat,
                    LegendKey = pieSeries.LegendKey,
                    OutsideLabelFormat = pieSeries.OutsideLabelFormat,
                    RenderInLegend = pieSeries.RenderInLegend,
                    Selectable = pieSeries.Selectable,
                    SelectionMode = pieSeries.SelectionMode,
                    SeriesGroupName = pieSeries.SeriesGroupName,
                    StartAngle = pieSeries.StartAngle,
                    Stroke = pieSeries.Stroke,
                    StrokeThickness = pieSeries.StrokeThickness,
                    Tag = pieSeries.Tag,
                    TextColor = pieSeries.TextColor,
                    TickDistance = pieSeries.TickDistance,
                    TickHorizontalLength = pieSeries.TickHorizontalLength,
                    TickLabelDistance = pieSeries.TickLabelDistance,
                    Title = string.Format(pieSeries.LegendFormat ?? "{0} - {1} ({3:F2}%)",
                        slice.Label,
                        slice.Value,
                        total,
                        slice.Value / total * 100),
                    TickRadialLength = pieSeries.TickRadialLength,
                    ToolTip = pieSeries.ToolTip,
                    TrackerFormatString = pieSeries.TrackerFormatString,
                    TrackerKey = pieSeries.TrackerKey,
                    ValueField = pieSeries.ValueField,
                };
                sliceSeries.total = total;
                for (int j = 0; j < pieSeries.Slices.Count; j++)
                {
                    PieSlice pieSlice = pieSeries.Slices[j];
                    sliceSeries.Slices.Add(new PieSlice(pieSlice.Label, pieSlice.Value)
                    {
                        Fill = j != i ? OxyColors.Transparent : pieSlice.Fill,
                        IsExploded = pieSlice.IsExploded,
                    });
                }
                model.Series.Add(sliceSeries);
            }
            ((IPlotModel)model).Update(false);
        }
        private double total;
    }
}

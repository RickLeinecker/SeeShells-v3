using OxyPlot;
using SeeShellsV3.Services;
using SeeShellsV3.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using SeeShellsV3.Factories;
using Unity;
using MahApps.Metro.Controls;

namespace SeeShellsV3.Services
{
    public class PaletteManager : IPaletteManager
    {
        public IPaletteManager.OxyPaletteWrap CurrentPalette { get; set; }
        public Collection<IPaletteManager.OxyPaletteWrap> Palettes { get; init; }
        public Collection<string> PaletteNames { get; init; }
        private TimeSeriesHistogram Histo { get; set; }

        public PaletteManager()
        {
            PaletteNames = new Collection<string>((IList<string>)Application.Current.Resources["paletteNames"]);
            Palettes = new Collection<IPaletteManager.OxyPaletteWrap>();
            LoadPalettes();
            CurrentPalette = Palettes[0];
        }

        private void LoadPalettes()
        {
            List<Array> palettes = new List<Array>((IEnumerable<Array>)Application.Current.Resources["palettes"]);
            int i = 0;
            foreach (Array paletteFromXAML in palettes)
            {
                List<OxyColor> palette = new();
                List<Color> colors = new((IEnumerable<Color>)paletteFromXAML);
                foreach (Color color in colors)
                {
                    palette.Add(
                        OxyColor.FromRgb(
                            color.R,
                            color.G,
                            color.B
                            )
                        );
                }
                IPaletteManager.OxyPaletteWrap newPalette = new IPaletteManager.OxyPaletteWrap() { Name = PaletteNames[i++], Palette = new OxyPalette(palette) };
                Palettes.Add(newPalette);
                Debug.Print("Loaded " + PaletteNames[i-1]);
            }
        }

        public IPaletteManager.OxyPaletteWrap GetPalette(string paletteName)
        {
            return Palettes[PaletteNames.IndexOf(paletteName)];
        }

        public void PaletteChangeHandler(string paletteName)
        {
            // This null check is added for performance. Can't get histogram reference in constructor due to dependency resolution order on startup. Further investigation called for
            if (this.Histo is null)
            {
                TimelineView timeline = Application.Current.Windows.OfType<MainWindow>().ElementAt(0).Timeline;
                Histo = timeline.FindChild<TimeSeriesHistogram>();
            }
            CurrentPalette = GetPalette(paletteName);
            Histo.HistPlotModel_setColors(CurrentPalette.Palette);
            Debug.WriteLine("Current Palette: " + CurrentPalette.Name);
        }
    }
}

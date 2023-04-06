using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV3.Services
{
    public interface IPaletteManager
    {
        public readonly struct OxyPaletteWrap
        {
            public string Name { get; init; }
            public OxyPalette Palette { get; init; }
        }
        public OxyPaletteWrap CurrentPalette { get; set; }
        public Collection<OxyPaletteWrap> Palettes { get; init; }
        public Collection<string> PaletteNames { get; init; }
        public OxyPaletteWrap GetPalette(string paletteName);
        public void PaletteChangeHandler(string paletteName);
    }
}

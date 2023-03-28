using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SeeShellsV3.Repositories;
using SeeShellsV3.UI;
using Unity;
using WpfHexaEditor;

namespace SeeShellsV3.Services
{
    public class HexViewerModule : IPdfModule
    {
        public string Name => "Hex View";
        [Unity.Dependency]
        public ISelected Selected { get; set; }
        private UserControl HexViewer { get; set; }
        public UIElement Render()
        {
            RenderTargetBitmap bm = new RenderTargetBitmap((int)HexViewer.ActualWidth,
                                                           (int)HexViewer.ActualHeight,
                                                           96,
                                                           96,
                                                           PixelFormats.Pbgra32);
            bm.Render(HexViewer);

            Image img = new Image();
            img.Source = bm;
            img.Height = bm.Height;
            img.Width = bm.Width;

            StackPanel panel = new StackPanel();
            panel.Children.Add(img);
            return panel as UIElement;
        }

        public FrameworkElement View()
        {
            string view = @"
            <UserControl  Name=""HexViewer"">
                <UserControl.Resources>
                    <Style TargetType=""{x:Type hex:HexEditor}"">
                        <Setter Property=""BytePerLine"" Value=""8"" />
                        <Setter Property=""StatusBarVisibility"" Value=""Hidden"" />
                    </Style>
                    <local:StreamConverter x:Key=""StreamConverter"" />
                </UserControl.Resources>
                <hex:HexEditor Stream=""{Binding Selected.CurrentData, Converter={StaticResource StreamConverter}}""
                       ReadOnlyMode=""True"" BorderThickness=""0"" Focusable=""False"" MaxHeight=""300""/>
			</UserControl>";

            ParserContext context = new ParserContext();
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("d", "http://schemas.microsoft.com/expression/blend/2008");
            context.XmlnsDictionary.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            context.XmlnsDictionary.Add("oxy", "http://oxyplot.org/wpf");
            context.XmlnsDictionary.Add("mah", "http://metro.mahapps.com/winfx/xaml/controls");

            HexView hexView = new HexView();
            Type type = hexView.GetType();
            context.XamlTypeMapper = new XamlTypeMapper(new string[0]);
            context.XamlTypeMapper.AddMappingProcessingInstruction("local", type.Namespace, type.Assembly.FullName);
            context.XmlnsDictionary.Add("local", "local");

            context.XamlTypeMapper.AddMappingProcessingInstruction("hex", "WpfHexaEditor", "WPFHexaEditor, Version=2.1.6.0, Culture=neutral, PublicKeyToken=null");
            context.XmlnsDictionary.Add("hex", "hex");

            FrameworkElement e = XamlReader.Parse(view, context) as FrameworkElement;

            e.DataContext = this;

            var hex = e.FindName("HexViewer") as UserControl;

            var converter = new StreamConverter();

            var test2 = new HexView();

            var test = new HexEditor();
            test.Stream = (System.IO.Stream) converter.Convert(Selected.CurrentData, null, null, System.Globalization.CultureInfo.CurrentCulture);
            test.ReadOnlyMode = true;
            test.Focusable = false;

            HexViewer = hex;


            return e;
        }

        public IPdfModule Clone()
        {
            return MemberwiseClone() as IPdfModule;
        }
    }
}
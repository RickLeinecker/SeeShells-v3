using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using SeeShellsV3.Data;
using SeeShellsV3.Repositories;
using SeeShellsV3.UI;
using Unity;
using WpfHexaEditor;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SeeShellsV3.Services
{
    public class HexViewerModule : IPdfModule
    {
        public string Name => "Hex View";
        [Unity.Dependency]
        public ISelected Selected { get; set; }
        private StackPanel HexViewer { get; set; }

        public ObservableCollection<ShellItem> Items { get; set; }

        [Unity.Dependency]
        public IReportEventCollection ReportEvents { get; set; }
        public UIElement Render()
        {
            RenderTargetBitmap bm = new RenderTargetBitmap((int)HexViewer.ActualWidth - 10,
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
            string test = "";
            string tempView = "";
            Selected.Items = new ObservableCollection<ShellItem>();
            Items = new ObservableCollection<ShellItem>();

            int index = 0;

            foreach (IShellEvent ev in ReportEvents.SelectedEvents)
            {
                foreach (ShellItem item in ev.Evidence)
                {
                    Selected.Items.Add(item);
                    Items.Add(item);

                    test = @"<hex:HexEditor Name = " + "\"HexEditor" + index + "\"" + @" Stream=""{Binding Items[" + index + "]" + @", Converter={StaticResource StreamConverter}}"" 
                            ReadOnlyMode = ""True"" BorderThickness = ""0"" Focusable = ""False"" MaxHeight = ""400"" />";

                    string temp = @"
                    <TextBlock Background = ""Silver"" FontSize=""30"" FontWeight=""SemiBold"">
                        Hex - " + ev.Description +@"
                    </TextBlock>
                    <UserControl>
                        <UserControl.Resources>
                            <Style TargetType=""{x:Type hex:HexEditor}"">
                                <Setter Property=""BytePerLine"" Value=""12"" />
                                <Setter Property=""StatusBarVisibility"" Value=""Hidden"" />
                            </Style>
                            <local:HexConverter x:Key=""HexConverter"" />
                            <local:StreamConverter x:Key=""StreamConverter""/>
                        </UserControl.Resources>
                           " + test + @"
                    </UserControl>";

                    tempView += temp;
                    index++;
                }
            }

            string view = "<StackPanel Name =\"HexViewerMod\">" + tempView + "</StackPanel>"; 




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

            StackPanel testObj = e.FindName("HexViewerMod") as StackPanel;
            HexViewer = testObj;


            return e;
        }

        public IPdfModule Clone()
        {
            return MemberwiseClone() as IPdfModule;
        }
    }
}
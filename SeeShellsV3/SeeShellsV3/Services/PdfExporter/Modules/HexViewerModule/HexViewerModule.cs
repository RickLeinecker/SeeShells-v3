using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SeeShellsV3.Data;
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

        public ObservableCollection<ReportEditor> Editors { get; set; }
        public ObservableCollection<ShellItem> Items { get; set; }

        [Unity.Dependency]
        public IReportEventCollection ReportEvents { get; set; }
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
            System.Diagnostics.Debug.WriteLine(typeof(FrameworkElement).IsAssignableFrom(typeof(HexEditor)));
            string view = @"
            <UserControl  Name=""HexViewer"">
                <UserControl.Resources>
                    <Style TargetType=""{x:Type hex:HexEditor}"">
                        <Setter Property=""BytePerLine"" Value=""8"" />
                        <Setter Property=""StatusBarVisibility"" Value=""Hidden"" />
                    </Style>
                    <local:HexConverter x:Key=""HexConverter"" />
                    <local:StreamConverter x:Key=""StreamConverter""/>
                </UserControl.Resources>
                <ItemsControl ItemsSource=""{Binding Items}"">
                    <ItemsControl.ItemTemplate>
                        <DataTemplate>
                            <hex:HexEditor Name=""HexEditor"" Stream=""{Binding Converter={StaticResource StreamConverter}}""
                                    ReadOnlyMode=""True"" BorderThickness=""0"" Focusable=""False"" MaxHeight=""400""/>
                        </DataTemplate>
                    </ItemsControl.ItemTemplate>
                </ItemsControl>
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

            Editors = new ObservableCollection<ReportEditor>();
            Items = new ObservableCollection<ShellItem>();

            foreach (IShellEvent ev in ReportEvents.SelectedEvents)
            {
                foreach (ShellItem item in ev.Evidence)
                {
                    ReportEditor editor = new ReportEditor();

                    Items.Add(item);

                    editor.name = item.Description;
                    editor.hexEditor = new HexEditor();

                    StreamConverter conv = new StreamConverter();

                    editor.hexEditor.Stream = (System.IO.Stream)conv.Convert(item, null, null, System.Globalization.CultureInfo.CurrentCulture);
                    editor.hexEditor.ReadOnlyMode = true;
                    editor.hexEditor.BorderThickness = new Thickness(0);
                    editor.hexEditor.Focusable = false;
                    editor.hexEditor.MaxHeight = 500;

                    Editors.Add(editor);
                }
            }

            System.Diagnostics.Debug.WriteLine("Getting here at this time");

            return e;
        }

        public IPdfModule Clone()
        {
            return MemberwiseClone() as IPdfModule;
        }
    }
}
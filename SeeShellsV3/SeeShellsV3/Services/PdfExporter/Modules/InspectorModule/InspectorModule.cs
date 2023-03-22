using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using SeeShellsV3.Data;
using SeeShellsV3.Repositories;
using SeeShellsV3.UI;
using Unity;

namespace SeeShellsV3.Services
{
    public class InspectorModule: IPdfModule
    {
        public string Name => "Inspector";
		[Dependency] public IReportEventCollection ReportEvents { get; set; }
        private FrameworkElement Inspector { get; set; }
        public UIElement Render()
        {
            if (Inspector == null)
                return null;
            string s = XamlWriter.Save(Inspector);
            StringReader sr = new StringReader(s);
            XmlReader reader = XmlTextReader.Create(sr, new XmlReaderSettings());
            FrameworkElement e = (FrameworkElement)XamlReader.Load(reader);
            return e.FindName("RichTextBox") as UIElement;
        }

        public FrameworkElement View()
        {
            FlowDocument fd = new FlowDocument();
            BuildFlowDocument(fd);

            string fdXaml = XamlWriter.Save(fd);

            string view = @"
			<StackPanel>
				<ToolBar>
					<Button Command=""ApplicationCommands.Cut"" ToolTip=""Cut"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8C6;""/>
					<Button Command = ""ApplicationCommands.Copy"" ToolTip = ""Copy"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8C8;""/>
					<Button Command = ""ApplicationCommands.Paste"" ToolTip = ""Paste"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE77F;""/>
					<ToggleButton Command = ""EditingCommands.ToggleBold"" ToolTip = ""Bold"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8DD;""/>
					<ToggleButton Command = ""EditingCommands.ToggleItalic"" ToolTip = ""Italic""  FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8DB;""/>
					<ToggleButton Command = ""EditingCommands.ToggleUnderline"" ToolTip = ""Underline"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8DC;""/>
					<Button Command = ""EditingCommands.IncreaseFontSize"" ToolTip = ""Grow Font"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E8;""/>
					<Button Command = ""EditingCommands.DecreaseFontSize"" ToolTip = ""Shrink Font"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E7;""/>
					<Button Command = ""EditingCommands.ToggleBullets"" ToolTip = ""Bullets"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8FD;""/>
					<Button Command = ""EditingCommands.AlignLeft"" ToolTip = ""Align Left"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E4;"" />
					<Button Command = ""EditingCommands.AlignCenter"" ToolTip = ""Align Center"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E3;""/>
					<Button Command = ""EditingCommands.AlignRight"" ToolTip = ""Align Right"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E2;""/>
					<Button Command = ""EditingCommands.AlignJustify"" ToolTip = ""Align Justify"">
						<Grid>
							<TextBlock FontFamily=""Segoe MDL2 Assets"" Text=""&#xE8E4;"" VerticalAlignment=""Center"" TextAlignment=""Center""/>
							<TextBlock FontFamily=""Segoe MDL2 Assets"" Text=""&#xE8E2;"" VerticalAlignment=""Center"" TextAlignment=""Center""/>
						</Grid>
					</Button>			   
				</ToolBar>
				<RichTextBox Name=""RichTextBox"" BorderBrush=""Transparent"" CaretBrush=""Black"" Foreground=""Black"" Background=""White"" AcceptsTab=""True"">
					" + fdXaml + @"
				</RichTextBox>
			</StackPanel>";

            ParserContext context = new ParserContext();
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("d", "http://schemas.microsoft.com/expression/blend/2008");
            context.XmlnsDictionary.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");

            InspectorView inspector = new InspectorView();
            Type type = inspector.GetType();
            context.XamlTypeMapper = new XamlTypeMapper(new string[0]);
            context.XamlTypeMapper.AddMappingProcessingInstruction("local", type.Namespace, type.Assembly.FullName);
            context.XmlnsDictionary.Add("local", "local");

            FrameworkElement e = XamlReader.Parse(view, context) as FrameworkElement;

            e.DataContext = this;

            var insp = e.FindName("RichTextBox") as RichTextBox;

            Inspector = insp;

            return e;
        }

        public IPdfModule Clone()
        {
            return MemberwiseClone() as IPdfModule;
        }

        public void BuildFlowDocument(FlowDocument fd)
        {
            foreach (IShellEvent shell in ReportEvents.SelectedEvents)
            {
                IShellItem item = shell.Evidence.First();
                // Construct a header for this module
                Paragraph header = new Paragraph(new Run($"Inspector - {shell.Description}"));
                header.Background = Brushes.Silver;
                header.FontSize = 24;
                header.FontWeight = FontWeights.Bold;
                fd.Blocks.Add(header);

                // ----Formatting below follows pattern from the InspectorView----

                // Show long description of the Shell Event
                fd.Blocks.Add(new Paragraph(new Run(shell?.LongDescription)));

                // Create overview list of the Shell Event
                List identifiers = new List();
                identifiers.ListItems.Add(new ListItem(new Paragraph(new Run($"Description: {shell.Description}"))));
                identifiers.ListItems.Add(new ListItem(new Paragraph(new Run($"Event Time: {shell.TimeStamp}"))));
                identifiers.ListItems.Add(new ListItem(new Paragraph(new Run($"User: {shell.User}"))));
                identifiers.ListItems.Add(new ListItem(new Paragraph(new Run($"Location Name: {shell.Place.Name}"))));
                identifiers.ListItems.Add(new ListItem(new Paragraph(new Run($"Location Path: {shell.Place.PathName}"))));
                fd.Blocks.Add(identifiers);

                // Add Shellbag Evidence header
                Paragraph informationHeader = new Paragraph(new Run("Shellbag Information"));
                informationHeader.FontSize = 20;
                informationHeader.FontWeight = FontWeights.Bold;
                fd.Blocks.Add(informationHeader);

                // Create list storing Shellbag Information
                List bagInfo = new List();
                bagInfo.ListItems.Add(new ListItem(new Paragraph(new Run($"Description: {item.Description}"))));
                bagInfo.ListItems.Add(new ListItem(new Paragraph(new Run($"Type: {item.TypeName}"))));
                bagInfo.ListItems.Add(new ListItem(new Paragraph(new Run($"Subtype: {item.SubtypeName}"))));
                bagInfo.ListItems.Add(new ListItem(new Paragraph(new Run($"Location Name: {item.Place.Name}"))));
                bagInfo.ListItems.Add(new ListItem(new Paragraph(new Run($"Location Path: {item.Place.PathName}"))));
                bagInfo.ListItems.Add(new ListItem(new Paragraph(new Run($"Registry Path: {item.RegistryHive.Path}"))));
                bagInfo.ListItems.Add(new ListItem(new Paragraph(new Run($"Registry Owner: {item.RegistryHive.User}"))));
                bagInfo.ListItems.Add(new ListItem(new Paragraph(new Run($"Last Registry Write Date: {item.LastRegistryWriteDate}"))));
                fd.Blocks.Add(bagInfo);

                // Create Shellbag Fields header
                Paragraph fieldsHeader = new Paragraph(new Run("Shellbag Fields"));
                fieldsHeader.FontSize = 20;
                fieldsHeader.FontWeight = FontWeights.Bold;
                fd.Blocks.Add(fieldsHeader);

                // Create list storing Fields
                List fields = new List();
                foreach (KeyValuePair<string, object> field in item.Fields)
                {
                    fields.ListItems.Add(new ListItem(new Paragraph(new Run($"{field.Key}: {field.Value}"))));
                }
                fd.Blocks.Add(fields);
            }
        }
    }
}
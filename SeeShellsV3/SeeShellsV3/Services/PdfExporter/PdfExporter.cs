﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Unity;
using System.Windows.Documents;
using SeeShellsV3.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using SeeShellsV3.Repositories;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Imaging;
using NLog.Filters;

namespace SeeShellsV3.Services
{
    public class PdfExporter : IPdfExporter
    {
		public IEnumerable<IPdfModule> modules;
		public Dictionary<string, IPdfModule> moduleNames;

        [Dependency] 
        IReportEventCollection ReportEvents { get; set; }
        public PdfExporter([Dependency] IUnityContainer container)
		{
			// construct an instance of each implementation of IPdfModule.
			// these instances will be used as blind templates for constructing
			// the report UI in the export window
			modules = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => typeof(IPdfModule).IsAssignableFrom(p))
				.Where(q => q.IsClass)
				.Select(r => (IPdfModule)container.Resolve(r));

			moduleNames = new Dictionary<string, IPdfModule>();
			foreach (IPdfModule module in modules)
			{
				moduleNames.Add(module.Name, module);
			}
		}

		public void Export(IEnumerable<IPdfModule> moduleList)
		{

			PrintDialog pd = new PrintDialog();
			if ((pd.ShowDialog() == true))
			{
				FlowDocument fd = new FlowDocument();
				fd.PagePadding = new Thickness(48);
				fd.ColumnGap = 0;
				fd.ColumnWidth = pd.PrintableAreaWidth;
				fd.IsColumnWidthFlexible = true;
				foreach (IPdfModule module in moduleList)
				{
					BlockUIContainer bc = new BlockUIContainer();

                    // Renders the module to the FlowDocument, using special rendering methods where necessary.
                    switch (module.GetType().Name)
                    {
                        case "RTFModule":
                        case "HeaderModule":
                        case "InspectorModule":
                            RenderTextBox(module, fd);
                            break;
                        case "ShellbagTableModule":
                            RenderShellbagTable(module, fd);
                            break;
                        case "HexViewerModule":
                            RenderHex(module, fd);
                            break;
                        default:
                            bc.Child = module.Render();
                            fd.Blocks.Add(bc);
                            break;
                    }
                }

				pd.PrintDocument(((IDocumentPaginatorSource)fd).DocumentPaginator, "SeeShellsReport");
			}
        }

        /// <summary>
        /// Renders modules that are textboxes or similar to textboxes.
        /// </summary>
        /// <param name="module">The <see cref="IPdfModule"/> to be rendered.</param>
        /// <param name="fd">The <see cref="FlowDocument"/> to that <see cref="module"/> will be rendered to.</param>
        public void RenderTextBox(IPdfModule module, FlowDocument fd)
        {
            List<Block> fdBlocks = new List<Block>((module.Render() as RichTextBox).Document.Blocks);
            foreach (Block block in fdBlocks)
            {
                fd.Blocks.Add(block);
            }
        }

        public void RenderHex(IPdfModule module, FlowDocument fd)
        {
            int index = 0;

            HexViewerModule mod = module as HexViewerModule;

            foreach (IShellEvent ev in mod.ReportEvents.SelectedEvents)
            {
                int count = mod.ReportEvents.SelectedEvents.Count;
                foreach (ShellItem item in ev.Evidence)
                {

                    BlockUIContainer bc = new BlockUIContainer();

                    //System.Diagnostics.Debug.WriteLine(mod.Render(index));

                    StackPanel panel = new StackPanel();

                    TextBlock block = new TextBlock();

                    block.Background = Brushes.Silver;
                    block.FontSize= 18;
                    block.FontWeight = FontWeights.SemiBold; 
                    block.Text = "Hex - " + ev.Description;
                    block.Margin = new Thickness(5);
                    panel.Children.Add(block);
                    panel.Children.Add(mod.Render(index));


                    bc.Child = panel;

                    fd.Blocks.Add(bc);


                    index++;

                }
            }




        }

        /// <summary>
        /// Renders the ShellbagTable module.
        /// </summary>
        /// <param name="module">The <see cref="IPdfModule"/> to be rendered.</param>
        /// <param name="fd">The <see cref="FlowDocument"/> to that <see cref="module"/> will be rendered to.</param>
        public void RenderShellbagTable(IPdfModule module, FlowDocument fd)
        {
            Table table = new Table();
            DataGrid data = (module.Render() as DataGrid);

            var headerList = data.Columns.Select(e => e.Header.ToString()).ToList();

            TableColumn num = new TableColumn();
            num.Width = new GridLength(20);
            table.Columns.Add(num);

            for (int j = 0; j < headerList.Count; j++)
            {
                TableColumn c = new TableColumn();
                c.Width = new GridLength(175);
                table.Columns.Add(c);
            }

            table.RowGroups.Add(new TableRowGroup());
            table.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            TableRow currentRow = table.RowGroups[0].Rows[0];

            // Global formatting for the title row.
            currentRow.Background = Brushes.Silver;
            currentRow.FontSize = 24;
            currentRow.FontWeight = FontWeights.Bold;

            // Add the header row with content,
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Filtered Shellbag Events"))));
            // and set the row to span all 6 columns.
            currentRow.Cells[0].ColumnSpan = 6;

            // Add the second (header) row.
            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[1];

            // Global formatting for the header row.
            currentRow.FontSize = 18;
            currentRow.FontWeight = FontWeights.Bold;

            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("#"))));

            for (int j = 0; j < headerList.Count; j++)
            {
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(headerList[j]))));
            }

            List<IShellEvent> list = data.SelectedItems.OfType<IShellEvent>().ToList();
            if (list.Count == 0)
            {
                list = data.Items.OfType<IShellEvent>().ToList();
            }
            int k = 2;
            foreach (IShellEvent shell in list)
            {
                if (k == 32)
                    break;

                table.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table.RowGroups[0].Rows[k];
                currentRow.FontSize = 12;
                currentRow.FontWeight = FontWeights.Normal;

                currentRow.Cells.Add(new TableCell(new Paragraph(new Run((k - 1).ToString()))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(shell.TimeStamp.ToString()))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(shell.Place.Name))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(shell.TypeName))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(shell.User.Name))));
                k++;
            }

            fd.Blocks.Add(table);
        }

        /// <summary>
        /// Renders the InspectorView module.
        /// </summary>
        /// <param name="module">The <see cref="IPdfModule"/> to be rendered.</param>
        /// <param name="fd">The <see cref="FlowDocument"/> to that <see cref="module"/> will be rendered to.</param>
        public void RenderInspector(IPdfModule module, FlowDocument fd)
        {
            InspectorModule inspector = module as InspectorModule;
            inspector.BuildFlowDocument(fd);
        }
	}
}

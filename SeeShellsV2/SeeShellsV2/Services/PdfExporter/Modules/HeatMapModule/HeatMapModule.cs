﻿using SeeShellsV2.Data;
using SeeShellsV2.Repositories;
using SeeShellsV2.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml;
using Unity;

namespace SeeShellsV2.Services
{
	class HeatMapModule : IPdfModule
	{
		public string Name => "HeatMap";

		public FrameworkElement HeatMap { get; set; }

		//[Dependency]
		//public ITimelineViewAltVM vm {get; set;}

		[Dependency]
		public IShellEventCollection ShellEvents { get; set; }


		public ICollectionView FilteredShellEvents => ShellEvents.FilteredView;

		public HeatMapModule([Dependency] IShellEventCollection shellEvents)
		{
			ShellEvents = shellEvents;
		}

		public IPdfModule Clone()
		{
			// we only really need a basic copy here to let the exporter
			// use this object as a template to make more objects
			return MemberwiseClone() as IPdfModule;
		}

		public UIElement Render()
		{
			if (HeatMap == null)
				return null;
			var plot = (HeatMap as CalendarHeatMap).HeatMapPlot;
			plot.Title = (HeatMap as CalendarHeatMap).Year.ToString();
			var s = plot.ToBitmap();
			Image image = new Image();
			image.Source = s;
			image.Width = s.Width;
			image.Height = s.Height;
			//StringReader sr = new StringReader(s);
			//XmlReader reader = XmlTextReader.Create(sr, new XmlReaderSettings());
			//FrameworkElement e = (FrameworkElement)XamlReader.Load(reader);

			return image;
		}

		public FrameworkElement View()
		{

			// the view we will display for the user to configure this object
			string view = @"
			<Grid>
			<local:CalendarHeatMap x:Name = ""Heatmap""  Height=""800"" Width=""1000""
									Background=""White""
								   ColorAxisTitle = ""User Action Frequency""
								   ItemsSource = ""{Binding FilteredShellEvents}""
								   SelectionBegin = ""{Binding DateSelectionBegin}""
								   SelectionEnd = ""{Binding DateSelectionEnd}""
								   DateTimeProperty = ""TimeStamp"" Orientation = ""Vertical"" />
			</Grid>";


			//// add WPF namespaces to a parser context so we can parse WPF tags like StackPanel
			ParserContext context = new ParserContext();
			context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
			context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
			context.XmlnsDictionary.Add("d", "http://schemas.microsoft.com/expression/blend/2008");
			context.XmlnsDictionary.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			context.XmlnsDictionary.Add("oxy", "http://oxyplot.org/wpf");
			context.XmlnsDictionary.Add("mah", "http://metro.mahapps.com/winfx/xaml/controls");

			CalendarHeatMap heat = new CalendarHeatMap();
			Type type = heat.GetType();
			context.XamlTypeMapper = new XamlTypeMapper(new string[0]);
			context.XamlTypeMapper.AddMappingProcessingInstruction("local", type.Namespace, type.Assembly.FullName);
			context.XmlnsDictionary.Add("local", "local");

			//// construct the view using an XAML parser
			FrameworkElement e = XamlReader.Parse(view, context) as FrameworkElement;

			//// assign this object as the data context so the view can get/set module properties
			e.DataContext = this;

			//// extract the elements from the view. this is only necessary if we
			//// want to use these elements *directly* to do work inside this class.
			//// **any data access that can be done with xaml bindings should be done with xaml bindings**
			var hm = e.FindName("Heatmap") as CalendarHeatMap;

			//// hook up event listeners

			//// save RTB element for later
			HeatMap = hm;

			return e;
		}
	}
}
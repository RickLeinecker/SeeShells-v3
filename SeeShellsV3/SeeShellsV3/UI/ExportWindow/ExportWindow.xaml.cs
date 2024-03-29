﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Unity;
using SeeShellsV3.Repositories;
using System.Collections;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using SeeShellsV3.Services;
using System.Security.Policy;
using MahApps.Metro.Controls;

namespace SeeShellsV3.UI
{
    public interface IExportWindowVM : IViewModel
    {
        ObservableCollection<IPdfModule> moduleList { get; }
        ObservableCollection<string> moduleSelector { get; }
        public void Export_PDF();
        public bool HasSelectedEvents();
		void Remove(IPdfModule sender);
		void MoveDown(IPdfModule pdfModule);
		void MoveUp(IPdfModule pdfModule);
		void AddModule(string module);


	}

    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : Window, IWindow
    {
        [Dependency]
        public IExportWindowVM ViewModel { get => DataContext as IExportWindowVM; set => DataContext = value; }

		[Dependency]
		public ISelected Selected { get; set; }
		public string LoadStatus { get; set; }
		public string StatusVisibility { get; set; }
        public ExportWindow()
        {
            InitializeComponent();
        }

		private void Export_Click(object sender, RoutedEventArgs e)
		{
			// do file save here in the view (presentation logic)
			//SaveFileDialog svg = new SaveFileDialog();
			//svg.Filter = "XPS Documents (.xps)|*.xps";
			//svg.DefaultExt = ".xps";
			//svg.FileName = "SeeShellsReport";
			//if (svg.ShowDialog() == true)
			//	ViewModel.Export_PDF(svg.FileName);
			//svg.Filter = "PDF Document (*.pdf)|*.pdf";
			//svg.DefaultExt = ".pdf";
			//svg.FileName = "SeeShellsReport";
			//if (svg.ShowDialog() == true)
            ViewModel.Export_PDF();
		}


		private void Remove_Click(object sender, RoutedEventArgs e)
		{
            ViewModel.Remove((sender as Button).DataContext as IPdfModule);
		}

		private void MoveUp_Click(object sender, RoutedEventArgs e)
		{
            ViewModel.MoveUp((sender as Button).DataContext as IPdfModule);
        }

		private void MoveDown_Click(object sender, RoutedEventArgs e)
		{
            ViewModel.MoveDown((sender as Button).DataContext as IPdfModule);
        }

		private void Add_Module_Click(object sender, RoutedEventArgs e)
		{
			Mouse.OverrideCursor = Cursors.Wait;

            if (moduleSelector.SelectedIndex != 0)
			{
                ViewModel.AddModule(moduleSelector.SelectedItem as string);
            }
		}

        public void LoadHandler(object sender, EventArgs args)
		{
			Mouse.OverrideCursor = null;
			SelectedEvents.Visibility = ViewModel.HasSelectedEvents() ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}

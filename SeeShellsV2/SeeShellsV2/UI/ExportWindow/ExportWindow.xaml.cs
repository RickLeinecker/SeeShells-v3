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
using SeeShellsV2.Repositories;

namespace SeeShellsV2.UI
{
    public interface IExportWindowVM : IViewModel
    { 
        
    }

    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : Window, IWindow
    {
        [Dependency]
        public IExportWindowVM ViewModel { set => DataContext = value; }
        public ExportWindow()
        {
            InitializeComponent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using SeeShellsV3.Data;
using SeeShellsV3.Services;
using Unity;

namespace SeeShellsV3.UI
{
    public interface ITimezoneWindowVM : IViewModel
    {
        ITimezoneManager TimezoneManager { get; }
        Timezone SelectedTimeZone { get; }
        void SelectTimezone(Timezone zone);
        bool UpdateTimezone();
    }

    /// <summary>
    /// Interaction logic for TimezoneWindow.xaml
    /// </summary>
    public partial class TimezoneWindow : Window, IWindow
    {
        [Dependency]
        public ITimezoneWindowVM ViewModel {get => DataContext as ITimezoneWindowVM; set => DataContext = value; }

        public TimezoneWindow()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectedSellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Timezone selected = (sender as DataGrid).CurrentCell.Item as Timezone;

            ViewModel.SelectTimezone(selected);
        }

        private void UpdateTimezone_Click(object sender, RoutedEventArgs e)
        {
            bool successful = ViewModel.UpdateTimezone();

            if (successful)
            {
                this.Close();
            }
        }
    }
}

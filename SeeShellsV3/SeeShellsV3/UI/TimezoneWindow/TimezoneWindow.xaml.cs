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
        TimezoneManager TimezoneManager { get; }
        public Collection<Timezone> supportedTimezones { get; }
        string Keyword { get; set; }
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
    }
}

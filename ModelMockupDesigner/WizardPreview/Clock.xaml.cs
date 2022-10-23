using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModelMockupDesigner.WizardPreview
{
    /// <summary>
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : UserControl
    {
        System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public Clock()
        {
            InitializeComponent();
            Loaded += Clock_Loaded;
            Unloaded += Clock_Unloaded;

        }

        private void Clock_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= Clock_Unloaded;
            myDispatcherTimer.Stop();
            myDispatcherTimer = null;
        }

        private void Clock_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= Clock_Loaded;
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            myDispatcherTimer.Tick += myDispatcherTimer_Tick;
            myDispatcherTimer.Start();
        }

        void myDispatcherTimer_Tick(object sender, EventArgs e)
        {
            k2clock.Text = DateTime.Now.ToString("HH:mm:ss");
            dateLabel.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
}

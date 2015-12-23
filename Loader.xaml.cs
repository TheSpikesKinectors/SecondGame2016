using System;
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
using System.Windows.Threading;
namespace BucketGame
{
    /// <summary>
    /// Interaction logic for Loader.xaml
    /// </summary>
    public partial class Loader : Window
    {
        private DispatcherTimer timer;
        public Loader()
        {
            InitializeComponent();
            timer = new DispatcherTimer() { Interval = Props.Default.TimeForExcuse };
            timer.Tick += OnTimerTick;
            timer.Start();
            MainWindow window = new MainWindow();
            window.Loaded += GameLoaded;
       }

        private void GameLoaded(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            excuse.Text = Props.Default.Excuses.Sample();
        }
    }
}

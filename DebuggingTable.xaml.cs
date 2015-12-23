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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BucketGame
{
    /// <summary>
    /// Interaction logic for DebuggingTable.xaml
    /// </summary>
    public partial class DebuggingTable : UserControl
    {
        private Dictionary<string, string> hash;
        public static DebuggingTable LatestCreated { get; private set; }
        public DebuggingTable()
        {
            InitializeComponent();
            hash = new Dictionary<string, string>();
            table.ItemsSource = hash;
            DebuggingTable.LatestCreated = this;
        }

        public string this[string key]
        {
            get
            {
                return hash[key];
            }
            set
            {
                hash[key] = value;
                table.Items.Refresh();
            }
        }
        
    }
}

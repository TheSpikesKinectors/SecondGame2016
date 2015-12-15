using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Kinect;
namespace BucketGame
{
    /// <summary>
    /// A window that controls the joints that are used in the game
    /// </summary>
    public partial class JointSelection : TabItem
    {
        /// <summary>
        /// a list of checkboxes used to select the joints that are used in the game
        /// </summary>
        private List<CheckBox> checkboxes = new List<CheckBox>();

        /// <summary>
        /// The joints that are available for selection
        /// </summary>
        public static readonly List<JointType> availableJoints = Consts.ReasonableJoints.ToList();//Util.GetValues<JointType>().ToList();
        
        /// <summary>
        /// the MainWindow that uses this window
        /// </summary>
        MainWindow owner;

        public JointSelection(MainWindow owner)
        {
            this.Owner = owner;
           
            InitializeComponent();
            foreach (JointType joint in availableJoints) //initialize the checkboxes
            {
                CheckBox cb = new CheckBox();
                cb.Content = joint.ToString();
                checkboxes.Add(cb);
                if (Consts.DefaultJoints.Contains(joint))
                {
                    cb.IsChecked = true;
                }
                else
                {
                    cb.IsChecked = false;
                }
                Panel.Children.Add(cb);
                Label label = new Label();
                label.Content = "        ";
                Panel.Children.Add(label);
                cb.Checked += cb_Checked;
                
            }
        }

        /// <summary>
        /// this is called once a checkbox is checked (or unchecked)
        /// </summary>
        void cb_Checked(object sender, RoutedEventArgs e)
        {
            Owner.ChangeJoint();
        }

        /// <summary>
        /// A list of the joints selected in the windows
        /// (i.e.: the joints correlated to the checked checkboxes in the window)
        /// </summary>
        public List<JointType> Selected
        {
            get
            {
                List<JointType> ret = new List<JointType>();
                int i = 0;
                foreach(JointType joint in availableJoints)
                {
                    if(checkboxes[i].IsChecked.HasValue)
                    {
                        if ((bool) checkboxes[i].IsChecked)
                        {
                            ret.Add(joint);   
                        }
                    }
                    i++;
                }
                return ret;
            }
        }

        public MainWindow Owner
        {
            get
            {
                return owner;
            }

            set
            {
                owner = value;
            }
        }

        /// <summary>
        /// this method is called when the "All Bits" button is clicked
        /// </summary>
        private void AllBits_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox cb in checkboxes)
            {
                cb.IsChecked = true;
            }
        }

        /// <summary>
        /// This method is called when the "reset" button is clicked
        /// </summary>
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (JointType joint in availableJoints)
            {
                CheckBox cb = checkboxes[i];
                if (Consts.DefaultJoints.Contains(joint))
                {
                    cb.IsChecked = true;
                }
                else
                {
                    cb.IsChecked = false;
                }
                i++;

            }
        }

    }
}

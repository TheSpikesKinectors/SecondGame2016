using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Kinect;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace BucketGame
{
    /// <summary>
    /// Interaction logic for Status.xaml
    /// </summary>
    public partial class Status: TabItem
    {
        private MainWindow parent;
        
        public int InitialReachDistance
        {
            get
            {
                return (int)Owner.measurement.MeasuredPixelLength;
            }
        }

        public int InitialCameraDistance
        {
            get
            {
                return (int)Owner.measurement.MeasuringDistance;
            }
        }
        

        /// <summary>
        /// Updates the label showing the current time
        /// </summary>
        public void Update()
        {
            LabelTime.Content = "Time: " + Owner.game.TimeElapsed.ToString(Props.Default.TimeFormat);
            LabelScore.Content = Owner.game.TotalScore.ToString();
            if (Joint == JointType.HandLeft)
            {
                LabelJoint.Content = "!יד שמאל";
            }
            else if (Joint == JointType.HandRight)
            {
                LabelJoint.Content = "!יד ימין";
            }
            else
            {
                LabelJoint.Content = "Use " + Joint;
            }
        }


        public Status(MainWindow owner)
        {
            this.Owner = owner;
            this.DataContext = this.Owner;
            try {
                InitializeComponent();
            }catch(NullReferenceException e)
            {
                
            }
        }

        /// <summary>
        /// The content of the Extra label in the window
        /// </summary>
        public string ExtraInfo
        {
            get { return LabelNothing.Content.ToString(); }
            set { LabelNothing.Content = value; }
        }


        /// <summary>
        /// The joint shown by the window
        /// </summary>
        private JointType Joint
        {
            get
            {
                return Owner.currentlyUsedJoint;
            }
        }


        private void sliderTouchingDistance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            labelTouchingDistance.Content = "מרחק מינימלי לנגיעה: " + sliderTouchingDistance.Value;
        }

        public double TouchingDistance
        {
            get
            {
                return sliderTouchingDistance.Value;
            }
        }

        public MainWindow Owner
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);

        }
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        public int GetReachDistance(int currentCameraDistance)
        {
            if (currentCameraDistance == 0) { currentCameraDistance = 500; }
            return ((int)InitialReachDistance) * ((int)InitialCameraDistance) / currentCameraDistance;
        }
        
        public void ClickMeasure()
        {
            buttonMeasure.PerformClick();
        }
    }
}

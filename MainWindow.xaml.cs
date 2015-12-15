//this is a comment. don't ask me why it's here.
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using System.Windows.Media.Imaging;
using KinectorsLibrary;

namespace BucketGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal Game game = new Game();
        internal LengthMeasurment measurement = new LengthMeasurment();
        private Label[] labels = new Label[Consts.BagPaths.Length];
        private bool bagsDown;
        private bool BagsDown
        {
            get
            {
                return bagsDown;
            }
            set
            {
                if (bagsDown == value) return;
                bagsDown = value;
                int y = bagsDown ? (int)Height - Consts.TargetDiameter : 0;

                for (int i = 0; i < targets.Length; i++)
                {
                    Canvas.SetTop(targets[i], y);
                }
            }
        }

        private Kinecterface kinecterface;

        /// <summary>
        /// this variable determines whether not the game is being played right now;
        /// its value will be true when the game is being played, and false when the game is stopped.
        /// </summary>
        bool currentlyPlaying = false;

        /// <summary>
        /// The panel through which the joints to be used are selected.
        /// </summary>
        JointSelection jointSelectionPanel;

        /// <summary>
        /// the joint that is current used. That is, the joint which the player needs to use.
        /// </summary>
        internal JointType currentlyUsedJoint = JointType.HandRight;

        /// <summary>
        /// The Kinect
        /// </summary>
        KinectSensor sensor
        {
            get
            {
                return chooser.KinectSensorChooser.Kinect;
            }
        }
        
        /// <summary>
        /// The skeleton of the player in front of the Kinect
        /// </summary>
        Skeleton skeleton;

        /// <summary>
        /// The randomizator object
        /// </summary>
        static Random random = new Random();

        /// <summary>
        /// this variable determines whether not the player touched the current object.
        /// Its value will be false when the player hasn't yet touched the object, and therefore touching
        /// it will be the next action the player needs to do,
        /// and true when it already touched it, meaning that the object should stick to the player's
        /// currently used joint, and that the player's next action should be placing the object in the target.
        /// </summary>
        bool hasTouchedObject = false;

        MediaPlayer mediaPlayer, jointsPlayer;
        /// <summary>
        /// The array of all the targets that are on screen
        /// </summary>
        ImageObject[] targets = new ImageObject[Consts.BagPaths.Length];

        /// <summary>
        /// This variable will be a reference to the target that should be used right now, that is also
        /// one of the element of the targets array.
        /// </summary>
        ImageObject currentTarget
        {
            get
            {
                return targets[indexOfCurrentTarget];
            }
        }

        /// <summary>
        /// this variable is used by the CreateNextImage method to determine if the method was called
        /// because the game just started (in such case, its value will be true), or because the player
        /// scored (in such case, its value will be false). 
        /// </summary>
        bool firstTime = true;
        private int indexOfCurrentTarget;

        DrawingSettings BitmapPrefferences
        {
            get
            {
                return Consts.DrawingSettings;
            }
        }

        Status status;

        public MainWindow()
        {


            InitializeComponent();

            chooser.KinectSensorChooser = new Microsoft.Kinect.Toolkit.KinectSensorChooser();
            kinecterface = new Kinecterface(chooser.KinectSensorChooser);
            kinecterface.Start();
            kinecterface.DataRecieved += DataRecieved;

            jointSelectionPanel = new JointSelection(this);
            jointSelectionPanel.Show();

            mediaPlayer = new MediaPlayer();
            jointsPlayer = new MediaPlayer();

            status = new Status(this);
            status.Visibility = Visibility.Visible;
            status.checkboxShowOnlyWantedTarget.Checked += (o, e) => ChangedShowAllTargets();
            status.checkboxShowOnlyWantedTarget.Unchecked += (o, e) => ChangedShowAllTargets();
            status.Header = Props.Default.TabHeaderStatus;
            tabControl.Items.Add(status);

            InitializeTargets();


            //if this occurs, then FirstOrDefault returned null - therefore, there was no kinect object
            //that wasn't null in the KinectSensor.KinectSensors array. This probably means
            //there is no kinect sensor connected to the computer (and to power)
            if (sensor == default(KinectSensor))
            {
                return;
            }

        }

        private void DataRecieved(Skeleton[] skeletons, short[,] depth, byte[] colorFrame)
        {

            frame.Source = Consts.DrawingSettings.CreateBitmapSource(colorFrame);

            //this weird line just reads:
            //taken our array, called "skeletons", and from which choose the first that is not-null
            //and is tracked. If there is no such skeleton, choose null.
            skeleton = skeletons.FirstTracked();

            if (skeleton == default(Skeleton)) //if the FirstOrDefault returned null,
                                               //which happens when the array has only null or
                                               //non-tracked skeletons.
            {
                StatusLabel.Content = Props.Default.MessageCantSeePlayer;
                return;
            }
            else
            {
                StatusLabel.Content = "";
            }

            if (currentlyPlaying)
            {
                status.Update();
            }
            else
            {
                return;
            } //if we aren't currently playing, then stop this method here.

            Point3D locationOfCurrentJoint = BitmapPrefferences.ConvertToPoint(skeleton, currentlyUsedJoint, depth);
            Point3D leftHand = BitmapPrefferences.ConvertToPoint(skeleton, JointType.HandLeft, depth);
            Point3D rightHand = BitmapPrefferences.ConvertToPoint(skeleton, JointType.HandLeft, depth);
            Point3D shoulderCenter = BitmapPrefferences.ConvertToPoint(skeleton, JointType.ShoulderCenter, depth);

            int radius = status.GetReachDistance(shoulderCenter.Z);
            Canvas.SetLeft(arc, shoulderCenter.X - radius);
            Canvas.SetTop(arc, shoulderCenter.Y - radius);
            arc.Width = arc.Height = 2 * radius;


            //distance of the player's joint from the target
            double distance = Util.Distance(locationOfCurrentJoint, currentTarget.CenterLocation);
            //status.ExtraInfo = distance.ToString();

            if (hasTouchedObject) //if the player already touched the object...
            {
                //move the object to the player's joint
                currentTarget.CenterLocation = (Point)locationOfCurrentJoint;

                //this will be the player's joint's distance from the target
                double distanceFromTarget = Util.Distance(locationOfCurrentJoint, locationOfCurrentJoint);

                //if we basically touched the target - then...
                if (distanceFromTarget <= status.TouchingDistance)
                {
                    hasTouchedObject = false;
                    CreateNextImage(BitmapPrefferences.ConvertToPoint(skeleton, JointType.ShoulderCenter, depth));
                }

            }
            //if we haven't touched the target, but now we just firsly did, then..
            else if (distance <= status.TouchingDistance)
            {
                hasTouchedObject = true;
            }
        }

        private void InitializeMeasurments(Point3D leftHand, Point3D rightHand, Point3D shoulderCenter)
        {
            double distanceLeft, distanceRight;
            distanceLeft = leftHand.Distance2D(shoulderCenter);
            distanceRight = rightHand.Distance2D(shoulderCenter);

            measurement.MeasuredPixelLength = Math.Min(distanceLeft, distanceRight);
            measurement.MeasuringDistance = shoulderCenter.Z;
        }


        private void InitializeTargets()
        {
            //the following loop initializes and locates the targets on the screen.
            int x = 25, y = Consts.DrawingSettings.PixelHeight - Consts.TargetDiameter;
            for (int i = 0; i < Consts.ImageObjectPaths.Length; i++)
            {
                labels[i] = new Label()
                {
                    Content = "0", FontSize = 42,
                    Foreground = Brushes.Yellow, FontWeight = FontWeights.UltraBold
                };
                //iteratively initialize the Targets array
                targets[i] =
                    new ImageObject(Consts.BagPaths[i])
                    {
                       Width = Consts.TargetDiameter, Height = Consts.TargetDiameter
                    };

                //this is just a reference for conviniece. Everytime in this loop when there is an "it"
                // (short for iterated), it is equivalent to writing Targets[i]
                ImageObject it = targets[i];

                //just alignment stuff
                it.HorizontalAlignment = HorizontalAlignment.Left;
                it.VerticalAlignment = VerticalAlignment.Top;

                //Add this to the container
                targetsCanvas.Children.Add(it);
                targetsCanvas.Children.Add(labels[i]);
                //Locate on the screen
                Canvas.SetLeft(it, x);
                Canvas.SetTop(it, y);

                Canvas.SetLeft(labels[i], x);
                Canvas.SetTop(labels[i], y);

                //the variable x changes so each target will be located just in the right place.
                x += Consts.DistanceBetweenTargets;

            }//Yay, we were done with initializing the targets!! that wasn't so hard, now, wasn't it?
        }

        public void ChangedShowAllTargets()
        {
            bool? hideOtherTargets = status.checkboxShowOnlyWantedTarget.IsChecked;
            if (hideOtherTargets != null && (bool)hideOtherTargets) //should hide other targets
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i] == currentTarget)
                    {
                        targets[i].Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        targets[i].Visibility = System.Windows.Visibility.Hidden;
                    }
                }
            }
            else
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        //this function is called when the game first starts.
        public void GameStart()
        {
            for (int i = 0; i < Consts.BagPaths.Length; i++)
            {
                labels[i].Content = "0";
                game.SetScore(i, 0);
            }
            //this variable is so that we know whether not to add points in the CreateNext method.
            firstTime = true;
            if (!measurement.IsInitialized)
            {
                MessageBox.Show(Props.Default.MessageMeasurementNotInitialize);
                return;
            }

            //update the status window
            game.Start();

            CreateNextImage(new Point3D(Consts.DrawingSettings.PixelWidth / 2,
                Consts.DrawingSettings.PixelHeight / 2,
                measurement.IsInitialized ? (int)measurement.MeasuringDistance : 250));

        }

        //this method safely changes the current joint randomly, and updates the status window
        public void ChangeJoint()
        {
            BagsDown = !BagsDown;
            try
            {
                currentlyUsedJoint = jointSelectionPanel.Selected.ChooseRandom(random);
            }
            catch (Exception)
            {
                currentlyUsedJoint = Consts.DefaultJoints[0];
            }

            if (currentlyUsedJoint == JointType.HandRight)
            {
                mediaPlayer.PlayFromRelativePath("../../sounds/right.mp3");
            }
            else if (currentlyUsedJoint == JointType.HandLeft)
            {
                mediaPlayer.PlayFromRelativePath("../../sounds/left.mp3");
            }
        }

        //this method creates the next shape and updates everything when the game starts
        //and whenever the player scores. BTW, we know to differ these situations with the firstTime variable.
        public void CreateNextImage(Point3D shoulderCenter)
        {
            if (!firstTime)
            {
                game.IncrementScore(indexOfCurrentTarget);
                labels[indexOfCurrentTarget].Content = game.GetScoreOfTarget(indexOfCurrentTarget).ToString();
            }
            //current willl be the index of the next target.
            indexOfCurrentTarget = random.Next(0, Consts.ImageObjectPaths.Length);

            currentTarget.RelativePath = (Consts.ImageObjectPaths[indexOfCurrentTarget]);



            //choose a random point at the top half of the screen
            //(the top half - because we don't want it to be too close to the targets)
            Point p;
            do
            {
                double r = status.GetReachDistance(shoulderCenter.Z) * random.NextDouble();

                double dy = (random.NextDouble() * r);
                double dx = Math.Sqrt(r * r - dy * dy);

                if (random.NextBoolean())
                {
                    dx *= -1;
                }
                p = new Point(shoulderCenter.X + dx, shoulderCenter.Y - dy);
            } while (!(p.X.IsStrictlyInRange(0, Consts.DrawingSettings.PixelWidth)
                && p.Y.IsStrictlyInRange(0, Consts.DrawingSettings.PixelHeight)));

            currentTarget.RelativePath = Consts.ImageObjectPaths[indexOfCurrentTarget];

            ChangedShowAllTargets();

            //Move the imageObejct to this point
            currentTarget.MoveTo(p);

            //we haven't touched THIS new object yet...
            hasTouchedObject = false;

            if (!firstTime)
            {
                jointsPlayer.PlayFromRelativePath("sounds/score.mp3");
            }
            ChangeJoint();
            //from now on and until the next game starts, we only call this method when the player scroed.
            firstTime = false;
        }

        private void OnWin()
        {
            currentlyPlaying = false; //we're done playing
            GameButton.Content = "Play"; //so change the button's content
            mediaPlayer.PlayFromRelativePath("sounds/victory.mp3");
            MessageBox.Show("You scored " + game.TotalScore + "points in " + game.TimeElapsed.ToString(Props.Default.TimeFormat));
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            jointSelectionPanel.Close();
            if (sensor == null) return;
            sensor.ColorStream.Disable();
            sensor.AudioSource.Stop();
            sensor.DepthStream.Disable();
            sensor.Stop();
            sensor.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            currentlyPlaying = !currentlyPlaying;
            GameButton.Content = currentlyPlaying ? "Stop" : "Start";
            if (currentlyPlaying)
            {
                GameStart();
            }
        }
    }
}

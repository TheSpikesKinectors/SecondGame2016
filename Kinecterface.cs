using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
namespace KinectorsLibrary
{
    class Kinecterface
    {
        private KinectSensorChooser chooser;
        public event Action<Skeleton[], short[,], byte[]> DataRecieved;
        private bool backgroundRemoval;

        public bool BackgroundRemoval
        {
            get
            {
                return backgroundRemoval;
            }

            set
            {
                backgroundRemoval = value;
            }
        }

        public Kinecterface(KinectSensorChooser chooser)
        {
            this.chooser = chooser;
            this.chooser.KinectChanged += KinectChanged;
            this.DataRecieved = (skels, depth, colors) => { };
        }

        private void KinectChanged(object sender, KinectChangedEventArgs e)
        {
            if (e.OldSensor != null)
            {
                Disable(e.OldSensor);
            }
            if (e.NewSensor != null)
            {
                Enable(e.NewSensor);
                e.NewSensor.AllFramesReady += AllFramesReady;
            }
        }

        private void AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {

                Skeleton[] skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                skeletonFrame.CopySkeletonDataTo(skeletons);
                short[,] depths = new short[depthFrame.Width, depthFrame.Height];
                short[] fromDepthFrame = new short[depthFrame.Width * depthFrame.Height];
                depthFrame.CopyPixelDataTo(fromDepthFrame);
                for (int i = 0; i < fromDepthFrame.Length; i++)
                {
                    depths[i / depthFrame.Height, i % depthFrame.Height] = fromDepthFrame[i];
                }
                byte[] colorPixels = new byte[colorFrame.PixelDataLength];
                colorFrame.CopyPixelDataTo(colorPixels);
                
                //background removal
                if (BackgroundRemoval)
                {
                    Skeleton skeleton = skeletons.FirstOrDefault();
                    int maxDepth = -1, minDepth = -1;
                    if (skeleton != default(Skeleton)) {
                        short max = skeleton.Joints.Max(
                            joint => depths[(int)(depthFrame.Width * joint.Position.X),
                            (int)(depthFrame.Height * joint.Position.Y)]);
                        short min = skeleton.Joints.Max(
                            joint => depths[(int)(depthFrame.Width * joint.Position.X),
                            (int)(depthFrame.Height * joint.Position.Y)]);
                        int diff = max - min;
                        maxDepth = max + diff;
                        minDepth = min - diff;
                    }
                    for(int x = 0; x < colorFrame.Width; x++)
                    {
                        for(int y = 0; y < colorFrame.Height; y++)
                        {
                            bool isProbablyPerson;
                            if(skeleton == default(Skeleton))
                            {
                                isProbablyPerson = false;
                            }
                            else
                            {
                                short depth = depths[x, y];
                                isProbablyPerson = depth > minDepth && depth < maxDepth;
                            }
                            colorPixels[(y*colorFrame.Width + x) * 4] = (byte)(isProbablyPerson ? 255 : 0);
                        }
                    }
                }
                
                DataRecieved(skeletons, depths, colorPixels);
            }
        }

        public static void Disable(KinectSensor sensor)
        {
            sensor.AudioSource.Stop();
            sensor.ColorStream.Disable();
            sensor.DepthStream.Disable();
            sensor.Stop();
            sensor.Dispose();
        }

        public static void Enable(KinectSensor sensor)
        {
            sensor.Start();
            sensor.ColorStream.Enable();
            sensor.DepthStream.Enable();
            sensor.AudioSource.Start();
        }

        public void Start()
        {
            chooser.Start();
        }

        public void Stop()
        {
            chooser.Stop();
        }

        public bool Seated
        {
            get
            {
                return this.Sensor.SkeletonStream.TrackingMode == SkeletonTrackingMode.Seated;
            }
            set
            {
                this.Sensor.SkeletonStream.TrackingMode =
                    value ? SkeletonTrackingMode.Seated : SkeletonTrackingMode.Default;
            }
        }

        private KinectSensor Sensor
        {
            get
            {
                return chooser.Kinect;
            }
        }
    }
}
using Microsoft.Kinect;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace BucketGame
{
    class Consts
    {
        /// <summary>
        /// The amount of points that scoring is worth
        /// </summary>
        public static readonly int ScorePerShape = 1;

        /// <summary>
        /// The amount of "Ticks" in a millisecond.
        /// </summary>
        public static readonly long TicksPerMilisecond = 10000;

        /// <summary>
        /// The joints to be selected by default on the JointSelection window
        /// </summary>
        public static readonly JointType[] DefaultJoints = { JointType.HandRight, JointType.HandLeft };

        /// <summary>
        /// the joints that will be offered by the JointSelection window
        /// </summary>
        public static readonly JointType[] ReasonableJoints = DefaultJoints;
       
        /// <summary>
        /// The amount of score that once recieved, the game is won
        /// </summary>
        public static readonly int WantedScore = 10*ScorePerShape;

        /// <summary>
        /// The relative path of the candy image
        /// </summary>
        public static readonly string RelativeCandy = "imgs\\candy.gif";

        /// <summary>
        /// The relative path of the chocolate image
        /// </summary>
        public static readonly string RelativeChocolate = "imgs\\chocolate.png";

        /// <summary>
        /// The relative path of the balloon image
        /// </summary>
        public static readonly string RelativeBalloon = "imgs\\balloon.png";

        /// <summary>
        /// The relative path of the candy bag image
        /// </summary>
        public static readonly string RelativeCandyBag = "imgs\\candyBag.png";

        /// <summary>
        /// The relative path of the chocolate bag image
        /// </summary>
        public static readonly string RelativeChocolateBag = "imgs\\chocolateBag.png";

        /// <summary>
        /// The relative path of the balloon bag image
        /// </summary>
        public static readonly string RelativeBalloonBag = "imgs\\balloonBag.png";

        //IT IS VERY IMPORTANT FOR TWO ARRAYS BELOW TO BE IN THE SAME ORDER

        /// <summary>
        /// The paths, by order, of the objects images in the game
        /// </summary>
        public static readonly string[] ImageObjectPaths = { RelativeBalloon, RelativeCandy, RelativeChocolate };


        /// <summary>
        /// The paths, by order, of the bags images in the game
        /// </summary>
        public static readonly string[] BagPaths = { RelativeBalloonBag, RelativeCandyBag, RelativeChocolateBag };

        internal static readonly DepthImageFormat DepthFormat = DepthImageFormat.Resolution640x480Fps30;
        internal static readonly ColorImageFormat ColorFormat = ColorImageFormat.RgbResolution640x480Fps30;

        public static readonly DrawingSettings
            DrawingSettings = new DrawingSettings()
            {
                PixelWidth = 640,
                PixelHeight = 480,
                DpiX = 96,
                DpiY = 96,
                PixelFromatUsed = PixelFormats.Bgr32,
                BitmapPalleteUsed = null,
                Stride = 4
            };
        public static readonly int TargetDiameter = 200;
        public static readonly int DistanceBetweenTargets = 200;
        internal static readonly int ObjectDiameter = 100;
    }
}

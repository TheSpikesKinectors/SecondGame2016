using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BucketGame
{
    /// <summary>
    /// This object lets you place an image on the screen with ease.
    /// </summary>
    public partial class ImageObject : UserControl
    {
        /// <summary>
        /// Creates an ImageObject by a relative path to the image and its dimensions
        /// </summary>
        /// <param name="relativePath">the relative path to the image</param>
        /// <param name="width">the width of the ImageObject wanted</param>
        /// <param name="height">the height of the ImageObject wanted</param>
        public ImageObject(String relativePath)
        {
            InitializeComponent();
            RelativePath = relativePath;
            grid.Width = image.Source.Width;
            grid.Height = image.Source.Height;
        }

        public ImageObject(string relativePath, Point centerLocation): this(relativePath)
        {
            this.CenterLocation = centerLocation;
        }

        /// <summary>
        /// The relative path of the image
        /// </summary>
        public string RelativePath
        {
            set
            {
                image.Source = new BitmapImage(new Uri(value, UriKind.Relative));
            }
        }

        /// <summary>
        /// The absolute path of the image
        /// </summary>
        public string AbsolutePath
        {
            set
            {
                image.Source = new BitmapImage(new Uri(value, UriKind.Absolute));
            }
        }

        public Point CenterLocation
        {
            get
            {
                return new Point(Canvas.GetLeft(this) + grid.Width/2,
                    Canvas.GetTop(this) + grid.Height/2);               
            }
            set
            {
                Canvas.SetLeft(this,value.X - grid.Width/2);
                Canvas.SetTop(this,value.Y - grid.Height / 2);
            }
        }

        public void SetLocation(int x, int y)
        {
            this.SetX(x);
            this.SetY(y);
        }
    }
}

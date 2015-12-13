using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BucketGame
{
    class DrawingSettings
    {
        private double dpiX, dpiY;
        private int pixelsWidth, pixelHeight, stride;
        private PixelFormat pixelFromatUsed;
        private BitmapPalette bitmapPalleteUsed;

        public DrawingSettings() { }

        public DrawingSettings(int pixelWidth, int pixelHeight, double dpiX, double dpiY,
            PixelFormat pixelFormat,
            BitmapPalette palette, int stride)
        {
            PixelWidth = pixelWidth;
            PixelHeight = pixelHeight;
            DpiX = dpiX;
            DpiY = dpiY;
            PixelFromatUsed = pixelFormat;
            BitmapPalleteUsed = palette;
            Stride = stride;
        }

        public int ConvertX(float x) { return (int)(PixelWidth * x); }
        public int ConvertY(float y) { return (int)(PixelHeight * y); }
        public Point3D ConvertToPoint(SkeletonPoint position, short[,] depthInfo)
        {
            int x = ConvertX(position.X), y = ConvertY(position.Y);
            return new Point3D(x, y, depthInfo[x, y]);
        }

        public Point3D ConvertToPoint(Skeleton skeleton, JointType joint, short[,] depthInfo)
        {
            return ConvertToPoint(skeleton.Joints[joint].Position, depthInfo);
        }

        public BitmapSource CreateBitmapSource(byte[] pixels)
        {
            return BitmapSource.Create(PixelWidth, PixelHeight,
                DpiX, DpiY, PixelFromatUsed, BitmapPalleteUsed, pixels, Stride);
        }

        public double DpiX
        {
            get
            {
                return dpiX;
            }

            set
            {
                dpiX = value;
            }
        }

        public double DpiY
        {
            get
            {
                return dpiY;
            }

            set
            {
                dpiY = value;
            }
        }

        public int PixelWidth
        {
            get
            {
                return pixelsWidth;
            }

            set
            {
                pixelsWidth = value;
            }
        }

        public int PixelHeight
        {
            get
            {
                return pixelHeight;
            }

            set
            {
                pixelHeight = value;
            }
        }

        public int Stride
        {
            get
            {
                return stride;
            }

            set
            {
                stride = value;
            }
        }

        public PixelFormat PixelFromatUsed
        {
            get
            {
                return pixelFromatUsed;
            }

            set
            {
                pixelFromatUsed = value;
            }
        }

        public BitmapPalette BitmapPalleteUsed
        {
            get
            {
                return bitmapPalleteUsed;
            }

            set
            {
                bitmapPalleteUsed = value;
            }
        }
    }
}

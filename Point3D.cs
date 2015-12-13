using System;
using System.Windows;

namespace BucketGame
{
    public struct Point3D
    {
        public readonly int X, Y, Z;
        public Point3D(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public double Distance2D(Point3D point)
        {
            return Math.Sqrt((X - point.X).Pow(2) + (Y - point.Y).Pow(2));
        }

        public static explicit operator Point(Point3D point)
        {
            return new Point(point.X, point.Y);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
namespace BucketGame
{
    /// <summary>
    /// This class contains util functions that could be used everywhere. 
    /// </summary>
    static class Util
    {
        
        
        /// <param name="random">the randomizator object</param>
        /// <returns>a random point at the top half of the frame from the Kinect sensor</returns>
        public static Point RandomPointAtTopHalfOfScreen(Random random)
        {
            return new Point()
            {
                X = random.Next(Consts.TargetDiameter / 2,
                    Consts.DrawingSettings.PixelWidth - Consts.TargetDiameter / 2),
                Y = random.Next(0, Consts.DrawingSettings.PixelHeight / 2)
            };
        }

        public static bool NextBoolean(this Random random)
        {
            return random.NextDouble() >= 0.5;
        }
        
        public static bool IsTracked(this Skeleton skeleton)
        {
            return skeleton != null && skeleton.TrackingState == SkeletonTrackingState.Tracked;
        }

        public static Skeleton FirstTracked(this IEnumerable<Skeleton> skeletons)
        {
            return skeletons.FirstOrDefault(IsTracked);
        }

         /// <summary>
         /// Rounds the given number to the percision of 3 decimal points.
         /// </summary>
         /// <param name="num">The number to round</param>
         /// <returns>The rounded number</returns>
        public static float Round(this float num)
        {
            return ((int)(num * 1000)) / 1000.0f;
        }
        
        
        /// <summary>
        /// This method return a point of the top left corner of an object, given its center point
        /// </summary>
        /// <param name="centerPoint">the point of the center of the object</param>
        /// <returns>the coordinates of the top left corner of the object</returns>
        public static Point FromCenterPointToTopLeftPoint(Point centerPoint)
        {
            return new Point(centerPoint.X - Consts.TargetDiameter/2, centerPoint.Y - Consts.TargetDiameter/2);
        }

        /// <summary>
        /// return the distance between two given points.
        /// </summary>
        /// <param name="p1">the first point</param>
        /// <param name="p2">the second point</param>
        /// <returns>the distance between the two given points</returns>
        public static double Distance(dynamic p1, dynamic p2)
        {
            double dx = p2.X - p1.X, dy = p2.Y - p1.Y;
            return Math.Sqrt(dx*dx + dy*dy);
        }

        /// <summary>
        /// converts a dynamic-type object, that  has int property of X,Y
        /// to a point object.
        /// </summary>
        /// <param name="point">the dynamic-type object, that has int
        /// properties of X,Y</param>
        /// <returns>a Point object with the corresponding coordinates</returns>
        public static Point ToPoint(dynamic point)
        {
            return new Point(point.X, point.Y);
        }

        /// <summary>
        /// convers a time in MS to time in CS-Ticks.
        /// </summary>
        /// <param name="ms">the time in MS</param>
        /// <returns>the time in units of CS-Ticks</returns>
        public static long Miliseconds(this long ms) //THIS LONG HAHA
        {
            return ms * Consts.TicksPerMilisecond;
        }
        /// <summary>
        /// convers a time in seconds to time in CS-Ticks.
        /// </summary>
        /// <param name="ms">the time in seconds</param>
        /// <returns>the time in units of CS-Ticks</returns>
        public static long Seconds(this long secs)
        {
            return Miliseconds(secs) * 1000;
        }
        /// <summary>
        /// gets a time unit and converts it to a string
        /// </summary>
        /// <param name="time">a time (for example - 9, for 9 seconds)</param>
        /// <returns>the time as a string (for example, if 
        /// the paramter is 9, returns "09"</returns>
        public static string Stringify(this long time)
        {
            return time < 10 ? "0" + time : "" + time;
        }

        /// <summary>
        /// Sums a list of doubles.
        /// </summary>
        /// <param name="list">a list of doubles</param>
        /// <returns>the list's sum</returns>
        public static double Sum(this List<double> list)
        {
            double sum = 0;
            foreach(double d in list)
            {
                sum += d;
            }
            return sum;
        }

        /// <summary>
        /// Avarages a list of doubles.
        /// </summary>
        /// <param name="list">a list of doubles</param>
        /// <returns>the list's avarage</returns>
        public static double Avg(this List<double> list)
        {
            return list.Sum() / list.Count;
        }

        /// <summary>
        /// Checks and returns if the given value is NaN
        /// </summary>
        /// <param name="x">The value to check is is NaN</param>
        /// <returns>true if the value is NaN, false otherwise</returns>
        public static bool IsNaN(this double x)
        {
            return double.IsNaN(x);
        }

        /// <summary>
        /// Returns an IEnumerable of the enum values.
        /// </summary>
        /// <typeparam name="T">An enum type</typeparam>
        /// <returns>An IEnumerable of the enum values</returns>
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
        
        /// <summary>
        /// chooses a random element from a list
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list</typeparam>
        /// <param name="list">The list of elements to choose from</param>
        /// <param name="rand">The randomizator</param>
        /// <returns>A random element from the array</returns>
        public static T ChooseRandom<T>(this List<T> list, Random rand)
        {
            return list[rand.Next(list.Count)];
        }

        public static void MoveTo(this UIElement elem, Point point) //moves the given UIElement to the given point
        {
            Canvas.SetLeft(elem, point.X);
            Canvas.SetTop(elem, point.Y);
        }

        public static void SetX(this UIElement elem, double x)
        {
            Canvas.SetLeft(elem, x);
        }

        public static void SetY(this UIElement elem, double y)
        {
            Canvas.SetTop(elem, y);
        }

        public static double GetX(this UIElement elem)
        {
            return Canvas.GetLeft(elem);
        }

        public static double GetY(this UIElement elem)
        {
            return Canvas.GetTop(elem);
        }

        /// <summary>
        /// Makes the media player open the file given by its relative path
        /// </summary>
        /// <param name="mediaPlayer">the media player that should open the file</param>
        /// <param name="relativePath">the relative path of the file</param>
        public static void OpenFromRelativePath(this MediaPlayer mediaPlayer, string relativePath)
        {
            if (!relativePath.StartsWith("../../"))
            {
                relativePath = "../../" + relativePath;
            }
            mediaPlayer.Open((new Uri(relativePath, UriKind.Relative)));
            
        }

        public static void PlayFromRelativePath(this MediaPlayer mediaPlayer, string relativePath)
        {
            mediaPlayer.OpenFromRelativePath(relativePath);
            mediaPlayer.Play();
        }

        public static void SetNearMode(this KinectSensor sensor, bool nearMode)
        {
            sensor.DepthStream.Range = nearMode ? DepthRange.Near : DepthRange.Default;
        }

        public static bool IsStrictlyInRange(this double x, double min, double limit)
        {
            return x > min && x < limit;
        }

        public static object Synchronizedly(object locker, Action action)
        {
            lock (locker)
            {
                action();
            }
            return locker;
        }


        public static void PerformClick(this Button btn)
        {
            btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public static double Pow(this double x, double y)
        {
            return Math.Pow(x, y);
        }

        public static double Pow(this int x, double y)
        {
            return Math.Pow(x, y);
        }

        public static IEnumerable<T> AsReadOnly<T>(this IEnumerable<T> ie)
        {
            foreach(T t in ie)
            {
                yield return t;
            }
        }

        public static IEnumerable<double> Skips(double starting, double end, double skip)
        {
            double it = starting;
            if (skip == 0) throw new ArgumentException("cannot be zero", "skip");
            Func<bool> goOn;
            if (skip > 0)
            {
                goOn = delegate () { return it < end; };
            }
            else
            {
                goOn = delegate () { return it >= end; };
            }

            for (it = starting; goOn(); it += skip)
            {
                yield return it;
            }
        }


        public static IEnumerable<int> Skips(int starting, int end, int skip)
        {
            int it = starting;
            if (skip == 0) throw new ArgumentException("cannot be zero", "skip");
            Func<bool> goOn;
            if (skip > 0)
            {
                goOn = delegate () { return it < end; };
            }
            else
            {
                goOn = delegate () { return it >= end; };
            }

            for (it = starting; goOn(); it += skip)
            {
                yield return it;
            }
        }

        public static IEnumerable<int> Upto(int from, int to)
        {
            return Skips(from, to, 1);
        }


        public static IEnumerable<int> Upto(int to)
        {
            return Skips(0, to, 1);
        }


        public static IEnumerable<int> Dwonto(int from, int to)
        {
            return Skips(from, to, -1);
        }


        public static IEnumerable<int> DownFrom(int from)
        {
            return Skips(from, 0, -1);
        }

        public static IEnumerable<Exception> GetInnerExceptionsChain(this Exception e)
        {
            Exception it = e, prev = null;
            while(it != null)
            {
                if(prev == it)
                {
                    throw new Exception("inner exception loop");
                }
                yield return it;
                prev = it;
                it = it.InnerException;
            }
        }
        
    }
}

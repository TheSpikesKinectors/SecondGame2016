using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BucketGame
{
    class LengthMeasurment
    {
        private double measuredPixelLength;
        private double measuringDistance;

        public bool IsInitialized
        {
            get
            {
                return MeasuredPixelLength * MeasuringDistance == 0;
            }
        }

        public double MeasuredPixelLength
        {
            get
            {
                return measuredPixelLength;
            }

            set
            {
                measuredPixelLength = value;
            }
        }

        public double MeasuringDistance
        {
            get
            {
                return measuringDistance;
            }

            set
            {
                measuringDistance = value;
            }
        }

        public double ApproximatePixelLength(double distanceFromCamera)
        {
            return (MeasuredPixelLength) * (MeasuringDistance) / distanceFromCamera;
        }
    }
}
}

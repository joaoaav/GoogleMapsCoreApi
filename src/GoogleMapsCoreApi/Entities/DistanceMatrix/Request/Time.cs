using GoogleMapsCoreApi.Engine;
using System;
using System.Globalization;

namespace GoogleMapsCoreApi.Entities.DistanceMatrix.Request
{
    public class Time
    {
        public DateTime Value { get; set; }

        public bool IsNow { get; set; }

        public Time()
        {
            IsNow = true;
        }

        public override string ToString()
        {
            return IsNow ? "now" : UnixTimeConverter.DateTimeToUnixTimestamp(Value).ToString(CultureInfo.InvariantCulture);
        }
    }
}

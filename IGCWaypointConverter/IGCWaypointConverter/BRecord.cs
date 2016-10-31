using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGCWaypointConverter {

    /// <summary>
    /// Contains and extracts data from the B-record line of an IGC logger file.
    /// Known bugs: Does not correctly handle timestamps of b-records where the flight extends through midnight UTC
    /// </summary>
    class BRecord {

        public readonly IGCFile Parent;
        public readonly string Line;

        public BRecord(IGCFile parent, string line) {
            Parent = parent;
            Line = line;
        }

        public DateTime GetTimeUTC() {
            return Parent.FlightDate.Add(TimeSpan.ParseExact(Line.Substring(1, 6), "hhmmss", null));
        }

        public double GetLatitude() {
            var degrees = int.Parse(Line.Substring(7, 2), NumberStyles.Any);
            var minutes = int.Parse(Line.Substring(9, 5), NumberStyles.Any) / 1000.0;
            return (degrees + minutes / 60.0) * (Line[14] == 'S' ? -1 : 1);
        }

        public double GetLongitude() {
            var degrees = int.Parse(Line.Substring(15, 3), NumberStyles.Any);
            var minutes = int.Parse(Line.Substring(18, 5), NumberStyles.Any) / 1000.0;
            return (degrees + minutes / 60.0) * (Line[23] == 'W' ? -1 : 1);
        }

        public int GetPressureAltitude() {
            return int.Parse(Line.Substring(25, 5), NumberStyles.Any);
        }

        public int GetGPSAltitude() {
            return int.Parse(Line.Substring(30, 5), NumberStyles.Any);
        }
    }
}

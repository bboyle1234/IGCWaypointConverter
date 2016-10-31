using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGCWaypointConverter {
    class MAVRecord {

        public int Index;
        public int CurrentWP;
        public int CoordFrame;
        public int Command = 16;
        public double Param1, Param2, Param3, Param4;
        public double Latitude;
        public double Longitude;
        public int Altitude;
        public bool AutoContinue = true;

        public string Generate() {
            return string.Join("\t", new[] {
                Index.ToString(),
                CurrentWP.ToString(),
                CoordFrame.ToString(),
                Command.ToString(),
                Param1.ToString(),
                Param2.ToString(),
                Param3.ToString(),
                Param4.ToString(),
                Latitude.ToString(),
                Longitude.ToString(),
                Altitude.ToString(),
                AutoContinue ? "1" : "0",
            });
        }
    }
}

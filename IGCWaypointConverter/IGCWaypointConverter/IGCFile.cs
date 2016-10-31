using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGCWaypointConverter {
    class IGCFile {

        public readonly string[] Lines;
        public readonly BRecord[] BRecords;
        public readonly DateTime FlightDate;
        public readonly DateTime FromUTC;
        public readonly DateTime ToUTC;

        public IGCFile(string text) {
            Lines = text.Split('\n').Where(l => !string.IsNullOrEmpty(l)).Select(l => l.Replace("\r", "")).ToArray();
            FlightDate = GetFlightDate();
            BRecords = GetBRecords();
            FromUTC = BRecords[0].GetTimeUTC();
            ToUTC = BRecords[BRecords.Length - 1].GetTimeUTC(); 
        }

        DateTime GetFlightDate() {
            var dateLine = Lines.Single(l => l.StartsWith("HFDTE"));
            return DateTime.ParseExact(dateLine.Substring(5), "ddMMyy", null, System.Globalization.DateTimeStyles.AssumeUniversal).ToUniversalTime();
        }

        BRecord[] GetBRecords() {
            return Lines.Where(l => l.StartsWith("B")).Select(l => new BRecord(this, l)).ToArray();
        }
    }
}

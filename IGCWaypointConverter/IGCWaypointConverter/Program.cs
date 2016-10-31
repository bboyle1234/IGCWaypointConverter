using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGCWaypointConverter {
    class Program {

        static string igcFileName;
        static IGCFile igcFile;
        static DateTime fromUTC;
        static DateTime toUTC;
        static int sequencePointNumber;

        static void Main(string[] args) {
            try {
                igcFileName = args[0];
                igcFile = new IGCFile(File.ReadAllText(igcFileName));
                OutputProgramHeader();
                SetupTimes();
                SetupStartingSequencePointNumber();
                ProduceOutput();
            } catch (Exception x) {
                Console.WriteLine(x.Message);
                Console.ReadKey();
            }
        }

        static void OutputProgramHeader() {
            Console.WriteLine(new string('=', 80));
            Console.WriteLine("IGC Waypoint File Converter: " + igcFileName);
            Console.WriteLine(new string('=', 80));
        }

        static void SetupTimes() {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Data extends from '{igcFile.FromUTC.ToString("yyyy-MM-dd HH:mm:ss")}' until '{igcFile.ToUTC.ToString("yyyy-MM-dd HH:mm:ss")}', in UTC timezone.");
            Console.WriteLine("1: All data");
            Console.WriteLine("2: Custom data range");
            Console.Write("Choose --> ");
            var choice = Console.ReadKey().KeyChar;
            if (choice == '1') {
                Console.WriteLine("Processing entire data set...");
                fromUTC = igcFile.FromUTC;
                toUTC = igcFile.ToUTC;
            } else if (choice == '2') {
                Console.WriteLine("Please enter UTC start time in format yyyy-MM-dd HH:mm:ss");
                fromUTC = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.AssumeUniversal);
                Console.WriteLine("Now the end time in the same format. Or 'X' to read to the end of the file");
                var response = Console.ReadLine();
                if (response == "X") {
                    toUTC = igcFile.ToUTC;
                } else {
                    toUTC = DateTime.ParseExact(response, "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.AssumeUniversal);
                }
            } else {
                throw new Exception("Incorrect user input");
            }
        }

        static void SetupStartingSequencePointNumber() {
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Enter the starting sequence point number for your waypoints file: ");
            sequencePointNumber = int.Parse(Console.ReadLine());
        }

        static void ProduceOutput() {
            var sb = new StringBuilder();
            foreach (var bRecord in igcFile.BRecords) {
                if (bRecord.GetTimeUTC() < fromUTC) continue;
                if (bRecord.GetTimeUTC() > toUTC) continue;
                var mav = new MAVRecord() {
                    Index = sequencePointNumber++,
                    Latitude = bRecord.GetLatitude(),
                    Longitude = bRecord.GetLongitude(),
                    Altitude = bRecord.GetGPSAltitude(),
                };
                sb.AppendLine(mav.Generate());
            }
            var fileName = Path.GetTempFileName();
            File.WriteAllText(fileName, sb.ToString());
            Process.Start("notepad", fileName);
        }
    }
}

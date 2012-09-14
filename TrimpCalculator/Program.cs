using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TrimpCalculator
{
    class Program
    {
        private static double maxHeartRate = 195;
        static XNamespace ns = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2";

        static void Main(string[] args)
        {
            try
            {
                foreach (var fileName in Directory.GetFiles(args[0], "*.tcx"))
                {

                    double trimp = 0;
               
                    XDocument doc = XDocument.Load(fileName);
                    foreach (var trackPoint in doc.Descendants(ns + "Trackpoint"))
                    {
                        XElement bmpElement = trackPoint.Element(ns + "HeartRateBpm");
                        if (bmpElement == null)
                            continue;

                        XElement valueElement = bmpElement.Element(ns + "Value");

                        if(valueElement == null)
                            continue;

                        int bpm = int.Parse(valueElement.Value);

                        trimp += GetTrimpScore(bpm);
                    }

                    Console.WriteLine(Path.GetFileNameWithoutExtension(fileName) + " " + ((int)trimp).ToString());

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadLine();
        }

        private static double GetTrimpScore(int bpm)
        {
            double percentHRMax = bpm / maxHeartRate;
            int weight = 0;

            if (percentHRMax >= 0.5 && percentHRMax < 0.6)
                weight = 1;
            else if (percentHRMax >= 0.6 && percentHRMax < 0.7)
                weight = 2;
            else if (percentHRMax >= 0.7 && percentHRMax < 0.8)
                weight = 3;
            else if (percentHRMax >= 0.8 && percentHRMax < 0.9)
                weight = 4;
            else if (percentHRMax >= 0.9)
                weight = 5;

            return 1D / 60D * weight;
        }
    }
}

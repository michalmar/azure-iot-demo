//********************************************************* 
// 
//    Copyright (c) Microsoft. All rights reserved. 
//    This code is licensed under the Microsoft Public License. 
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF 
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY 
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR 
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT. 
// 
//********************************************************* 

using System.Collections.Generic;
using System.IO;

using Microsoft.Azure.StreamAnalytics;
using Microsoft.Azure.StreamAnalytics.Serialization;

using Newtonsoft.Json.Linq;

namespace ASADeserializer
{
    public class MessageBodyDeserializer : StreamDeserializer<CustomEvent>
    {
        // streamingDiagnostics is used to write error to diagnostic logs
        private StreamingDiagnostics streamingDiagnostics;

        // Initializes the operator and provides context that is required for publishing diagnostics
        public override void Initialize(StreamingContext streamingContext)
        {
            this.streamingDiagnostics = streamingContext.Diagnostics;
        }


        public override IEnumerable<CustomEvent> Deserialize(Stream stream)
        {
            bool err = false;
            using (var sr = new StreamReader(stream))
            {

                string line = sr.ReadLine();
                while (line != null)
                {
                    if (line.Length > 0 && !string.IsNullOrWhiteSpace(line))
                    {
                        JObject json = JObject.Parse(line);

                        if (err)
                        {
                            //handle error.
                            streamingDiagnostics.WriteError("Did not get expected number of columns", $"Invalid line: {line}");
                        }

                        // create a new CustomEvent object 
                        yield return new CustomEvent()
                        {
                            c_class = (string)json["class"],
                            c_device = (string)json["device"],
                            c_mode = (int)json["mode"],
                            c_time = (string)json["time"],
                            c_lat = (double)json["lat"],
                            c_lon = (double)json["lon"],
                            c_altMSL = (double)json["altMSL"],
                            c_eph = (double)json["eph"],
                            c_epv = (double)json["epv"],
                            c_eps = (double)json["eps"],
                            c_track = (double)json["track"],
                            c_speed = (double)json["speed"],
                            c_climb = (double)json["climb"],
                            c_pDOP = (double)json["pDOP"]

                        };

                    }
                    line = sr.ReadLine();
                }

            }
        }
    

    }



    /*
   The CustomEvent class follows the rules mentioned below.

   All public fields are either:
       1. One of [long, DateTime, string, double] or their nullableequivalents
       2. Another struct or class following the same rules
       3. Array of type <T2> that follows the same rules
       4. IList`T2` where T2 follows the same rules
       5. Does not have any recursive types.

        SKODA Columns:
        "class": "TPV",
        "device": "/dev/ttyS0",
        "mode": 3,
        "time": "2020-03-31T13:32:11.2990000Z",
        "lat": 60.4626390959428,
        "lon": 24.2126380890667,
        "altMSL": 34.4734914738096,
        "eph": 5.11960312962514E-05,
        "epv": 1.17617510826149,
        "eps": 0.210804721904362,
        "track": 324.683499770092,
        "speed": 14.3356366499027,
        "climb": 0.230969511080053,
        "pDOP": 1.34433096942693,

    */
    public class CustomEvent
    {
        public string c_class { get; set; }
        public string c_device { get; set; }
        public int c_mode { get; set; }
        public string c_time { get; set; }
        public double c_lat { get; set; }
        public double c_lon { get; set; }
        public double c_altMSL { get; set; }
        public double c_eph { get; set; }
        public double c_epv { get; set; }
        public double c_eps { get; set; }
        public double c_track { get; set; }
        public double c_speed { get; set; }
        public double c_climb { get; set; }
        public double c_pDOP { get; set; }





    }
}
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
using System.Text.Json;
using System.Text.Json.Serialization;

using System.Diagnostics;
using System;

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
            using (var sr = new StreamReader(stream))
            {
                var line = sr.ReadToEnd();

                var val = "";
                var err = "X";


                /**********************************************************
                 * Option 1 - custom parse of escaped JSON format
                 **********************************************************/
                var input = line.Substring(2, line.Length - 3).Replace("\\", "");
                string[] elements = input.Split(',');

                CustomEvent ce = new CustomEvent();

                foreach (var el in elements)
                {
                    string[] parts = el.Split(':');
                    var k = parts[0].Trim().Replace("\"", "");
                    var v = parts[1].Trim().Replace("\"", "");
                    //Console.WriteLine(k + " :: " + v);

                    switch (k)
                    {
                        case "class": ce.c_class = v; break;
                        case "device": ce.device = v; break;
                        case "mode": ce.mode = int.Parse(v); break;
                        case "time": ce.time = v; break;
                        case "lat": ce.lat = double.Parse(v); break;
                        case "lon": ce.lon = double.Parse(v); break;
                        case "altMSL": ce.altMSL = double.Parse(v); break;
                        case "eph": ce.eph = double.Parse(v); break;
                        case "epv": ce.epv = double.Parse(v); break;
                        case "eps": ce.eps = double.Parse(v); break;
                        case "track": ce.track = double.Parse(v); break;
                        case "speed": ce.speed = double.Parse(v); break;
                        case "climb": ce.climb = double.Parse(v); break;
                        case "pDOP": ce.pDOP = double.Parse(v); break;

                        default:
                            //Console.WriteLine("ERROR: " + k + " not found ");
                            break;
                    }
                }
                yield return ce;

                /**********************************************************
                 * Option 2 - parse using specialized libraries
                 * 
                 *      **** PREFERED ****
                 *      *** NOT WORKING -> assemly Buffers..."
                 **********************************************************/
                /*
                try
                {
                    val = System.Text.Json.JsonDocument.Parse(line).RootElement.ToString();
                }
                catch (Exception e)
                {
                    err += "| ERR1:" + e.Message;
                }

                var jsonevent = new CustomEvent();
                try
                {
                    jsonevent = JsonSerializer.Deserialize<CustomEvent>(val);

                }
                catch (Exception e)
                {
                    err += "| ERR2:" + e.Message;
                }
                
                // yield return jsonevent;

                // only for DBG
                // create a new CustomEvent object 
                yield return new CustomEvent()
                {
                    c_class = err,
                    device = "device",
                    mode = 12,
                    time = "time",
                    lat = 1.1,
                    lon = 1.1,
                    altMSL = 1.1,
                    eph = 1.1,
                    epv = 1.1,
                    eps = 1.1,
                    track = 1.1,
                    speed = 1.1,
                    climb = 1.1,
                    pDOP = 1.1

                };*/
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
        [JsonPropertyName("class")]
        public string c_class { get; set; }
        public string device { get; set; }
        public int mode { get; set; }
        
        public string time { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public double altMSL { get; set; }
        public double eph { get; set; }
        public double epv { get; set; }
        public double eps { get; set; }
        public double track { get; set; }
        public double speed { get; set; }
        public double climb { get; set; }
        public double pDOP { get; set; }
        
        





    }
}
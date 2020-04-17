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
            //File.WriteAllText("xxx.out.txt","xxx");
            bool err = false;
            using (var sr = new StreamReader(stream))
            {

                string line = sr.ReadLine();
                while (line != null)
                {
                    if (line.Length > 0 && !string.IsNullOrWhiteSpace(line))
                    {

                        //var input = @"""{\""a\"":1}""";
                        //var input = @"""{\""class\"": \""TPV\"", \""device\"": \""/dev/ttyS0\"", \""mode\"": 3}""";
                        var input = line;
                        //File.AppendAllText("xxx.out.txt", line);
                        var val = System.Text.Json.JsonDocument.Parse(input).RootElement.ToString();                 
                        var jsonevent = JsonSerializer.Deserialize<CustomEvent>(val);
 
                        if (err)
                        {
                            //handle error.
                            streamingDiagnostics.WriteError("Did not get expected number of columns", $"Invalid line: {line}");
                        }

                        yield return jsonevent;
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
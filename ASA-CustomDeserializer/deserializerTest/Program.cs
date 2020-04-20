using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace deserializerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            bool err = false;


            var sr = File.ReadAllLines(@"c:\Users\mimarusa\OneDrive - Microsoft\Code\X_TESTS_\azure-iot-demo\ASA-CustomDeserializer\ASACloudDeserializerApplication\ASALocalRun\xxx.out.txt");
            foreach (var line in sr)
            {
                //var input = @"""{\""a\"":1}""";
                //var input = @"""{\""class\"": \""TPV\"", \""device\"": \""/dev/ttyS0\"", \""mode\"": 3}""";
                
                var input = line.Substring(2,line.Length-3).Replace("\\","");
                string[] elements = input.Split(',');

                CustomEvent ce = new CustomEvent();

                foreach (var el in elements)
                {
                    string[] parts = el.Split(":",2);
                    var k = parts[0].Trim().Replace("\"", "");
                    var v = parts[1].Trim().Replace("\"", "");
                    Console.WriteLine(k+" :: "+v);

                    switch (k)
                    {
                        case "class":ce.c_class = v; break;
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
                            Console.WriteLine("ERROR: "+k + " not found ");
                            break;
                    }
                }

                    



                var val = System.Text.Json.JsonDocument.Parse(line).RootElement.ToString();
                var jsonevent = new CustomEvent();
                try
                {
                    jsonevent = JsonSerializer.Deserialize<CustomEvent>(val);

                }
                catch (JsonException e) {
                    continue;
                } 
                //yield return jsonevent;
            }


        }
 
    }

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

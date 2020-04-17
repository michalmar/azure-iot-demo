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


            var sr = File.ReadAllLines(@"c:\Users\mimarusa\OneDrive - Microsoft\Code\X_TESTS_\azure-iot-demo\ASA-CustomDeserializer\ASACloudDeserializerApplication\ASALocalRun\2020-04-17-15-40-27\xxx.out.txt");
            foreach (var line in sr)
            {
                //var input = @"""{\""a\"":1}""";
                //var input = @"""{\""class\"": \""TPV\"", \""device\"": \""/dev/ttyS0\"", \""mode\"": 3}""";
                var input = line.Replace("\\","");
                //var val = System.Text.Json.JsonDocument.Parse(input).RootElement.ToString();
                
                var jsonevent = JsonSerializer.Deserialize<CustomEvent>(input);
 
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
    /*
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
    */






}
}

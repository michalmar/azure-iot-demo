// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Microsoft.Azure.Devices.Client.Samples
{
    public class MessageSample
    {
        private const int MessageCount = 10 ;
        private const int TemperatureThreshold = 30;
        private static Random s_randomGenerator = new Random();
        private float _temperature;
        private float _humidity;
        private readonly DeviceClient _deviceClient;

        public MessageSample(DeviceClient deviceClient)
        {
            _deviceClient = deviceClient ?? throw new ArgumentNullException(nameof(deviceClient));
        }

        public async Task RunSampleAsync()
        {
            // string dataBuffer = $"{{\"class\": \"TPV\", \"device\": \"/dev/ttyS{s_randomGenerator.Next(0,3)}\", \"mode\": 3, \"time\": \"2020-03-31T13:32:11.299Z\", \"lat\":{s_randomGenerator.Next(60, 61)+s_randomGenerator.NextDouble()}, \"lon\": {s_randomGenerator.Next(24, 25)+s_randomGenerator.NextDouble()}, \"altMSL\": {s_randomGenerator.Next(33, 36)+s_randomGenerator.NextDouble()}, \"eph\": {s_randomGenerator.NextDouble()/10000}, \"epv\": {1+s_randomGenerator.NextDouble()}, \"eps\": {s_randomGenerator.NextDouble()}, \"track\": {s_randomGenerator.Next(324, 326)+s_randomGenerator.NextDouble()}, \"speed\": {s_randomGenerator.Next(5,50)+s_randomGenerator.NextDouble()}, \"climb\": {s_randomGenerator.Next(-1, +1)+s_randomGenerator.NextDouble()}, \"pDOP\": {s_randomGenerator.Next(1, 2)+s_randomGenerator.NextDouble()}}}";
            // Console.WriteLine("DBG: Device sending {0} ...\n", dataBuffer);
            await SendEventAsync().ConfigureAwait(false);
            // await ReceiveMessagesAsync().ConfigureAwait(false);
        }

        private async Task SendEventAsync()
        {
            Console.WriteLine("Device sending {0} messages to IoTHub...\n", MessageCount);

            for (int count = 0; count < MessageCount; count++)
            {
                _temperature = s_randomGenerator.Next(20, 35);
                _humidity = s_randomGenerator.Next(60, 80);
                // string dataBuffer = $"{{\"messageId\":{count},\"temperature\":{_temperature},\"humidity\":{_humidity}}}";
                
                string dataBuffer = $"{{\"class\": \"TPV\", \"device\": \"/dev/ttyS{s_randomGenerator.Next(0,3)}\", \"mode\": 3, \"time\": \"2020-03-31T13:32:11.299Z\", \"lat\":{s_randomGenerator.Next(60, 61)+s_randomGenerator.NextDouble()}, \"lon\": {s_randomGenerator.Next(24, 25)+s_randomGenerator.NextDouble()}, \"altMSL\": {s_randomGenerator.Next(33, 36)+s_randomGenerator.NextDouble()}, \"eph\": {s_randomGenerator.NextDouble()/10000}, \"epv\": {1+s_randomGenerator.NextDouble()}, \"eps\": {s_randomGenerator.NextDouble()}, \"track\": {s_randomGenerator.Next(324, 326)+s_randomGenerator.NextDouble()}, \"speed\": {s_randomGenerator.Next(5,50)+s_randomGenerator.NextDouble()}, \"climb\": {s_randomGenerator.Next(-1, +1)+s_randomGenerator.NextDouble()}, \"pDOP\": {s_randomGenerator.Next(1, 2)+s_randomGenerator.NextDouble()}}}";
                
                using (var eventMessage = new Message(Encoding.UTF8.GetBytes(dataBuffer)))
                {
                    // eventMessage.Properties.Add("temperatureAlert", (_temperature > TemperatureThreshold) ? "true" : "false");
                    Console.WriteLine("\t{0}> Sending message: {1}, Data: [{2}]", DateTime.Now.ToLocalTime(), count, dataBuffer);
                    await _deviceClient.SendEventAsync(eventMessage).ConfigureAwait(false);
                }
                Thread.Sleep(s_randomGenerator.Next(500, 2000));
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            Console.WriteLine("\nDevice waiting for C2D messages from the hub...\n");
            Console.WriteLine("Use the IoT Hub Azure Portal or Azure IoT Explorer to send a message to this device.\n");

            using (Message receivedMessage = await _deviceClient.ReceiveAsync(TimeSpan.FromSeconds(30)).ConfigureAwait(false))
            {
                if (receivedMessage != null)
                {
                    string messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("\t{0}> Received message: {1}", DateTime.Now.ToLocalTime(), messageData);

                    int propCount = 0;
                    foreach (var prop in receivedMessage.Properties)
                    {
                        Console.WriteLine("\t\tProperty[{0}> Key={1} : Value={2}", propCount++, prop.Key, prop.Value);
                    }

                    await _deviceClient.CompleteAsync(receivedMessage).ConfigureAwait(false);
                }
                else
                {
                    Console.WriteLine("\t{0}> Timed out", DateTime.Now.ToLocalTime());
                }
            }
        }
    }
}

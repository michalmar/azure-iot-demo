# azure-iot-demo


This demo consists of two parts:
## 1. Message Generator
## 2. Stream Analytics Custom Deserializer


## 1. Message Generator

### .NET make application (exe)

Win
`dotnet publish -c Release -r win10-x64`

Linux
`dotnet publish -c Release -r ubuntu.16.10-x64`


**SQL Sample**
Used for processing messages
```SQL

SELECT
    class as class,
    device as device,
    MAX(mode) as mode,
    AVG(lat) as lat,
    AVG(lon) as lon,
    MAX(altMSL) as altMSL,
    MAX(eph) as eph,
    MAX(epv) as epv,
    MAX(eps) as eps,
    MAX(track) as track,
    MAX(speed) as speed,
    MAX(climb) as climb,
    MAX(pDOP) as pDOP,
    System.TimeStamp AS time
INTO [pbi]
FROM
    [iothub] TIMESTAMP BY EventProcessedUtcTime
GROUP BY
    class,
    device,
    TUMBLINGWINDOW(second,5)

```

## 2. Stream Analytics Custom Deserializer

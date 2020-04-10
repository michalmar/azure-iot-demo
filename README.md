# azure-iot-demo

## .NET make application (exe)

Win
`dotnet publish -c Release -r win10-x64`

Linux
`dotnet publish -c Release -r ubuntu.16.10-x64`


## SQL Sample
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

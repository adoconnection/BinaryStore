# BinaryStore
NET binary serializer optimized for random reads/writes. <br>
Database replacement for telemetry/sensor data or logs.

* Best works for lists of fixed length objects 
* Instant reads and writes for any position in list
* Individual records corruption does not affect others
* 5-10x faster writes vs NewtonsoftJson
* 2-5x faster reads vs NewtonsoftJson

## NuGet
```
coming soon
```

## How come?
Target object must be of fixed binary length (like we know int is 4 bytes)
Given fixed length, we can Seek stream position to read/write any record index.
Serialization is as fast as converting value to byte array and back.



## Examples

```cs
public record Person
{
    public int Id { get; set; } // 4 bytes
    public DateTime Birthday { get; set; } // 8 bytes

    [BinaryStoreRecordLength(30)] // limit attribute for strings, arrays atc
    public string FirstName { get; set; }

    [BinaryStoreRecordLength(30)]
    public string LastName { get; set; }
}
```

### Create store and append records
```cs
BinaryStore<Person> binaryStore = new BinaryStore<Person>(stream);

binaryStore.Append(new Person()
{
    Id = 1,
    FirstName = "FirstName 1",
    LastName = "LastName 1",
    Birthday = new DateTime(2001, 1, 1)
});

binaryStore.Append(new Person()
{
    Id = 2,
    FirstName = "FirstName 2",
    LastName = "LastName 2",
    Birthday = new DateTime(2002, 2, 2)
});

```


### Read store contents:
```cs
Console.WriteLine("Records in store: " + binaryStore.GetRecordsCount());

for (int i = 0; i < binaryStore.GetRecordsCount(); i++)
{
    Console.WriteLine(i + " - " + binaryStore.Read(i));
}
```

## External Lock

```cs
private static object LockObject = new object();

//...

BinaryStore<Person> binaryStore = new BinaryStore<Person>(stream, LockObject);
```

## Options

```cs
BinaryStore<Person> binaryStore = new BinaryStore<Person>(stream, builder =>
{
    builder.BinaryAttributePropertiesOnly = true; // serialize only properties with [BinaryStoreRecord] attribute
    builder.SeekToBeginningAtStartup = true; // goto stream 0 at stratup 
    builder.Serializers.Add(new BinaryStoreByteArraySerializer()); // add your own type serializers
});
```

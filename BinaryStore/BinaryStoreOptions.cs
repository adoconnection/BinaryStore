using BinaryStore.Serializers;

namespace BinaryStore;

public class BinaryStoreOptions
{
    public object LockObject { get; set; } = new object();
    public IList<string> TargetProperties { get; set; }
    public bool BinaryAttributePropertiesOnly { get; set; }
    public bool SeekToBeginningAtStartup { get; set; } = true;
    public bool FlushOnWrite { get; set; } = true;

    public IList<IBinaryStoreSerializer> Serializers { get; set; } = new List<IBinaryStoreSerializer>()
    {
        new BinaryStoreBoolSerializer(),
        new BinaryStoreByteSerializer(),
        new BinaryStoreIntegerSerializer(),
        new BinaryStoreLongSerializer(),
        new BinaryStoreDoubleSerializer(),
        new BinaryStoreDateTimeSerializer(),
        new BinaryStoreTimeSpanSerializer(),
        new BinaryStoreGuidSerializer(),
        new BinaryStoreStringSerializer(),

        new BinaryStoreByteArraySerializer(),
    };


}
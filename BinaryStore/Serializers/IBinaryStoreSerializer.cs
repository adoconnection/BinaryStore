namespace BinaryStore.Serializers;

public interface IBinaryStoreSerializer
{
    int Length { get; }
    bool RequiresLengthAttribute { get; }
    Type Type { get; }
    void Serialize(Stream stream, object value, long lengthLimit);
    object Deserialize(Stream stream, long lengthLimit);
}   
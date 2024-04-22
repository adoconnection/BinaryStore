using System.Reflection;
using BinaryStore.Serializers;

namespace BinaryStore;

internal record BinaryStoreTypeProperty
{
    public PropertyInfo PropertyInfo { get; set; }
    public long Length { get; set; }
    public IBinaryStoreSerializer Serializer { get; set; }

    public void Serialize(Stream stream, object source)
    {
        object? value = this.PropertyInfo.GetValue(source);
        this.Serializer.Serialize(stream, value, this.Length);
    }
    public void Deserialize(Stream stream, object target)
    {
        this.PropertyInfo.SetValue(target, this.Serializer.Deserialize(stream, this.Length));
    }
}


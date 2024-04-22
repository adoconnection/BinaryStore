namespace BinaryStore.Serializers;

public class BinaryStoreGuidSerializer : IBinaryStoreSerializer
{
    public int Length
    {
        get
        {
            return 16;
        }
    }


    public bool RequiresLengthAttribute
    {
        get
        {
            return false;
        }
    }
    public Type Type
    {
        get
        {
            return typeof(Guid);
        }
    }

    public void Serialize(Stream stream, object value, long lengthLimit)
    {
        stream.Write(((Guid)value).ToByteArray());
    }

    public object Deserialize(Stream stream, long lengthLimit)
    {
        byte[] buffer = new byte[this.Length];
        stream.Read(buffer, 0, this.Length);

        return new Guid(buffer);
    }
}
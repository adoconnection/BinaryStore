namespace BinaryStore.Serializers;

public class BinaryStoreLongSerializer : IBinaryStoreSerializer
{
    public int Length
    {
        get
        {
            return 8;
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
            return typeof(long);
        }
    }

    public void Serialize(Stream stream, object value, long lengthLimit)
    {
        stream.Write(BitConverter.GetBytes((long)value));
    }

    public object Deserialize(Stream stream, long lengthLimit)
    {
        byte[] buffer = new byte[this.Length];

        stream.Read(buffer, 0, this.Length);
        return BitConverter.ToInt64(buffer);
    }
}
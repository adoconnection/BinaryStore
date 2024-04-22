namespace BinaryStore.Serializers;

public class BinaryStoreDateTimeSerializer : IBinaryStoreSerializer
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
            return typeof(DateTime);
        }
    }

    public void Serialize(Stream stream, object value, long lengthLimit)
    {
        stream.Write(BitConverter.GetBytes(((DateTime)value).Ticks));
    }

    public object Deserialize(Stream stream, long lengthLimit)
    {
        byte[] buffer = new byte[this.Length];


        stream.Read(buffer, 0, this.Length);
        long tics = BitConverter.ToInt64(buffer);

        return new DateTime(tics);
    }
}
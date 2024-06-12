namespace BinaryStore.Serializers;

public class BinaryStoreByteArraySerializer : IBinaryStoreSerializer
{
    public int Length
    {
        get
        {
            return -1;
        }
    }

    public bool RequiresLengthAttribute
    {
        get
        {
            return true;
        }
    }

    public Type Type
    {
        get
        {
            return typeof(byte[]);
        }
    }

    public void Serialize(Stream stream, object value, long lengthLimit)
    {
        byte[] bytes = (byte[])value;

        stream.Write(BitConverter.GetBytes(bytes.Length));
        stream.Write(bytes);

        stream.Position += lengthLimit - bytes.Length - 4;
    }

    public object Deserialize(Stream stream, long lengthLimit)
    {
        byte[] buffer = new byte[4];

        stream.Read(buffer, 0, 4);
        int length = BitConverter.ToInt32(buffer);

        buffer = new byte[length];
        stream.Read(buffer, 0, length);

        stream.Position += lengthLimit - length - 4;

        return buffer;
    }
}
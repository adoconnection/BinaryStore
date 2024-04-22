using System.Text;

namespace BinaryStore.Serializers;

public class BinaryStoreStringSerializer : IBinaryStoreSerializer
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
            return typeof(string);
        }
    }

    public void Serialize(Stream stream, object value, long lengthLimit)
    {
        string stringValue = (string)value;

        byte[] bytes = Encoding.Unicode.GetBytes(stringValue);

        if (bytes.Length + 4 > lengthLimit)
        {
            throw new BinaryStoreException("Field limit exceeded");
        }

        stream.Write(BitConverter.GetBytes(bytes.Length));
        stream.Write(bytes);

        var bytesLength = lengthLimit - bytes.Length - 4;

        stream.Write(new byte[bytesLength], 0, (int)bytesLength);
    }

    public object Deserialize(Stream stream, long lengthLimit)
    {
        byte[] buffer = new byte[4];

        stream.Read(buffer, 0, 4);
        int length = BitConverter.ToInt32(buffer);

        buffer = new byte[length];
        stream.Read(buffer, 0, length);

        stream.Seek(lengthLimit - length - 4, SeekOrigin.Current);

        return Encoding.Unicode.GetString(buffer);
    }
}
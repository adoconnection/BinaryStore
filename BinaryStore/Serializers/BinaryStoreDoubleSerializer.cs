using System.Reflection;
using System;

namespace BinaryStore.Serializers;

public class BinaryStoreDoubleSerializer : IBinaryStoreSerializer
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
            return typeof(double);
        }
    }

    public void Serialize(Stream stream, object value, long lengthLimit)
    {
        stream.Write(BitConverter.GetBytes((double)value));
    }

    public object Deserialize(Stream stream, long lengthLimit)
    {
        byte[] buffer = new byte[this.Length];

        stream.Read(buffer, 0, this.Length);
        return BitConverter.ToDouble(buffer);
    }
}
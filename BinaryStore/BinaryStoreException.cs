using System.Runtime.Serialization;

namespace BinaryStore;

public class BinaryStoreException : Exception
{
    public BinaryStoreException()
    {
    }

    protected BinaryStoreException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public BinaryStoreException(string message) : base(message)
    {
    }

    public BinaryStoreException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
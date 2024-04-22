namespace BinaryStore
{
    public class BinaryStore<T> where T : new()
    {
        private BinaryStoreOptions options = new BinaryStoreOptions();
        private readonly Stream stream;

        private readonly long recordLength;
        private readonly IList<BinaryStoreTypeProperty> properties;
        private long position = 0;

        public BinaryStore(Stream stream) : this(stream, _ => {})
        {
        }

        public BinaryStore(Stream stream, object lockObject) : this(stream, storeOptions => { storeOptions.LockObject = lockObject;})
        {

        }

        public BinaryStore(Stream stream, Action<BinaryStoreOptions> builder)
        {
            this.stream = stream;
            builder(this.options);

            if (this.options.SeekToBeginningAtStartup)
            {
                this.stream.Seek(0, SeekOrigin.Begin);
            }

            this.properties = typeof(T).GetProperties(
                this.options.TargetProperties,
                this.options.BinaryAttributePropertiesOnly,
                this.options.Serializers);

            this.recordLength = this.properties.Sum(p => p.Length) + 1;
        }

        public T Read(int recordId)
        {
            lock (this.options.LockObject)
            {
                this.SeekToRecord(recordId);
                this.position = recordId + 1;

                bool value = this.stream.ReadBoolean();

                if (!value)
                {
                    stream.Seek(this.recordLength, SeekOrigin.Current);
                    return default(T);
                }

                T instance = new T();

                foreach (BinaryStoreTypeProperty property in this.properties)
                {
                    property.Deserialize(this.stream, instance);
                }

                return instance;
            }
        }

        public void Write(T entry, int recordId)
        {
            lock (this.options.LockObject)
            {
                this.SeekToRecord(recordId);
                this.position = recordId + 1;

                if (entry == null)
                {
                    this.stream.Write(new byte[this.recordLength]);
                    return;
                }

                this.stream.WriteBoolean(true);

                foreach (BinaryStoreTypeProperty property in this.properties)
                {
                    property.Serialize(stream, entry);
                }

                stream.Flush();
            }
        }

        public void Append(T entry)
        {
            long recordId = this.GetRecordsCount();
            this.Write(entry, (int)recordId);
        }

        public int GetRecordsCount()
        {
            return (int)(this.stream.Length / this.recordLength);
        }

        private void SeekToRecord(long recordId)
        {
            if (recordId - this.position != 0)
            {
                this.stream.Seek((recordId - this.position) * this.recordLength, SeekOrigin.Current);
            }
        }

    }
}

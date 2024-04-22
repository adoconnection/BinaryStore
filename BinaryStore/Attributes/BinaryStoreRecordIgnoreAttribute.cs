using System;

namespace BinaryStore.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BinaryStoreRecordIgnoreAttribute : Attribute
    {
        public BinaryStoreRecordIgnoreAttribute()
        {
        }
    }
}
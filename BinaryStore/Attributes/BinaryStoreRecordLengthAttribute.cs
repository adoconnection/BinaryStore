using System;

namespace BinaryStore.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BinaryStoreRecordLengthAttribute : Attribute
    {
        public int Length { get; set; }

        public BinaryStoreRecordLengthAttribute(int length)
        {
            Length = length;
        }
    }
}
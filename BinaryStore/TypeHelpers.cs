using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BinaryStore.Attributes;
using BinaryStore.Serializers;

namespace BinaryStore
{
    internal static class TypeHelpers
    {
        internal static bool ReadBoolean(this Stream stream)
        {
            byte[] buffer = new byte[1];
            stream.Read(buffer, 0, 1);

            return BitConverter.ToBoolean(buffer);
        }
        internal static void WriteBoolean(this Stream stream, bool value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }

        internal static IList<BinaryStoreTypeProperty> GetProperties(this Type type, IList<string> propertiesToTake, bool attributePropertiesOnly, IList<IBinaryStoreSerializer> serializers)
        {
            IList<BinaryStoreTypeProperty> result = new List<BinaryStoreTypeProperty>();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                BinaryStoreRecordIgnoreAttribute? recordIgnoreAttribute = propertyInfo.GetCustomAttribute<BinaryStoreRecordIgnoreAttribute>();

                if (recordIgnoreAttribute != null)
                {
                    continue;
                }

                if (propertiesToTake != null && propertiesToTake.Count > 0 && !propertiesToTake.Contains(propertyInfo.Name))
                {
                    continue;
                }

                if (attributePropertiesOnly && propertyInfo.GetCustomAttribute<BinaryStoreRecordAttribute>() == null)
                {
                    continue;
                }

                IBinaryStoreSerializer serializer = serializers.FirstOrDefault(s => s.Type == propertyInfo.PropertyType);

                if (serializer == null)
                {
                    throw new BinaryStoreException("Property " + propertyInfo.Name + " is not supported by any any of serializers");
                }

                int length = serializer.Length;

                if (serializer.RequiresLengthAttribute)
                {
                    BinaryStoreRecordLengthAttribute? recordLengthAttribute = propertyInfo.GetCustomAttribute<BinaryStoreRecordLengthAttribute>();

                    if (recordLengthAttribute == null)
                    {
                        throw new BinaryStoreException("Property " + propertyInfo.Name + " requires BinaryStoreRecordLength attribute");
                    }

                    length = recordLengthAttribute.Length;
                }

                result.Add(new BinaryStoreTypeProperty()
                {
                    Length = length ,
                    PropertyInfo = propertyInfo,
                    Serializer = serializer
                });
            }

            return result;
        }
    }
}
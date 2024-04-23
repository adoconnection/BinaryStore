using BinaryStore;
using BinaryStore.Serializers;

namespace ExampleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists("Persons.bin"))
            {
                File.Delete("Persons.bin");
            }

            using Stream stream = File.Open("Persons.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            BinaryStore<Person> binaryStore = new BinaryStore<Person>(stream, builder =>
            {
                builder.BinaryAttributePropertiesOnly = true;
                builder.SeekToBeginningAtStartup = true;
                builder.Serializers.Add(new BinaryStoreByteArraySerializer());
            });

            binaryStore.Append(new Person()
            {
                Id = 1,
                FirstName = "FirstName 1",
                LastName = "LastName 1",
                Birthday = new DateTime(2001, 1, 1)
            });

            binaryStore.Append(new Person()
            {
                Id = 2,
                FirstName = "FirstName 2",
                LastName = "LastName 2",
                Birthday = new DateTime(2002, 2, 2)
            });

            binaryStore.Append(new Person()
            {
                Id = 3,
                FirstName = "FirstName 3",
                LastName = "LastName 3",
                Birthday = new DateTime(2003, 3, 3)
            });


            Console.WriteLine("Records in store: " + binaryStore.GetRecordsCount());

            for (int i = 0; i < binaryStore.GetRecordsCount(); i++)
            {
                Console.WriteLine(i + " - " + binaryStore.Read(i));
            }


            Console.WriteLine("Hello, World!");
        }
    }
}

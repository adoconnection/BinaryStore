using BinaryStore;
using BinaryStore.Attributes;
using Newtonsoft.Json;

namespace Benchmark
{
    public record Person
    {
        public int Id { get; set; }

        [BinaryStoreRecordLength(40)]
        public string FirstName { get; set; }

        [BinaryStoreRecordLength(40)]
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            IList<Person> persons = new List<Person>();

            for (int i = 0; i < 1000; i++)
            {
                persons.Add(new Person()
                {
                    Id = i,
                    FirstName = "FirstName " + i,
                    LastName = "LastName " + i,
                    Birthday = new DateTime(2001, 1, 1)
                });
            }

            Write(persons);
            Read();
        }

        private static void Write(IList<Person> persons)
        {
            using Stream stream1 = File.Open("file1.binxx", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            using Stream stream2 = File.Open("file2.bin", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

            DateTime dateTime11 = DateTime.Now;

            string serializeObject = JsonConvert.SerializeObject(persons);
            File.WriteAllText("file1.bin", serializeObject);

            DateTime dateTime12 = DateTime.Now;

            Console.WriteLine("JsonConvert - " + (dateTime12 - dateTime11).Milliseconds + " ms");

            DateTime dateTime1 = DateTime.Now;


            BinaryStore<Person> binaryStore = new BinaryStore<Person>(stream2);

            for (var index = 0; index < persons.Count; index++)
            {
                var person = persons[index];
                binaryStore.Write(person, index);
            }

            DateTime dateTime2 = DateTime.Now;

            Console.WriteLine("BinaryStore - " + (dateTime2 - dateTime1).Milliseconds + " ms");

            stream1.Flush();
            stream2.Flush();
        }
        private static void Read()
        {
           
           
            DateTime dateTime11 = DateTime.Now;

            IList<Person> persons1 = JsonConvert.DeserializeObject<List<Person>>(File.ReadAllText("file1.bin"));

            DateTime dateTime12 = DateTime.Now;

            Console.WriteLine("JsonConvert - " + (dateTime12 - dateTime11).Milliseconds + " ms");


            DateTime dateTime1 = DateTime.Now;

            using Stream stream2 = File.Open("file2.bin", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            BinaryStore<Person> binaryStore = new BinaryStore<Person>(stream2);

            IList<Person> persons2 = new List<Person>();

            for (var index = 0; index < binaryStore.GetRecordsCount(); index++)
            {
                persons2.Add(binaryStore.Read(index));
            }

            DateTime dateTime2 = DateTime.Now;

            Console.WriteLine("BinaryStore - " + (dateTime2 - dateTime1).Milliseconds + " ms");

        }
    }
}

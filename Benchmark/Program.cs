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

            for (int i = 0; i < 50000; i++)
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

            DateTime dateTime11 = DateTime.Now;

            string serializeObject = JsonConvert.SerializeObject(persons);
            File.WriteAllText("newtonsoft.json", serializeObject);

            DateTime dateTime12 = DateTime.Now;

            Console.WriteLine("Write JsonConvert - " + (dateTime12 - dateTime11).Milliseconds + " ms");


            DateTime dateTime1 = DateTime.Now;

            using Stream stream2 = File.Open("binarystore.bin", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

            BinaryStore<Person> binaryStore = new BinaryStore<Person>(stream2, options =>
            {
                options.FlushOnWrite = false;
            });

            for (var index = 0; index < persons.Count; index++)
            {
                var person = persons[index];
                binaryStore.Write(person, index);
            }

            DateTime dateTime2 = DateTime.Now;

            Console.WriteLine("Write BinaryStore - " + (dateTime2 - dateTime1).Milliseconds + " ms");

            stream2.Flush();
            stream2.Dispose();

        }
        private static void Read()
        {
            DateTime dateTime11 = DateTime.Now;

            IList<Person> persons1 = JsonConvert.DeserializeObject<List<Person>>(File.ReadAllText("newtonsoft.json"));

            DateTime dateTime12 = DateTime.Now;

            Console.WriteLine(persons1.Count);
            Console.WriteLine("Read JsonConvert - " + (dateTime12 - dateTime11).Milliseconds + " ms");


            DateTime dateTime1 = DateTime.Now;

            using Stream stream2 = File.Open("binarystore.bin", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            BinaryStore<Person> binaryStore = new BinaryStore<Person>(stream2);

            IList<Person> persons2 = new List<Person>();

            for (var index = 0; index < binaryStore.GetRecordsCount(); index++)
            {
                persons2.Add(binaryStore.Read(index));
            }

            DateTime dateTime2 = DateTime.Now;

            Console.WriteLine(persons2.Count);
            Console.WriteLine("Read BinaryStore - " + (dateTime2 - dateTime1).Milliseconds + " ms");

        }
    }
}

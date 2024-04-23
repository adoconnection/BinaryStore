using System.IO;

namespace BinaryStore.Tests
{
    [TestClass]
    public class AppendTests
    {
        [TestMethod]
        public void AppendOne()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryStore<Person> binaryStore = new BinaryStore<Person>(memoryStream);

            Person source = new Person()
            {
                Id = 1,
                LastName = "Last Name",
                FirstName = "First Name",
                Birthday = new DateTime(2022, 01, 02, 12, 0,0)
            };

            binaryStore.Append(source);

            Person newPerson = binaryStore.Read(0);

            Assert.AreEqual(source.Id, newPerson.Id);
            Assert.AreEqual(source.LastName, newPerson.LastName);
            Assert.AreEqual(source.FirstName, newPerson.FirstName);
            Assert.AreEqual(source.Birthday, newPerson.Birthday);
        }

        [TestMethod]
        public void AppendMany()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryStore<Person> binaryStore = new BinaryStore<Person>(memoryStream);

            IList<Person> persons = new List<Person>()
            {
                new Person()
                {
                    Id = 1,
                    LastName = "Last Name 1",
                    FirstName = "First Name 1",
                    Birthday = new DateTime(2021, 01, 02, 12, 0, 1)
                },
                new Person()
                {
                    Id = 2,
                    LastName = "Last Name 2",
                    FirstName = "First Name 2",
                    Birthday = new DateTime(2022, 01, 02, 12, 0, 2)
                },
                new Person()
                {
                    Id = 3,
                    LastName = "Last Name 3",
                    FirstName = "First Name 3",
                    Birthday = new DateTime(2023, 01, 02, 12, 0, 3)
                }
            };

            foreach (Person person in persons)
            {
                binaryStore.Append(person);
            }


            for (var index = 0; index < persons.Count; index++)
            {
                Person source = persons[index];
                Person newPerson = binaryStore.Read(index);

                Assert.AreEqual(source.Id, newPerson.Id);
                Assert.AreEqual(source.LastName, newPerson.LastName);
                Assert.AreEqual(source.FirstName, newPerson.FirstName);
                Assert.AreEqual(source.Birthday, newPerson.Birthday);
            }
        }

        [TestMethod]
        public void AppendNullMany()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryStore<Person> binaryStore = new BinaryStore<Person>(memoryStream);

            IList<Person> persons = new List<Person>()
            {
                new Person()
                {
                    Id = 1,
                    LastName = "Last Name 1",
                    FirstName = "First Name 1",
                    Birthday = new DateTime(2021, 01, 02, 12, 0, 1)
                },
                null,
                new Person()
                {
                    Id = 3,
                    LastName = "Last Name 3",
                    FirstName = "First Name 3",
                    Birthday = new DateTime(2023, 01, 02, 12, 0, 3)
                }
            };

            foreach (Person person in persons)
            {
                binaryStore.Append(person);
            }

            for (var index = 0; index < persons.Count; index++)
            {
                Person source = persons[index];
                Person newPerson = binaryStore.Read(index);

                if (source == null)
                {
                    Assert.AreEqual(null, newPerson);
                }
                else
                {
                    Assert.AreEqual(source.Id, newPerson.Id);
                    Assert.AreEqual(source.LastName, newPerson.LastName);
                    Assert.AreEqual(source.FirstName, newPerson.FirstName);
                    Assert.AreEqual(source.Birthday, newPerson.Birthday);
                }
            }
        }
    }
}
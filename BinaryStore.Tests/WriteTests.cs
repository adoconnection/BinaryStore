using System;

namespace BinaryStore.Tests;
[TestClass]
public class WriteTests
{
    [TestMethod]
    public void WriteFirst()
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryStore<Person> binaryStore = new BinaryStore<Person>(memoryStream);

        Person source = new Person()
        {
            Id = 1,
            LastName = "Last Name",
            FirstName = "First Name",
            Birthday = new DateTime(2022, 01, 02, 12, 0, 0)
        };

        binaryStore.Write(source, 0);

        Person newPerson = binaryStore.Read(0);

        Assert.AreEqual(source.Id, newPerson.Id);
        Assert.AreEqual(source.LastName, newPerson.LastName);
        Assert.AreEqual(source.FirstName, newPerson.FirstName);
        Assert.AreEqual(source.Birthday, newPerson.Birthday);
    }

    [TestMethod]
    public void WritePosition()
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryStore<Person> binaryStore = new BinaryStore<Person>(memoryStream);

        Person source = new Person()
        {
            Id = 1,
            LastName = "Last Name",
            FirstName = "First Name",
            Birthday = new DateTime(2022, 01, 02, 12, 0, 0)
        };

        binaryStore.Write(source, 1);

        Assert.AreEqual(null, binaryStore.Read(0));

        Person person = binaryStore.Read(1);

        Assert.AreEqual(source.Id, person.Id);
        Assert.AreEqual(source.LastName, person.LastName);
        Assert.AreEqual(source.FirstName, person.FirstName);
        Assert.AreEqual(source.Birthday, person.Birthday);
    }

    [TestMethod]
    public void ReplaceWithData()
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

        var source = new Person()
        {
            Id = 100,
            LastName = "New Last",
            FirstName = "New First",
            Birthday = new DateTime(2000, 01, 02, 12, 0, 0)
        };

        binaryStore.Write(source, 1);

        Assert.AreEqual(persons[0].Id, binaryStore.Read(0).Id);
        Assert.AreEqual(persons[0].LastName, binaryStore.Read(0).LastName);
        Assert.AreEqual(persons[0].FirstName, binaryStore.Read(0).FirstName);
        Assert.AreEqual(persons[0].Birthday, binaryStore.Read(0).Birthday);

        Assert.AreEqual(source.Id, binaryStore.Read(1).Id);
        Assert.AreEqual(source.LastName, binaryStore.Read(1).LastName);
        Assert.AreEqual(source.FirstName, binaryStore.Read(1).FirstName);
        Assert.AreEqual(source.Birthday, binaryStore.Read(1).Birthday);

        Assert.AreEqual(persons[2].Id, binaryStore.Read(2).Id);
        Assert.AreEqual(persons[2].LastName, binaryStore.Read(2).LastName);
        Assert.AreEqual(persons[2].FirstName, binaryStore.Read(2).FirstName);
        Assert.AreEqual(persons[2].Birthday, binaryStore.Read(2).Birthday);
    }

    [TestMethod]
    public void ReplaceWithNull()
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

        binaryStore.Write(null, 1);

        Assert.AreEqual(persons[0].Id, binaryStore.Read(0).Id);
        Assert.AreEqual(persons[0].LastName, binaryStore.Read(0).LastName);
        Assert.AreEqual(persons[0].FirstName, binaryStore.Read(0).FirstName);
        Assert.AreEqual(persons[0].Birthday, binaryStore.Read(0).Birthday);

        Assert.AreEqual(null, binaryStore.Read(1));

        Assert.AreEqual(persons[2].Id, binaryStore.Read(2).Id);
        Assert.AreEqual(persons[2].LastName, binaryStore.Read(2).LastName);
        Assert.AreEqual(persons[2].FirstName, binaryStore.Read(2).FirstName);
        Assert.AreEqual(persons[2].Birthday, binaryStore.Read(2).Birthday);
    }
}
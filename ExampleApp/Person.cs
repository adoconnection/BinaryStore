using BinaryStore.Attributes;

namespace ExampleApp;

public record Person
{
    public int Id { get; set; }

    [BinaryStoreRecordLength(30)]
    public string FirstName { get; set; }

    [BinaryStoreRecordLength(30)]
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
}
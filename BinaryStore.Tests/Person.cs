using BinaryStore.Attributes;

namespace BinaryStore.Tests;

public record Person
{
    public int Id { get; set; }

    [BinaryStoreRecordLength(30)]
    public string FirstName { get; set; }

    [BinaryStoreRecordLength(30)]
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }

    [BinaryStoreRecordIgnore]
    public string Code { get; set; }
}
namespace SerialSnifferForCRT711;

public class ByteCommand(string name, byte[] data)
{
    public string Name { get; init; } = name;
    public byte[] Data { get; set;  } = data;
}
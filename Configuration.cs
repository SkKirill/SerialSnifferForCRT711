namespace SerialSniffer;

public class Configuration
{
    public int BaudRate { get; init; } = 115200;
    public PortSnifferConfiguration PortSnifferConfigurations { get; init; }
}

public class PortSnifferConfiguration
{
    public string Description { get; init; }
    public string SerialPortName { get; init; }
}
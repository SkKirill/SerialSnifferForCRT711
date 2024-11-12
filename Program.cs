using System.IO.Ports;

var baudRate = 115200;
var description = "считыватель карт";
var SerialPortName = "COM6";

Dictionary<string, SerialPort> serialPorts = new();

try
{
    try
    {
        var newPort = new SerialPort(SerialPortName, baudRate);
        newPort.Open();
        serialPorts.Add(SerialPortName, newPort);
    }
    catch (Exception exception)
    {
        Console.WriteLine($"Не удалось подключить порт {SerialPortName}" + exception.Message);
    }

    const int value30H = 0x30; //0b0001_1110;
    const int valuePm = 031; //0b0001_1111;
    var dataToSend = new byte[] { (byte)'C', value30H, valuePm};
    
    var port = serialPorts[SerialPortName];
    
    port.Write(dataToSend, 0, dataToSend.Length);
    Console.WriteLine($"Отправили на порт {SerialPortName}: {value30H}");

    Console.Write("Получили: ");
    while (port.BytesToRead > 0)
    {
        Console.Write($"0x{port.ReadByte():X2}, ");
    }

    Console.WriteLine();
}
catch (Exception exception)
{
    Console.WriteLine(exception);
}
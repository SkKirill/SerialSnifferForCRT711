using System.IO.Ports;

var baudRates = new[] { 9600, 19200, 38400, 57600 };
const string serialPortName = "/dev/ttyUSB0";

try
{
    const int valueCMT = 0x43; // (byte)'C'
    
    foreach (var baudRate in baudRates)
    {
        SerialPort port;
        // Открываем соединение с портом в новом baudRate
        try
        {
            port = new SerialPort(serialPortName, baudRate: baudRate);
            port.Open();
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Не удалось подключить порт {serialPortName}" + exception.Message);
            continue;
        }

        var dataToSend = new byte[] { valueCMT };

        port.Write(dataToSend, 0, dataToSend.Length);

        Console.Write($"Отправили port={serialPortName} baudRate={baudRate} -> ");
        foreach (var item in dataToSend)
        {
            Console.Write($"0x{item:X2}  ");
        }

        Console.WriteLine();

        await Task.Delay(1000);
        Console.Write("Получили: ");
        while (port.BytesToRead > 0)
        {
            Console.Write($"0x{port.ReadByte():X2}, ");
        }


        Console.WriteLine();
        port.Close();
    }
}
catch (Exception exception)
{
    Console.WriteLine(exception);
}
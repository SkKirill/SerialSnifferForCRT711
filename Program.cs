using System.IO.Ports;

internal class Program
{
    private const int ValueSTX = 0xF2; // Start Character
    private const int ValueADDR = 0x00; // 
    private const int ValueLENH = 0x00; // 
    private const int ValueLENL = 0x00; // 
    private const int ValueCmt = 0x43; // (byte)'C'
    private const int ValueCm = 0x30; // 30H
    private const int ValuePm = 0x34; // 34H
    private const int ValueData = 0x00; //
    private const int ValueETX = 0x00; //
    private const int ValueBCC = 0x00; //
    
    
    
    private static readonly int[] BaudRates = { 9600, 19200, 38400, 57600 };
    private const string SerialPortName = "/dev/ttyUSB0";

    public static async Task Main(string[] args)
    {
        await MethodIoPorts();
    }

    private static async Task MethodIoPorts()
    {
        try
        {
            foreach (var baudRate in BaudRates)
            {
                SerialPort port;
                // Открываем соединение с портом в новом baudRate
                try
                {
                    port = new SerialPort(SerialPortName, baudRate: baudRate);
                    port.Open();
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Не удалось подключить порт {SerialPortName}" + exception.Message);
                    continue;
                }

                byte[] dataToSend = new byte[] { 0xF2, 0x00, 0x00, 0x07, 0x43, 0x30, 0x34, 0x03, 0x75 };
                port.Write(dataToSend, 0, dataToSend.Length);

                Console.Write($"Отправили port={SerialPortName} baudRate={baudRate} -> ");
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
    }
}
using System.Globalization;
using System.IO.Ports;
using SerialSniffer;

internal class Program
{
    private const int ValueStx = 0xF2; // Start Character
    private const int ValueAddr = 0x02; // (byte)'F'
    private const int ValueLenh = 0x00; // Старший байт длинны
    private const int ValueLenl = 0x03; // Младший байт длинны
    private const int ValueCmt = 0x43; // (byte)'C'
    private const int ValueCm = 0x30; // Команда
    private const int ValuePm = 0x34; // Параметр команды
    private const int ValueEtx = 0x03; // End Character
    
    private static readonly int[] BaudRates = { /*9600, 19200, */38400, /*57600*/ }; 
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
            //for(int j = 0; j < 256; j++)
            {
                SerialPort port;
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
                
                byte[] packet1 =
                {
                    // 0xF2, (byte)'F', 0x00, 0x03, 0x43, 0x30, 0x34, 0x03
                    ValueStx, ValueAddr, ValueLenh, ValueLenl, ValueCmt, ValueCm, ValuePm, ValueEtx
                };

                byte bcc = packet1[0];
                for (int i = 1; i < packet1.Length; i++)
                {
                    bcc ^= packet1[i];
                }

                byte[] packet =
                {
                    //0xF2, 0x46, 0x00, 0x03, 0x43, 0x30, 0x34, 0x03, 0xC3
                    ValueStx, ValueAddr, ValueLenh, ValueLenl, ValueCmt, ValueCm, ValuePm, ValueEtx, bcc
                };

                // Отправляем пакет00
                port.Write(packet, 0, packet.Length);

                Console.Write($"Отправили port={SerialPortName} baudRate={baudRate} -> ");
                foreach (var item in packet)
                    Console.Write($"0x{item:X2}  ");
                Console.WriteLine();
                await Task.Delay(2000);

                Console.Write("Получили: ");
                while (port.BytesToRead > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write($"0x{port.ReadByte():X2}, ");
                    Console.ForegroundColor = ConsoleColor.Gray;
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
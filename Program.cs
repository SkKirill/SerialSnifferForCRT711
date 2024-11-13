using System.Globalization;
using System.IO.Ports;
using SerialSniffer;

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
        UInt32 Hdle = 0;
        
        Hdle = DllClass.CommOpen(SerialPortName);
        
        if (Hdle != 0)
        {
            Console.WriteLine("Comm. Port is Opened");
        }
        else
        {
            Console.WriteLine("Open Comm. Port Error");
        }


        if (Hdle != 0)
        {
            byte Addr;
            byte Cm, Pm;
            UInt16 TxDataLen, RxDataLen;
            byte[] TxData = new byte[1024];
            byte[] RxData = new byte[1024];
            byte ReType = 0;
            byte St0, St1, St2;

            Cm = 0x30;
            Pm = 0x34;
            St0 = St1 = St2 = 0;
            TxDataLen = 0;
            RxDataLen = 0;

            Addr = (byte)(byte.Parse("01".Substring(0, 2), NumberStyles.Number));
            int i = DllClass.ExecuteCommand(Hdle, Addr, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1,
                ref St2, ref RxDataLen, RxData);
            if (i == 0)
            {
                if (ReType == 0x50)
                {
                    Console.WriteLine("INITIALIZE OK" + "\r\n" + "Status Code : " + (char)St0 + (char)St1 + (char)St2);
                }
                else
                {
                    Console.WriteLine("INITIALIZE ERROR" + "\r\n" + "Error Code:  " + (char)St1 + (char)St2);
                }
            }
            else
            {
                Console.WriteLine("Communication Error");
            }
        }
        else
        {
            Console.WriteLine("Comm. port is not Opened");
        }

        //await MethodIoPorts();
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
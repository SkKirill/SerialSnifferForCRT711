using System.IO.Ports;
using System.Text;
using SerialSniffer;

public internal class Program
{
    private const string SerialPortName = "/dev/ttyUSB0";


    public static async Task Main(string[] args)
    {
        try
        {
            if (OpenPort(out var port))
                return;

            /*PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandInitialize30));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandInitialize31));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandInitialize33));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandInitialize34));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandInitialize35));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandInitialize37));

            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandStatusRequest30));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandStatusRequest31));*/

            /*PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandCardMove30));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandCardMove33));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandCardMove39));*/

            /*
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandCardEntry30));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandCardEntry31));

            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandRfidCardType));
            */

            /*PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandSamCardControl3030));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandSamCardControl3033));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandSamCardControl3035));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandSamCardControl32));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandSamCardControl31));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandSamCardControl33));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandSamCardControl34));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandSamCardControl38));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandSamCardControl39));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandSamCardControl4030));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandSamCardControl4031));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandSamCardControl4032));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandSamCardControl4033));*/

            /*PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandRfidCardControl1356MHz30));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandRfidCardControl1356MHz31));*/
            
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandRfidCardControl1356MHz32));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandRfidCardControl1356MHz3930));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandRfidCardControl1356MHz3931));

            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandBarcodeScan));

            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandReadMachineConfigInformation));

            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandReadMachineVersion30));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandReadMachineVersion31));

            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandErrorCardBinCounter30));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandErrorCardBinCounter31));

            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandEjectCardCounter30));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandEjectCardCounter31));
            

            port.Close();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private static void PrintAnswer(byte[] response)
    {
        Console.WriteLine("Полный ответ в ASCII = " + Encoding.ASCII.GetString(response));

        if (response.Length == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Не было получено ответа от устройства!\n");
            return;
        }
        
        var typeAnswer = Encoding.ASCII.GetString(new [] { response.First() });

        if (typeAnswer == "P")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"PMT = {typeAnswer}");
            Console.ResetColor();
            Console.WriteLine("s0 = " + Encoding.ASCII.GetString(response[3..4]));
            Console.WriteLine("s1 = " + Encoding.ASCII.GetString(response[4..5]));
            Console.WriteLine("s2 = " + Encoding.ASCII.GetString(response[5..6]));
            if (response.Length > 5)
                Console.WriteLine("data = " + Encoding.ASCII.GetString(response[6..^0]));
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"EMT = {typeAnswer}");
            Console.ResetColor();
            Console.WriteLine("e0 = " + Encoding.ASCII.GetString(response[3..4]));
            Console.WriteLine("e1 = " + Encoding.ASCII.GetString(response[4..5]));
        }

        Console.WriteLine();
    }
    
    private static bool OpenPort(out SerialPort port)
    {
        try
        {
            port = new SerialPort(SerialPortName);
            port.Open();
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Не удалось подключить порт {SerialPortName}" + exception.Message);
            port = null!;
            return true;
        }

        return false;
    }

    private static async Task<byte[]> ExecuteByteCommand(SerialPort port, ByteCommand byteCommand)
    {
        try
        {
            var commandData = byteCommand.Data;
            commandData[^1] = CalculateBcc(commandData);

            port.Write(commandData, 0, commandData.Length);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{byteCommand.Name} -> ");
            foreach (var item in commandData)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"0x{item:X2}  ");
            }

            Console.ResetColor();
            Console.WriteLine();

            await Task.Delay(300);
            Console.Write("Получили: ");
            List<byte> response = [];
            while (port.BytesToRead > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                response.Add((byte)port.ReadByte());
                Console.Write($"0x{response[^1]:X2}, ");
                Console.ResetColor();
            }


            if (response[^1] != CalculateBcc(response.ToArray()))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nОшибка: контрольная сумма(BCC) неверная!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("\nКонтрольная сумма(BCC) верна!");
                Console.ResetColor();
            }

            var answer = new byte[response[4]];
            for (var i = 5; i < response[4] + 5 && i < response.Count; i++)
            {
                answer[i - 5] = response[i];
            }

            return answer;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Ошибка при отправке команды:{exception.Message}");
            return Array.Empty<byte>();
        }
    }

    private static byte CalculateBcc(byte[] byteCommand)
    {
        var i = byteCommand[0] == CommandListInfo.ValueStx ? 0 : 1;
        var valueBcc = byteCommand[i];
        i++;
        for (; i < byteCommand.Length - 1; i++)
        {
            valueBcc ^= byteCommand[i];
        }

        return valueBcc;
    }
}


namespace SerialSniffer;

public static class CommandListInfo
{
    public const int ValueStx = 0xF2; // Start Character
    private const int ValueAddr = 0x00; // Адрес устройства (дефолтный 15, но у нас он 0x00)
    private const int ValueCmt = 0x43; // (byte)'C' - Команда
    private const int ValueEtx = 0x03; // End Character  

    public static readonly ByteCommand CommandInitialize30 = new(
        "4.1 Reset (Initialization) Command PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x30, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandInitialize31 = new(
        "4.1 Reset (Initialization) Command PM=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x30, 0x31, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandInitialize33 = new(
        "4.1 Reset (Initialization) Command PM=0x33",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x30, 0x33, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandInitialize34 = new(
        "4.1 Reset (Initialization) Command PM=0x34",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x30, 0x34, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandInitialize35 = new(
        "4.1 Reset (Initialization) Command PM=0x35",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x30, 0x35, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandInitialize37 = new(
        "4.1 Reset (Initialization) Command PM=0x37",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x30, 0x37, ValueEtx, 0x00]
    );


    public static readonly ByteCommand CommandStatusRequest30 = new(
        "4.2 Status Check Command PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x31, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandStatusRequest31 = new(
        "4.2 Status Check Command PM=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x31, 0x31, ValueEtx, 0x00]
    );


    public static readonly ByteCommand CommandCardMove30 = new(
        "4.3 Carry Card Command PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x32, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandCardMove33 = new(
        "4.3 Carry Card Command PM=0x33",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x32, 0x33, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandCardMove39 = new(
        "4.3 Carry Card Command PM=0x39",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x32, 0x39, ValueEtx, 0x00]
    );


    public static readonly ByteCommand CommandCardEntry30 = new(
        "4.4 Front Entry Enable Command PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x33, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandCardEntry31 = new(
        "4.4 Front Entry Enable Command PM=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x33, 0x31, ValueEtx, 0x00]
    );


    public static readonly ByteCommand CommandRfidCardType = new(
        "4.5 Check RF Card Type",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x50, 0x31, ValueEtx, 0x00]
    );


    public static readonly ByteCommand CommandSamCardControl3030 = new(
        "4.6.1 SAM Card Reset (Activation) PM=0x30 Vcc=0x30",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x30, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandSamCardControl3033 = new(
        "4.6.1 SAM Card Reset (Activation) PM=0x30 Vcc=0x33",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x30, 0x33, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandSamCardControl3035 = new(
        "4.6.1 SAM Card Reset (Activation) PM=0x30 Vcc=0x35",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x30, 0x35, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandSamCardControl31 = new(
        "4.6.2 Deactivate SAM Command PM=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x52, 0x31, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandSamCardControl32 = new(
        "4.6.3 Inquire SAM Status Command PM=0x32",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x52, 0x32, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandSamCardControl33 = new(
        "4.6.4 SAM APDU operation T=0 protocol PM=0x33 C-APDU=0x00, 0x00, 0x00, 0x00",
        [ValueStx, ValueAddr, 0x00, 0x07, ValueCmt, 0x52, 0x33, 0x00, 0x00, 0x00, 0x00, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandSamCardControl34 = new(
        "4.6.5 SAM APDU operation T=1 protocol PM=0x34 C-APDU=0x00, 0x00, 0x00, 0x00",
        [ValueStx, ValueAddr, 0x00, 0x07, ValueCmt, 0x52, 0x34, 0x00, 0x00, 0x00, 0x00, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandSamCardControl38 = new(
        "4.6.6 SAM Warm Reset PM=0x38",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x52, 0x38, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandSamCardControl39 = new(
        "4.6.7 Auto-Distinguish SAM Card T=0/T=1 Protocol PM=0x39 C-APDU=0x00, 0x00, 0x00, 0x00",
        [ValueStx, ValueAddr, 0x00, 0x07, ValueCmt, 0x52, 0x39, 0x00, 0x00, 0x00, 0x00, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandSamCardControl4030 = new(
        "4.6.8 Select SAM Stand PM=0x40 SAMn=0x30",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x40, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandSamCardControl4031 = new(
        "4.6.8 Select SAM Stand PM=0x40 SAMn=0x31",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x40, 0x31, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandSamCardControl4032 = new(
        "4.6.8 Select SAM Stand PM=0x40 SAMn=0x32",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x40, 0x32, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandSamCardControl4033 = new(
        "4.6.8 Select SAM Stand PM=0x40 SAMn=0x33",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x40, 0x33, ValueEtx, 0x00]
    );


    public static readonly ByteCommand CommandRfidCardControl1356MHz30 = new(
        "4.7.1 Activated contactless IC card PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x30, ValueEtx, 0x00]
    );
    
    public static readonly ByteCommand CommandRfidCardControl1356MHz31 = new(
        "4.7.2 Deactivate RFID card PM=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x31, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandRfidCardControl1356MHz32 = new(
        "4.7.3 Inquire status of RFID card PM=0x32",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x32, ValueEtx, 0x00]
    );

    /*public static readonly ByteCommand CommandRfidCardControl1356MHz33 = new(
        "4.7.4.1 Download password to EEPROM and 4.7.4.2 Read Key from EEPROM and 4.7.4.4 Upload key from EEPROM" +
        " “C” 60H 33H 00H D0H ks sn pdata and “C” 60H 33H 00H 21H ks sn and “C” 60H 33H 00H 21H ks sn PM=0x33",
        [
            ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x33, 0x00, 0xD0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, ValueEtx, 0x00
        ]
    );*/

    /*public static readonly ByteCommand CommandRfidCardControl1356MHz34 = new(
        "4.7.5 Type A RF card communication PM=0x34",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x34, ValueEtx, 0x00]
    );*/

    /*public static readonly ByteCommand CommandRfidCardControl1356MHz35 = new(
        "4.7.6 Type B RF card communication PM=0x35",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x35, ValueEtx, 0x00]
    );*/

    public static readonly ByteCommand CommandRfidCardControl1356MHz3930 = new(
        "4.7.7 RFID card sleep and wake-up operation PM=0x39 Set=30",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x60, 0x39, 0x30, ValueEtx, 0x00]
    );
    
    public static readonly ByteCommand CommandRfidCardControl1356MHz3931 = new(
        "4.7.7 RFID card sleep and wake-up operation PM=0x39 Set=31",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x60, 0x39, 0x31, ValueEtx, 0x00]
    );


    public static readonly ByteCommand CommandBarcodeScan = new(
        "4.8 Barcode scanning",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x70, 0x30, ValueEtx, 0x00]
    );


    public static readonly ByteCommand CommandReadMachineConfigInformation = new(
        "4.9 Read CRT-711 configuration",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0xA3, 0x30, ValueEtx, 0x00]
    );


    public static readonly ByteCommand CommandReadMachineVersion30 = new(
        "4.10 Read CRT-711 version information Pm=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0xA4, 0x30, ValueEtx, 0x00]
    );
    
    public static readonly ByteCommand CommandReadMachineVersion31 = new(
        "4.10 Read CRT-711 version information Pm=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0xA4, 0x31, ValueEtx, 0x00]
    );


    public static readonly ByteCommand CommandErrorCardBinCounter30 = new(
        "4.11.1 Read error-card bin counter PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0xA5, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandErrorCardBinCounter31 = new(
        "4.11.2 Set initial value of error-card bin PM=0x31 Count=0x00, 0x00, 0x00",
        [ValueStx, ValueAddr, 0x00, 0x06, ValueCmt, 0xA5, 0x31, 0x00, 0x00, 0x01, ValueEtx, 0x00]
    );


    public static readonly ByteCommand CommandEjectCardCounter30 = new(
        "4.12.1 Read eject card counter PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0xA6, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandEjectCardCounter31 = new(
        "4.12.2 Set initial value of eject card bin PM=0x31 Count=0x00",
        [ValueStx, ValueAddr, 0x00, 0x06, ValueCmt, 0xA6, 0x31, 0x00, 0x00, 0x01, ValueEtx, 0x00]
    );
}


public class ByteCommand(string name, byte[] data)
{
    public string Name { get; init; } = name;
    public byte[] Data { get; set;  } = data;
}



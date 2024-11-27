using System.IO.Ports;
using System.Text;

namespace SerialSnifferForCRT711;

public static class Program
{
    private const string SerialPortName = "/dev/ttyUSB0";

    public static async Task Main(string[] args)
    {
        try
        {
            if (OpenPort(out var port))
                return;
            
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandInitialize34));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandCardEntryEnable));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandStatusRequestWithSensor));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandErrorCardBinCounterSet));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandEjectCardCounterSet));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandErrorCardBinCounterRead));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandEjectCardCounterRead));
            PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandReadMachineConfigInformation));
            
            
            //PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandRfidCardType));
            //PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandRfidCardControl1356MHz32));
            //PrintAnswer(await ExecuteByteCommand(port, CommandListInfo.CommandCardMove33));
            
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
            Console.ResetColor();
            return;
        }

        var typeAnswer = Encoding.ASCII.GetString(new[] { response.First() });
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
            await Task.Delay(1000);
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
            return [];
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
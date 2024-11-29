using System.IO.Ports;

namespace SerialSnifferForCRT711;

public static class MainUtilities
{
    private const string SerialPortName = "/dev/ttyUSB0";

    public static bool OpenPort(out SerialPort port)
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

    public static async Task<byte[]> ExecuteByteCommand(SerialPort port, ByteCommand byteCommand)
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
            await Task.Delay(400);
            var counter = 5;
            while (port.BytesToRead == 0 && counter != 0)
            {
                counter--;
                await Task.Delay(1000);
            }

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
using System.IO.Ports;
using System.Text;

namespace SerialSnifferForCRT711;

public static class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            if (MainUtilities.OpenPort(out var port))
                return;

            PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandInitialize30));
            
            PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandCardMoveToFrontWithHolding));
            //PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo));   
            //PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandRfidCardActivation));
            
            while (true)
            {
                PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandStatusRequest));
                await Task.Delay(1000);
            }
            PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandInitialize30));
/*
            PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandReadMachineVersion30));
            PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandReadMachineVersion31));
            PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandReadMachineConfigInformation));*/
            
            //await CommandsBeforeTurnOn(port);
            //await CommandsReadIdRfidCard(port);

            port.Close();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private static async Task CommandsReadIdRfidCard(SerialPort port)
    {
        PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandRfidCardStatus));
        PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandRfidCardActivation));
        PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandRfidCardStatus));
        PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandRfidCardDeactivation));
        PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandRfidCardStatus));
        await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandCardMoveToFrontWithoutHolding);
    }

    private static async Task CommandsBeforeTurnOn(SerialPort port)
    {
        PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandInitialize34));
        await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandCardEntryEnable);
        PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandStatusRequestWithSensor));
        await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandErrorCardBinCounterSet);
        await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandEjectCardCounterSet);
        PrintAnswer(await MainUtilities.ExecuteByteCommand(port, CommandListInfo.CommandReadMachineConfigInformation));
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
            if (response.Length > 10 && response[1] == 0x60 && response[2] == 0x30)
            {
                Console.Write("id rfid card = ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(string.Join(' ', response[10..(response[9]+10)].Select(c => c.ToString("X2"))));
                Console.ResetColor();
            }
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
}
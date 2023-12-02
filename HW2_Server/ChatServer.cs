using System.Net;
using System.Net.Sockets;
using System.Text;
using HW1;

namespace HW2_Server;

public class ChatServer
{
    private readonly int _port;
    private static readonly IPAddress IpAddress = IPAddress.Any;
    private const string ShutdownString = "shutdown";

    public ChatServer() : this(12345)
    {
    }

    public ChatServer(int port)
    {
        _port = port;
        ServerStart();
    }

    private void ServerStart()
    {
        var udpClient = new UdpClient(_port);
        var ipEndPoint = new IPEndPoint(IpAddress, 0);

        Console.WriteLine("Press <Enter> or <Esc> key to exit from app.");
        Console.WriteLine($"Server started on port {_port}. Waiting for a message...");

        new Thread(ListenForExitKey).Start();

        while (true)
        {
            byte[] buffer = udpClient.Receive(ref ipEndPoint);
            var messageJson = Encoding.UTF8.GetString(buffer);
            var message = Message.DeserializeFromJson(messageJson);

            if (message.Text.ToLower().Equals(ShutdownString))
            {
                udpClient.Send("close"u8, ipEndPoint);
                CloseServer();
            }

            Console.WriteLine($"{message} Ip: {ipEndPoint}");
            new Thread(() => { udpClient.Send("Ok"u8, ipEndPoint); }).Start();
        }
    }

    private static void ListenForExitKey()
    {
        while (true)
        {
            var key = Console.ReadKey().Key;
            if (key is not (ConsoleKey.Enter or ConsoleKey.Escape)) continue;
            CloseServer();
        }
    }

    private static void CloseServer()
    {
        Console.WriteLine("Closing app...");
        Environment.Exit(0);
    }
}
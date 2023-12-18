using System.Net;
using System.Net.Sockets;
using System.Text;
using HW1;

namespace HW3_Server;

public class ChatServer
{
    private readonly int _port;
    private static readonly IPAddress IpAddress = IPAddress.Any;
    private readonly UdpClient _udpClient;
    private IPEndPoint _ipEndPoint;
    private const string ShutdownString = "shutdown";
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly CancellationToken _cancellationToken;

    public ChatServer() : this(12345)
    {
    }

    public ChatServer(int port)
    {
        _port = port;
        _udpClient = new UdpClient(_port);
        _ipEndPoint = new IPEndPoint(IpAddress, 0);

        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;

        ServerStart();
    }

    private void ServerStart()
    {
        Console.WriteLine("Press <Enter> or <Esc> key to exit from app.");
        Console.WriteLine($"Server started on port {_port}. Waiting for a message...");

        var keyHandler = new Task(ListenForExitKey);
        keyHandler.Start();

        while (!_cancellationToken.IsCancellationRequested)
        {
            byte[] buffer = _udpClient.Receive(ref _ipEndPoint);
            var messageJson = Encoding.UTF8.GetString(buffer);
            var message = Message.DeserializeFromJson(messageJson);

            if (message.Text.ToLower().Equals(ShutdownString))
            {
                _udpClient.Send("close"u8, _ipEndPoint);
                break;
            }

            Console.WriteLine($"{message} Ip: {_ipEndPoint}");
            new Task(() => { _udpClient.Send("Ok"u8, _ipEndPoint); }).Start();
        }

        CloseServer();
    }

    private void ListenForExitKey()
    {
        while (true)
        {
            var key = Console.ReadKey().Key;
            if (key is not (ConsoleKey.Enter or ConsoleKey.Escape)) continue;
            Console.WriteLine("Shutting down server. Waiting for current receiving done...");
            _cancellationTokenSource.Cancel();
        }
    }

    private static void CloseServer()
    {
        Console.WriteLine("Closing app...");
    }
}
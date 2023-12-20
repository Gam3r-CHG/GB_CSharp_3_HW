using System.Net;
using System.Net.Sockets;
using System.Text;
using HW4;


namespace HW4_Client;

public class ChatClient
{
    private readonly string _from;
    private readonly string _to;
    private readonly string _ipAddress;
    private readonly int _port;
    private const string ExitString = "exit";
    private const string ShutdownString = "shutdown";

    public ChatClient() : this(12345)
    {
    }

    public ChatClient(int port) : this("Max", "Server", "127.0.0.1", port)
    {
    }

    public ChatClient(string from, string ipAddress) : this(from, "Server", ipAddress, 12345)
    {
    }

    public ChatClient(string from, string to, string ipAddress, int port)
    {
        _from = from;
        _to = to;
        _ipAddress = ipAddress;
        _port = port;
        StartChat();
    }

    private void StartChat()
    {
        var udpClient = new UdpClient();
        var ipEndPoint = new IPEndPoint(IPAddress.Parse(_ipAddress), _port);

        Console.WriteLine($"Connected to {_ipAddress} on port {_port}");
        Console.WriteLine($"To exit from chat enter empty string or command: '{ExitString}'.");
        Console.WriteLine($"To exit from chat and shutdown server enter command: '{ShutdownString}';)");
        Console.WriteLine("Enter text message: ");

        while (true)
        {
            var messageText = Console.ReadLine();

            if (string.IsNullOrEmpty(messageText) || messageText.ToLower().Equals(ExitString))
            {
                break;
            }

            // Example of using pattern "Builder"
            var message = new MessageBuilder()
                .BuildDate(DateTime.UtcNow)
                .BuildFrom(_from)
                .BuildTo(_to)
                .BuildText(messageText)
                .Build();

            var messageJson = message.SerializeToJson();

            byte[] data = Encoding.UTF8.GetBytes(messageJson);
            udpClient.Send(data, data.Length, ipEndPoint);
            new Task(() =>
            {
                var result = udpClient.Receive(ref ipEndPoint);
                var serverMessage = Encoding.UTF8.GetString(result);

                if (serverMessage.ToLower().Equals("close"))
                {
                    Console.WriteLine("Server shutdown...");
                    CloseChat();
                }

                Console.WriteLine(serverMessage);
            }).Start();
        }

        CloseChat();
    }

    private static void CloseChat()
    {
        Console.WriteLine("Closing chat...");
        Environment.Exit(0);
    }
}
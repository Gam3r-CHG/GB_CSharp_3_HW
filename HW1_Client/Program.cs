using System.Net;
using System.Net.Sockets;
using System.Text;
using HW1;

namespace HW1_Client;

internal static class Program
{
    private static string _from = "Max";
    private const string To = "Server";
    private static string _ipAddress = "127.0.0.1";
    private const int Port = 12345;

    public static void Main(string[] args)
    {
        if (args.Length == 2)
        {
            _from = args[0];
            _ipAddress = args[1];
        }

        SentMessage(_from, To, _ipAddress);
    }

    private static void SentMessage(string from, string to, string ip)
    {
        var udpClient = new UdpClient();
        var ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), Port);

        while (true)
        {
            string? messageText;
            do
            {
                Console.Write("Enter text message: ");
                messageText = Console.ReadLine();
            } while (string.IsNullOrEmpty(messageText));

            var message = new Message {DateTime = DateTime.UtcNow, From = from, To = to, Text = messageText};
            var messageJson = message.SerializeToJson();

            byte[] data = Encoding.UTF8.GetBytes(messageJson);
            udpClient.Send(data, data.Length, ipEndPoint);
            Console.Write("Waiting response...");
            var result = udpClient.Receive(ref ipEndPoint);
            Console.WriteLine(Encoding.UTF8.GetString(result));
        }
    }
}
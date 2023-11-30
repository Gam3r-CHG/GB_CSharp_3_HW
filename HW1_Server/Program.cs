using System.Net;
using System.Net.Sockets;
using System.Text;
using HW1;

namespace HW1_Server;

internal static class Program
{
    private const int Port = 12345;
    private static readonly IPAddress IpAddress = IPAddress.Any;

    public static void Main(string[] args)
    {
        ServerStart();
    }

    private static void ServerStart()
    {
        var udpClient = new UdpClient(Port);
        var ipEndPoint = new IPEndPoint(IpAddress, 0);

        Console.WriteLine("Server is waiting for a message...");

        while (true)
        {
            byte[] buffer = udpClient.Receive(ref ipEndPoint);
            var messageJson = Encoding.UTF8.GetString(buffer);
            var message = Message.DeserializeFromJson(messageJson);
            Console.WriteLine($"{message} Ip: {ipEndPoint}");
            udpClient.Send("Ok"u8, ipEndPoint);
        }
    }
}
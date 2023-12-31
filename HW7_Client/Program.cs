using HW7;

namespace HW7_Client;

internal static class Program
{
    public static void Main(string[] args)
    {
        // Для наглядности реализации кода с NetMQ - упростил код клиента и сервера.
        var client = new Client("Max");
        client.Start();
    }
}
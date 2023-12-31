using HW7;

namespace HW7_Server;

internal class Program
{
    public static void Main(string[] args)
    {
        // Для наглядности реализации кода с NetMQ - упростил код клиента и сервера.
        var server = new Server();
        server.Start();
    }
}
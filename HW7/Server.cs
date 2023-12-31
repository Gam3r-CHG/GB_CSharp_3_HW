using HW7.Abstracts;
using HW7.Models;
using NetMQ.Sockets;

namespace HW7;

public class Server
{
    private readonly IMessageSource _messageSource;

    public Server()
    {
        _messageSource = new UdpMessageSource();
    }

    public void Start()
    {
        Console.WriteLine("Сервер ожидает сообщения ");

        using (var server = new RouterSocket())
        {
            server.Bind($"tcp://*:5557");
            while (true)
            {
                var message = _messageSource.ReceiveServer(server);
                ProcessMessage(message); // Эмуляция работы (полная версия в прошлых ДЗ)
                Console.WriteLine(message + " to " + message.NickNameTo);
            }
        }
    }

    private void ProcessMessage(NetMessage message)
    {
        // Код по работе с БД
        // Для наглядности реализации кода с NetMQ не стал переносить из прошлого ДЗ
    }
}
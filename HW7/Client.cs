using HW7.Abstracts;
using HW7.Models;

namespace HW7;

public class Client
{
    private readonly string _clientName;
    private readonly IMessageSource _messageSource;

    public Client(string clientName)
    {
        _clientName = clientName;
        _messageSource = new UdpMessageSource();
    }

    void Register()
    {
        var message = new NetMessage
        {
            DateTime = DateTime.UtcNow,
            NickNameFrom = _clientName,
            NickNameTo = "Server",
            Text = "Registration message",
            Command = Command.Register,
            IpAddress = "127.0.0.1",
            Port = 12345
        };

        _messageSource.SendClient(message);
        Console.WriteLine("Reg message sent.");
    }

    void ClientSender()
    {
        while (true)
        {
            Console.Write("Введите имя получателя: ");
            var nameTo = Console.ReadLine();

            Console.Write("Введите сообщение и нажмите Enter: ");
            var messageText = Console.ReadLine();

            var message = new NetMessage
            {
                DateTime = DateTime.UtcNow,
                Command = Command.Message,
                NickNameFrom = _clientName,
                NickNameTo = nameTo,
                Text = messageText ?? ""
            };

            _messageSource.SendClient(message);
        }
    }

    public void Start()
    {
        Register();
        ClientSender();
    }
}
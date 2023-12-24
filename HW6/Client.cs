using System.Net;
using HW6.Abstracts;
using HW6.Models;

namespace HW6;

public class Client
{
    private readonly string _name;
    private readonly IMessageSource _messageSource;
    private IPEndPoint _remoteEndPoint;
    private const int ClientPort = 12348;

    public Client(string name, string address, int port)
    {
        _name = name;
        _messageSource = new UdpMessageSource(ClientPort);
        _remoteEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
    }

    async Task ClientListener()
    {
        while (true)
        {
            try
            {
                var messageReceived = _messageSource.Receive(ref _remoteEndPoint);

                Console.WriteLine($"Получено сообщение от {messageReceived.NickNameFrom}:");
                Console.WriteLine(messageReceived.Text);

                await Confirm(messageReceived, _remoteEndPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении сообщения: " + ex.Message);
            }
        }
    }

    async Task Confirm(NetMessage message, IPEndPoint remoteEndPoint)
    {
        message.Command = Command.Confirmation;
        await _messageSource.SendAsync(message, remoteEndPoint);
    }

    public async Task Register()
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
        ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), ClientPort);
        var message = new NetMessage
        {
            DateTime = DateTime.UtcNow,
            NickNameFrom = _name,
            NickNameTo = "Server",
            Text = "User registration",
            Command = Command.Register,
            IpAddress = "127.0.0.1",
            Port = ClientPort
        };

        await _messageSource.SendAsync(message, _remoteEndPoint);
        Console.WriteLine("Reg message sent.");
    }

    async Task ClientSender()
    {
        while (true)
        {
            try
            {
                Console.Write("Введите имя получателя: ");
                var nameTo = Console.ReadLine();

                Console.Write("Введите сообщение и нажмите Enter: ");
                var messageText = Console.ReadLine();

                await SendMessage(nameTo, messageText);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
            }
        }
    }

    public async Task SendMessage(string nameTo, string messageText)
    {
        var message = new NetMessage
        {
            DateTime = DateTime.UtcNow,
            Command = Command.Message,
            NickNameFrom = _name,
            NickNameTo = nameTo,
            Text = messageText
        };

        await _messageSource.SendAsync(message, _remoteEndPoint);

        Console.WriteLine("Сообщение отправлено.");
    }

    public async void Start()
    {
        await Register();

        var task1 = Task.Run(ClientListener);
        var task2 = Task.Run(ClientSender);

        Task.WaitAll(task1, task2);
    }
}
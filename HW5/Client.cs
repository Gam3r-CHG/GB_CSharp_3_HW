using System.Net;
using System.Net.Sockets;
using HW5.Abstracts;
using HW5.Models;

namespace HW5;

public class Client
{
    private readonly string _name;
    private readonly IMessageSource _messageSource;
    private IPEndPoint _remoteEndPoint;

    public Client(string name, string address, int port)
    {
        _name = name;
        _messageSource = new UdpMessageSource();
        _remoteEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
    }

    UdpClient udpClientClient = new UdpClient();

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


    void Register(IPEndPoint remoteEndPoint)
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
        var message = new NetMessage
            {NickNameFrom = _name, NickNameTo = null, Text = null, Command = Command.Register, EndPoint = ep};
        _messageSource.SendAsync(message, remoteEndPoint);
    }

    async Task ClientSender()
    {
        Register(_remoteEndPoint);

        while (true)
        {
            try
            {
                Console.Write("Введите имя получателя: ");
                var nameTo = Console.ReadLine();

                Console.Write("Введите сообщение и нажмите Enter: ");
                var messageText = Console.ReadLine();

                var message = new NetMessage()
                    {Command = Command.Message, NickNameFrom = _name, NickNameTo = nameTo, Text = messageText};

                await _messageSource.SendAsync(message, _remoteEndPoint);

                Console.WriteLine("Сообщение отправлено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
            }
        }
    }

    public async Task Start()
    {
        await ClientListener();

        await ClientSender();
    }
}
using System.Net;
using HW5.Models;
using HW6.Abstracts;
using Command = HW6.Models.Command;
using NetMessage = HW6.Models.NetMessage;

namespace HW6;

public class Server
{
    private readonly Dictionary<string, IPEndPoint> _clients = new();
    private readonly IMessageSource _messageSource;
    private IPEndPoint _ep;
    private bool _isWorking = true;

    public Server()
    {
        _messageSource = new UdpMessageSource(12345);
        _ep = new IPEndPoint(IPAddress.Any, 0);
    }

    public Server(IMessageSource messageSource)
    {
        _messageSource = messageSource;
        _ep = new IPEndPoint(IPAddress.Any, 0);
    }

    public void Stop()
    {
        _isWorking = false;
    }

    private async Task Register(NetMessage message)
    {
        Console.WriteLine($" Message Register name = {message.NickNameFrom}");

        if (_clients.TryAdd(message.NickNameFrom,
                new IPEndPoint(IPAddress.Parse(message.IpAddress), message.Port)))
        {
            using (ChatContext context = new ChatContext())
            {
                if (context.Users.Where(x => x.FullName == message.NickNameFrom).ToList().Count == 0)
                {
                    context.Users.Add(new User {FullName = message.NickNameFrom});
                    await context.SaveChangesAsync();
                }
            }
        }
    }

    private async Task RelyMessage(NetMessage message)
    {
        if (_clients.TryGetValue(message.NickNameTo, out IPEndPoint ep))
        {
            int? id;
            using (var ctx = new ChatContext())
            {
                var fromUser = ctx.Users.First(x => x.FullName == message.NickNameFrom);
                var toUser = ctx.Users.First(x => x.FullName == message.NickNameTo);
                var msg = new Message
                {
                    DateSend = message.DateTime,
                    UserFrom = fromUser,
                    UserTo = toUser,
                    IsSent = false,
                    Text = message.Text
                };
                ctx.Messages.Add(msg);
                await ctx.SaveChangesAsync();
                id = msg.MessageId;
            }

            message.Id = id;
            await _messageSource.SendAsync(message, ep);
            Console.WriteLine($"Message Relied, from = {message.NickNameFrom} to = {message.NickNameTo}");
        }
        else
        {
            Console.WriteLine("Пользователь не найден.");
        }
    }

    async Task ConfirmMessageReceived(int? id)
    {
        Console.WriteLine("Message confirmation id=" + id);

        using (var ctx = new ChatContext())
        {
            var msg = ctx.Messages.FirstOrDefault(x => x.MessageId == id);

            if (msg != null)
            {
                msg.IsSent = true;
                await ctx.SaveChangesAsync();
            }
        }
    }

    private async Task ProcessMessage(NetMessage message)
    {
        switch (message.Command)
        {
            case Command.Register:
                await Register(message);
                break;
            case Command.Message:
                await RelyMessage(message);
                break;
            case Command.Confirmation:
                await ConfirmMessageReceived(message.Id);
                break;
        }
    }

    public async Task Start()
    {
        Console.WriteLine("Сервер ожидает сообщения ");

        while (_isWorking)
        {
            try
            {
                var message = _messageSource.Receive(ref _ep);

                // For UTests mock data
                if (!_isWorking)
                {
                    break;
                }

                Console.WriteLine(message.ToString());
                await ProcessMessage(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
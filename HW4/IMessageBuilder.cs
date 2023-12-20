namespace HW4;

public interface IMessageBuilder
{
    public IMessageBuilder BuildDate(DateTime dateTime);
    public IMessageBuilder BuildFrom(string from);
    public IMessageBuilder BuildTo(string to);
    public IMessageBuilder BuildText(string text);
    public Message Build();
}
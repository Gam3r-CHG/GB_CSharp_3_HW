namespace HW4;

public class MessageBuilder : IMessageBuilder
{
    private DateTime _dateTime;
    private string _from = "";
    private string _to = "";
    private string _text = "";

    public IMessageBuilder BuildDate(DateTime dateTime)
    {
        _dateTime = dateTime;
        return this;
    }

    public IMessageBuilder BuildFrom(string from)
    {
        _from = from;
        return this;
    }

    public IMessageBuilder BuildTo(string to)
    {
        _to = to;
        return this;
    }

    public IMessageBuilder BuildText(string text)
    {
        _text = text;
        return this;
    }

    public Message Build()
    {
        return new Message {DateTime = _dateTime, From = _from, To = _to, Text = _text};
    }
}
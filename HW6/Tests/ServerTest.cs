using HW5.Models;

namespace HW6.Tests;

public class ServerTest
{
    [SetUp]
    public void Setup()
    {
        using (ChatContext chatContext = new ChatContext())
        {
            chatContext.RemoveRange(chatContext.Messages);
            chatContext.RemoveRange(chatContext.Users);
            chatContext.SaveChanges();
        }
    }

    [TearDown]
    public void TearDown()
    {
        using (ChatContext chatContext = new ChatContext())
        {
            chatContext.RemoveRange(chatContext.Messages);
            chatContext.RemoveRange(chatContext.Users);
            chatContext.SaveChanges();
        }
    }

    [Test]
    public async Task TestServer()
    {
        var mock = new MockMessageSource();
        var server = new Server(mock);
        mock.AddServer(server);
        await server.Start();


        using (ChatContext chatContext = new ChatContext())
        {
            Assert.IsTrue(chatContext.Users.Count() == 2, "Users not created");
            var user1 = chatContext.Users.FirstOrDefault(x => x.FullName == "Max");
            var user2 = chatContext.Users.FirstOrDefault(x => x.FullName == "Test");

            Assert.IsNotNull(user1, "User1 not created");
            Assert.IsNotNull(user2, "User2 not created");

            Assert.IsTrue(user1.MessagesFrom.Count == 1);
            Assert.IsTrue(user2.MessagesFrom.Count == 1);

            Assert.IsTrue(user1.MessagesTo.Count == 1);
            Assert.IsTrue(user2.MessagesTo.Count == 1);

            var msg1 = chatContext.Messages.FirstOrDefault(x => x.UserFrom == user1 && x.UserTo == user2);
            var msg2 = chatContext.Messages.FirstOrDefault(x => x.UserFrom == user2 && x.UserTo == user1);

            Assert.AreEqual("Test2", msg1.Text);
            Assert.AreEqual("Test1", msg2.Text);
        }
    }
}
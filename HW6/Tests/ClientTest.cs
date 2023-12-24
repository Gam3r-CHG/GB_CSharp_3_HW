using HW5.Models;

namespace HW6.Tests;

public class ClientTest
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

        var server = new Server();
        Task.Run(() => server.Start());
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
    public async Task TestClientRegistrationAndSendingMessage()
    {
        var testClient = new Client("Max", "127.0.0.1", 12345);

        await testClient.Register();

        await testClient.SendMessage("Max", "Test1");
        await Task.Delay(100);

        await testClient.SendMessage("Max", "Test2");
        await Task.Delay(100);

        using (ChatContext chatContext = new ChatContext())
        {
            var testUser = chatContext.Users.FirstOrDefault(x => x.FullName == "Max");
            Assert.IsNotNull(testUser, "Test user didn't created");

            Assert.IsTrue(testUser.MessagesFrom.Count == 2);
            Assert.IsTrue(testUser.MessagesTo.Count == 2);

            Assert.IsTrue(chatContext.Messages
                .Count(x => x.UserTo == testUser && x.UserFrom == testUser && x.Text == "Test1") == 1);
            Assert.IsTrue(chatContext.Messages
                .Count(x => x.UserTo == testUser && x.UserFrom == testUser && x.Text == "Test2") == 1);
        }
    }
}
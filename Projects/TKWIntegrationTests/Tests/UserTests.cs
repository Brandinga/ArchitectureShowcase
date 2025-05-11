using Microsoft.EntityFrameworkCore;

namespace TKWIntegrationTests;

[TestClass]
public class UserCommandTests
{
    [ClassInitialize]
    public static void ClassInitialize(TestContext testcontext)
    {
        TestHelper.Initialize();
    }

    [TestInitialize]
    public void TestInitialize()
    {
        TestHelper.InitializeTest();

        //seed data
    }

    [TestCleanup]
    public void TestCleanup()
    {
        TestHelper.CleanupTest();
    }

    [TestMethod]
    public void TestRegisterUserCommand()
    {
        RegisterUserCommand command = new RegisterUserCommand()
        {
            Name = "MyName",
            FirstName = "TestFirstName",
            Password = ""
        };

        TestHelper.Resolver.HandleCommand(command, TestHelper.CreateT04Context());

        Assert.AreEqual(1, TestHelper.DbContext.User.AsNoTracking().ToList().Count);
    }
}

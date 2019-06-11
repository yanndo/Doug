using Doug;
using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class WholastCommandTest
    {
        private const string CommandText = "<@otherUserid|username>";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private SlursCommands _slursCommands;

        private readonly Mock<ISlurRepository> _slurRepository = new Mock<ISlurRepository>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IAuthorizationService> _adminValidator = new Mock<IAuthorizationService>();

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 68 });

            _slurRepository.Setup(repo => repo.GetRecentSlurs()).Returns(new List<RecentFlame>() { new RecentFlame() });
            _slurRepository.Setup(repo => repo.GetSlur(It.IsAny<int>())).Returns(new Slur("ffff", "asdf"));

            _slursCommands = new SlursCommands(_slurRepository.Object, _userRepository.Object, _slack.Object, _adminValidator.Object);
        }

        [TestMethod]
        public void WhenCheckingLastSlurAuthor_Remove2CreditsToUser()
        {
            _slursCommands.WhoLast(command);

            _userRepository.Verify(repo => repo.RemoveCredits(User, 2));
        }

        [TestMethod]
        public void WhenCheckingLastSlurAuthor_LastSlurAuthorIsReturned()
        {
            var result = _slursCommands.WhoLast(command);

            Assert.AreEqual("<@asdf> created that slur.", result.Message);
        }

        [TestMethod]
        public void GivenUserHasBotEnoughCredits_WhenCheckingLastSlurAuthor_ErrorMessageIsSent()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 0 });

            var result = _slursCommands.WhoLast(command);

            Assert.AreEqual("You need 2 " + DougMessages.CreditEmoji + " to do this and you have 0 " + DougMessages.CreditEmoji, result.Message);
        }
    }
}
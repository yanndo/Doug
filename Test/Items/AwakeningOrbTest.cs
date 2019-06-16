using Doug.Items;
using Doug.Models;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Items
{
    [TestClass]
    public class AwakeningOrbTest
    {
        private const string CommandText = "<@otherUserid|username>";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command _command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private AwakeningOrb _awakeningOrb;

        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();

        [TestMethod]
        public void WhenGettingFlamed_TargetIsNotifiedWithTheCallersName()
        {
            _awakeningOrb = new AwakeningOrb();

            _awakeningOrb.OnGettingFlamed(_command, "hehehee", _slack.Object);

            _slack.Verify(slack => slack.SendEphemeralMessage(It.IsRegex("testuser"), "otherUserid", Channel));
        }

        [TestMethod]
        public void WhenGettingFlamed_SlurDoesNotChange()
        {
            _awakeningOrb = new AwakeningOrb();

            var result = _awakeningOrb.OnGettingFlamed(_command, "hehehee", _slack.Object);

            Assert.AreEqual("hehehee", result);
        }
    }
}
using System;
using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Shop
{
    [TestClass]
    public class BuyTest
    {
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Interaction _interaction = new Interaction
        {
            ChannelId = Channel,
            UserId = User,
            Action = "buy",
            Value = "apple"
        };

        private ShopService _shopService;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IInventoryRepository>  _inventoryRepository = new Mock<IInventoryRepository>();

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 79});

            _shopService = new ShopService(_userRepository.Object, _slack.Object, _inventoryRepository.Object);
        }

        [TestMethod]
        public void GivenEnoughCredits_WhenBuyingAnApple_AppleIsAdded()
        {
            _shopService.Buy(_interaction);

            _inventoryRepository.Verify(repo => repo.AddItem(User, "apple"));
        }

        [TestMethod]
        public void GivenEnoughCredits_WhenBuyingAnApple_CreditsAreRemovedFromGiver()
        {
            _shopService.Buy(_interaction);

            _userRepository.Verify(repo => repo.RemoveCredits(User, 25));
        }

        [TestMethod]
        public void GivenNotEnoughCredits_WhenBuyingAnApple_NotEnoughCreditsMessageIsSent()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 22 });

            _shopService.Buy(_interaction);

            _slack.Verify(slack => slack.SendEphemeralMessage(It.IsAny<string>(), User, Channel));
        }
    }
}
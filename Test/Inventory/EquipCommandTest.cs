using System.Collections.Generic;
using Doug.Commands;
using Doug.Items;
using Doug.Items.Consumables;
using Doug.Items.Equipment;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Inventory
{
    [TestClass]
    public class EquipCommandTest
    {
        private const string CommandText = "6";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command _command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private readonly EquipmentItem _item = new AwakeningOrb();

        private InventoryCommands _inventoryCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IStatsRepository> _statsRepository = new Mock<IStatsRepository>();
        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();

        [TestInitialize]
        public void Setup()
        {
            var loadout = new Loadout(null, null, null, null, null, null, null, null);
            var items = new List<InventoryItem>() {new InventoryItem("testuser", "testitem") {InventoryPosition = 6, Item = _item } };
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", InventoryItems = items, Loadout = loadout });

            _inventoryCommands = new InventoryCommands(_userRepository.Object, _slack.Object, _statsRepository.Object, _inventoryRepository.Object);
        }

        [TestMethod]
        public void GivenEmptyEquipmentSlot_WhenEquippingItem_ItemIsEquippedInSlot()
        {
            _inventoryCommands.Equip(_command);

            _inventoryRepository.Verify(repo => repo.EquipItem(User, _item));
        }

        [TestMethod]
        public void GivenEmptyEquipmentSlot_WhenEquippingItem_ItemIsRemovedFromInventory()
        {
            _inventoryCommands.Equip(_command);

            _inventoryRepository.Verify(repo => repo.RemoveItem(User, 6));
        }

        [TestMethod]
        public void GivenUserHasNoItemInSlot_WhenEquipping_NoItemMessageSent()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", InventoryItems = new List<InventoryItem>() });

            var result = _inventoryCommands.Equip(_command);

            Assert.AreEqual("There is no item in slot 6.", result.Message);
        }

        [TestMethod]
        public void GivenItemIsNotEquipAble_WhenEquipping_NotEquipAbleMessageSent()
        {
            var items = new List<InventoryItem>() { new InventoryItem("testuser", "testitem") { InventoryPosition = 6, Item = new Apple() } };
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", InventoryItems = items });

            var result = _inventoryCommands.Equip(_command);

            Assert.AreEqual("This item is not equipable.", result.Message);
        }
    }
}
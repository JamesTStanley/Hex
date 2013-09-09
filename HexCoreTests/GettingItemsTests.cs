using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hex;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HexCoreTests
{
    [TestClass]
    public class GettingItemsTests
    {
        [TestMethod]
        public void GettingItemThatDoesNotExistReturnsNull()
        {
            // Create 1-ring map
            var sut = new HexMap<object>(MapShape.HexagonFlatTopped, 1);

            // Try to get an item from the 2nd ring
            Assert.IsNull(sut.Item(2, 0, -2));
        }

        [TestMethod]
        public void GetRing1ReturnsCorrectResults()
        {
            var sut = new HexMap<object>(MapShape.HexagonFlatTopped, 1);
            var result = sut.Ring(1);

            // First ring
            Assert.AreEqual(6, result.Count);
            Assert.IsNotNull(result.First(i => i.X == 1 && i.Y == 0 && i.Z == -1));
            Assert.IsNotNull(result.First(i => i.X == 1 && i.Y == -1 && i.Z == 0));
            Assert.IsNotNull(result.First(i => i.X == 0 && i.Y == -1 && i.Z == 1));
            Assert.IsNotNull(result.First(i => i.X == -1 && i.Y == 0 && i.Z == 1));
            Assert.IsNotNull(result.First(i => i.X == -1 && i.Y == 1 && i.Z == 0));
            Assert.IsNotNull(result.First(i => i.X == 0 && i.Y == 1 && i.Z == -1));
        }

        [TestMethod]
        public void GetRing2ReturnsCorrectResults()
        {
            var sut = new HexMap<object>(MapShape.HexagonFlatTopped, 2);
            var result = sut.Ring(2);

            // Second ring
            Assert.AreEqual(12, result.Count);
            Assert.IsNotNull(result.First(i => i.X == 2 && i.Y == 0 && i.Z == -2));
            Assert.IsNotNull(result.First(i => i.X == 1 && i.Y == 1 && i.Z == -2));
            Assert.IsNotNull(result.First(i => i.X == 0 && i.Y == 2 && i.Z == -2));
            Assert.IsNotNull(result.First(i => i.X == -1 && i.Y == 2 && i.Z == -1));
            Assert.IsNotNull(result.First(i => i.X == -2 && i.Y == 2 && i.Z == 0));
            Assert.IsNotNull(result.First(i => i.X == -2 && i.Y == 1 && i.Z == 1));
            Assert.IsNotNull(result.First(i => i.X == -2 && i.Y == 0 && i.Z == 2));
            Assert.IsNotNull(result.First(i => i.X == -1 && i.Y == -1 && i.Z == 2));
            Assert.IsNotNull(result.First(i => i.X == 0 && i.Y == -2 && i.Z == 2));
            Assert.IsNotNull(result.First(i => i.X == 1 && i.Y == -2 && i.Z == 1));
            Assert.IsNotNull(result.First(i => i.X == 2 && i.Y == -2 && i.Z == 0));
            Assert.IsNotNull(result.First(i => i.X == 2 && i.Y == -1 && i.Z == -1));
        }

        [TestMethod]
        public void SettingValuesWorks()
        {
            var sut = new HexMap<string>(MapShape.HexagonFlatTopped, 2);
            foreach (var item in sut.Ring(2))
            {
                item.Value = "red";
            }
            foreach (var item in sut.Ring(1))
            {
                item.Value = "yellow";
            }
            foreach (var item in sut.Ring(0))
            {
                item.Value = "blue";
            }

            Assert.AreEqual(0, sut.Ring(2).Count(i => i.Value != "red"));
            Assert.AreEqual(12, sut.Ring(2).Count(i => i.Value == "red"));

            Assert.AreEqual(0, sut.Ring(1).Count(i => i.Value != "yellow"));
            Assert.AreEqual(6, sut.Ring(1).Count(i => i.Value == "yellow"));

            Assert.AreEqual(0, sut.Ring(0).Count(i => i.Value != "blue"));
            Assert.AreEqual(1, sut.Ring(0).Count(i => i.Value == "blue"));
        }
    }
}

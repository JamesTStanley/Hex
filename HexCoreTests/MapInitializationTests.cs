using Hex;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HexCoreTests
{
    [TestClass]
    public class MapInitializationTests
    {
        [TestMethod]
        public void CtorSizeFactor0Yields1Item()
        {
            var sut = new HexMap<object>(HexOrientation.FlatTopped, 0);

            Assert.AreEqual(1, sut.Map.Count);
        }

        [TestMethod]
        public void CtorSizeFactor1Yields7Items()
        {
            var sut = new HexMap<object>(HexOrientation.FlatTopped, 1);

            Assert.AreEqual(7, sut.Map.Count);
        }

        [TestMethod]
        public void CtorSizeFactor2Yields19Items()
        {
            var sut = new HexMap<object>(HexOrientation.FlatTopped, 2);

            Assert.AreEqual(19, sut.Map.Count);
        }

        [TestMethod]
        public void CtorSizeFactor0YieldsCorrectCoordinates()
        {
            var sut = new HexMap<object>(HexOrientation.FlatTopped, 0);

            // Center hex
            Assert.IsNotNull(sut.Item(0,0,0));
        }

        [TestMethod]
        public void CtorSizeFactor1YieldsCorrectCoordinates()
        {
            var sut = new HexMap<object>(HexOrientation.FlatTopped, 1);

            // Center hex
            Assert.IsNotNull(sut.Item(0, 0, 0));

            // First ring
            Assert.IsNotNull(sut.Item(1, 0, -1));
            Assert.IsNotNull(sut.Item(1, -1, 0));
            Assert.IsNotNull(sut.Item(0, -1, 1));
            Assert.IsNotNull(sut.Item(-1, 0, 1));
            Assert.IsNotNull(sut.Item(-1, 1, 0));
            Assert.IsNotNull(sut.Item(0, 1, -1));
        }

        [TestMethod]
        public void CtorSizeFactor2YieldsCorrectCoordinates()
        {
            var sut = new HexMap<object>(HexOrientation.FlatTopped, 2);

            // Center hex
            Assert.IsNotNull(sut.Item(0, 0, 0));

            // First ring
            Assert.IsNotNull(sut.Item(1, 0, -1));
            Assert.IsNotNull(sut.Item(1, -1, 0));
            Assert.IsNotNull(sut.Item(0, -1, 1));
            Assert.IsNotNull(sut.Item(-1, 0, 1));
            Assert.IsNotNull(sut.Item(-1, 1, 0));
            Assert.IsNotNull(sut.Item(0, 1, -1));

            // Second ring
            Assert.IsNotNull(sut.Item(2, 0, -2));
            Assert.IsNotNull(sut.Item(1, 1, -2));
            Assert.IsNotNull(sut.Item(0, 2, -2));
            Assert.IsNotNull(sut.Item(-1, 2, -1));
            Assert.IsNotNull(sut.Item(-2, 2, 0));
            Assert.IsNotNull(sut.Item(-2, 1, 1));
            Assert.IsNotNull(sut.Item(-2, 0, 2));
            Assert.IsNotNull(sut.Item(-1, -1, 2));
            Assert.IsNotNull(sut.Item(0, -2, 2));
            Assert.IsNotNull(sut.Item(1, -2, 1));
            Assert.IsNotNull(sut.Item(2, -2, 0));
            Assert.IsNotNull(sut.Item(2, -1, -1));
        }
    }
}

using System;
using System.Collections.Generic;
using Hex;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HexCoreTests
{
    [TestClass]
    public class HexVisualCoordinatesTests
    {
        private HexMap<object> _hexMapFlat;
        private HexMap<object> _hexMapPointy;

        [TestInitialize]
        public void SetupSut()
        {
            _hexMapFlat = new HexMap<object>(HexOrientation.FlatTopped, 2);
            _hexMapPointy = new HexMap<object>(HexOrientation.PointyTopped, 2);
        }

        [TestMethod]
        public void CenterCoordinatesFlatTopCalculatedCorrectly()
        {
            var sut = _hexMapFlat.Item(1, 0);
            var expectedValue = new Tuple<double, double>(1.5, 0.8660254037844386);

            Assert.AreEqual(expectedValue.Item1, sut.CenterPoint.Item1);
            Assert.AreEqual(expectedValue.Item2, sut.CenterPoint.Item2);
        }

        [TestMethod]
        public void CenterCoordinatesPointyTopCalculatedCorrectly()
        {
            var sut = _hexMapPointy.Item(1, 0);
            var expectedValue = new Tuple<double, double>(1.7320508075688772, 0);

            Assert.AreEqual(expectedValue.Item1, sut.CenterPoint.Item1);
            Assert.AreEqual(expectedValue.Item2, sut.CenterPoint.Item2);
        }

        [TestMethod]
        public void VerticiesFlatTopCalculatedCorrectly()
        {
            var sut = _hexMapFlat.Item(1, 0);
            var expectedValues = new List<Tuple<double, double>>
                {
                    new Tuple<double, double>(2.5, 0.8660),
                    new Tuple<double, double>(2, 1.7321),
                    new Tuple<double, double>(1, 1.7321),
                    new Tuple<double, double>(0.5, 0.8660),
                    new Tuple<double, double>(1, 0),
                    new Tuple<double, double>(2, 0)
                };

            for (int i = 0; i <= 5; i++)
            {
                Assert.IsTrue(DoublesAreApproximatelyEqual(expectedValues[i].Item1, sut.Vertices[i].Item1));
                Assert.IsTrue(DoublesAreApproximatelyEqual(expectedValues[i].Item2, sut.Vertices[i].Item2));
            }
        }

        [TestMethod]
        public void VerticieDirectionsFlatTopAreSetCorrectly()
        {
            var sut = _hexMapFlat.Item(1, 0);

            var centerX = sut.CenterPoint.Item1;
            var centerY = sut.CenterPoint.Item2;

            for (int i = 0; i <= 5; i++)
            {
                var vertexX = sut.Vertices[i].Item1;
                var vertexY = sut.Vertices[i].Item2;

                switch (sut.VerticeDirections[i])
                {
                    case HexDirection.E:
                        Assert.IsTrue(vertexX > centerX);
                        Assert.IsTrue(DoublesAreApproximatelyEqual(centerY, vertexY));
                        break;
                    case HexDirection.W:
                        Assert.IsTrue(vertexX < centerX);
                        Assert.IsTrue(DoublesAreApproximatelyEqual(centerY, vertexY));
                        break;
                    case HexDirection.NE:
                        Assert.IsTrue(vertexX > centerX);
                        Assert.IsTrue(vertexY > centerY);
                        break;
                    case HexDirection.NW:
                        Assert.IsTrue(vertexX < centerX);
                        Assert.IsTrue(vertexY > centerY);
                        break;
                    case HexDirection.SE:
                        Assert.IsTrue(vertexX > centerX);
                        Assert.IsTrue(vertexY < centerY);
                        break;
                    case HexDirection.SW:
                        Assert.IsTrue(vertexX < centerX);
                        Assert.IsTrue(vertexY < centerY);
                        break;
                }
            }
        }

        [TestMethod]
        public void VerticieDirectionsPointyTopAreSetCorrectly()
        {
            var sut = _hexMapPointy.Item(1, 0);

            var centerX = sut.CenterPoint.Item1;
            var centerY = sut.CenterPoint.Item2;

            for (int i = 0; i <= 5; i++)
            {
                var verticieX = sut.Vertices[i].Item1;
                var verticieY = sut.Vertices[i].Item2;

                switch (sut.VerticeDirections[i])
                {
                    case HexDirection.N:
                        Assert.IsTrue(DoublesAreApproximatelyEqual(centerX, verticieX));
                        Assert.IsTrue(verticieY > centerY);
                        break;
                    case HexDirection.S:
                        Assert.IsTrue(DoublesAreApproximatelyEqual(centerX, verticieX));
                        Assert.IsTrue(verticieY < centerY);
                        break;
                    case HexDirection.NE:
                        Assert.IsTrue(verticieX > centerX);
                        Assert.IsTrue(verticieY > centerY);
                        break;
                    case HexDirection.NW:
                        Assert.IsTrue(verticieX < centerX);
                        Assert.IsTrue(verticieY > centerY);
                        break;
                    case HexDirection.SE:
                        Assert.IsTrue(verticieX > centerX);
                        Assert.IsTrue(verticieY < centerY);
                        break;
                    case HexDirection.SW:
                        Assert.IsTrue(verticieX < centerX);
                        Assert.IsTrue(verticieY < centerY);
                        break;
                }
            }
        }

        [TestMethod]
        public void VerticiesPointyTopCalculatedCorrectly()
        {
            var sut = _hexMapPointy.Item(1, 0);
            var expectedValues = new List<Tuple<double, double>>
                {
                    new Tuple<double, double>(2.5981, 0.5),
                    new Tuple<double, double>(1.7321, 1),
                    new Tuple<double, double>(0.8660, 0.5),
                    new Tuple<double, double>(0.8660, -0.5),
                    new Tuple<double, double>(1.7321, -1),
                    new Tuple<double, double>(2.5981, -0.5)
                };

            for (int i = 0; i <= 5; i++)
            {
                Assert.IsTrue(DoublesAreApproximatelyEqual(expectedValues[i].Item1, sut.Vertices[i].Item1));
                Assert.IsTrue(DoublesAreApproximatelyEqual(expectedValues[i].Item2, sut.Vertices[i].Item2));
            }
        }

        [TestMethod]
        public void FacesFlatTopDerivedCorrectly()
        {
            var sut = _hexMapFlat.Item(1, 0);
            var expectedValues = new List<Tuple<Tuple<double, double>, Tuple<double, double>>>
                {
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(
                        new Tuple<double, double>(2.5, 0.8660), new Tuple<double, double>(2, 1.7321)),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(
                        new Tuple<double, double>(2, 1.7321), new Tuple<double, double>(1, 1.7321)),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(
                        new Tuple<double, double>(1, 1.7321), new Tuple<double, double>(0.5, 0.8660)),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(
                        new Tuple<double, double>(0.5, 0.8660), new Tuple<double, double>(1, 0)),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(
                        new Tuple<double, double>(1, 0), new Tuple<double, double>(2, 0)),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(
                        new Tuple<double, double>(2, 0), new Tuple<double, double>(2.5, 0.8660))
                };

            for (int i = 0; i <= 5; i++)
            {
                Assert.IsTrue(DoublesAreApproximatelyEqual(expectedValues[i].Item1.Item1, sut.Faces[i].Item1.Item1));
                Assert.IsTrue(DoublesAreApproximatelyEqual(expectedValues[i].Item1.Item2, sut.Faces[i].Item1.Item2));
                Assert.IsTrue(DoublesAreApproximatelyEqual(expectedValues[i].Item2.Item1, sut.Faces[i].Item2.Item1));
                Assert.IsTrue(DoublesAreApproximatelyEqual(expectedValues[i].Item2.Item2, sut.Faces[i].Item2.Item2));
            }
        }

        [TestMethod]
        public void FacesPointyTopDerivedCorrectly()
        {
            var sut = _hexMapPointy.Item(1, 0);
            var expectedValues = new List<Tuple<Tuple<double, double>, Tuple<double, double>>>
                {
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(
                        new Tuple<double, double>(2.5981, 0.5), new Tuple<double, double>(1.7321, 1)),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(
                        new Tuple<double, double>(1.7321, 1), new Tuple<double, double>(0.8660, 0.5)),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(
                        new Tuple<double, double>(0.8660, 0.5), new Tuple<double, double>(0.8660, -0.5)),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(
                        new Tuple<double, double>(0.8660, -0.5), new Tuple<double, double>(1.7321, -1)),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(
                        new Tuple<double, double>(1.7321, -1), new Tuple<double, double>(2.5981, -0.5)),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(
                        new Tuple<double, double>(2.5981, -0.5), new Tuple<double, double>(2.5981, 0.5))
                };

            for (int i = 0; i <= 5; i++)
            {
                Assert.IsTrue(DoublesAreApproximatelyEqual(expectedValues[i].Item1.Item1, sut.Faces[i].Item1.Item1));
                Assert.IsTrue(DoublesAreApproximatelyEqual(expectedValues[i].Item1.Item2, sut.Faces[i].Item1.Item2));
                Assert.IsTrue(DoublesAreApproximatelyEqual(expectedValues[i].Item2.Item1, sut.Faces[i].Item2.Item1));
                Assert.IsTrue(DoublesAreApproximatelyEqual(expectedValues[i].Item2.Item2, sut.Faces[i].Item2.Item2));
            }
        }

        /// <summary>
        /// Returns true if the specified values are withing 0.0001 of each other
        /// </summary>
        private static bool DoublesAreApproximatelyEqual(double item1, double item2)
        {
            return Math.Abs(item1 - item2) <= 0.0001;
        }
    }
}

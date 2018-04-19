using System;
using GraphLibrary.DataModel;
using GraphLibrary.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphUnitTests.Basic
{
    [TestClass]
    public class BasicFunction
    {
        /// <summary>
        /// Test des Bildens des Identifiers im EdgeIdHelper
        /// </summary>
        [TestMethod]
        public void TestEdgeIDHelper()
        {
            Graph gDirected = new Graph("Test", true);
            Graph gNotDirected = new Graph("Test", false);

            var v10 = new Vertex("10");
            var v150 = new Vertex("150");

            // directed
            Assert.AreEqual("10 -> 150", EdgeIdHelper.GetId(gDirected, v10 , v150) , "1. Fall");
            Assert.AreEqual("150 -> 10", EdgeIdHelper.GetId(gDirected, v150, v10), "2. Fall");

            // not directed
            Assert.AreEqual("10 -> 150", EdgeIdHelper.GetId(gNotDirected, v10, v150), "3. Fall");
            Assert.AreEqual("10 -> 150", EdgeIdHelper.GetId(gNotDirected, v150, v10), "4. Fall");

        }
    }
}

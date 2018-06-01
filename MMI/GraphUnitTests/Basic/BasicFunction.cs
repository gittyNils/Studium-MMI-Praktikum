using System;
using GraphLibrary.Algorithm;
using GraphLibrary.DataModel;
using GraphLibrary.Helper;
using GraphLibrary.Interface;
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


        [TestMethod]
        public void PathFinding()
        {
            IGraph gDirected = new Graph("directed", true);
            gDirected.AddVertex("1");
            gDirected.AddVertex("2");
            gDirected.AddVertex("3");
            gDirected.AddVertex("4");
            gDirected.AddVertex("5");

            gDirected.AddEdge(gDirected.Vertices["1"], gDirected.Vertices["2"]);
            gDirected.AddEdge(gDirected.Vertices["1"], gDirected.Vertices["4"]);
            gDirected.AddEdge(gDirected.Vertices["1"], gDirected.Vertices["5"]);
            gDirected.AddEdge(gDirected.Vertices["2"], gDirected.Vertices["3"]);
            gDirected.AddEdge(gDirected.Vertices["3"], gDirected.Vertices["4"]);
            gDirected.AddEdge(gDirected.Vertices["5"], gDirected.Vertices["3"]);



            var resDirected = Traversing.FindPathBF(gDirected, "1", "3");


            Assert.AreEqual(2, resDirected.Count, "Count directed");
            Assert.AreEqual("2", resDirected[0].ToVertex.Identifier, "1 directed");
            Assert.AreEqual("3", resDirected[1].ToVertex.Identifier, "2 directed");



            IGraph gUnDirected = new Graph("directed", true);
            gUnDirected.AddVertex("1");
            gUnDirected.AddVertex("2");
            gUnDirected.AddVertex("3");
            gUnDirected.AddVertex("4");
            gUnDirected.AddVertex("5");

            gUnDirected.AddEdge(gUnDirected.Vertices["1"], gUnDirected.Vertices["2"]);
            gUnDirected.AddEdge(gUnDirected.Vertices["1"], gUnDirected.Vertices["4"]);
            gUnDirected.AddEdge(gUnDirected.Vertices["1"], gUnDirected.Vertices["5"]);
            gUnDirected.AddEdge(gUnDirected.Vertices["2"], gUnDirected.Vertices["3"]);
            gUnDirected.AddEdge(gUnDirected.Vertices["3"], gUnDirected.Vertices["4"]);
            gUnDirected.AddEdge(gUnDirected.Vertices["5"], gUnDirected.Vertices["3"]);


            var resUnDirected = Traversing.FindPathBF(gUnDirected, "1", "3");


            Assert.AreEqual(2, resUnDirected.Count, "Count directed");
            Assert.AreEqual("2", resUnDirected[0].ToVertex.Identifier, "1 undirected");
            Assert.AreEqual("3", resUnDirected[1].ToVertex.Identifier, "2 undirected");

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using GraphLibrary.Algorithm;
using GraphLibrary.Factory;
using GraphLibrary.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphUnitTests
{
    [TestClass]
    public class TestA1
    {
        [TestMethod]
        public void A1()
        {
            string name;


            name = "Graph 1";
            string s1 = File.ReadAllText(@"A1\SampleData\Graph1.txt");
            IGraph g1 = GraphFactory.GraphFromAdjMatrixStringWithoutCost(s1, name, false);

            Assert.AreEqual(3, ConnectedGraphComponents.CountComponentsWithBreadthFirst(g1), $"{name} Breitensuche");
            Assert.AreEqual(3, ConnectedGraphComponents.CountComponentsWithDepthFirst(g1), $"{name} Tiefensuche");




            name = "Graph 2";
            string s2 = File.ReadAllText(@"A1\SampleData\Graph2.txt");
            IGraph g2 = GraphFactory.GraphFromAdjListStringWithoutCost(s2, name, false);

            Assert.AreEqual(2, ConnectedGraphComponents.CountComponentsWithBreadthFirst(g2), $"{name} Breitensuche");
            Assert.AreEqual(2, ConnectedGraphComponents.CountComponentsWithDepthFirst(g2), $"{name} Tiefensuche");




            name = "Graph 3";
            string s3 = File.ReadAllText(@"A1\SampleData\Graph3.txt");
            IGraph g3 = GraphFactory.GraphFromAdjListStringWithoutCost(s3, name, false);

            Assert.AreEqual(4, ConnectedGraphComponents.CountComponentsWithBreadthFirst(g3), $"{name} Breitensuche");
            Assert.AreEqual(4, ConnectedGraphComponents.CountComponentsWithDepthFirst(g3), $"{name} Tiefensuche");




            name = "Graph 4";
            string s4 = File.ReadAllText(@"A1\SampleData\Graph4.txt");
            IGraph g4 = GraphFactory.GraphFromAdjListStringWithoutCost(s4, name, false);

            Assert.AreEqual(4, ConnectedGraphComponents.CountComponentsWithBreadthFirst(g4), $"{name} Breitensuche");
            Assert.AreEqual(4, ConnectedGraphComponents.CountComponentsWithDepthFirst(g4), $"{name} Tiefensuche");





        }
    }

}

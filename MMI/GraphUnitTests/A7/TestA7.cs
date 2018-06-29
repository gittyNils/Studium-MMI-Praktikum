using System;
using System.Collections.Generic;
using System.IO;
using GraphLibrary.Algorithm;
using GraphLibrary.Factory;
using GraphLibrary.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphUnitTests.A7
{
    [TestClass]
    public class TestA7
    {
        [TestMethod]
        public void A7()
        {
            List<IVertex> setA;
            List<IVertex> setB;

            var s = File.ReadAllText(@"A7\SampleData\Matching_100_100.txt");
            IGraph graph = GraphFactory.BipartitGraphFromAdjListString(s, "T1", true, out setA, out setB);


            var matching = Matching.BipartitMatching(graph, setA, setB);
            Assert.AreEqual(100, matching.Count, "Problem mit " + graph.Identifier);



            //////////////////////////////////////////////////

            s = File.ReadAllText(@"A7\SampleData\Matching2_100_100.txt");
            graph = GraphFactory.BipartitGraphFromAdjListString(s, "T2", true, out setA, out setB);


            matching = Matching.BipartitMatching(graph, setA, setB);
            Assert.AreEqual(99, matching.Count, "Problem mit " + graph.Identifier);


        }
    }
}

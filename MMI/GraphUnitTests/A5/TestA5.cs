using System;
using System.IO;
using GraphLibrary.Algorithm;
using GraphLibrary.Factory;
using GraphLibrary.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphUnitTests.A5
{
    [TestClass]
    public class TestA5
    {
        [TestMethod]
        public void A5()
        {
            string kapazität = Flow.KAPAZITÄT_VALUE;

            
            var flussFile = File.ReadAllText(@"A5\SampleData\Fluss.txt");
            var g1 = GraphFactory.GraphFromAdjListStringWithCost(flussFile, "Fluss.txt", kapazität, true);

            var s = g1.Vertices["0"];
            var t = g1.Vertices["7"];
            Flow.EdmondsKarp(g1, s, t);

            var maxFluss = Flow.ReadMaxFlow(s);

            Assert.AreEqual(4, maxFluss, "Fluss.txt");




            var g1g2 = File.ReadAllText(@"A5\SampleData\G_1_2.txt");
            var g2 = GraphFactory.GraphFromAdjListStringWithCost(g1g2, "G_1_2.txt", kapazität, true);

            s = g2.Vertices["0"];
            t = g2.Vertices["7"];
            Flow.EdmondsKarp(g2, s, t);

            maxFluss = Flow.ReadMaxFlow(s);

            Assert.AreEqual(0.735802d, Math.Round(maxFluss, 6), "G_1_2.txt");

        }
    }
}

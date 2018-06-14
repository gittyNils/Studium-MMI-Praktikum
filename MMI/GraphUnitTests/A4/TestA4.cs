using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GraphLibrary.Algorithm;
using GraphLibrary.Factory;
using GraphLibrary.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphUnitTests.A4
{
    [TestClass]
    public class TestA4
    {
        [TestMethod]
        public void A4()
        {
            string costKey = CONST.KOSTEN_VALUE;
            IEdge cycleEdge;


            string wege1 = File.ReadAllText(@"A4\SampleData\Wege1.txt");
            IGraph g1 = GraphFactory.GraphFromAdjListStringWithCost(wege1, nameof(wege1), costKey, true);

            var dijkstra1 = ShortestPath.Dijkstra(g1, "2", costKey);
            // weg von 2 zu 0 soll 6 sein
            double costs1 = ShortestPath.GetWayCost("2", "0", g1, dijkstra1, costKey);
            Assert.AreEqual(6d, costs1, "1");


            var mbf1 = ShortestPath.MooreBellmanFord(g1, "2", costKey, out cycleEdge);
            // weg von 2 zu 0 soll 6 sein
            costs1 = ShortestPath.GetWayCost("2", "0", g1, mbf1, costKey);
            Assert.AreEqual(6d, costs1, "2");





            string wege2 = File.ReadAllText(@"A4\SampleData\Wege2.txt");
            IGraph g2 = GraphFactory.GraphFromAdjListStringWithCost(wege2, nameof(wege2), costKey, true);

            // negative Kantengewichte -> keint Dijkstra


            var mbf2 = ShortestPath.MooreBellmanFord(g2, "2", costKey, out cycleEdge);
            // weg von 2 zu 0 soll 2 sein
            var costs2 = ShortestPath.GetWayCost("2", "0", g2, mbf2, costKey);
            Assert.AreEqual(2d, costs2, "4");









            string wege3 = File.ReadAllText(@"A4\SampleData\Wege3.txt");
            IGraph g3 = GraphFactory.GraphFromAdjListStringWithCost(wege3, nameof(wege3), costKey, true);

            // negative Kantengewichte -> keint Dijkstra

            var mbf3 = ShortestPath.MooreBellmanFord(g3, "0", costKey, out cycleEdge);
            // hier soll ein negtiver Zykel drin sein
            Assert.IsNull(mbf3, "4 NULL");
            Assert.IsNotNull(cycleEdge, "4 NOT NULL");








            // gerichtet
            string wege4 = File.ReadAllText(@"A4\SampleData\G_1_2.txt");
            IGraph g4 = GraphFactory.GraphFromAdjListStringWithCost(wege4, nameof(wege4), costKey, true);

            var dijkstra4 = ShortestPath.Dijkstra(g4, "0", costKey);
            // weg von 2 zu 0 soll 2 sein
            double costs4 = ShortestPath.GetWayCost("0", "1", g4, dijkstra4, costKey);
            Assert.AreEqual(5.54417d, Math.Round(costs4, 5), "10");


            var mbf4 = ShortestPath.MooreBellmanFord(g4, "0", costKey, out cycleEdge);
            // weg von 2 zu 0 soll 6 sein
            costs4 = ShortestPath.GetWayCost("0", "1", g4, mbf4, costKey);
            Assert.AreEqual(5.54417d, Math.Round(costs4, 5), "11");


            //// ungerichtet

            string wege5 = File.ReadAllText(@"A4\SampleData\G_1_2.txt");
            IGraph g5 = GraphFactory.GraphFromAdjListStringWithCost(wege5, nameof(wege5), costKey, false);

            var dijkstra5 = ShortestPath.Dijkstra(g5, "0", costKey);
            // weg von 2 zu 0 soll 2 sein
            double costs5 = ShortestPath.GetWayCost("0", "1", g5, dijkstra5, costKey);
            Assert.AreEqual(2.36796d, Math.Round(costs5, 5), "12");


            var mbf5 = ShortestPath.MooreBellmanFord(g5, "0", costKey, out cycleEdge);
            // weg von 2 zu 0 soll 6 sein
            costs5 = ShortestPath.GetWayCost("0", "1", g5, mbf5, costKey);
            Assert.AreEqual(2.36796d, Math.Round(costs5, 5), "13");
        }
    }
}

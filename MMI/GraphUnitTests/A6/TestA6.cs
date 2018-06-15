using System;
using System.IO;
using GraphLibrary.Algorithm;
using GraphLibrary.Factory;
using GraphLibrary.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphUnitTests.A6
{
    [TestClass]
    public class TestA6
    {
        [TestMethod]
        public void A6()
        {
            string kosten = CONST.KOSTEN_VALUE;
            string kapazität = CONST.KAPAZITÄT_VALUE;
            string balance = CONST.BALANCE_VALUE;

            {
                var s = File.ReadAllText(@"A6\SampleData\Kostenminimal1.txt");
                IGraph graph = GraphFactory.GraphFromAdjListStringWithDoubleCost(s, "T1", kosten, kapazität, balance, true);

                var ok = MinCostFlow.CycleCanceling(graph);
                double cost = double.NegativeInfinity;
                if (ok)
                {
                    cost = MinCostFlow.GetFlowCost(graph);
                }

                Assert.IsTrue(ok, "T1 CycleCanceling ok");
                Assert.AreEqual(3, cost, "T1 CycleCanceling cost");
            }


            {
                var s = File.ReadAllText(@"A6\SampleData\Kostenminimal2.txt");
                IGraph graph = GraphFactory.GraphFromAdjListStringWithDoubleCost(s, "T2", kosten, kapazität, balance, true);

                var ok = MinCostFlow.CycleCanceling(graph);
                double cost = double.NegativeInfinity;
                if (ok)
                {
                    cost = MinCostFlow.GetFlowCost(graph);
                }
                Assert.IsFalse(ok, "T2 CycleCanceling ok");
            }

            {
                var s = File.ReadAllText(@"A6\SampleData\Kostenminimal3.txt");
                IGraph graph = GraphFactory.GraphFromAdjListStringWithDoubleCost(s, "T3", kosten, kapazität, balance, true);

                var ok = MinCostFlow.CycleCanceling(graph);
                double cost = double.NegativeInfinity;
                if (ok)
                {
                    cost = MinCostFlow.GetFlowCost(graph);
                }

                Assert.IsTrue(ok, "T3 CycleCanceling ok");
                Assert.AreEqual(1537, cost, "T3 CycleCanceling cost");
            }

            {
                var s = File.ReadAllText(@"A6\SampleData\Kostenminimal4.txt");
                IGraph graph = GraphFactory.GraphFromAdjListStringWithDoubleCost(s, "T4", kosten, kapazität, balance, true);

                var ok = MinCostFlow.CycleCanceling(graph);
                double cost = double.NegativeInfinity;
                if (ok)
                {
                    cost = MinCostFlow.GetFlowCost(graph);
                }
                
                Assert.IsTrue(ok, "T4 CycleCanceling ok");
                Assert.AreEqual(0, cost, "T4 CycleCanceling cost");
            }

            {
                var s = File.ReadAllText(@"A6\SampleData\Kostenminimal5.txt");
                IGraph graph = GraphFactory.GraphFromAdjListStringWithDoubleCost(s, "T5", kosten, kapazität, balance, true);

                var ok = MinCostFlow.CycleCanceling(graph);
                double cost = double.NegativeInfinity;
                if (ok)
                {
                    cost = MinCostFlow.GetFlowCost(graph);
                }

                Assert.IsTrue(ok, "T5 CycleCanceling ok");
                Assert.AreEqual(0, cost, "T5 CycleCanceling cost");
            }

        }
    }
}

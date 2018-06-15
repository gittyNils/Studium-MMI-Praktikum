using GraphLibrary.Algorithm;
using GraphLibrary.Factory;
using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A6
{
    class Program
    {
        static void Main(string[] args)
        {
            string kosten = CONST.KOSTEN_VALUE;
            string kapazität = CONST.KAPAZITÄT_VALUE;
            string balance = CONST.BALANCE_VALUE;

            var s = File.ReadAllText(@"SampleData\Kostenminimal3.txt");
            IGraph graph = GraphFactory.GraphFromAdjListStringWithDoubleCost(s, "T1", kosten ,kapazität, balance, true);

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var ok = MinCostFlow.CycleCanceling(graph);
            sw.Stop();
            Console.WriteLine($"CycleCanceling {sw.ElapsedMilliseconds} ms");

            double cost = double.NegativeInfinity;
            if (ok)
            {
                cost = MinCostFlow.GetFlowCost(graph);
            }

            string costString = cost == double.NegativeInfinity ? "-" : cost.ToString();
            Console.WriteLine($"ok = {ok}, cost = {costString}");



            sw.Restart();
            ok = MinCostFlow.SuccessiveShortestPah(graph);
            sw.Stop();
            Console.WriteLine($"SuccessiveShortestPah {sw.ElapsedMilliseconds} ms");

            cost = double.NegativeInfinity;
            if (ok)
            {
                cost = MinCostFlow.GetFlowCost(graph);
            }

            costString = cost == double.NegativeInfinity ? "-" : cost.ToString();
            Console.WriteLine($"ok = {ok}, cost = {costString}");


            Console.ReadLine();


        }
    }
}

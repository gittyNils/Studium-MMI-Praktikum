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
            string fluss = CONST.FLUSS_VALUE;
            string balance = CONST.BALANCE_VALUE;

            var s = File.ReadAllText(@"SampleData\Kostenminimal1.txt");
            IGraph graph = GraphFactory.GraphFromAdjListStringWithDoubleCost(s, "T1", kosten ,kapazität, balance, true);

        }
    }
}

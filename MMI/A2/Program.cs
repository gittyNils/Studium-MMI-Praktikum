using GraphLibrary.Algorithm;
using GraphLibrary.Interface;
using GraphLibrary.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2
{
    class Program
    {
        static void Main(string[] args)
        {

            Graph g = new Graph("Test1", false);

            g.AddVertex("1");
            g.AddVertex("2");
            g.AddVertex("3");
            g.AddVertex("4");
            g.AddVertex("5");


            g.AddEdge(g.Vertices["1"], g.Vertices["2"], new Dictionary<string, int> { { "C", 13 } });
            g.AddEdge(g.Vertices["1"], g.Vertices["3"], new Dictionary<string, int> { { "C", 4 } });
            g.AddEdge(g.Vertices["1"], g.Vertices["5"], new Dictionary<string, int> { { "C", 9 } });
            g.AddEdge(g.Vertices["3"], g.Vertices["2"], new Dictionary<string, int> { { "C", 18 } });
            g.AddEdge(g.Vertices["3"], g.Vertices["4"], new Dictionary<string, int> { { "C", 7 } });
            g.AddEdge(g.Vertices["3"], g.Vertices["5"], new Dictionary<string, int> { { "C", 10 } });
            g.AddEdge(g.Vertices["2"], g.Vertices["4"], new Dictionary<string, int> { { "C", 12 } });
            g.AddEdge(g.Vertices["4"], g.Vertices["5"], new Dictionary<string, int> { { "C", 5 } });


            var a = MST.Prim(g, "C");


            Console.ReadLine();
        }
    }
}

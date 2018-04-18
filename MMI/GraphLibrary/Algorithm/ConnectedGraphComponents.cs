using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Algorithm
{
    /// <summary>
    /// Klasse mit Funktionen zu Zusammenhangskomponenten im Graph
    /// </summary>
    public static class ConnectedGraphComponents
    {


        public static int CountComponents(IGraph graph, bool breadthFirst)
        {
            int count = 0;
            int visitedCount = 0;
            graph.ResetSeen();

            if (breadthFirst)
            {
                do
                {
                    // Starte beim ersten nicht gesehenen Knoten
                    visitedCount += Traversing.BreadthFirstTraversal(graph, graph.Vertices.Values.First(x => x.Seen == false), false).Count;
                    count++;
                }
                while (visitedCount < graph.Vertices.Count);
            }


            return count;
        }


    }
}

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
            Dictionary<string, IVertex> visited = new Dictionary<string, IVertex>();

            if (breadthFirst)
            {
                do
                {
                    // Starte beim ersten nicht gesehenen Knoten
                    IVertex start = graph.Vertices.First(x => !visited.Keys.Contains(x.Key)).Value;

                    var newVisited = Traversing.BreadthFirst(graph, start);

                    // und als gesehen vermerken
                    foreach (var pair in newVisited)
                    {
                        visited.Add(pair.Key, pair.Value);
                    }

                    count++;
                }
                // Solange noch nicht alle Knoten gesehen wurden
                while (visited.Count < graph.Vertices.Count);
            }
            else
            {
                do
                {
                    // Starte beim ersten nicht gesehenen Knoten
                    IVertex start = graph.Vertices.First(x => !visited.Keys.Contains(x.Key)).Value;

                    var newVisited = Traversing.BreadthFirst(graph, start);

                    // und als gesehen vermerken
                    foreach (var pair in newVisited)
                    {
                        visited.Add(pair.Key, pair.Value);
                    }

                    count++;
                }
                // Solange noch nicht alle Knoten gesehen wurden
                while (visited.Count < graph.Vertices.Count);
            }


            return count;
        }


    }
}

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
        /// <summary>
        /// Ermittelt die Anzahl Zusammenhangskomponenten mit einer Breitensuchen-Traversierung
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static int CountComponentsWithBreadthFirst(IGraph graph)
        {
            return CountComponents(graph, Traversing.BreadthFirst);
        }


        /// <summary>
        /// Ermittelt die Anzahl Zusammenhangskomponenten mit einer Tiefensuchen-Traversierung
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static int CountComponentsWithDepthFirst(IGraph graph)
        {
            return CountComponents(graph, Traversing.DepthFirst);
        }


        /// <summary>
        /// Ermittelt die Anzahl Zusammenhangskomponenten mit einer beliebigen Traversierungsfunktion
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="traverseFunc"></param>
        /// <returns></returns>
        public static int CountComponents(IGraph graph, Func<IGraph, IVertex, Dictionary<string, IVertex>> traverseFunc)
        {
            int count = 0;
            Dictionary<string, IVertex> visited = new Dictionary<string, IVertex>();

            do
            {
                // Starte beim ersten nicht gesehenen Knoten
                IVertex start = graph.Vertices.First(x => !visited.Keys.Contains(x.Key)).Value;

                // Mit dem gewünschten Algorithmus Traversieren
                Dictionary<string, IVertex> newVisited = traverseFunc(graph, start);

                // und als gesehen vermerken
                foreach (var pair in newVisited)
                {
                    visited.Add(pair.Key, pair.Value);
                }

                count++;
            }
            // Solange noch nicht alle Knoten gesehen wurden
            while (visited.Count < graph.Vertices.Count);


            return count;
        }


    }
}

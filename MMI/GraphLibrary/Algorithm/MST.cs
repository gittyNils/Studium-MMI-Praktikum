using GraphLibrary.DataModel;
using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Algorithm
{
    /// <summary>
    /// Funktionen zur Bestimmung des MST (minimaler Spannbaum)
    /// </summary>
    public static class MST
    {
        public static IGraph Kruskal(IGraph graph, string costKey)
        {

            return null;
        }




        public static IGraph Prim(IGraph graph, string costKey)
        {
            // Minimale Kosten, um zu einer Edge zu kommen
            Dictionary<string, int> minCostsToVertexId = new Dictionary<string, int>();

            // Vorgänger/Parent
            Dictionary<string, string> parent = new Dictionary<string, string>();


            foreach (var entry in graph.Vertices)
            {
                // maximale Kosten eintragen
                minCostsToVertexId[entry.Key] = int.MaxValue;
                parent[entry.Key] = null;
            }

            // Kosten des Startknotens (einfach des ersten)
            string first = graph.Vertices.First().Key;
            minCostsToVertexId[first] = 0;

            // alle Knoten, die noch zu besuchen/in MST zu übernehmen sind
            Dictionary<string, IVertex> vertices = new Dictionary<string, IVertex>(graph.Vertices);
            while (vertices.Count > 0)
            {
                string key = GetMinsId(minCostsToVertexId);

                IVertex obj = vertices[key];

                // aus vertices raus
                vertices.Remove(key);
                // Auch aus Kosten raus, da diese nun Final
                minCostsToVertexId.Remove(key);

                // für alle Nachbarn
                foreach (var neighbour in obj.Neighbours)
                {
                    // hole die entsprechende Kante
                    var connectingEdge = graph.GetEdge(obj, neighbour.Value);

                    // wenn neighbour noch in der Liste der nicht besuchten ist
                    // und es ist noch besser als bisher von den Kosten her
                    if (vertices.ContainsKey(neighbour.Key) && connectingEdge.Costs[costKey] < minCostsToVertexId[neighbour.Key])
                    {
                        // Vorgänger/Parent setzen
                        parent[neighbour.Key] = key;

                        // Kosten anpassen
                        minCostsToVertexId[neighbour.Key] = connectingEdge.Costs[costKey];
                    }
                }
            }


            // Graph zusammenbauen
            IGraph mstGraph = new Graph("MST of" + graph.Identifier, graph.IsDirected);

            // alle Knoten rein
            foreach (var vertex in graph.Vertices)
            {
                mstGraph.AddVertex(vertex.Value.Identifier);
            }

            //Nun die Kanten aus der Parent rausholen:
            foreach (var entry in parent)
            {
                // startknoten hat null, also keinen Vorgänger
                if (entry.Value != null)
                {
                    var fromInNew = mstGraph.Vertices[entry.Value];
                    var toInNew = mstGraph.Vertices[entry.Key];

                    var fromInOrig = graph.Vertices[entry.Value];
                    var toInOrig = graph.Vertices[entry.Key];

                    var origEdge = graph.GetEdge(fromInOrig, toInOrig);

                    mstGraph.AddEdge(fromInNew, toInNew, origEdge.Costs);
                }
            }

            return mstGraph;
        }


        private static string GetMinsId(Dictionary<string, int> dict)
        {
            // Initialize min value
            int min = int.MaxValue;
            string minsId = null;

            foreach (var pair in dict)
            {
                if (dict[pair.Key] < min)
                {
                    min = dict[pair.Key];
                    minsId = pair.Key;
                }
            }

            return minsId;

        }

    }
}

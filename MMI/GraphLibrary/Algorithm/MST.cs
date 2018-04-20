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
            // Graph zusammenbauen
            IGraph mstGraph = new Graph("MST of" + graph.Identifier, graph.IsDirected);

            // alle Knoten rein
            foreach (var vertex in graph.Vertices)
            {
                mstGraph.AddVertex(vertex.Value.Identifier);
            }



            //sortierte Edges
            var sortedEdges = graph.Edges.Values.OrderBy(x => x.Costs[costKey]);


            //Durchgehen und einfügen, wenn kein Kreis entsteht im Baum
            foreach (var edge in sortedEdges)
            {
                //Try and Error


                var fromInNew = mstGraph.Vertices[edge.FromVertex.Identifier];
                var toInNew = mstGraph.Vertices[edge.ToVertex.Identifier];
                mstGraph.AddEdge(fromInNew, toInNew, new Dictionary<string, double>(edge.Costs));


                // ist nun ein Kreis?
                // Wenn der minimalgrad von G kleinSigma >= 2, dann enthält er auch einen einfachen Kreis von mindestens der Länge kleinSigma + 1
                // Isolierte Punkte nicht betrachten
                var minGrad = mstGraph.Vertices.Values
                    .Where(x => x.Edges.Count > 0)
                    .Min(x => x.Edges.Count);

                if (minGrad >= 2)
                {
                    // rückgängig machen
                    var newEdge = mstGraph.GetEdge(fromInNew, toInNew);
                    mstGraph.RemoveEdge(newEdge.Identifier);
                }
            }

            return mstGraph;
        }




        public static IGraph Prim(IGraph graph, string costKey)
        {
            // Initialisierung der Merker für Vorgänger (Dictionary TO -> From für Edges später zu bilden)

            // Minimale Kosten, um zu einer Edge zu kommen
            Dictionary<string, double> minCostsToVertexId = new Dictionary<string, double>();

            // Vorgänger
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

                    // wenn neighbour noch in der Liste der nicht besuchten ist bzw. noch nicht im MST ist
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

                    mstGraph.AddEdge(fromInNew, toInNew, new Dictionary<string, double>(origEdge.Costs));
                }
            }

            return mstGraph;
        }


        private static string GetMinsId(Dictionary<string, double> dict)
        {
            // Initialize min value
            double min = double.MaxValue;
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

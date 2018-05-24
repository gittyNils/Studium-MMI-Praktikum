using GraphLibrary.DataModel;
using GraphLibrary.DataStructures;
using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Algorithm
{
    /// <summary>
    /// Klasse mit Funktionen zur Bestimmung von kürzesten Wegen
    /// </summary>
    public static class ShortestPath
    {

        /// <summary>
        /// Abgeleitete Klasse vom PriorityQueueNode, die neben einer Priorität (hier Kosten) einen Knoten enthält
        /// </summary>
        class VertexNode : PriorityQueueNode
        {
            /// <summary>
            /// Knoten-Objekt
            /// </summary>
            public IVertex Vertex { get; set; }
        }


        public static IGraph Dijkstra(IGraph graph, string startId, string costKey)
        {
            // Ergebnis. Hat auch alle Knoten, die nicht erreich werden, aber dann entsprechend zu diesen keine Kanten.
            IGraph resultGraph = new Graph($"Dijkstra of {graph.Identifier} from {startId}", graph.IsDirected);

            // Vorgänger
            Dictionary<string, string> pred = new Dictionary<string, string>();


            // *** DIST ***
            // PriorityQueue zur Suche von dem unbesuchten Knoten, der minimales dist hat
            PriorityQueue<VertexNode> q = new PriorityQueue<VertexNode>(graph.Vertices.Count);
            // Schneller Zugriff auf die Elemente der Queue. Für jeden Knoten die bisher minimalen Kosten von startId aus ("Distanz")
            Dictionary<string, VertexNode> fastAccessAndDist = new Dictionary<string, VertexNode>();

            // Initialisieren
            foreach (var vertex in graph.Vertices)
            {
                // Ergebnisgraph mit den vorhandenen Knoten füllen
                resultGraph.AddVertex(vertex.Value.Identifier);

                // maximale Kosten eintragen
                var node = new VertexNode { Vertex = vertex.Value };
                q.Enqueue(node, double.PositiveInfinity);
                fastAccessAndDist[vertex.Key] = node;
                pred.Add(vertex.Key, null);

            }

            var startElem = fastAccessAndDist[startId];
            q.UpdatePriority(startElem, 0);
            pred[startId] = startId;



            // Solange noch weitere Knoten erreichbar sind (wer PositiveInfinity hat ist nicht erreichbar)
            VertexNode currentFromQueue;
            while (q.Count > 0 && (currentFromQueue = q.Dequeue()).Priority < double.PositiveInfinity)
            {
                IVertex currentVertex = currentFromQueue.Vertex;

                foreach (var neighbour in currentVertex.Neighbours.Values)
                {
                    var connectingEdge = graph.GetEdge(currentVertex, neighbour);
                    var neighbourInQueue = fastAccessAndDist[neighbour.Identifier];

                    // kann man vom aktuell betrachteten Knoten über die Kante zum Nachbarn günstiger als es bisher möglich war?
                    if (currentFromQueue.Priority + connectingEdge.Costs[costKey] < neighbourInQueue.Priority)
                    {
                        // Dann Weg über diesen nehmen und Vorgänger und Kosten aktualisieren
                        q.UpdatePriority(neighbourInQueue, currentFromQueue.Priority + connectingEdge.Costs[costKey]);
                        pred[neighbour.Identifier] = currentVertex.Identifier;
                    }
                }
            }




            // Ergebnis in Graph zusammenstellen
            // Pred hat die Vorgänger und fastAccess noch die letzten Priorities bzw. Kosten
            // der start kann aus dem Dictionary raus, da dieser Eintrag immer identisch ist und nicht zum resultGraph beiträgt.
            pred.Remove(startId);
            foreach (var elem in pred)
            {
                var edgeInInput = graph.GetEdge(graph.Vertices[elem.Value], graph.Vertices[elem.Key]);

                var fromVertex = resultGraph.Vertices[elem.Value];
                var toVertex = resultGraph.Vertices[elem.Key];
                resultGraph.AddEdge(fromVertex, toVertex, new Dictionary<string, double>(edgeInInput.Costs));
            }

            return resultGraph;
        }




        public static IGraph MooreBellmanFord(IGraph graph, string startId, string costKey)
        {
            // Ergebnis. Hat auch alle Knoten, die nicht erreich werden, aber dann entsprechend zu diesen keine Kanten.
            IGraph resultGraph = new Graph($"MooreBellmanFord of {graph.Identifier} from {startId}", graph.IsDirected);

            // Vorgänger
            Dictionary<string, string> pred = new Dictionary<string, string>();
            // *** DIST ***   Für jeden Knoten die bisher minimalen Kosten von startId aus ("Distanz")
            Dictionary<string, double> dist = new Dictionary<string, double>();

            // Initialisieren
            foreach (var vertex in graph.Vertices)
            {
                // Ergebnisgraph mit den vorhandenen Knoten füllen
                resultGraph.AddVertex(vertex.Value.Identifier);

                // maximale Kosten eintragen
                dist.Add(vertex.Key, double.PositiveInfinity);
                pred.Add(vertex.Key, null);

            }

            dist[startId] = 0;
            pred[startId] = startId;



            // n-1 mal aufrühren
            for (int i = 0; i < graph.Vertices.Count - 1; i++)
            {
                // Prüfe alle Kanten
                foreach (var edge in graph.Edges.Values)
                {
                    var from = edge.FromVertex.Identifier;
                    var to = edge.ToVertex.Identifier;

                    // Nutze ich die aktuelle Kante, kann ich damit die Kosten zum Zielknoten Verbessern?
                    if (dist[from] + edge.Costs[costKey] < dist[to])
                    {
                        // Dann Weg über diesen nehmen und Vorgänger und Kosten aktualisieren
                        dist[to] = dist[from] + edge.Costs[costKey];
                        pred[to] = from;
                    }
                }
            }


            // Ergebnis in Graph zusammenstellen
            // Pred hat die Vorgänger und fastAccess noch die letzten Priorities bzw. Kosten
            // der start kann aus dem Dictionary raus, da dieser Eintrag immer identisch ist und nicht zum resultGraph beiträgt.
            pred.Remove(startId);
            foreach (var elem in pred)
            {
                var edgeInInput = graph.GetEdge(graph.Vertices[elem.Value], graph.Vertices[elem.Key]);

                var fromVertex = resultGraph.Vertices[elem.Value];
                var toVertex = resultGraph.Vertices[elem.Key];
                resultGraph.AddEdge(fromVertex, toVertex, new Dictionary<string, double>(edgeInInput.Costs));
            }

            return resultGraph;
        }

    }
}

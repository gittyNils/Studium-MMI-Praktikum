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


        /// <summary>
        /// Bestimmt den Kürzeste-Wege-Baum von startId aus mit dem Dijkstra-Algorithmus
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="startId"></param>
        /// <param name="costKey"></param>
        /// <returns></returns>
        public static IGraph DijkstraForTree(IGraph graph, string startId, string costKey)
        {
            IGraph ret = null;
            var pred = Dijkstra(graph, startId, costKey);

            if (pred != null)
            {
                // Ergebnis in Graph zusammenstellen
                ret = BuildResultGraph(graph, pred, startId);
            }

            return ret;
        }


        /// <summary>
        /// Liefert die Vorgänger-Matrix von startId aus mit dem Dijkstra-Algorithmus
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="startId"></param>
        /// <param name="costKey"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Dijkstra(IGraph graph, string startId, string costKey)
        {
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
                // maximale Kosten eintragen
                var node = new VertexNode { Vertex = vertex.Value };
                q.Enqueue(node, double.PositiveInfinity);
                fastAccessAndDist[vertex.Key] = node;
                pred.Add(vertex.Key, null);
            }

            var startElem = fastAccessAndDist[startId];
            q.UpdatePriority(startElem, 0);
            pred[startId] = startId;





            // Merke, welche bereits gesehen wurden, damit diese nicht nochmal betrachtet werden
            HashSet<IVertex> seen = new HashSet<IVertex>();

            // Solange noch weitere Knoten erreichbar sind (wer PositiveInfinity hat ist nicht erreichbar)
            VertexNode currentFromQueue;
            while (q.Count > 0 && (currentFromQueue = q.Dequeue()).Priority < double.PositiveInfinity)
            {
                IVertex currentVertex = currentFromQueue.Vertex;
                seen.Add(currentVertex);

                foreach (var neighbour in currentVertex.Neighbours.Values)
                {
                    if (seen.Contains(neighbour))
                    {
                        continue;
                    }

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

            return pred;
        }




        /// <summary>
        /// Bestimmt den Kürzeste-Wege-Baum von startId aus mit dem Moore-Bellman-Ford-Algorithmus
        /// Dieser prüft auch auf negative Zykel und liefert, falls einer existiert null zurück
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="startId"></param>
        /// <param name="costKey"></param>
        /// <returns></returns>
        public static IGraph MooreBellmanFordForTree(IGraph graph, string startId, string costKey, out IEdge cycleEdge)
        {
            IGraph ret = null;
            var pred = MooreBellmanFord(graph, startId, costKey, out cycleEdge);

            if (pred != null)
            {
                // Ergebnis in Graph zusammenstellen
                ret = BuildResultGraph(graph, pred, startId);
            }

            return ret;
        }



        /// <summary>
        /// Bestimmt den Kürzeste-Wege-Baum von startId aus mit dem Moore-Bellman-Ford-Algorithmus
        /// Dieser prüft auch auf negative Zykel und liefert, falls einer existiert null zurück
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="startId"></param>
        /// <param name="costKey"></param>
        /// <param name="cycleEdge">Falls negativer Zykel, dann wurde dies bei dieser Kante erkannt.</param>
        /// <returns></returns>
        public static Dictionary<string, string> MooreBellmanFord(IGraph graph, string startId, string costKey, out IEdge cycleEdge)
        {
            cycleEdge = null;

            // Vorgänger
            Dictionary<string, string> pred = new Dictionary<string, string>();
            // *** DIST ***   Für jeden Knoten die bisher minimalen Kosten von startId aus ("Distanz")
            Dictionary<string, double> dist = new Dictionary<string, double>();

            // Initialisieren
            foreach (var vertex in graph.Vertices)
            {
                // maximale Kosten eintragen
                dist.Add(vertex.Key, double.PositiveInfinity);
                pred.Add(vertex.Key, null);
            }

            dist[startId] = 0;
            pred[startId] = startId;




            // Merker, ob sich in einer Iteration eine Änderung ergeben hat (benutzt zum Abbruch, wenn schon fertig)
            bool changedInIteration = true;

            int counter = 0;
            // n-1 mal aufrühren + ein mal für Prüfung auf negativen Zykel
            // wenn keine Änderung in einer Iteration, dann Fertig und Problem gelöst.
            while (++counter < graph.Vertices.Count && changedInIteration)
            {
                bool lastIteration = counter == graph.Vertices.Count - 1;
                changedInIteration = false;

                // Prüfe alle Kanten
                foreach (var edge in graph.Edges.Values)
                {
                    var from = edge.FromVertex.Identifier;
                    var to = edge.ToVertex.Identifier;

                    // Nutze ich die aktuelle Kante, kann ich damit die Kosten zum Zielknoten Verbessern?
                    if (dist[from] + edge.Costs[costKey] < dist[to])
                    {
                        changedInIteration = true;

                        // Die letzte iteration ist zur Prüfung auf Zykel
                        if (lastIteration)
                        {
                            cycleEdge = edge;
                            break;
                        }

                        // Dann Weg über diesen nehmen und Vorgänger und Kosten aktualisieren
                        dist[to] = dist[from] + edge.Costs[costKey];
                        pred[to] = from;
                    }

                    // Wenn Graph nicht gerichtet, dann muss die Kante in beide Richtungen getestet werden
                    if (!graph.IsDirected)
                    {
                        to = edge.FromVertex.Identifier;
                        from = edge.ToVertex.Identifier;

                        // Nutze ich die aktuelle Kante, kann ich damit die Kosten zum Zielknoten Verbessern?
                        if (dist[from] + edge.Costs[costKey] < dist[to])
                        {
                            changedInIteration = true;

                            // Die letzte iteration ist zur Prüfung auf Zykel
                            if (lastIteration)
                            {
                                cycleEdge = edge;
                                break;
                            }

                            // Dann Weg über diesen nehmen und Vorgänger und Kosten aktualisieren
                            dist[to] = dist[from] + edge.Costs[costKey];
                            pred[to] = from;
                        }
                    }

                }
            }


            // wenn negativer Zykel gefunden, dann steht in pred nichts sinnvolles.
            // return mit null als zeichen, dass hier neg. Zykel vorliergt
            if (cycleEdge != null)
            {
                pred = null;
            }

            return pred;
        }


        /// <summary>
        /// Mit der Vorgänger-Matrix die Kosten des Weges berechnen
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="graph"></param>
        /// <param name="pred"></param>
        /// <param name="costKey"></param>
        public static double GetWayCost(string from, string to, IGraph graph, Dictionary<string, string> pred, string costKey)
        {
            double costs = 0;

            // laufe rückwärts bis zum from von to aus
            IVertex currentVertex = graph.Vertices[to];

            while (currentVertex.Identifier != from)
            {
                var parent = pred[currentVertex.Identifier];
                var parentVertex = graph.Vertices[parent];
                costs += graph.GetEdge(parentVertex, currentVertex).Costs[costKey];
                currentVertex = parentVertex;
            }


            return costs;
        }


        /// <summary>
        /// Erstellt den Ergebnis-Graphen anhand der Vorgänger-Informationen
        /// </summary>
        /// <param name="graph">Original-Graph</param>
        /// <param name="pred">Ergebnis als Vorgänger-Matrik</param>
        /// <param name="startId">Knoten-ID, bei der gestartet wurde</param>
        /// <returns></returns>
        private static IGraph BuildResultGraph(IGraph graph, Dictionary<string, string> pred, string startId)
        {
            // Ergebnis. Hat auch alle Knoten, die nicht erreich werden, aber dann entsprechend zu diesen keine Kanten.
            IGraph resultGraph = new Graph($"MooreBellmanFord of {graph.Identifier} from {startId}", graph.IsDirected);

            // erst alle Knoten, die erreicht wurden hinzufügen
            foreach (var elem in pred)
            {
                // Wurde Knoten erreicht?
                if (elem.Value != null)
                {
                    resultGraph.AddVertex(elem.Key);
                }
            }

            foreach (var elem in pred)
            {
                // Kanten einfügen und so den Baum bilden (Ignoriere dabei den Vorgänger des StartKnotens, der er stelbst ist.)
                if (elem.Value != null && elem.Key != startId)
                {
                    var edgeInInput = graph.GetEdge(graph.Vertices[elem.Value], graph.Vertices[elem.Key]);

                    var fromVertex = resultGraph.Vertices[elem.Value];
                    var toVertex = resultGraph.Vertices[elem.Key];
                    resultGraph.AddEdge(fromVertex, toVertex, new Dictionary<string, double>(edgeInInput.Costs));
                }
            }


            return resultGraph;
        }
    }
}

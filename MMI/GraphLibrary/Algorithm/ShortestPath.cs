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
                    if (currentFromQueue.Priority + connectingEdge.Values[costKey] < neighbourInQueue.Priority)
                    {
                        // Dann Weg über diesen nehmen und Vorgänger und Kosten aktualisieren
                        q.UpdatePriority(neighbourInQueue, currentFromQueue.Priority + connectingEdge.Values[costKey]);
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
        /// Nutzt den Moore-Bellman-Ford Algorithmus um einen negativen Zykel zu finden.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="startId"></param>
        /// <param name="costKey"></param>
        /// <returns>negativer Zykel oder null, falls keiner da</returns>
        public static List<IEdge> MooreBellmanFordForNegativeCycle(IGraph graph, string startId, string costKey)
        {
            List<IEdge> ret = null;

            IEdge cycleEdge;
            var pred = MooreBellmanFord(graph, startId, costKey, out cycleEdge, true);

            if (cycleEdge != null)
            {
                // wenn negativer Zykel gefunden, dann die Kanten ermitteln, die im Zykel sind.
                // dazu mit Pred n Schritte rückwärts durch die Menge laufen, damit wir an einem Knoten auskommen, der auf jeden Fall in dem Zykel ist.
                // Nun über Parent-Beziehungen so lange laufen, bis wir wieder hier auskommen und wir haben den Zykel
                ret = new List<IEdge>();

                string tmpVertex = cycleEdge.ToVertex.Identifier;
                for (int i = 0; i < graph.Vertices.Count; i++)
                {
                    tmpVertex = pred[tmpVertex];
                }

                string startPoint = tmpVertex;
                do
                {
                    var toVertex = graph.Vertices[tmpVertex];
                    tmpVertex = pred[tmpVertex];
                    var fromVertex = graph.Vertices[tmpVertex];


                    ret.Add(graph.GetEdge(fromVertex, toVertex));
                }
                while (startPoint != tmpVertex);

                // Liste nochmal umdrehen, damit man mit den Kanten laufen kann
                ret.Reverse();

            }


            return ret;
        }


        /// <summary>
        /// Bestimmt den Kürzeste-Wege-Baum von startId aus mit dem Moore-Bellman-Ford-Algorithmus
        /// Dieser prüft auch auf negative Zykel und liefert, falls einer existiert null zurück (siehe auch getPredForNegCycle)
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="startId"></param>
        /// <param name="costKey"></param>
        /// <param name="cycleEdge">Falls negativer Zykel, dann wurde dies bei dieser Kante erkannt.</param>
        /// <param name="getPredForNegCycle">Gibt an, ob die Vorgängermatrix auch bei einem Negativen Zykel zurück gegeben werden soll.
        /// Bei false liefert der Algorithmus sonst null.</param>
        /// <returns></returns>
        public static Dictionary<string, string> MooreBellmanFord(IGraph graph, string startId, string costKey, out IEdge cycleEdge, bool getPredForNegCycle = false)
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





            // Zu untersuchende Kanten zusammenstellen
            List<IEdge> edges = new List<IEdge>();
            foreach (var edge in graph.Edges.Values)
            {
                edges.Add(edge);

                // Wenn Graph nicht gerichtet, dann muss die Kante in beide Richtungen getestet werden
                if (!graph.IsDirected)
                {
                    edges.Add(new Edge("TEMP of "+ edge.Identifier, edge.ToVertex, edge.FromVertex, edge.Values));
                }
            }


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
                foreach (var edge in edges)
                {
                    var from = edge.FromVertex.Identifier;
                    var to = edge.ToVertex.Identifier;

                    // Nutze ich die aktuelle Kante, kann ich damit die Kosten zum Zielknoten Verbessern?
                    if (dist[from] + edge.Values[costKey] < dist[to])
                    {
                        changedInIteration = true;

                        // Die letzte iteration ist zur Prüfung auf Zykel
                        if (lastIteration)
                        {
                            cycleEdge = edge;
                            break;
                        }

                        // Dann Weg über diesen nehmen und Vorgänger und Kosten aktualisieren
                        dist[to] = dist[from] + edge.Values[costKey];
                        pred[to] = from;
                    }
                }
            }


            // wenn negativer Zykel gefunden, dann steht in pred nichts sinnvolles.
            // return mit null als zeichen, dass hier neg. Zykel vorliergt
            // es sein denn anders überschrieben
            if (cycleEdge != null && !getPredForNegCycle)
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

            double costs = double.PositiveInfinity;

            var way = GetWay(from, to, graph, pred);

            if (way != null)
            {
                costs = way.Sum(x => x.Values[costKey]);
            }

            return costs;
        }



        /// <summary>
        /// Mit der Vorgänger-Matrix den kürzesten Weg zwischen zwei Knoten bilden.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="graph"></param>
        /// <param name="pred"></param>
        public static List<IEdge> GetWay(string from, string to, IGraph graph, Dictionary<string, string> pred)
        {
            List<IEdge> way = new List<IEdge>();

            // laufe rückwärts bis zum from von to aus
            IVertex currentVertex = graph.Vertices[to];

            while (pred[currentVertex.Identifier] != null && currentVertex.Identifier != from)
            {
                var parent = pred[currentVertex.Identifier];
                var parentVertex = graph.Vertices[parent];
                way.Add(graph.GetEdge(parentVertex, currentVertex));
                currentVertex = parentVertex;
            }

            // Reihenfolge richtig drehen
            way.Reverse();

            // es gibt den Weg, wenn der From-Knoten der ersten Kante der from-Knoten ist
            // sonst gibt es den nicht
            if (!(way.Count > 0 && way[0].FromVertex.Identifier == from))
            {
                // es gibt keinen Weg
                way = null;
            }

            return way;
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
                    resultGraph.AddEdge(fromVertex, toVertex, new Dictionary<string, double>(edgeInInput.Values));
                }
            }


            return resultGraph;
        }
    }
}

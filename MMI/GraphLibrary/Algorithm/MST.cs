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
        // Hier ist die Idee mit den Gruppen gut, aber der Haushalt damit zu aufwändig (Vereinen von zwei großen Gruppen hat zu viele Änderungen zur Auswirkung)
        // Auch das Verbinden der Gruppen kann dynamisch besser ablaufen (kleinere in größere)
        public static IGraph KruskalV1(IGraph graph, string costKey)
        {
            // From ist immer der Gruppengeber
            Dictionary<string, string> groupMapPerVertex = new Dictionary<string, string>();
            Dictionary<string, List<string>> groupMap = new Dictionary<string, List<string>>();


            // Graph zusammenbauen
            IGraph mstGraph = new Graph("MST of" + graph.Identifier, graph.IsDirected);

            // alle Knoten rein
            foreach (var vertex in graph.Vertices)
            {
                mstGraph.AddVertex(vertex.Value.Identifier);
                groupMapPerVertex[vertex.Value.Identifier] = vertex.Value.Identifier;
                groupMap[vertex.Value.Identifier] = new List<string> { vertex.Value.Identifier };
            }



            //sortierte Edges
            var sortedEdges = graph.Edges.Values.OrderBy(x => x.Costs[costKey]);


            //Durchgehen und einfügen, wenn kein Kreis entsteht im Baum
            foreach (var edge in sortedEdges)
            {
                string fromId = edge.FromVertex.Identifier;
                string toId = edge.ToVertex.Identifier;

                //Wäre nun bei hinzunahme ein Kreis?

                string fromGroup = groupMapPerVertex[fromId];
                string toGroup = groupMapPerVertex[toId];



                bool wouldBeCycle = fromGroup == toGroup;
                if (!wouldBeCycle)
                {
                    // von ToGroup in FromGroup
                    // Dabei ToGroup die kleinere
                    if (groupMap[toGroup].Count > groupMap[fromGroup].Count)
                    {
                        var tmp = toGroup;
                        toGroup = fromGroup;
                        fromGroup = tmp;
                    }


                    // to in Gruppe von From
                    foreach (var member in groupMap[toGroup])
                    {
                        groupMapPerVertex[member] = groupMapPerVertex[fromGroup];
                    }

                    // direkt ganze Liste rüber schieben
                    groupMap[fromGroup].AddRange(groupMap[toGroup]);

                    //alte Gruppe auflösen
                    groupMap.Remove(toGroup);

                    var from = mstGraph.Vertices[fromId];
                    var to = mstGraph.Vertices[toId];
                    mstGraph.AddEdge(from, to, new Dictionary<string, double>(edge.Costs));
                }


            }

            return mstGraph;
        }



        public static IGraph Kruskal(IGraph graph, string costKey)
        {
            // Graph zusammenbauen
            IGraph mstGraph = new Graph("MST of" + graph.Identifier, graph.IsDirected);

            var disJointSet = new DisJointSet<string>();

            // alle Knoten rein
            foreach (var vertex in graph.Vertices)
            {
                mstGraph.AddVertex(vertex.Value.Identifier);
                disJointSet.MakeSet(vertex.Key);
            }



            //sortierte Edges
            var sortedEdges = graph.Edges.Values.OrderBy(x => x.Costs[costKey]);


            //Durchgehen und einfügen, wenn kein Kreis entsteht im Baum
            foreach (var edge in sortedEdges)
            {
                string fromId = edge.FromVertex.Identifier;
                string toId = edge.ToVertex.Identifier;

                //Wäre nun bei hinzunahme ein Kreis?
                // Wenn man zwei Mengen nochmal verbindet, dann entstände ein Kreis

                var setA = disJointSet.FindSet(fromId);
                var setB = disJointSet.FindSet(toId);

                //can't pick edge with both ends already in MST
                if (!setA.Equals(setB))
                {
                    //union picked edge vertice sets
                    disJointSet.Union(setA, setB);

                    var from = mstGraph.Vertices[fromId];
                    var to = mstGraph.Vertices[toId];
                    mstGraph.AddEdge(from, to, new Dictionary<string, double>(edge.Costs));
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
                minCostsToVertexId[entry.Key] = double.MaxValue;
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


























        class HeapElem : IComparable
        {
            public double Cost { get; set; }
            public string ToVertexKey { get; set; }
            

            public int CompareTo(object other)
            {
                if (!(other is HeapElem))
                {
                    return -1;
                }

                return Cost.CompareTo(((HeapElem)other).Cost);
            }
        }


        // Hier kommen leicht andere Werte bei den Kosten raus, also Funktioniert vermutlich nicht korrekt

        public static IGraph PrimV2(IGraph graph, string costKey)
        {
            // Initialisierung der Merker für Vorgänger (Dictionary TO -> From für Edges später zu bilden)

            // Minimale Kosten, um zu einer Edge zu kommen
            FibornacciMinHeap<HeapElem> heap = new FibornacciMinHeap<HeapElem>();
            Dictionary<string, FibornacciHeapNode<HeapElem>> fastAccess = new Dictionary<string, FibornacciHeapNode<HeapElem>>();

            // Vorgänger
            Dictionary<string, string> parent = new Dictionary<string, string>();

            foreach (var entry in graph.Vertices)
            {
                // maximale Kosten eintragen
                var node = heap.Insert(new HeapElem { Cost = double.MaxValue, ToVertexKey = entry.Key });
                fastAccess[entry.Key] = node;
                parent[entry.Key] = null;
            }

            // Kosten des Startknotens (einfach des ersten)
            string first = graph.Vertices.First().Key;
            var elem = fastAccess[first];
            elem.Value.Cost = 0;
            heap.DecrementKey(elem);



            // alle Knoten, die noch zu besuchen/in MST zu übernehmen sind
            Dictionary<string, IVertex> vertices = new Dictionary<string, IVertex>(graph.Vertices);
            while (vertices.Count > 0)
            {
                var minObj = heap.ExtractMin();

                IVertex obj = vertices[minObj.ToVertexKey];

                // aus vertices raus
                vertices.Remove(minObj.ToVertexKey);

                // für alle Nachbarn
                foreach (var neighbour in obj.Neighbours)
                {
                    // hole die entsprechende Kante
                    var connectingEdge = graph.GetEdge(obj, neighbour.Value);

                    var heapElem = fastAccess[neighbour.Key];

                    // wenn neighbour noch in der Liste der nicht besuchten ist bzw. noch nicht im MST ist
                    // und es ist noch besser als bisher von den Kosten her
                    if (vertices.ContainsKey(neighbour.Key) && connectingEdge.Costs[costKey] < heapElem.Value.Cost)
                    {
                        // Vorgänger/Parent setzen
                        parent[neighbour.Key] = minObj.ToVertexKey;

                        // Kosten anpassen
                        heapElem.Value.Cost = connectingEdge.Costs[costKey];
                        heap.DecrementKey(heapElem);
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
















        public class MyNode : FastPriorityQueueNode
        {
            //Put custom properties here
            public string Id { get; set; }
        }



        public static IGraph PrimV3(IGraph graph, string costKey)
        {
            // Initialisierung der Merker für Vorgänger (Dictionary TO -> From für Edges später zu bilden)

            // Minimale Kosten, um zu einer Edge zu kommen
            FastPriorityQueue<MyNode> q = new FastPriorityQueue<MyNode>(graph.Vertices.Count);
            Dictionary<string, MyNode> fastAccess = new Dictionary<string, MyNode>();

            // Vorgänger
            Dictionary<string, string> parent = new Dictionary<string, string>();

            foreach (var entry in graph.Vertices)
            {
                // maximale Kosten eintragen
                var node = new MyNode { Id = entry.Key };
                q.Enqueue(node, double.MaxValue);
                fastAccess[entry.Key] = node;
                parent[entry.Key] = null;
            }

            // Kosten des Startknotens (einfach des ersten)
            string first = graph.Vertices.First().Key;
            var elem = fastAccess[first];
            q.UpdatePriority(elem, 0);



            // alle Knoten, die noch zu besuchen/in MST zu übernehmen sind
            Dictionary<string, IVertex> vertices = new Dictionary<string, IVertex>(graph.Vertices);
            while (vertices.Count > 0)
            {
                var minObj = q.Dequeue();

                IVertex obj = vertices[minObj.Id];

                // aus vertices raus
                vertices.Remove(minObj.Id);

                // für alle Nachbarn
                foreach (var neighbour in obj.Neighbours)
                {
                    // hole die entsprechende Kante
                    var connectingEdge = graph.GetEdge(obj, neighbour.Value);

                    var neighbourElem = fastAccess[neighbour.Key];

                    // wenn neighbour noch in der Liste der nicht besuchten ist bzw. noch nicht im MST ist
                    // und es ist noch besser als bisher von den Kosten her
                    if (vertices.ContainsKey(neighbour.Key) && connectingEdge.Costs[costKey] < neighbourElem.Priority)
                    {
                        // Vorgänger/Parent setzen
                        parent[neighbour.Key] = minObj.Id;

                        // Kosten anpassen
                        q.UpdatePriority(neighbourElem, connectingEdge.Costs[costKey]);
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


























        class DoubleWrapper : IComparable
        {
            public double Val { get; set; }

            public string Key { get; set; }

            public int CompareTo(object obj)
            {
                return Val.CompareTo(((DoubleWrapper)obj).Val);
            }
        }
    }
}
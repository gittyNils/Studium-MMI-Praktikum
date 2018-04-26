using GraphLibrary.DataModel;
using GraphLibrary.Interface;
using GraphLibrary.DataStructures;
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
        /// <summary>
        /// Bestimmung des MST mit Hilfe des Algorithmus von Kruskal.
        /// (Variante mit Dictionaries)
        /// (Verbinde je zwei Knoten mit global kostengünstigster Kante, es sei denn, es würde ein Kreis entstehen.)
        /// Die Prüfung des Kreises erfolgt über eine Mengenbildung der Knoten mit je einem Stellvertreter.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="costKey">Key, unter dem die zu betrachtenden Kosten abgespeichert wurden</param>
        /// <returns></returns>
        public static IGraph KruskalV1(IGraph graph, string costKey)
        {
            // TODO: als ein Dict
            // Pairing von einzelnem Knoten zu Stellvertreter
            Dictionary<string, string> groupMapPerVertex = new Dictionary<string, string>();
            // Anhand des Stellvertreters alle Mitglieder der Gruppe speichern
            Dictionary<string, List<string>> groupMap = new Dictionary<string, List<string>>();


            // Neuen Graphen zusammenbauen
            IGraph mstGraph = new Graph("MST of" + graph.Identifier, graph.IsDirected);

            // alle Knoten rein und Mengen mit nur einem Knoten als Mitglied initialisieren
            foreach (var vertex in graph.Vertices)
            {
                mstGraph.AddVertex(vertex.Value.Identifier);
                groupMapPerVertex[vertex.Value.Identifier] = vertex.Value.Identifier;
                groupMap[vertex.Value.Identifier] = new List<string> { vertex.Value.Identifier };
            }



            //sortierte Edges nach Kosten
            var sortedEdges = graph.Edges.Values.OrderBy(x => x.Costs[costKey]);


            //Durchgehen und einfügen, wenn kein Kreis entsteht im Baum
            foreach (var edge in sortedEdges)
            {
                string fromId = edge.FromVertex.Identifier;
                string toId = edge.ToVertex.Identifier;

                // Wäre nun bei hinzunahme ein Kreis? Vergleiche dazu die Mengen durch die Stellvertreter.
                string fromGroup = groupMapPerVertex[fromId];
                string toGroup = groupMapPerVertex[toId];


                // Wenn man zwei diskunjte Mengen verbindet, entsteht kein Kreis. In die gleiche Menge noch eine Verbindung/Kante, dann gäbe es einen Kreis.
                bool wouldBeCycle = fromGroup == toGroup;
                if (!wouldBeCycle)
                {
                    // Sie Gruppe von To geht in die Gruppe von From. Dabei ggf. Tauschen, wenn To-Gruppe größer ist.
                    if (groupMap[toGroup].Count > groupMap[fromGroup].Count)
                    {
                        var tmp = toGroup;
                        toGroup = fromGroup;
                        fromGroup = tmp;
                    }


                    // To in Gruppe von From
                    foreach (var member in groupMap[toGroup])
                    {
                        groupMapPerVertex[member] = groupMapPerVertex[fromGroup];
                    }
                    // direkt ganze Liste rüber schieben
                    groupMap[fromGroup].AddRange(groupMap[toGroup]);

                    //alte Gruppe auflösen
                    groupMap.Remove(toGroup);

                    // Kante in MST aufnehmen
                    var from = mstGraph.Vertices[fromId];
                    var to = mstGraph.Vertices[toId];
                    mstGraph.AddEdge(from, to, new Dictionary<string, double>(edge.Costs));
                }
            }

            return mstGraph;
        }


        /// <summary>
        /// Bestimmung des MST mit Hilfe des Algorithmus von Kruskal.
        /// (Variante mit disjunkten Mengen)
        /// (Verbinde je zwei Knoten mit global kostengünstigster Kante, es sei denn, es würde ein Kreis entstehen.)
        /// Die Prüfung des Kreises erfolgt über eine Mengenbildung der Knoten mit je einem Stellvertreter.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="costKey">Key, unter dem die zu betrachtenden Kosten abgespeichert wurden</param>
        /// <returns></returns>
        public static IGraph Kruskal(IGraph graph, string costKey)
        {
            // Neuen Graphen zusammenbauen
            IGraph mstGraph = new Graph("MST of" + graph.Identifier, graph.IsDirected);

            // eine Menge von disjunkten Mengen
            var disJointSet = new DisJointSet<string>();

            // alle Knoten rein und Mengen mit nur einem Knoten als Mitglied initialisieren
            foreach (var vertex in graph.Vertices)
            {
                mstGraph.AddVertex(vertex.Value.Identifier);
                disJointSet.AddSet(vertex.Key);
            }



            //sortierte Edges nach Kosten
            var sortedEdges = graph.Edges.Values.OrderBy(x => x.Costs[costKey]);


            // Durchgehen und einfügen, wenn kein Kreis entsteht im Baum
            foreach (var edge in sortedEdges)
            {
                string fromId = edge.FromVertex.Identifier;
                string toId = edge.ToVertex.Identifier;

                // Wäre nun bei hinzunahme ein Kreis? Vergleiche dazu die Mengen durch die Stellvertreter.
                var setA = disJointSet.FindSet(fromId);
                var setB = disJointSet.FindSet(toId);


                // Wenn man zwei diskunjte Mengen verbindet, entsteht kein Kreis. In die gleiche Menge noch eine Verbindung/Kante, dann gäbe es einen Kreis.
                if (!setA.Equals(setB))
                {
                    // vereinige die Mengen
                    disJointSet.Union(setA, setB);

                    // Kante in MST aufnehmen
                    var from = mstGraph.Vertices[fromId];
                    var to = mstGraph.Vertices[toId];
                    mstGraph.AddEdge(from, to, new Dictionary<string, double>(edge.Costs));
                }
            }

            return mstGraph;
        }






        /// <summary>
        /// Abgeleitete Klasse vom PriorityQueueNode, die den Identifier eines Knotens zu einer Priorität(hier Kosten) enthält
        /// </summary>
        public class VertexNode : PriorityQueueNode
        {
            /// <summary>
            /// Identifier eines Knotens
            /// </summary>
            public string Id { get; set; }
        }


        /// <summary>
        /// Bestimmung des MST mit Hilfe des Algorithmus von Prim. (Nutzen Priority Queue)
        /// (Verbinde einen im MST befindlichen Knoten mit kostengünstigster Kante zu einem Nachbarn, der nicht schon im MST ist.)
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="costKey">Key, unter dem die zu betrachtenden Kosten abgespeichert wurden</param>
        /// <returns></returns>
        public static IGraph Prim(IGraph graph, string costKey)
        {
            // alle Knoten, die noch zu besuchen/in MST zu übernehmen sind
            Dictionary<string, IVertex> vertices = new Dictionary<string, IVertex>(graph.Vertices);

            // Minimale Kosten, um zu einer Edge zu kommen
            PriorityQueue<VertexNode> q = new PriorityQueue<VertexNode>(graph.Vertices.Count);
            // Schneller Zugriff auf die Elemente der Queue
            Dictionary<string, VertexNode> fastAccess = new Dictionary<string, VertexNode>();

            // Initialisierung der Merker für Vorgänger (Dictionary TO -> From für Edges später zu bilden)
            Dictionary<string, string> parent = new Dictionary<string, string>();


            foreach (var entry in graph.Vertices)
            {
                // maximale Kosten eintragen
                var node = new VertexNode { Id = entry.Key };
                q.Enqueue(node, double.MaxValue);
                fastAccess[entry.Key] = node;
                parent[entry.Key] = null;
            }

            // Kosten des Startknotens (einfach des ersten)
            string first = graph.Vertices.First().Key;
            var elem = fastAccess[first];
            q.UpdatePriority(elem, 0);


            // alle Knoten abarbeiten, da alle in den MST müssen
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
                        // Vorgänger setzen
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
                // Startknoten hat null, also keinen Vorgänger
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





        

        #region Obsolete (Too Slow or Wrong)


        ///// <summary>
        ///// Bestimmung des MST mit Hilfe des Algorithmus von Prim. (Durch Lineare Suche des Minimums sehr sehr Langsam)
        ///// (Verbinde einen im MST befindlichen Knoten mit kostengünstigster Kante zu einem Nachbarn, der nicht schon im MST ist.)
        ///// </summary>
        ///// <param name="graph"></param>
        ///// <param name="costKey">Key, unter dem die zu betrachtenden Kosten abgespeichert wurden</param>
        ///// <returns></returns>
        //public static IGraph PrimLinearSearchAndSlow(IGraph graph, string costKey)
        //{
        //    // Minimale Kosten, um vom Vorgänger zu einer Edge zu kommen. Hier sind nur Knoten drin, die von der bisherigen Menge benachbart sind.
        //    Dictionary<string, double> minCostsForVertexId = new Dictionary<string, double>();

        //    // Initialisierung der Merker für Vorgänger (Dictionary TO -> From für Edges später zu bilden)
        //    Dictionary<string, string> parent = new Dictionary<string, string>();

        //    // alle Knoten, die noch zu besuchen/in MST zu übernehmen sind
        //    Dictionary<string, IVertex> vertices = new Dictionary<string, IVertex>(graph.Vertices);

        //    foreach (var entry in graph.Vertices)
        //    {
        //        // maximale Kosten eintragen
        //        minCostsForVertexId[entry.Key] = double.MaxValue;
        //        parent[entry.Key] = null;
        //    }

        //    // Kosten des Startknotens (einfach des ersten)
        //    string first = graph.Vertices.First().Key;
        //    minCostsForVertexId[first] = 0;


        //    // alle Knoten abarbeiten, da alle in den MST müssen
        //    while (vertices.Count > 0)
        //    {
        //        string key = PopMinId(minCostsForVertexId);

        //        IVertex currentVertex = vertices[key];

        //        // aus Knoten raus
        //        vertices.Remove(key);

        //        // für alle Nachbarn
        //        foreach (var neighbour in currentVertex.Neighbours)
        //        {
        //            // hole die entsprechende Kante
        //            var connectingEdge = graph.GetEdge(currentVertex, neighbour.Value);

        //            // wenn Neighbour noch in der Liste der nicht besuchten ist bzw. noch nicht im MST ist
        //            // und es ist noch besser als bisher von den Kosten her
        //            if (vertices.ContainsKey(neighbour.Key) && connectingEdge.Costs[costKey] < minCostsForVertexId[neighbour.Key])
        //            {
        //                // Vorgänger setzen
        //                parent[neighbour.Key] = key;

        //                // Kosten anpassen
        //                minCostsForVertexId[neighbour.Key] = connectingEdge.Costs[costKey];
        //            }
        //        }
        //    }


        //    // Graph zusammenbauen
        //    IGraph mstGraph = new Graph("MST of" + graph.Identifier, graph.IsDirected);

        //    // alle Knoten rein
        //    foreach (var vertex in graph.Vertices)
        //    {
        //        mstGraph.AddVertex(vertex.Value.Identifier);
        //    }

        //    //Nun die Kanten aus der Parent rausholen:
        //    foreach (var entry in parent)
        //    {
        //        // Startknoten hat null, also keinen Vorgänger
        //        if (entry.Value != null)
        //        {
        //            var fromInOrig = graph.Vertices[entry.Value];
        //            var toInOrig = graph.Vertices[entry.Key];
        //            var origEdge = graph.GetEdge(fromInOrig, toInOrig);

        //            var fromInNew = mstGraph.Vertices[entry.Value];
        //            var toInNew = mstGraph.Vertices[entry.Key];

        //            mstGraph.AddEdge(fromInNew, toInNew, new Dictionary<string, double>(origEdge.Costs));
        //        }
        //    }

        //    return mstGraph;
        //}


        ///// <summary>
        ///// Liefert den kleinsten Wert aus dem Dictionary und liefert dessen Identifier zurück.
        ///// Dabei wir der Eintrag direkt aus dem Dictionary gelöscht.
        ///// </summary>
        ///// <param name="dict"></param>
        ///// <returns></returns>
        //private static string PopMinId(Dictionary<string, double> dict)
        //{
        //    // Initialize min value
        //    double min = double.MaxValue;
        //    string minsId = null;

        //    foreach (var pair in dict)
        //    {
        //        if (dict[pair.Key] < min)
        //        {
        //            min = dict[pair.Key];
        //            minsId = pair.Key;
        //        }
        //    }

        //    dict.Remove(minsId);

        //    return minsId;
        //}




        //class HeapElem : IComparable
        //{
        //    public double Cost { get; set; }
        //    public string ToVertexKey { get; set; }


        //    public int CompareTo(object other)
        //    {
        //        if (!(other is HeapElem))
        //        {
        //            return -1;
        //        }

        //        return Cost.CompareTo(((HeapElem)other).Cost);
        //    }
        //}


        //// Hier kommen leicht andere Werte bei den Kosten raus, also Funktioniert vermutlich nicht korrekt

        //public static IGraph PrimV2(IGraph graph, string costKey)
        //{
        //    // Initialisierung der Merker für Vorgänger (Dictionary TO -> From für Edges später zu bilden)

        //    // Minimale Kosten, um zu einer Edge zu kommen
        //    FibornacciMinHeap<HeapElem> heap = new FibornacciMinHeap<HeapElem>();
        //    Dictionary<string, FibornacciHeapNode<HeapElem>> fastAccess = new Dictionary<string, FibornacciHeapNode<HeapElem>>();

        //    // Vorgänger
        //    Dictionary<string, string> parent = new Dictionary<string, string>();

        //    foreach (var entry in graph.Vertices)
        //    {
        //        // maximale Kosten eintragen
        //        var node = heap.Insert(new HeapElem { Cost = double.MaxValue, ToVertexKey = entry.Key });
        //        fastAccess[entry.Key] = node;
        //        parent[entry.Key] = null;
        //    }

        //    // Kosten des Startknotens (einfach des ersten)
        //    string first = graph.Vertices.First().Key;
        //    var elem = fastAccess[first];
        //    elem.Value.Cost = 0;
        //    heap.DecrementKey(elem);



        //    // alle Knoten, die noch zu besuchen/in MST zu übernehmen sind
        //    Dictionary<string, IVertex> vertices = new Dictionary<string, IVertex>(graph.Vertices);
        //    while (vertices.Count > 0)
        //    {
        //        var minObj = heap.ExtractMin();

        //        IVertex obj = vertices[minObj.ToVertexKey];

        //        // aus vertices raus
        //        vertices.Remove(minObj.ToVertexKey);

        //        // für alle Nachbarn
        //        foreach (var neighbour in obj.Neighbours)
        //        {
        //            // hole die entsprechende Kante
        //            var connectingEdge = graph.GetEdge(obj, neighbour.Value);

        //            var heapElem = fastAccess[neighbour.Key];

        //            // wenn neighbour noch in der Liste der nicht besuchten ist bzw. noch nicht im MST ist
        //            // und es ist noch besser als bisher von den Kosten her
        //            if (vertices.ContainsKey(neighbour.Key) && connectingEdge.Costs[costKey] < heapElem.Value.Cost)
        //            {
        //                // Vorgänger/Parent setzen
        //                parent[neighbour.Key] = minObj.ToVertexKey;

        //                // Kosten anpassen
        //                heapElem.Value.Cost = connectingEdge.Costs[costKey];
        //                heap.DecrementKey(heapElem);
        //            }
        //        }
        //    }


        //    // Graph zusammenbauen
        //    IGraph mstGraph = new Graph("MST of" + graph.Identifier, graph.IsDirected);

        //    // alle Knoten rein
        //    foreach (var vertex in graph.Vertices)
        //    {
        //        mstGraph.AddVertex(vertex.Value.Identifier);
        //    }

        //    //Nun die Kanten aus der Parent rausholen:
        //    foreach (var entry in parent)
        //    {
        //        // Startknoten hat null, also keinen Vorgänger
        //        if (entry.Value != null)
        //        {
        //            var fromInNew = mstGraph.Vertices[entry.Value];
        //            var toInNew = mstGraph.Vertices[entry.Key];

        //            var fromInOrig = graph.Vertices[entry.Value];
        //            var toInOrig = graph.Vertices[entry.Key];

        //            var origEdge = graph.GetEdge(fromInOrig, toInOrig);

        //            mstGraph.AddEdge(fromInNew, toInNew, new Dictionary<string, double>(origEdge.Costs));
        //        }
        //    }

        //    return mstGraph;
        //}



        #endregion Obsolete (Too Slow or Wrong)



    }
}
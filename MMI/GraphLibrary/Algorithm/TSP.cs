using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Algorithm
{
    /// <summary>
    /// Funktionen zum Traveling Salesman Problem (TSP) -> Ermittlung der billigsten Tour bzw. Heuristiken dazu
    /// </summary>
    public static class TSP
    {

        /// <summary>
        /// Nearest Neighbour
        /// </summary>
        /// <param name="graph">Graph,, auf dem der Algorithmus angewandt werden soll</param>
        /// <param name="costKey">Key, unter dem die zu betrachtenden Kosten abgespeichert wurden</param>
        /// <returns></returns>
        public static List<IEdge> NearestNeighbour(IGraph graph, string costKey)
        {

            List<IEdge> usedEdges = new List<IEdge>();

            // Zur Prüfung auf bereits besuchte Knoten
            Dictionary<string, bool> isVertexSeen = new Dictionary<string, bool>(graph.Vertices.Count);
            foreach (var vertex in graph.Vertices)
            {
                isVertexSeen.Add(vertex.Key, false);
            }


            // Startknoten einfach erster Knoten
            IVertex startVertex = graph.Vertices.First().Value;

            IVertex currentVertex = startVertex;
            isVertexSeen[currentVertex.Identifier] = true;

            // Wenn alle gesehen, dann ist der Hamilton-Kreis komplett. Dazu n-1 Kanten. Die letzte zum Kreisschluss wird hier hinzugefügt
            while (usedEdges.Count != graph.Vertices.Count - 1)
            {
                // nehme die billigste Kante zum noch nicht gesehenen Knoten
                var useEdge = currentVertex.Edges.Where(x => isVertexSeen[x.Value.GetOtherVertex(currentVertex).Identifier] == false)
                                    .OrderBy(x => x.Value.Costs[costKey])
                                    .First().Value;

                // Kante hinzunehmen und aktuellen Knoten als gesehen markieren
                usedEdges.Add(useEdge);
                currentVertex = useEdge.GetOtherVertex(currentVertex);
                isVertexSeen[currentVertex.Identifier] = true;
            }


            // Letzte Kante noch zum Schließen des Kreises
            usedEdges.Add(graph.GetEdge(currentVertex, startVertex));
            

            return usedEdges;
        }












        /// <summary>
        /// Doppelter Baum
        /// </summary>
        /// <param name="graph">Graph,, auf dem der Algorithmus angewandt werden soll</param>
        /// <param name="costKey">Key, unter dem die zu betrachtenden Kosten abgespeichert wurden</param>
        /// <returns></returns>
        public static List<IEdge> DoubleTree(IGraph graph, string costKey)
        {

            List<IEdge> usedEdges = new List<IEdge>();

            // MST erstellen
            var mst = MST.KruskalV1(graph, costKey);

            // Tiefensuche auf MST liefert Reihenfolge der Knoten
            // starte beim ersten des MST
            var vertexOrder = Traversing.DepthFirst(mst, mst.Vertices.First().Value);

            // aus Dictionary eine Liste machen, damit einfacher iteriert werden kann
            var vertexOrderAsList = vertexOrder.Values.ToList();

            // Kanten je zwischen den Knoten in Ergebnismenge einfügen
            for (int i = 0; i < vertexOrderAsList.Count - 1; i++)
            {
                usedEdges.Add(graph.GetEdge(vertexOrderAsList[i], vertexOrderAsList[i+1]));
            }

            // und die letzte Kante nicht vergessen, die den Kreis schießt
            usedEdges.Add(graph.GetEdge(vertexOrderAsList[vertexOrderAsList.Count -1], vertexOrderAsList[0]));

            return usedEdges;
        }
















        /// <summary>
        /// Ausprobieren aller Touren zur Ermittlung der billigsten.
        /// </summary>
        /// <param name="graph">Graph,, auf dem der Algorithmus angewandt werden soll</param>
        /// <param name="costKey">Key, unter dem die zu betrachtenden Kosten abgespeichert wurden</param>
        /// <returns></returns>
        public static List<IEdge> TryAllTours(IGraph graph, string costKey)
        {
            List<IEdge> bestWay = null;
            double bestCost = double.MaxValue;


            foreach (var vertex in graph.Vertices.Values)
            {
                List<IEdge> usedEdges = new List<IEdge>();
                Dictionary<string, bool> seenVertices = new Dictionary<string, bool>();

                foreach (var key in graph.Vertices.Keys)
                {
                    seenVertices.Add(key, false);
                }

                TryAllToursRecursion(graph, costKey, vertex, vertex, 0, seenVertices, usedEdges, ref bestWay, ref bestCost);
            }

            /*List<IEdge>[] bestWays = new List<IEdge>[graph.Vertices.Count];
            double[] bestCosts = Enumerable.Repeat(double.MaxValue, graph.Vertices.Count).ToArray();

            int cnt = -1;

            Parallel.ForEach(graph.Vertices.Values, (vertex) => 
            {
                int localCnt = ++cnt;
                List<IEdge> usedEdges = new List<IEdge>();
                Dictionary<string, bool> seenVertices = new Dictionary<string, bool>();

                foreach (var key in graph.Vertices.Keys)
                {
                    seenVertices.Add(key, false);
                }

                TryAllRecursion(graph, costKey, vertex, vertex, 0, seenVertices, usedEdges, ref bestWays[localCnt], ref bestCosts[localCnt]);
            });


            List<IEdge> bestWay = null;
            foreach (var way in bestWays)
            {
                if (bestWay == null || way.Sum(x => x.Costs[costKey]) < bestWay.Sum(x => x.Costs[costKey]))
                {
                    bestWay = way;
                }
            }*/

            return bestWay;
        }


        private static void TryAllToursRecursion(IGraph graph, string costKey, IVertex start, IVertex current, double currentCosts, Dictionary<string, bool> seenVertices, List<IEdge> usedEdges, ref List<IEdge> bestWay, ref double bestCost)
        {
            // current wurde nun besucht
            seenVertices[current.Identifier] = true;

            // Wenn alle gesehen, dann ist der Hamilton-Kreis komplett. Dazu n-1 Kanten. Die letzte zum Kreisschluss wird hier hinzugefügt
            if (usedEdges.Count == graph.Vertices.Count - 1)
            {
                var closingEdge = graph.GetEdge(current, start);
                usedEdges.Add(closingEdge);

                // Erstes ist bis dahin bestes oder Kosten noch günstiger
                double costToCheck = currentCosts + closingEdge.Costs[costKey];
                if (bestWay == null || costToCheck < bestCost)
                {
                    bestWay = new List<IEdge>(usedEdges);
                    bestCost = costToCheck;
                }

                usedEdges.RemoveAt(usedEdges.Count - 1);
            }
            else
            {
                // es geht noch weiter

                // nehme eine Kante und gehe zu einem noch nicht besuchten
                foreach (var edge in current.Edges.Values)
                {
                    // wurde der andere schon besucht?
                    var other = edge.GetOtherVertex(current);
                    if (seenVertices[other.Identifier] == false)
                    {
                        double newCosts = currentCosts + edge.Costs[costKey];

                        //if ((bestWay == null || newCosts < bestCost)) //Branch and Bound
                        {
                            // Kante als genutzt vermerken
                            usedEdges.Add(edge);
                            TryAllToursRecursion(graph, costKey, start, other, newCosts, seenVertices, usedEdges, ref bestWay, ref bestCost);
                            usedEdges.RemoveAt(usedEdges.Count - 1);
                        }
                    }

                }
            }

            // und man verlässt current wieder
            seenVertices[current.Identifier] = false;
        }


    }
}

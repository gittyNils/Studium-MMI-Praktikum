using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Algorithm
{
    /// <summary>
    /// Funktionen zum Traveling Salesman Problem (TSP)
    /// </summary>
    public static class TSP
    {

        public static List<IEdge> NearestNeighbour()
        {




            return null;
        }




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

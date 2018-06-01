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
    /// Algorithmen zum Fluss in Graphen
    /// </summary>
    public static class Flow
    {
        /// <summary>
        /// Key für den Value des Flusses einer Kante eines Graphen
        /// </summary>
        public const string FLUSS_VALUE = "Fluss";

        /// <summary>
        /// Key für den Value der Kapazität einer Kante eines Graphen
        /// </summary>
        public const string KAPAZITÄT_VALUE = "Kapazität";


        /// <summary>
        /// Richtung einer Kante. 
        /// Gibt im Residual-Graph an, ob es sich bei einer Kante um die Originale, also die Hinrichtung, oder die Rückrichtung der Kante handelt.
        /// Nur intern im Residualgraphen verwendet.
        /// 1 = Hin/original, sonst Rück
        /// </summary>
        private const string RICHTUNG_VALUE = "Richtung";



        /// <summary>
        /// Liefert auf einem Graphen mit Flüssen den maximalen Fluss (der vorher schon durch EdmondsKarp berechnet wurde).
        /// </summary>
        /// <param name="source">Source des Flusses</param>
        /// <returns></returns>
        public static double ReadMaxFlow(IVertex source)
        {
            var outKanten = source.Edges.Values.Where(z => z.FromVertex == source);
            var inKanten = source.Edges.Values.Except(outKanten);

            var maxFluss = outKanten.Sum(z => z.Values[FLUSS_VALUE]) - inKanten.Sum(z => z.Values[FLUSS_VALUE]);
            return maxFluss;
        }


        /// <summary>
        /// Berechnet den maximalen Fluss im Graphen mit Hilfe des Algorithmus von Edmonds-Karp.
        /// Der Fluss wird dabei in das übergebene Graphobjekt eingetragen.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void EdmondsKarp(IGraph graph, IVertex source, IVertex target)
        {
            // Fluss initialisieren
            foreach (var edge in graph.Edges.Values)
            {
                edge.Values[FLUSS_VALUE] = 0;
            }

            bool foundFAugmentierenderWeg;
            do
            {
                foundFAugmentierenderWeg = false;

                // Residualgraphen erstellen
                var residualGraph = CreateResidualGraph(graph);

                // Den kürzesten s,t-Weg ermitteln
                var fAugPath = Traversing.FindPathBF(residualGraph, source.Identifier, target.Identifier);

                if (fAugPath != null)
                {
                    foundFAugmentierenderWeg = true;

                    // minimale Residualkapazität:
                    var minResKap = fAugPath.Min(x => x.Values[KAPAZITÄT_VALUE]);

                    // den Fluss entlang des Weges verändern
                    foreach (var edge in fAugPath)
                    {
                        // bei Hinrichtung addieren und bei Rückrichtung subtrahieren im Fluss des Graphen
                        // Hinrichtung?
                        if (edge.Values[RICHTUNG_VALUE] == 1)
                        {
                            // Hinrichtung
                            var edgeInGraph = graph.GetEdge(edge.FromVertex, edge.ToVertex);
                            edgeInGraph.Values[FLUSS_VALUE] += minResKap;
                        }
                        else
                        {
                            // Rückrichtung, also die Kante im Originalgraphen mit vertauschten Vertices suchen
                            var edgeInGraph = graph.GetEdge(edge.ToVertex, edge.FromVertex);
                            edgeInGraph.Values[FLUSS_VALUE] -= minResKap;
                        }
                    }
                }
            }
            // wenn kein f-augmentierender Weg mehr da, dann ist der Fluss optimal
            while (foundFAugmentierenderWeg);


        }



        /// <summary>
        /// Erstellt anhand des aktuellen Graphen und dessen Fluss den Residualgraph mit den Residualkapazitäten
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        private static IGraph CreateResidualGraph(IGraph graph)
        {
            IGraph residualGraph = new Graph($"residual graph of {graph.Identifier}", graph.IsDirected);

            //Die Knoten müssen alle übernommen werden
            foreach (var vertex in graph.Vertices.Values)
            {
                residualGraph.AddVertex(vertex.Identifier);
            }


            // der Residual-Graph hat zu jeder Kante noch eine Rückwärts-Kante
            // Kanten mit der Residualkapazität = 0 fallen weg
            foreach (var edge in graph.Edges.Values)
            {
                var fluss = edge.Values[FLUSS_VALUE];
                // Korrespondierende Knoten der Kante in Residualgraph finden
                var fromVertex = residualGraph.Vertices[edge.FromVertex.Identifier];
                var toVertex = residualGraph.Vertices[edge.ToVertex.Identifier];

                // Residualkapazität für Hin-Kante:
                var uHin = edge.Values[KAPAZITÄT_VALUE] - fluss;

                // Residualkapazität für Rück-Kante
                var uRück = fluss;

                // Kanten einfügen, falls Werte > 0
                // HIN
                if (uHin > 0)
                {
                    residualGraph.AddEdge(fromVertex, toVertex, new Dictionary<string, double> {
                        { KAPAZITÄT_VALUE, uHin },
                        { RICHTUNG_VALUE, 1 }
                    });
                }

                // RÜCK
                if (uRück > 0)
                {
                    residualGraph.AddEdge(toVertex, fromVertex, new Dictionary<string, double> {
                        { KAPAZITÄT_VALUE, uRück },
                        { RICHTUNG_VALUE, 0}
                    });
                }
            }

            return residualGraph;
        }

        private static void RecycleResidualGraph(IGraph graph, IGraph residualGraph, List<IEdge> lastPath)
        {
            // entfernen der Edges, die von Lastpath betroffen waren. Dabei dann vermerken, dass diese wieder erneuert werden müssen



            var edgesToRenew = graph.Edges.Values;


            // der Residual-Graph hat zu jeder Kante noch eine Rückwärts-Kante
            // Kanten mit der Residualkapazität = 0 fallen weg
            foreach (var edge in edgesToRenew)
            {
                var fluss = edge.Values[FLUSS_VALUE];
                // Korrespondierende Knoten der Kante in Residualgraph finden
                var fromVertex = residualGraph.Vertices[edge.FromVertex.Identifier];
                var toVertex = residualGraph.Vertices[edge.ToVertex.Identifier];

                // Residualkapazität für Hin-Kante:
                var uHin = edge.Values[KAPAZITÄT_VALUE] - fluss;

                // Residualkapazität für Rück-Kante
                var uRück = fluss;

                // Kanten einfügen, falls Werte > 0
                // HIN
                if (uHin > 0)
                {
                    residualGraph.AddEdge(fromVertex, toVertex, new Dictionary<string, double> {
                        { KAPAZITÄT_VALUE, uHin },
                        { RICHTUNG_VALUE, 1 }
                    });
                }

                // RÜCK
                if (uRück > 0)
                {
                    residualGraph.AddEdge(toVertex, fromVertex, new Dictionary<string, double> {
                        { KAPAZITÄT_VALUE, uRück },
                        { RICHTUNG_VALUE, 0}
                    });
                }
            }



            /*
            foreach (var edgeOfResidualGraph in lastPath)
            {
                // wenn die Rückkante im Residual-Graphen genommen wurde, dann kann von dieser nicht der Fluss direkt bestimmt werden
                // also prüfen und die richtung der Kante nehmen, die im Original-Graph war.
                IEdge edgeInOrigGraph;
                if ((edgeInOrigGraph = graph.GetEdge(edgeOfResidualGraph.FromVertex, edgeOfResidualGraph.ToVertex)) == null)
                {
                    edgeInOrigGraph = graph.GetEdge(edgeOfResidualGraph.ToVertex, edgeOfResidualGraph.FromVertex);
                }
                var fluss = edgeInOrigGraph.Values[FLUSS_VALUE];

                // Korrespondierende Knoten der Kante in Residualgraph finden
                var fromVertex = residualGraph.Vertices[edgeOfResidualGraph.FromVertex.Identifier];
                var toVertex = residualGraph.Vertices[edgeOfResidualGraph.ToVertex.Identifier];

                // Residualkapazität für Hin-Kante:
                var uHin = edgeOfResidualGraph.Values[KAPAZITÄT_VALUE] - fluss;

                // Residualkapazität für Rück-Kante
                var uRück = fluss;

                // Kanten einfügen, falls Werte > 0
                // HIN
                var existingEdgeHin = residualGraph.GetEdge(fromVertex, toVertex);
                if (uHin > 0)
                {
                    if (existingEdgeHin != null)
                    {
                        // update
                        existingEdgeHin.Values[KAPAZITÄT_VALUE] = uHin;
                    }
                    else
                    {
                        // add
                        residualGraph.AddEdge(fromVertex, toVertex, new Dictionary<string, double> {
                        { KAPAZITÄT_VALUE, uHin },
                        { RICHTUNG_VALUE, 1 } });
                    }
                }
                else
                {
                    // removen. nicht mehr da
                    residualGraph.RemoveEdge(existingEdgeHin.Identifier);
                }

                // RÜCK
                var existringEdgeRück = residualGraph.GetEdge(toVertex, fromVertex);
                if (uRück > 0)
                {
                    if (existringEdgeRück != null)
                    {
                        // update
                        existringEdgeRück.Values[KAPAZITÄT_VALUE] = uRück;
                    }
                    else
                    {
                        // add
                        residualGraph.AddEdge(toVertex, fromVertex, new Dictionary<string, double> {
                        { KAPAZITÄT_VALUE, uRück },
                        { RICHTUNG_VALUE, 0} });
                    }
                }
                else
                {
                    // removen. nicht mehr da
                    residualGraph.RemoveEdge(existringEdgeRück.Identifier);
                }
            }*/
        }

    }
}

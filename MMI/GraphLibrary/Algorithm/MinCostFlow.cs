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
    /// Algorithmen zur Berechnung von Kostenminimalen Flüssen
    /// </summary>
    public static class MinCostFlow
    {
        /// <summary>
        /// Ermittelt die Gesamtkosten für den vorliegenden Fluss.
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static double GetFlowCost(IGraph graph)
        {
            double cost = 0;
            foreach (var edge in graph.Edges.Values)
            {
                cost += edge.Values[CONST.FLUSS_VALUE] * edge.Values[CONST.KOSTEN_VALUE];
            }


            return cost;
        }



        /// <summary>
        /// Cycle-Canceling Algorithmus zur Berechnung des Kostenminimalen Flusses.
        /// </summary>
        /// <returns>True = Fluss gefunden, Fals wenn nicht</returns>
        public static bool CycleCanceling(IGraph graph)
        {
            string kosten = CONST.KOSTEN_VALUE;
            string kapazität = CONST.KAPAZITÄT_VALUE;
            string balance = CONST.BALANCE_VALUE;

            string superSource = CONST.SUPER_SOURCE;
            string superTarget = CONST.SUPER_TARGET;
            string superStart = "SuperStart";

            bool ret = false;

            // SuperSenke und SuperQuelle hinzufügen
            var sources = graph.Vertices.Values.Where(x => x.Values[balance] > 0).ToList();
            var targets = graph.Vertices.Values.Where(x => x.Values[balance] < 0).ToList();

            graph.AddVertex(superSource);
            graph.AddVertex(superTarget);

            var superSourceVert = graph.Vertices[superSource];
            var superTargetVert = graph.Vertices[superTarget];

            foreach (var source in sources)
            {
                graph.AddEdge(superSourceVert, source, new Dictionary<string, double> { { kapazität, source.Values[balance] }, { kosten, 0 } });
            }

            foreach (var target in targets)
            {
                graph.AddEdge(target, superTargetVert, new Dictionary<string, double> { { kapazität, Math.Abs(target.Values[balance]) }, { kosten, 0 } });
            }



            // prüfe den b-Fluss
            Flow.EdmondsKarp(graph, superSourceVert, superTargetVert);
            var maxFluss = Flow.ReadMaxFlow(superSourceVert);


            // SuperSenke und SuperQuelle sind ab hier wieder egal
            foreach (var source in sources)
            {
                var edge = graph.GetEdge(superSourceVert, source);
                graph.RemoveEdge(edge.Identifier);
            }

            foreach (var target in targets)
            {
                var edge = graph.GetEdge(target, superTargetVert);
                graph.RemoveEdge(edge.Identifier);
            }

            graph.RemoveVertex(superSource);
            graph.RemoveVertex(superTarget);



            // b-Fluss existiert, wenn gleich die Summe der Source-Balancen
            if (maxFluss == sources.Sum(x => x.Values[balance]))
            {
                ret = true;
                bool foundCircle;

                do
                {
                    foundCircle = false;

                    // Residualgraph bauen
                    var residualGraph = CreateResidualGraph(graph);

                    // einen SuperStart-Knoten hinzufügen, der mit allen anderen Knoten im Residualgraphen verbunden ist
                    // so finden wir auf jeden fall alle negativen Zykel, auch wenn verschiedene Zusammenhangskomponenten
                    // wähle Kosten 0 für die neuen Kanten (Kosten sind egal, da für alle gleich. Hauptsache verbunden. Wir wollen ja nur Zykel finden)
                    residualGraph.AddVertex(superStart);
                    var superStartVert = residualGraph.Vertices[superStart];
                    foreach (var vertex in residualGraph.Vertices.Values)
                    {
                        if (vertex != superStartVert)
                        {
                            residualGraph.AddEdge(superStartVert, vertex, new Dictionary<string, double> { { kosten, 0 } });
                        }
                    }

                    var fAugmentingCycle = ShortestPath.MooreBellmanFordForNegativeCycle(residualGraph, superStart, kosten);

                    if (fAugmentingCycle != null)
                    {
                        foundCircle = true;

                        var minCapacity = fAugmentingCycle.Min(x => x.Values[kapazität]);

                        foreach (var edge in fAugmentingCycle)
                        {
                            // bei Hinrichtung addieren und bei Rückrichtung subtrahieren im b-Fluss des Graphen
                            // Hinrichtung?
                            if (edge.Values[CONST.RICHTUNG_VALUE] == 1)
                            {
                                // Hinrichtung
                                var edgeInGraph = graph.GetEdge(edge.FromVertex, edge.ToVertex);
                                edgeInGraph.Values[CONST.FLUSS_VALUE] += minCapacity;
                            }
                            else
                            {
                                // Rückrichtung, also die Kante im Originalgraphen mit vertauschten Vertices suchen
                                var edgeInGraph = graph.GetEdge(edge.ToVertex, edge.FromVertex);
                                edgeInGraph.Values[CONST.FLUSS_VALUE] -= minCapacity;
                            }
                        }
                    }
                }
                while (foundCircle);
            }

            return ret;
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
                var fluss = edge.Values[CONST.FLUSS_VALUE];
                // Korrespondierende Knoten der Kante in Residualgraph finden
                var fromVertex = residualGraph.Vertices[edge.FromVertex.Identifier];
                var toVertex = residualGraph.Vertices[edge.ToVertex.Identifier];

                // Residualkapazität für Hin-Kante:
                var uHin = edge.Values[CONST.KAPAZITÄT_VALUE] - fluss;

                // Residualkapazität für Rück-Kante
                var uRück = fluss;

                // Residualkosten für Hin-Kante
                var cHin = edge.Values[CONST.KOSTEN_VALUE];

                // Residualkosten für Rück-Kante
                var cRück = -cHin;

                // Kanten einfügen, falls Werte > 0
                // HIN
                if (uHin > 0)
                {
                    residualGraph.AddEdge(fromVertex, toVertex, new Dictionary<string, double> {
                        { CONST.KAPAZITÄT_VALUE, uHin },
                        { CONST.KOSTEN_VALUE, cHin },
                        { CONST.RICHTUNG_VALUE, 1 }
                    });
                }

                // RÜCK
                if (uRück > 0)
                {
                    residualGraph.AddEdge(toVertex, fromVertex, new Dictionary<string, double> {
                        { CONST.KAPAZITÄT_VALUE, uRück },
                        { CONST.KOSTEN_VALUE, cRück },
                        { CONST.RICHTUNG_VALUE, 0}
                    });
                }
            }

            return residualGraph;
        }







        /// <summary>
        /// Successive-Shortest-Path Algorithmus zur Berechnung des Kostenminimalen Flusses.
        /// </summary>
        /// <returns>True = Fluss gefunden, Fals wenn nicht</returns>
        public static bool SuccessiveShortestPah(IGraph graph)
        {
            string kosten = CONST.KOSTEN_VALUE;
            string kapazität = CONST.KAPAZITÄT_VALUE;
            string balance = CONST.BALANCE_VALUE;
            string pseudoBalance = CONST.PSEUDO_BALANCE_VALUE;
            string fluss = CONST.FLUSS_VALUE;


            // Initialisierung im Graphen
            // bei negativen Kosten an Kanten diese voll auslasten (Fluss = Kapazität), sonst Fluss = 0
            foreach (var edge in graph.Edges.Values)
            {
                if (edge.Values[kosten] < 0)
                {
                    edge.Values[fluss] = edge.Values[kapazität];
                }
                else
                {
                    edge.Values[fluss] = 0;
                }
            }



            // Pseudo-Balancen setzen und Pseudo-Quellen und -Senken ermitteln
            List<IVertex> pseudoQuellen = new List<IVertex>();
            List<IVertex> pseudoSenken = new List<IVertex>();
            foreach (var vertex in graph.Vertices.Values)
            {
                // Summe Fluss ausgehende Kanten - Summe Fluss eingehende Kanten
                var outEdges = vertex.Edges.Values.Where(x => x.FromVertex == vertex);
                var inEdges = vertex.Edges.Values.Where(x => x.ToVertex == vertex);                
                vertex.Values[pseudoBalance] = outEdges.Sum(x => x.Values[fluss]) - inEdges.Sum(x => x.Values[fluss]);

                if (vertex.Values[balance] > vertex.Values[pseudoBalance])
                {
                    // Qelle
                    pseudoQuellen.Add(vertex);
                }
                else if (vertex.Values[balance] < vertex.Values[pseudoBalance])
                {
                    // Senke
                    pseudoSenken.Add(vertex);
                }
            }







            bool iterationSuccessful;

            do
            {
                iterationSuccessful = false;

                // Wenn Pseudo-Quelle und erreichbare -Senke in Residualgraph existiert
                if (pseudoQuellen.Count > 0 && pseudoSenken.Count > 0)
                {
                    var pseudoQuelle = pseudoQuellen[0];

                    var residualGraph = CreateResidualGraph(graph);

                    // erreichbar und kürzester Weg bzgl. Kosten direkt in einem. Mit MooreBellmanFord, da auch negative Gewichte möglich
                    IEdge neverUsedCylceEdge; // Zykel könen nicht vorkommen
                    var pred = ShortestPath.MooreBellmanFord(residualGraph, pseudoQuelle.Identifier, kosten, out neverUsedCylceEdge);

                    // Suche eine erste erreichbare Pseudo-Senke
                    IVertex pseudoSenke = null;
                    List<IEdge> shortestPathToPseudoSenke = null;
                    foreach (var senke in pseudoSenken)
                    {
                        var way = ShortestPath.GetWay(pseudoQuelle.Identifier, senke.Identifier, residualGraph, pred);
                        if (way != null)
                        {
                            // erreichbar
                            pseudoSenke = senke;
                            shortestPathToPseudoSenke = way;
                            break;
                        }
                    }


                    // Gab es eine erreichbare PseudoSenke?
                    if (pseudoSenke != null)
                    {
                        // Flusserhöhung durchführen
                        // Wert der Flusserhöhung bestimmen
                        var minCapacity = shortestPathToPseudoSenke.Min(x => x.Values[kapazität]);

                        // Beschränkt durch Restbalance in Pseudo-Quelle und/oder Pseudo-Senke?
                        var quelleRestBalance = pseudoQuelle.Values[balance] - pseudoQuelle.Values[pseudoBalance];
                        var senkeRestBalance = pseudoSenke.Values[pseudoBalance] - pseudoSenke.Values[balance];

                        minCapacity = Math.Min(minCapacity, Math.Min(quelleRestBalance, senkeRestBalance));

                        // Wenn eine Beschränkung aktiv, dann Pseudo-Quelle und/oder -Senke entfernen
                        if (minCapacity == quelleRestBalance)
                        {
                            pseudoQuellen.Remove(pseudoQuelle);
                        }
                        if (minCapacity == senkeRestBalance)
                        {
                            pseudoSenken.Remove(pseudoSenke);
                        }


                        foreach (var edge in shortestPathToPseudoSenke)
                        {
                            // bei Hinrichtung addieren und bei Rückrichtung subtrahieren im b-Fluss des Graphen
                            // Hinrichtung?
                            if (edge.Values[CONST.RICHTUNG_VALUE] == 1)
                            {
                                // Hinrichtung
                                var edgeInGraph = graph.GetEdge(edge.FromVertex, edge.ToVertex);
                                edgeInGraph.Values[CONST.FLUSS_VALUE] += minCapacity;
                            }
                            else
                            {
                                // Rückrichtung, also die Kante im Originalgraphen mit vertauschten Vertices suchen
                                var edgeInGraph = graph.GetEdge(edge.ToVertex, edge.FromVertex);
                                edgeInGraph.Values[CONST.FLUSS_VALUE] -= minCapacity;
                            }
                        }


                        // Pseudo-Balance aktualisieren an Pseudo-Quelle und -Senke (dazwischen geht der Fluss rein und direkt wieder raus, also keine Veränderung)
                        foreach (var vertex in new IVertex[] { pseudoQuelle, pseudoSenke })
                        {
                            // Summe Fluss ausgehende Kanten - Summe Fluss eingehende Kanten
                            var outEdges = vertex.Edges.Values.Where(x => x.FromVertex == vertex);
                            var inEdges = vertex.Edges.Values.Where(x => x.ToVertex == vertex);
                            vertex.Values[pseudoBalance] = outEdges.Sum(x => x.Values[fluss]) - inEdges.Sum(x => x.Values[fluss]);
                        }


                        // dann war diese Iteration erfolgreich
                        iterationSuccessful = true;
                    }                
                }
            }
            while (iterationSuccessful);



            // wenn Balancen ausgeglichen, dann erfolgreich einen Kostenminimalen Fluss gefunden
            bool foundFlow = graph.Vertices.Values.All(x => x.Values[pseudoBalance] == x.Values[balance]);

            // Pseudo-Balancen wieder aus dem Graphen entfernen
            foreach (var vertex in graph.Vertices.Values)
            {
                vertex.Values.Remove(pseudoBalance);
            }

            return foundFlow;
        }
    }
}

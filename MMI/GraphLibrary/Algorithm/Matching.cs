using GraphLibrary.DataModel;
using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Algorithm
{
    public static class Matching
    {

        /// <summary>
        /// Matchings bei Bipartiten Graphen unter angabe der beiden Mengen in diesem Graphen über den maximalen Fluss.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="setA"></param>
        /// <param name="setB"></param>
        public static List<IEdge> BipartitMatching(IGraph graph, List<IVertex> setA, List<IVertex> setB)
        {
            List<IEdge> matching = new List<IEdge>();
            IGraph tmpGraph = graph.Copy();

            // SuperSenke und SuperQuelle hinzufügen
            string superSource = CONST.SUPER_SOURCE;
            string superTarget = CONST.SUPER_TARGET;

            tmpGraph.AddVertex(superSource);
            tmpGraph.AddVertex(superTarget);

            var superSourceVert = tmpGraph.Vertices[superSource];
            var superTargetVert = tmpGraph.Vertices[superTarget];

            setA.ForEach(source => tmpGraph.AddEdge(superSourceVert, source));
            setB.ForEach(target => tmpGraph.AddEdge(target, superTargetVert));
            
            

            // Kapazität für alle Kanten auf 1 setzen
            foreach (var edge in tmpGraph.Edges.Values)
            {
                edge.Values[CONST.KAPAZITÄT_VALUE] = 1;
            }
            
            Flow.EdmondsKarp(tmpGraph, superSourceVert, superTargetVert);


            // Matchings in Liste tun
            // Alle Kanten von Menge A zu Menge B mit dem Fluss = 1
            foreach (var vertex in setA)
            {
                var vertInTmpGraph = tmpGraph.Vertices[vertex.Identifier];
                foreach (var edge in vertInTmpGraph.Edges.Values)
                {
                    if (edge.Values[CONST.FLUSS_VALUE] == 1)
                    {
                        matching.Add(edge);
                    }
                }
            }


            return matching;
        }
    }
}

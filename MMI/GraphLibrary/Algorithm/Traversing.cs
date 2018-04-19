using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Algorithm
{
    /// <summary>
    /// Klasse mit Funktionen zum Durchlaufen eines Graphen
    /// </summary>
    public static class Traversing
    {
        


        public static Dictionary<string, IVertex> BreadthFirst(IGraph graph, IVertex start)
        {
            // Weg beim Durchlaufen
            Dictionary<string, IVertex> pathWithSeenKnodes = new Dictionary<string, IVertex>();

            // Knoten als gesehen vermerken und direkt in die Queue einfügen
            pathWithSeenKnodes.Add(start.Identifier, start);
            Queue<IVertex> queue = new Queue<IVertex>();
            queue.Enqueue(start);

            // Solange die Queue nicht leer ist laufen.
            IVertex currentItem;
            while (queue.Count > 0)
            {
                currentItem = queue.Dequeue();
                
                // jeden direkten Nachbarn in die Queue, wenn noch nicht zuvor gesehen
                foreach (var neighbour in currentItem.Neighbours.Values)
                {
                    if (!pathWithSeenKnodes.ContainsKey(neighbour.Identifier))
                    {
                        // Knoten als gesehen vermerken und in die Queue einfügen
                        pathWithSeenKnodes.Add(neighbour.Identifier, neighbour);
                        queue.Enqueue(neighbour);
                    }
                }
            }

            return pathWithSeenKnodes;
        }





        //public static List<IVertex> DepthFirst(IGraph graph, IVertex start, bool resetFlagsBeforeStart)
        //{
        //    if (resetFlagsBeforeStart)
        //    {
        //        // Flags zur Sicherheit Resetten
        //        graph.ResetSeen();
        //    }

        //    // Weg beim Durchlaufen
        //    List<IVertex> path = new List<IVertex>();

        //    // TODO

        //    return path;
        //}


        //private static void DepthFirstTraversalRecursive(IGraph graph, IVertex currentItem, List<IVertex> path)
        //{
        //    // markiere mich als Seen und adde mich in den Pfad
        //    currentItem.Seen = true;

        //}

    }
}

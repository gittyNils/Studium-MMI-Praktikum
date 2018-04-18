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


        // Flags müssen vorher zurückgesetzt werden
        public static List<IVertex> BreadthFirstTraversal(IGraph graph, IVertex start, bool resetFlagsBeforeStart)
        {
            if (resetFlagsBeforeStart)
            {
                // Flags zur Sicherheit Resetten
                graph.ResetSeen();
            }

            // Weg beim Durchlaufen
            List<IVertex> path = new List<IVertex>();

            // Queue und start direkt rein und als gesehen markieren. 
            Queue<IVertex> queue = new Queue<IVertex>();
            queue.Enqueue(start);
            start.Seen = true;

            // Solange die Queue nicht leer ist laufen.
            IVertex currentItem;
            while (queue.Count > 0)
            {
                currentItem = queue.Dequeue();

                // Merken, dass der Weg über dieses Item ging
                path.Add(currentItem);

                // jeden direkten Nachbarn in die Queue, wenn noch nicht zuvor gesehen
                foreach (var neighbour in currentItem.Neighbours.Values)
                {
                    if (!neighbour.Seen)
                    {
                        // in die Queue und als gesehen markieren
                        queue.Enqueue(neighbour);
                        neighbour.Seen = true;
                    }
                }
            }

            return path;
        }





        public static List<IVertex> DepthFirstTraversal(IGraph graph, IVertex start, bool resetFlagsBeforeStart)
        {
            if (resetFlagsBeforeStart)
            {
                // Flags zur Sicherheit Resetten
                graph.ResetSeen();
            }

            // Weg beim Durchlaufen
            List<IVertex> path = new List<IVertex>();

            // TODO

            return path;
        }


        private static void DepthFirstTraversalRecursive(IGraph graph, IVertex currentItem, List<IVertex> path)
        {
            // markiere mich als Seen und adde mich in den Pfad
            currentItem.Seen = true;

        }

    }
}

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

        /// <summary>
        /// Traversieren mit Hilfe einer Breitensuche
        /// </summary>
        /// <param name="graph">Graph zum Traversieren</param>
        /// <param name="start">Start-Knoten</param>
        /// <returns></returns>
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
        


        /// <summary>
        /// Traversieren mit Hilfe einer Tiefensuche
        /// </summary>
        /// <param name="graph">Graph zum Traversieren</param>
        /// <param name="start">Start-Knoten</param>
        public static Dictionary<string, IVertex> DepthFirst(IGraph graph, IVertex start)
        {
            // Weg beim Durchlaufen
            Dictionary<string, IVertex> pathWithSeenKnodes = new Dictionary<string, IVertex>();

            DepthFirstRecursion(graph, start, pathWithSeenKnodes);
            
            return pathWithSeenKnodes;
        }

        /// <summary>
        /// Interne Funktion für die Rekursive Traversierung mit Hilfe der Tiefensuche
        /// </summary>
        /// <param name="graph">Graph zum Traversieren</param>
        /// <param name="currentItem">aktueller Knoten</param>
        /// <param name="pathWithSeenKnodes">Liste von bereits besuchten Knoten</param>
        private static void DepthFirstRecursion(IGraph graph, IVertex currentItem, Dictionary<string, IVertex> pathWithSeenKnodes)
        {
            // aktellen Knoten als gesehen vermerken
            pathWithSeenKnodes.Add(currentItem.Identifier, currentItem);

            //Gehe über alle Nachbarn und, falls dieser noch nicht gesehen, auch für diesen den Rekursions-Schritt
            foreach (var neighbour in currentItem.Neighbours.Values)
            {
                if (!pathWithSeenKnodes.ContainsKey(neighbour.Identifier))
                {
                    DepthFirstRecursion(graph, neighbour, pathWithSeenKnodes);
                }
            }
        }



        /// <summary>
        /// Suche einen Pfad zwischen Knoten from und to mit einer Breitensuche.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <returns></returns>
        public static List<IEdge> FindPathBF(IGraph graph, string fromId, string toId)
        {
            IVertex toVertex = graph.Vertices[toId];
            IVertex fromVertex = graph.Vertices[fromId];

            // Vorgänger-Matrix
            Dictionary<string, string> pred = new Dictionary<string, string>();

            // Knoten als gesehen vermerken (in Vorgängermatrix) und direkt in die Queue einfügen
            pred.Add(fromId, fromId);
            Queue<IVertex> queue = new Queue<IVertex>();
            queue.Enqueue(fromVertex);

            // Solange die Queue nicht leer ist oder bereits gefunden laufen.
            bool found = false;
            IVertex currentItem;
            while (!found && queue.Count > 0)
            {
                currentItem = queue.Dequeue();

                // jeden direkten Nachbarn in die Queue, wenn noch nicht zuvor gesehen
                foreach (var neighbour in currentItem.Neighbours.Values)
                {
                    if (!pred.ContainsKey(neighbour.Identifier))
                    {
                        // Knoten als gesehen vermerken und in die Queue einfügen
                        pred.Add(neighbour.Identifier, currentItem.Identifier);
                        queue.Enqueue(neighbour);

                        // haben wir den Richtigen gefunden
                        if (neighbour == toVertex)
                        {
                            found = true;
                            break;
                        }
                    }
                }
            }

            List<IEdge> path = null;

            if (found)
            {
                path = new List<IEdge>();

                // Pfad rekonstruieren
                // laufe rückwärts bis zum from von to aus
                IVertex currentVertex = toVertex;

                while (currentVertex != fromVertex)
                {
                    var parent = pred[currentVertex.Identifier];
                    var parentVertex = graph.Vertices[parent];
                    path.Add(graph.GetEdge(parentVertex, currentVertex));

                    currentVertex = parentVertex;
                }

                // Reihenfolge noch richtig drehen
                path.Reverse();
            }
            
            return path;
        }

    }
}

using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Helper
{
    /// <summary>
    /// Klasse mit Codierungs-Funktionen für Identifier von Kanten.
    /// </summary>
    public static class EdgeIdHelper
    {
        /// <summary>
        /// Liefert die ID einer Kante in einem Graphen
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string GetId(IGraph graph, IVertex from, IVertex to)
        {
            string fromId = from.Identifier;
            string toId = to.Identifier;

            // Wenn ungerichtet, dann ist der IVertex mit dem kleineren String (binär) immer der FromVertex
            // >0 bedeutet bei Compare(a,b), dass b vor a kommt, also muss getauscht werden
            if (!graph.IsDirected && string.CompareOrdinal(from.Identifier, to.Identifier) > 0)
            {
                var tmp = fromId;
                fromId = toId;
                toId = tmp;
            }


            // Damit sind parallele Kanten ausgeschlossen, wenn das die ID ist.
            return $"{fromId} -> {toId}";
        }
    }
}

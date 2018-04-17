using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Interface
{
    /// <summary>
    /// Schnittstelle eines Graphen
    /// </summary>
    public interface IGraph
    {
        #region Properties

        /// <summary>
        /// Identifizierung dieses Graphen
        /// </summary>
        string Identifier { get; }


        #endregion Properties


        // Edges

        // Vertices

        // Directed-Flag, nur Getter, Setzen in Konstruktor


        #region Methods

        /// <summary>
        /// Seen-Flag in Knoten und Kanten zurücksetzen
        /// </summary>
        void ResetSeen();



        #endregion Methods


        /*   Methoden   */

        // AddEdge (From, To)

        // AddVertex
        
    }
}

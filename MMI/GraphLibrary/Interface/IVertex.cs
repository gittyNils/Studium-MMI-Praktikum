using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Interface
{
    /// <summary>
    /// Schnittstelle eines Knotens
    /// </summary>
    public interface IVertex
    {
        #region Properties
        
        /// <summary>
        /// Identifizierung dieses Knotens
        /// </summary>
        string Identifier { get; }


        /// <summary>
        /// Nachbarknoten, die von mir aus direkt erreicht werden können
        /// </summary>
        Dictionary<string, IVertex> Neighbours { get; }


        /// <summary>
        /// Fremde Nachbarknoten, die mich von sich aus direkt erreichen können
        /// </summary>
        Dictionary<string, IVertex> ForeignNeighbours { get; }


        /// <summary>
        /// Kanten an diesem Knoten
        /// </summary>
        Dictionary<string, IEdge> Edges { get; }

        /// <summary>
        /// Werte an diesem Knoten. Werden als Key-Value-Pair abgespeichert
        /// </summary>
        Dictionary<string, double> Values { get; set; }

        #endregion Properties



        #region Methods

        /// <summary>
        /// Hinzufügen einer Kante, die mit diesem Knoten verbunden ist.
        /// Dabei die Neighbours und ForeignNeighbours aktualisieren.
        /// </summary>
        /// <param name="edge">neue Kante</param>
        /// <param name="directed">gibt an, ob die neue Kante gerichtet ist</param>
        void AddEdge(IEdge edge, bool directed);



        /// <summary>
        /// Entfernen einer Kante, die mit diesem Knoten verbunden ist.
        /// Dabei die Neighbours und ForeignNeighbours aktualisieren.
        /// </summary>
        /// <param name="edge">zu entfernene Kante</param>
        /// <param name="directed">gibt an, ob die neue Kante gerichtet ist</param>
        void RemoveEdge(IEdge edge, bool directed);

        #endregion Methods
    }
}

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
        /// Kanten an diesem Knoten
        /// </summary>
        Dictionary<string, IEdge> Edges { get; }

        #endregion Properties



        #region Methods

        /// <summary>
        /// Hinzufügen einer Kante, die mit diesem Knoten verbunden ist.
        /// Dabei die Neighbours aktualisieren.
        /// </summary>
        /// <param name="edge">neue Kante</param>
        void AddEdge(IEdge edge);


        #endregion Methods
    }
}

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
        /// Seen-Flag zur Markierung
        /// </summary>
        bool Seen { get; set; }

        /// <summary>
        /// Identifizierung dieses Knotens
        /// </summary>
        string Identifier { get; }


        /// <summary>
        /// Nachbarknoten
        /// </summary>
        List<IVertex> Neighbours { get; }


        /// <summary>
        /// Kanten an diesem Knoten
        /// </summary>
        List<IEdge> Edges { get; }

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

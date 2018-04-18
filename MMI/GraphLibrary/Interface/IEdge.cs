using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Interface
{
    /// <summary>
    /// Schnittstelle einer Kante
    /// </summary>
    public interface IEdge
    {
        #region Properties

        /// <summary>
        /// Seen-Flag zur Markierung
        /// </summary>
        bool Seen { get; set; }

        // -> Ist Eigenschaft des Graphen
        ///// <summary>
        ///// Gibt an, ob die Kante gerichtet ist
        ///// </summary>
        //bool Directed { get; }


        /// <summary>
        /// Identifizierung dieser Kante
        /// </summary>
        string Identifier { get; }


        /// <summary>
        /// Kante startet an diesem Knoten
        /// </summary>
        IVertex FromVertex { get; }

        /// <summary>
        /// Kante endet an diesem Knoten
        /// </summary>
        IVertex ToVertex { get; }

        
        /// <summary>
        /// Kosten an dieser Kante. Werden als Key-Value-Pair abgespeichert
        /// </summary>
        Dictionary<string, int> Costs { get; set; }

        #endregion Properties


        #region Methods

        /// <summary>
        /// Liefert zum übergebenen Knoten den der anderen Seite der Kante.
        /// </summary>
        /// <param name="vert"></param>
        /// <returns>Null, wenn vert nicht zur Kante gehört.</returns>
        IVertex GetOtherVertex(IVertex vert);

        #endregion Methods

    }
}

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

        
        /// <summary>
        /// Gibt an, ob die Kante gerichtet ist
        /// </summary>
        bool Directed { get; set; }


        /// <summary>
        /// Kante startet an diesem Vertex
        /// </summary>
        IVertex FromVertex { get; set; }

        /// <summary>
        /// Kante endet an diesem Vertex
        /// </summary>
        IVertex ToVertex { get; set; }

        
        /// <summary>
        /// Kosten an dieser Kante. Werden als Key-Value-Pair abgespeichert
        /// </summary>
        Dictionary<string, int> Costs { get; set; }

        #endregion Properties


        #region Methods

        /// <summary>
        /// Liefert zum übergebenen Vertex den der anderen Seite der Kante.
        /// </summary>
        /// <param name="vert"></param>
        /// <returns>Null, wenn vert nicht zur Kante gehört.</returns>
        IVertex GetOtherVertex(IVertex vert);

        #endregion Methods

    }
}

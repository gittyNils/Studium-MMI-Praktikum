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
        string Identifier { get; set; }




        #endregion Properties



        #region Methods


        #endregion Methods

        

        // Neighbours (Nachbarknoten)

        // Edges (Kanten an mir dran)
        



        /*   Methoden   */

        // AddEdge
    }
}

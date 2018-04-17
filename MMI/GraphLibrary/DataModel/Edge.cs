using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.DataModel
{
    /// <summary>
    /// Repräsentierte eine Kante im Graphen
    /// </summary>
    public class Edge : IEdge
    {
        #region Properties

        /// <summary>
        /// Seen-Flag zur Markierung
        /// </summary>
        public bool Seen { get; set; }


        /// <summary>
        /// Gibt an, ob die Kante gerichtet ist
        /// </summary>
        public bool Directed { get; set; }


        /// <summary>
        /// Kante startet an diesem Vertex
        /// </summary>
        public IVertex FromVertex { get; set; }

        /// <summary>
        /// Kante endet an diesem Vertex
        /// </summary>
        public IVertex ToVertex { get; set; }


        /// <summary>
        /// Kosten an dieser Kante. Werden als Key-Value-Pair abgespeichert
        /// </summary>
        public Dictionary<string, int> Costs { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Standard-Konstruktor
        /// </summary>
        public Edge()
        {
            Costs = new Dictionary<string, int>();
        }


        #endregion Constructor



        #region Methods

        /// <summary>
        /// Liefert zum übergebenen Vertex den der anderen Seite der Kante.
        /// </summary>
        /// <param name="vert"></param>
        /// <returns>Null, wenn vert nicht zur Kante gehört.</returns>
        public IVertex GetOtherVertex(IVertex vert)
        {
            IVertex ret = null;

            if (vert == FromVertex)
            {
                ret = ToVertex;
            }
            else if (vert == ToVertex)
            {
                ret = FromVertex;
            }

            return ret;
        }


        /// <summary>
        /// Überschriebene ToString-Methode
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{FromVertex} -> {ToVertex}";
        }


        #endregion Methods


    }
}

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
        /// Identifizierung dieses Knotens. Damit sind parallele Kanten ausgeschlossen, wenn das die ID ist.
        /// </summary>
        public string Identifier { get { return $"{FromVertex} -> {ToVertex}"; } }

        /// <summary>
        /// Kante startet an diesem Knoten
        /// </summary>
        public IVertex FromVertex { get; private set; }

        /// <summary>
        /// Kante endet an diesem Knoten
        /// </summary>
        public IVertex ToVertex { get; private set; }


        /// <summary>
        /// Kosten an dieser Kante. Werden als Key-Value-Pair abgespeichert
        /// </summary>
        public Dictionary<string, int> Costs { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="from">Von welchem Knoten</param>
        /// <param name="to">zu welchem Knoten</param>
        public Edge(IVertex from, IVertex to, Dictionary<string, int> costs = null)
        {
            FromVertex = from;
            ToVertex = to;

            if (costs == null)
            {
                //Vorbelegen
                Costs = new Dictionary<string, int>();
            }
            else
            {
                Costs = costs;
            }
        }


        #endregion Constructor



        #region Methods

        /// <summary>
        /// Liefert zum übergebenen Knoten den der anderen Seite der Kante.
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
            else
            {
                // Exception Werfen. 
                throw new ArgumentException($"Vertex={vert} not Part of Edge={this}");
            }

            return ret;
        }


        /// <summary>
        /// Überschriebene ToString-Methode
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Edge={Identifier}";
        }


        #endregion Methods


    }
}

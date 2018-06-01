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
        /// Identifizierung dieses Knotens. 
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// Kante startet an diesem Knoten
        /// </summary>
        public IVertex FromVertex { get; private set; }

        /// <summary>
        /// Kante endet an diesem Knoten
        /// </summary>
        public IVertex ToVertex { get; private set; }


        /// <summary>
        /// Werte an dieser Kante. Werden als Key-Value-Pair abgespeichert
        /// </summary>
        public Dictionary<string, double> Values { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="id">Eindeutige ID des Knotens</param>
        /// <param name="from">Von welchem Knoten</param>
        /// <param name="to">zu welchem Knoten</param>
        /// <param name="costs">Kosten dieser Kante</param>
        public Edge(string id, IVertex from, IVertex to, Dictionary<string, double> costs = null)
        {
            Identifier = id;
            FromVertex = from;
            ToVertex = to;

            if (costs == null)
            {
                //Vorbelegen
                Values = new Dictionary<string, double>();
            }
            else
            {
                Values = costs;
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

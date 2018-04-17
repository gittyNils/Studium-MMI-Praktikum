using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.DataModel
{
    /// <summary>
    /// Repräsentiert einen Knoten im Graphen
    /// </summary>
    public class Vertex : IVertex
    {
        #region Properties

        /// <summary>
        /// Seen-Flag zur Markierung
        /// </summary>
        public bool Seen { get; set; }

        /// <summary>
        /// Identifizierung dieses Knotens
        /// </summary>
        public string Identifier { get; set; }


        #endregion Properties


        #region Constructor

        /// <summary>
        /// Standard-Konstruktur
        /// </summary>
        /// <param name="id"><Eindeutige ID des Knotens/param>
        public Vertex(string id)
        {
            Identifier = id;
        }


        #endregion Constructor


        #region Methods



        /// <summary>
        /// Überschriebene ToString-Methode
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Identifier;
        }

        #endregion Methods

    }
}

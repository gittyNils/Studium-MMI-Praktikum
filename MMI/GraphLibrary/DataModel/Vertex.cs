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
        public string Identifier { get; private set; }


        /// <summary>
        /// Nachbarknoten
        /// </summary>
        public Dictionary<string, IVertex> Neighbours { get; private set; }


        /// <summary>
        /// Kanten an diesem Knoten
        /// </summary>
        public Dictionary<string, IEdge> Edges { get; private set; }

        #endregion Properties


        #region Constructor

        /// <summary>
        /// Standard-Konstruktur
        /// </summary>
        /// <param name="id"><Eindeutige ID des Knotens/param>
        public Vertex(string id)
        {
            Identifier = id;

            Neighbours = new Dictionary<string, IVertex>();
            Edges = new Dictionary<string, IEdge>();
        }


        #endregion Constructor


        #region Methods


        /// <summary>
        /// Hinzufügen einer Kante, die mit diesem Knoten verbunden ist.
        /// Dabei die Neighbours aktualisieren.
        /// </summary>
        /// <param name="edge">neue Kante</param>
        public void AddEdge(IEdge edge)
        {
            Edges.Add(edge.Identifier, edge);

            // Nachbarn ggf. aktualisieren
            // Suche dem anderen Ende der Kante
            IVertex other = edge.GetOtherVertex(this);

            if (!Neighbours.ContainsKey(other.Identifier))
            {
                Neighbours.Add(other.Identifier, other);
            }
            else
            {
                throw new Exception($"Duplicate Key Identifier={other.Identifier} Objet={other}");
            }
        }


        /// <summary>
        /// Überschriebene ToString-Methode
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Vertex={Identifier}";
        }


        #endregion Methods

    }
}

using GraphLibrary.Helper;
using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.DataModel
{
    /// <summary>
    /// Objekt für einen Graphen
    /// </summary>
    public class Graph : IGraph
    {
        #region Properties

        /// <summary>
        /// Identifizierung dieses Graphen
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// Gibt an, ob es sich um einen gerichteten Graphen, also mit Richtung bei den Kanten, handelt.
        /// </summary>
        public bool IsDirected { get; private set; }

        /// <summary>
        /// Alle Kanten des Graphen
        /// </summary>
        public Dictionary<string, IEdge> Edges { get; private set; }

        /// <summary>
        /// Alle Knoten des Graphen
        /// </summary>
        public Dictionary<string, IVertex> Vertices { get; private set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="directed">Flag, ob gerichteter Graph</param>
        public Graph(string id, bool directed)
        {
            IsDirected = directed;
            Identifier = id;

            Edges = new Dictionary<string, IEdge>();
            Vertices = new Dictionary<string, IVertex>();
        }

        #endregion Constructor



        #region Methods

        


        #region Modify Graph

        /// <summary>
        /// Liefert, falls vorhanden, die Kante zwischen from und to
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Kante oder null, wenn keine vorhanden</returns>
        public IEdge GetEdge(IVertex from, IVertex to)
        {
            IEdge ret = null;
            string edgeId = EdgeIdHelper.GetId(this, from, to);

            if (Edges.ContainsKey(edgeId))
            {
                ret = Edges[edgeId];
            }

            return ret;
        }

        /// <summary>
        /// Hinzufügen einer Kante von einem Knoten zum anderen
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="costs">Kosten</param>
        public void AddEdge(IVertex from, IVertex to, Dictionary<string, int> costs = null)
        {
            string edgeId = EdgeIdHelper.GetId(this, from, to);

            IEdge edge = new Edge(edgeId, from, to, costs);

            // Ist die Kante schon da?
            if (!Edges.ContainsKey(edge.Identifier))
            {
                // Hinzufügen in eigene Verwaltung
                Edges.Add(edge.Identifier, edge);

                // Und noch die Nachbar-/Indirekter-Nachbar-Beziehungen in den Knoten Pflegen.
                // Das geht über die Knoten
                from.AddEdge(edge, IsDirected);
                to.AddEdge(edge, IsDirected);
            }
            else
            {
                throw new InvalidOperationException($"Duplicate Key Identifier={edge.Identifier} Object={edge}");
            }
        }


        /// <summary>
        /// Hinzufügen eines Knoten mit Id
        /// </summary>
        /// <param name="id"></param>
        public void AddVertex(string id)
        {
            IVertex vert = new Vertex(id);

            // Ist der Knoten schon da?
            if (!Vertices.ContainsKey(vert.Identifier))
            {
                // Nur in eigene Verwaltung hinzufügen
                Vertices.Add(vert.Identifier, vert);
            }
            else
            {
                throw new InvalidOperationException($"Duplicate Key Identifier={vert.Identifier} Object={vert}");
            }
        }



        /// <summary>
        /// Entfernen einer Kante anhand des Identifiers.
        /// </summary>
        /// <param name="id"></param>
        public void RemoveEdge(string id)
        {
            if (Edges.ContainsKey(id))
            {
                IEdge edge = Edges[id];

                // Kante entfernen
                Edges.Remove(id);

                // Und noch die Nachbar-/Indirekter-Nachbar-Beziehungen in den Knoten Pflegen.
                // Das geht über die Knoten
                edge.FromVertex.RemoveEdge(edge);
                edge.ToVertex.RemoveEdge(edge);
            }
            else
            {
                throw new InvalidOperationException($"Edge with Identifier={id} not found.");
            }
        }


        /// <summary>
        /// Entfernen eines Knotens anhand des Identifiers.
        /// Achtung: Alle Kanten zu und von dieser Kante müssen zuvor entfernt worden sein!
        /// </summary>
        /// <param name="id"></param>
        public void RemoveVertex(string id)
        {
            if (Vertices.ContainsKey(id))
            {
                IVertex vert = Vertices[id];

                // nur entfernen, wenn alle Verbindungen weg sind
                if (vert.Neighbours.Count == 0 && vert.ForeignNeighbours.Count == 0)
                {
                    Vertices.Remove(vert.Identifier);
                }
                else
                {
                    throw new InvalidOperationException($"Vertex Identifier={vert.Identifier} has still edges and can not be removed.");
                }
            }
            else
            {
                throw new InvalidOperationException($"Vertex with Identifier={id} not found.");
            }
        }

        #endregion Modify Graph


        /// <summary>
        /// Überschriebene ToString-Methode
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Graph={Identifier}";
        }

        #endregion Methods
    }
}

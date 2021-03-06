﻿using GraphLibrary.Interface;
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
        /// Identifizierung dieses Knotens
        /// </summary>
        public string Identifier { get; private set; }


        /// <summary>
        /// Nachbarknoten, die von mir aus direkt erreicht werden können
        /// </summary>
        public Dictionary<string, IVertex> Neighbours { get; private set; }


        /// <summary>
        /// Fremde Nachbarknoten, die mich von sich aus direkt erreichen können
        /// </summary>
        public Dictionary<string, IVertex> ForeignNeighbours { get; private set; }


        /// <summary>
        /// Kanten an diesem Knoten
        /// </summary>
        public Dictionary<string, IEdge> Edges { get; private set; }


        /// <summary>
        /// Werte an diesem Knoten. Werden als Key-Value-Pair abgespeichert
        /// </summary>
        public Dictionary<string, double> Values { get; set; }

        #endregion Properties


        #region Constructor

        /// <summary>
        /// Standard-Konstruktur
        /// </summary>
        /// <param name="id">Eindeutige ID des Knotens</param>
        /// <param name="values">Kosten an diesem Knoten</param>
        public Vertex(string id, Dictionary<string, double> values = null)
        {
            Identifier = id;

            Neighbours = new Dictionary<string, IVertex>();
            ForeignNeighbours = new Dictionary<string, IVertex>();
            Edges = new Dictionary<string, IEdge>();

            if (values == null)
            {
                //Vorbelegen
                Values = new Dictionary<string, double>();
            }
            else
            {
                Values = values;
            }
        }


        #endregion Constructor


        #region Methods


        /// <summary>
        /// Hinzufügen einer Kante, die mit diesem Knoten verbunden ist.
        /// Dabei die Neighbours und ForeignNeighbours aktualisieren.
        /// </summary>
        /// <param name="edge">neue Kante</param>
        /// <param name="directed">gibt an, ob die neue Kante gerichtet ist</param>
        public void AddEdge(IEdge edge, bool directed)
        {
            Edges.Add(edge.Identifier, edge);
            

            // Nachbarn ggf. aktualisieren
            // Suche das anderen Ende der Kante
            IVertex other = edge.GetOtherVertex(this);

            bool otherIsForeignN = false;
            bool otherISN = false;

            // Ermittlung, ob Neighbour und ForeignNeighbour

            if (directed)
            {
                // Geht die Kante zu mir?
                if (edge.ToVertex == this)
                {
                    // Kante zu mir, also ein ForeignNeighbour
                    otherIsForeignN = true;
                }
                else
                {
                    // Kante von mir weg , also mein Neighbour
                    otherISN = true;
                }
            }
            else
            {
                // Bei nicht Directed wird in Neighbours und ForeignNeighbours eingefügt
                otherISN = true;
                otherIsForeignN = true;
            }


            // Eigentliches Hinzufügen
            if (otherISN)
            {
                Neighbours.Add(other.Identifier, other);
            }

            if(otherIsForeignN)
            {
                ForeignNeighbours.Add(other.Identifier, other);
            }
        }


        /// <summary>
        /// Entfernen einer Kante, die mit diesem Knoten verbunden ist.
        /// Dabei die Neighbours und ForeignNeighbours aktualisieren.
        /// </summary>
        /// <param name="edge">zu entfernene Kante</param>
        /// <param name="directed">gibt an, ob die neue Kante gerichtet ist</param>
        public void RemoveEdge(IEdge edge, bool directed)
        {
            Edges.Remove(edge.Identifier);


            // Nachbarn ggf. aktualisieren
            // Suche das anderen Ende der Kante
            IVertex other = edge.GetOtherVertex(this);

            bool otherIsForeignN = false;
            bool otherISN = false;

            // Ermittlung, ob Neighbour und ForeignNeighbour

            if (directed)
            {
                // Geht die Kante zu mir?
                if (edge.ToVertex == this)
                {
                    // Kante zu mir, also ein ForeignNeighbour
                    otherIsForeignN = true;
                }
                else
                {
                    // Kante von mir weg , also mein Neighbour
                    otherISN = true;
                }
            }
            else
            {
                // Bei nicht Directed wird in Neighbours und ForeignNeighbours eingefügt
                otherISN = true;
                otherIsForeignN = true;
            }


            // Eigentliches Hinzufügen
            if (otherISN)
            {
                Neighbours.Remove(other.Identifier);
            }

            if (otherIsForeignN)
            {
                ForeignNeighbours.Remove(other.Identifier);
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

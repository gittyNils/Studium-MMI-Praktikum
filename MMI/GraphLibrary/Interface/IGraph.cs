﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Interface
{
    /// <summary>
    /// Schnittstelle eines Graphen
    /// </summary>
    public interface IGraph
    {
        #region Properties

        /// <summary>
        /// Identifizierung dieses Graphen
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// Gibt an, ob es sich um einen gerichteten Graphen, also mit Richtung bei den Kanten, handelt.
        /// </summary>
        bool IsDirected { get; }

        /// <summary>
        /// Alle Kanten des Graphen
        /// </summary>
        Dictionary<string, IEdge> Edges { get; }

        /// <summary>
        /// Alle Knoten des Graphen
        /// </summary>
        Dictionary<string, IVertex> Vertices { get; }

        #endregion Properties


        #region Methods

        
        /// <summary>
        /// Liefert, falls vorhanden, die Kante zwischen from und to
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Kante oder null, wenn keine vorhanden</returns>
        IEdge GetEdge(IVertex from, IVertex to);


        /// <summary>
        /// Hinzufügen einer Kante von einem Knoten zum anderen
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="costs">Kosten</param>
        void AddEdge(IVertex from, IVertex to, Dictionary<string, int> costs = null);


        /// <summary>
        /// Hinzufügen eines Knoten mit Id
        /// </summary>
        /// <param name="identifier"></param>
        void AddVertex(string identifier);


        #endregion Methods
    }
}

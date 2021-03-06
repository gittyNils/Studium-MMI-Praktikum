﻿using GraphLibrary.DataModel;
using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Factory
{
    /// <summary>
    /// Factory-Klasse zur Erstellung eines Graphen-Objektes.
    /// Dabei können verschiedene Quellen genutzt werden, z.B. eine Zeichenfolge in Form einer Adjazenzmatrix oder Adjazenzliste.
    /// Zudem wird angegeben, ob es sich um einen gerichteten oder ungerichteten Graphen handelt, worauf die Quelle aber nicht geprüft wird.
    /// Die Nummerierung der Knoten beginnt immer mit 0.
    /// </summary>
    public static class GraphFactory
    {
        //Aspekte: Directed oder nicht, From String, FromFile, AdjazenzListe oder Matrix


        #region Without Costs


        /// <summary>
        /// Einlesen eine Graphen von einer Adjazenzmatrix aus einem String ohne Kostenangaben
        /// </summary>
        /// <param name="matrix">Matrix als String</param>
        /// <param name="id">Identifier des Graphen</param>
        /// <param name="directed">Gibt an, ob Graph gerichtet ist</param>
        /// <param name="splitChar">Zeichen, mit dem Elemente in der Matrix in einer Zeile gesplittet wurden (Default \t)</param>
        /// <returns></returns>
        public static IGraph GraphFromAdjMatrixStringWithoutCost(string matrix, string id, bool directed, char splitChar = '\t')
        {
            IGraph ret = null;

            var tmpGraph = new Graph(id, directed);

            // Erste Zeile ist Anzahl Knoten. Diese einfach durchnummerieren
            using (StringReader sr = new StringReader(matrix))
            {
                int vertexCount = int.Parse(sr.ReadLine());

                FillGraphWithXElements(tmpGraph, vertexCount);

                // Nun Kanten

                for (int row = 0; row < vertexCount; row++)
                {
                    // Einzelne Elemente der Row rausziehen
                    var rowElements = sr.ReadLine().Split(splitChar);

                    // Wenn nicht directed, dann nur obere Dreiecksmatrix einlesen, da Rest dann klar
                    int startcolumn = 0;
                    if (!directed)
                    {
                        startcolumn = row;
                    }

                    for (int column = startcolumn; column < vertexCount; column++)
                    {
                        // from Row to Column läuft eine Kante
                        int val = int.Parse(rowElements[column]);

                        if (val != 0)
                        {
                            // es gibt eine Verbindung.
                            var from = tmpGraph.Vertices[row.ToString()];
                            var to = tmpGraph.Vertices[column.ToString()];
                            tmpGraph.AddEdge(from, to);
                        }
                    }
                }


                ret = tmpGraph;
            }


            return ret;
        }




        /// <summary>
        /// Einlesen eine Graphen von einer Adjazenzliste aus einem String ohne Kostenangaben
        /// </summary>
        /// <param name="inputString">Eingabestring</param>
        /// <param name="id">Identifier des Graphen</param>
        /// <param name="directed">Gibt an, ob Graph gerichtet ist</param>
        /// <param name="splitChar">Zeichen, mit dem Elemente in der Matrix in einer Zeile gesplittet wurden (Default \t)</param>
        /// <returns></returns>
        public static IGraph GraphFromAdjListStringWithoutCost(string inputString, string id, bool directed, char splitChar = '\t')
        {
            IGraph ret = null;

            var tmpGraph = new Graph(id, directed);

            // Erste Zeile ist Anzahl Knoten. Diese einfach durchnummerieren
            using (StringReader sr = new StringReader(inputString))
            {
                int vertexCount = int.Parse(sr.ReadLine());

                FillGraphWithXElements(tmpGraph, vertexCount);

                // Nun Kanten anhand der Rows. So viele, wie eben da sind
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var elements = line.Split(splitChar);

                    // vom ersten Element zum zweiten Element läuft eine Kante
                    var from = tmpGraph.Vertices[elements[0]];
                    var to = tmpGraph.Vertices[elements[1]];
                    tmpGraph.AddEdge(from, to);
                }


                ret = tmpGraph;
            }


            return ret;
        }




        #endregion Without Costs


        #region With Costs



        /// <summary>
        /// Einlesen eine Graphen von einer Adjazenzliste aus einem String mit Kostenangaben
        /// </summary>
        /// <param name="inputString">Eingabestring</param>
        /// <param name="id">Identifier des Graphen</param>
        /// <param name="costName">Name der Kosten</param>
        /// <param name="directed">Gibt an, ob Graph gerichtet ist</param>
        /// <param name="splitChar">Zeichen, mit dem Elemente in der Matrix in einer Zeile gesplittet wurden (Default \t)</param>
        /// <returns></returns>
        public static IGraph GraphFromAdjListStringWithCost(string inputString, string id, string costName, bool directed, char splitChar = '\t')
        {
            IGraph ret = null;

            var tmpGraph = new Graph(id, directed);

            // Erste Zeile ist Anzahl Knoten. Diese einfach durchnummerieren
            using (StringReader sr = new StringReader(inputString))
            {
                int vertexCount = int.Parse(sr.ReadLine());

                FillGraphWithXElements(tmpGraph, vertexCount);

                // Nun Kanten anhand der Rows. So viele, wie eben da sind
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var elements = line.Split(splitChar);

                    // vom ersten Element zum zweiten Element läuft eine Kante
                    var from = tmpGraph.Vertices[elements[0]];
                    var to = tmpGraph.Vertices[elements[1]];
                    double cost = double.Parse(elements[2], CultureInfo.InvariantCulture);
                    var dict = new Dictionary<string, double> { { costName, cost } };
                    tmpGraph.AddEdge(from, to, dict);
                }


                ret = tmpGraph;
            }


            return ret;
        }



        /// <summary>
        /// Einlesen eine Graphen von einer Adjazenzliste aus einem String mit Kostenangaben für Knoten und Kanten
        /// </summary>
        /// <param name="inputString">Eingabestring</param>
        /// <param name="id">Identifier des Graphen</param>
        /// <param name="costNameEdge">Name der Kosten an den Kanten</param>
        /// <param name="capacityNameEdge">Name der Kapazität an den Kanten</param>
        /// <param name="balanceNameVertex">Name der Balance an den Knoten</param>
        /// <param name="directed">Gibt an, ob Graph gerichtet ist</param>
        /// <param name="splitChar">Zeichen, mit dem Elemente in der Matrix in einer Zeile gesplittet wurden (Default \t)</param>
        /// <returns></returns>
        public static IGraph GraphFromAdjListStringWithDoubleCost(string inputString, string id, string costNameEdge, string capacityNameEdge, string balanceNameVertex, bool directed, char splitChar = '\t')
        {
            IGraph ret = null;

            var tmpGraph = new Graph(id, directed);

            // Erste Zeile ist Anzahl Knoten. Diese einfach durchnummerieren
            using (StringReader sr = new StringReader(inputString))
            {
                int vertexCount = int.Parse(sr.ReadLine());

                // Beginne mit Knoten 0
                for (int i = 0; i < vertexCount; i++)
                {
                    string nodeCost = sr.ReadLine();
                    double cost = double.Parse(nodeCost, CultureInfo.InvariantCulture);
                    var dict = new Dictionary<string, double> { { balanceNameVertex, cost } };

                    // Knoten Einfügen
                    tmpGraph.AddVertex(i.ToString(), dict);
                }

                // Nun Kanten anhand der Rows. So viele, wie eben da sind
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var elements = line.Split(splitChar);

                    // vom ersten Element zum zweiten Element läuft eine Kante
                    var from = tmpGraph.Vertices[elements[0]];
                    var to = tmpGraph.Vertices[elements[1]];
                    double cost = double.Parse(elements[2], CultureInfo.InvariantCulture);
                    double capacity = double.Parse(elements[3], CultureInfo.InvariantCulture);
                    var dict = new Dictionary<string, double> { { costNameEdge, cost }, { capacityNameEdge, capacity } };
                    tmpGraph.AddEdge(from, to, dict);
                }


                ret = tmpGraph;
            }


            return ret;
        }


        #endregion With Costs


        #region Bipartit

        public static IGraph BipartitGraphFromAdjListString(string inputString, string id, bool directed, out List<IVertex> setA, out List<IVertex> setB, char splitChar = '\t')
        {
            IGraph ret = null;
            setA = new List<IVertex>();
            setB = new List<IVertex>();

            var tmpGraph = new Graph(id, directed);

            // Erste Zeile ist Anzahl Knoten. Diese einfach durchnummerieren
            using (StringReader sr = new StringReader(inputString))
            {
                int vertexCount = int.Parse(sr.ReadLine());

                FillGraphWithXElements(tmpGraph, vertexCount);

                // Einlesen, wie viele in der ersten Menge sind
                int vertexCountInFirstSet = int.Parse(sr.ReadLine());

                // Nun Kanten anhand der Rows. So viele, wie eben da sind
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var elements = line.Split(splitChar);

                    // vom ersten Element zum zweiten Element läuft eine Kante
                    var from = tmpGraph.Vertices[elements[0]];
                    var to = tmpGraph.Vertices[elements[1]];
                    tmpGraph.AddEdge(from, to);
                }

                // Menge A und B füllen
                setA = tmpGraph.Vertices.Values.Take(vertexCountInFirstSet).ToList();
                setB = tmpGraph.Vertices.Values.Except(setA).ToList();

                ret = tmpGraph;
            }


            return ret;
        }


        #endregion Bipartit




        #region Helper

        /// <summary>
        /// Füllt den Übergebenen Graphen mit x Knoten, beginnend bei 0
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="x"></param>
        private static void FillGraphWithXElements(IGraph graph, int x)
        {
            // Beginne mit Knoten 0
            for (int i = 0; i < x; i++)
            {
                // Knoten Einfügen
                graph.AddVertex(i.ToString());
            }
        }



        #endregion Helper


    }
}

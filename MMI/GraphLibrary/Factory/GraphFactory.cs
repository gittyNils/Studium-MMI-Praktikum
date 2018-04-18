using GraphLibrary.DataModel;
using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
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
    /// </summary>
    public class GraphFactory
    {

        //Aspekte: Directed oder nicht, From String, FromFile, AdjazenzListe oder Matrix


        #region Without Costs


        /// <summary>
        /// Einlesen eine Graphen von einer Adjazenzmatrix aus einem String ohne Kostenangaben
        /// </summary>
        /// <param name="matrix">Matrix als String</param>
        /// <param name="id">Identifier des Graphen</param>
        /// <param name="directed">Gibt an, ob Graph gerichtet ist</param>
        /// <returns></returns>
        public static IGraph GraphFromAdjMatrixStringWithoutCost(string matrix, string id, bool directed)
        {
            IGraph ret = null;

            var tmpGraph = new Graph(id, directed);

            // Erste Zeile ist Anzahl Knoten. Diese einfach durchnummerieren
            using (StringReader sr = new StringReader(matrix))
            {
                int vertexCount = int.Parse(sr.ReadLine());

                for (int i = 1; i <= vertexCount; i++)
                {
                    // Knoten Einfügen
                    tmpGraph.AddVertex(i.ToString());
                }

                // Nun Kanten

                for (int row = 0; row < vertexCount; row++)
                {
                    // Einzelne Elemente der Row rausziehen
                    var rowElements = sr.ReadLine().Split('\t');

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
                            // es gibt eine Verbindung. Immer + 1, da Variablen hier Index
                            var from = tmpGraph.Vertices[(row + 1).ToString()];
                            var to = tmpGraph.Vertices[(column + 1).ToString()];
                            tmpGraph.AddEdge(from, to);
                        }
                    }
                }


                ret = tmpGraph;
            }


            return ret;
        }




        #endregion Without Costs


    }
}

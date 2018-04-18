using GraphLibrary.Factory;
using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A1
{
    /*
     *
     * 1. Aufgabe:
     * Mit einer Tiefensuche und einer Breitensuche sollen Zusammenhangskomponenten in einem Grapen gezählt werden.
     * 
     * Graph: Aufbau und Speicherformat Breitensuche (iterativ) und Tiefensuche (rekursiv) zur Bestimmung der Anzahl der Zusammenhangskomponenten
     * 
     */

    class Program
    {
        static void Main(string[] args)
        {

            string s1 = File.ReadAllText(@"SampleData\Graph1.txt");
            IGraph g = GraphFactory.GraphFromAdjMatrixStringWithoutCost(s1, "Graph 1", false);

        }
    }
}

using GraphLibrary.Algorithm;
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
     * 
     * Lösungen:
     * Graph1: 3
     * Graph2: 2
     * Graph3: 4
     * Graph4: 4
     * 
     */

    class Program
    {
        static void Main(string[] args)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();


            string s1 = File.ReadAllText(@"SampleData\Graph1.txt");
            IGraph g1 = GraphFactory.GraphFromAdjMatrixStringWithoutCost(s1, "Graph 1", false);

            int anz1 = ConnectedGraphComponents.CountComponents(g1, true);







            string s2 = File.ReadAllText(@"SampleData\Graph2.txt");
            IGraph g2 = GraphFactory.GraphFromAdjListStringWithoutCost(s2, "Graph 2", false);

            int anz2 = ConnectedGraphComponents.CountComponents(g2, true);



            string s3 = File.ReadAllText(@"SampleData\Graph3.txt");
            IGraph g3 = GraphFactory.GraphFromAdjListStringWithoutCost(s3, "Graph 3", false);


            int anz3 = ConnectedGraphComponents.CountComponents(g3, true);



            string s4 = File.ReadAllText(@"SampleData\Graph4.txt");
            IGraph g4 = GraphFactory.GraphFromAdjListStringWithoutCost(s4, "Graph 4", false);

            
            int anz4 = ConnectedGraphComponents.CountComponents(g4, true);


            sw.Stop();
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms elapsed");
            Console.ReadLine();

        }
    }
}

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
        /// <summary>
        /// Informationen für einen Testlauf in dieser Aufgabe
        /// </summary>
        public class TestData
        {
            public string Name { get; set; }
            public string File { get; set; }
            public bool IsAdjazenzList { get; set; }
        }


        static void Main(string[] args)
        {
            List<TestData> testFiles = new List<TestData>();

            testFiles.Add(new TestData { Name = "Graph 1", File = @"SampleData\Graph1.txt", IsAdjazenzList = false });
            testFiles.Add(new TestData { Name = "Graph 2", File = @"SampleData\Graph2.txt", IsAdjazenzList = true });
            testFiles.Add(new TestData { Name = "Graph 3", File = @"SampleData\Graph3.txt", IsAdjazenzList = true });
            testFiles.Add(new TestData { Name = "Graph 4", File = @"SampleData\Graph4.txt", IsAdjazenzList = true });



            foreach (var data in testFiles)
            {
                Console.WriteLine($"------------------ Start Name={data.Name} ------------------");

                // Initialisieren

                // Einlesen in String 
                string s = File.ReadAllText(data.File);

                var swInit = System.Diagnostics.Stopwatch.StartNew();

                IGraph g;
                if (data.IsAdjazenzList)
                {
                    g = GraphFactory.GraphFromAdjListStringWithoutCost(s, data.Name, false);
                }
                else
                {
                    g = GraphFactory.GraphFromAdjMatrixStringWithoutCost(s, data.Name, false);
                }

                swInit.Stop();
                Console.WriteLine($"Init took {swInit.ElapsedMilliseconds} ms");

                // Rechnen

                var swCalc = System.Diagnostics.Stopwatch.StartNew();
                int anzBF = ConnectedGraphComponents.CountComponentsWithBreadthFirst(g);
                swCalc.Stop();
                Console.WriteLine($"Calc Components with BreadthFirst took {swCalc.ElapsedMilliseconds} ms");


                swCalc.Restart();
                int anzDF = ConnectedGraphComponents.CountComponentsWithDepthFirst(g);
                swCalc.Stop();
                Console.WriteLine($"Calc Components with DepthFirst took {swCalc.ElapsedMilliseconds} ms");


                Console.WriteLine($"Zusammenhangskomponenten-Anzahl: {anzBF}(BreadthFirst) und {anzDF}(DepthFirst) \n");
            }


            Console.ReadLine();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Algorithm
{
    /// <summary>
    /// Klasse mit Konstanten
    /// </summary>
    public static class CONST
    {

        /// <summary>
        /// Key für den Value von Kosten.
        /// </summary>
        public const string KOSTEN_VALUE = "Kosten";

        /// <summary>
        /// Key für den Value des Flusses einer Kante eines Graphen
        /// </summary>
        public const string FLUSS_VALUE = "Fluss";

        /// <summary>
        /// Key für den Value der Kapazität einer Kante eines Graphen
        /// </summary>
        public const string KAPAZITÄT_VALUE = "Kapazität";


        /// <summary>
        /// Richtung einer Kante. 
        /// Gibt im Residual-Graph an, ob es sich bei einer Kante um die Originale, also die Hinrichtung, oder die Rückrichtung der Kante handelt.
        /// Nur intern im Residualgraphen verwendet.
        /// 1 = Hin/original, sonst Rück
        /// </summary>
        public const string RICHTUNG_VALUE = "Richtung";


        /// <summary>
        /// Key für den Value der Balance bei einem Knoten eines Graphen
        /// </summary>
        public const string BALANCE_VALUE = "Balance";



        /// <summary>
        /// Key für den Value der Pseudo-Balance bei einem Knoten eines Graphen
        /// </summary>
        public const string PSEUDO_BALANCE_VALUE = "PseudoBalance";


        /// <summary>
        /// Knotenname für SuperQuelle
        /// </summary>
        public const string SUPER_SOURCE = "SuperSource";

        /// <summary>
        /// Knotenname für Superquele
        /// </summary>
        public const string SUPER_TARGET = "SuperTarget";

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.DataStructures
{
    /// <summary>
    /// Stellt eine Menge disjunkter Mengen dar, wobei jede Menge einen Stellvertreter hat, der ebenfalls Teil der Menge ist.
    /// Der Weg zum Stellvertreter geht durch eine Parent-Beziehung der Elemente in der Menge.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisJointSet<T>
    {
        #region Member

        /// <summary>
        /// Zu einem Element feststellen, dass es in dieser Menge von Mengen ist und den Knoten davon liefern.
        /// </summary>
        private Dictionary<T, DisJointSetNode<T>> _elementToNodeMap = new Dictionary<T, DisJointSetNode<T>>();

        #endregion Member


        #region Methods

        /// <summary>
        /// Fügt den Mengen eine neue einelementige Menge hinzu
        /// </summary>
        /// <param name="element"></param>
        public void AddSet(T element)
        {
            var newSet = new DisJointSetNode<T> { Data = element };

            // oberster Parent zeigt auf sich selbst
            newSet.Parent = newSet;

            _elementToNodeMap.Add(element, newSet);
        }


        /// <summary>
        /// Gibt den Stellvertreter für das übergebene Mitglied einer Menge zurück.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public T FindSet(T element)
        {
            if (!_elementToNodeMap.ContainsKey(element))
            {
                throw new Exception("No such set with given member.");
            }

            // hole über den Wert das Element in der Verwaltung und Suche den Stellvertreter
            return FindSet(_elementToNodeMap[element]).Data;
        }

        /// <summary>
        /// Rekursiv in den Knoten nach oben bewegen und den Stellvertreter der dazugehörigen Menge bestimmen.
        /// Die Pfade können dabei dadurch gekürzt werden, dass der besuchte Knoten, wenn er nicht der Stellvertreter ist, mit Parent direkt auf den Stellvertreter gesetzt wird.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private DisJointSetNode<T> FindSet(DisJointSetNode<T> node)
        {
            var parent = node.Parent;

            if (node != parent)
            {
                // ich bin nicht Stellvertreter, aber ich merke mir diesen als meinen Parent.
                node.Parent = FindSet(node.Parent);
                return node.Parent;
            }
            else
            {
                // Stellvertreter gefunden
                return parent;
            }
        }

        /// <summary>
        /// Vereinen der zu übergebenen Werten gehörenden Mengen nur, wenn die Mengen verschieden.
        /// Sonst passiert nichts.
        /// </summary>
        /// <param name="setAMember"></param>
        /// <param name="setBMember"></param>
        public void Union(T elementA, T elementB)
        {
            // Stellvertreter raussuchen
            var rootA = FindSet(elementA);
            var rootB = FindSet(elementB);

            if (rootA.Equals(rootB))
            {
                // Die Mengen gehören schon zusammen
                return;
            }

            var nodeA = _elementToNodeMap[rootA];
            var nodeB = _elementToNodeMap[rootB];


            // nun die beiden Mengen verknüpfen
            // Einfach node A als Parent für alle nehmen. Beim suchen wird ja ggf. "nachgeflacht"
            nodeB.Parent = nodeA;
        }

        #endregion Methods

    }


    /// <summary>
    /// Ein Knoten/Element in einer disjunkten Menge
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DisJointSetNode<T>
    {
        /// <summary>
        /// Daten
        /// </summary>
        internal T Data { get; set; }


        /// <summary>
        /// Zeiger auf Parent (für Weg zum Stellvertreter)
        /// </summary>
        internal DisJointSetNode<T> Parent { get; set; }
    }

}

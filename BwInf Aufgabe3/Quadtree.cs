using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BwInf_Aufgabe3
{
    /// <summary>
    /// Implementiert einen Quadtree
    /// </summary>
    class Quadtree
    {
        public QuadtreeElement root;
        public Map map;
        public Quadtree(Map map)
        {
            this.map = map;
            root = new QuadtreeElement(new Position(0, 0), map.nodeMap.GetLength(0), map);
        }


        /// <summary>
        /// Gibt die Geländeart eines Punktes über eine rekursive Methode zurück
        /// </summary>
        /// <param name="n">Der Punkt</param>
        /// <returns></returns>
        public GroundType GetGroundTypeOfNode(Node n)
        {
            return root.GetGroundTypeOfNode(n);
        }

        /// <summary>
        /// Gibt über eine rekursive Methode die Anzahl aller Knoten zurück
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return root.Count(0);
        }

        /// <summary>
        /// Zeichnet den Quadtree in ein Bild
        /// </summary>
        /// <returns>Das fertiggestellte Bild</returns>
        public Bitmap ToImage()
        {
            int size = this.map.nodeMap.GetLength(0);
            Bitmap bitmap = new Bitmap(size, size);
            return root.ToImage(bitmap);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwInf_Aufgabe3
{
    /// <summary>
    /// Implementiert einen Punkt
    /// </summary>
    class Node : IHeapItem<Node>
    {
        public Position position;
        public Node parent;
        public int hCost, gCost;
        //public GroundType groundType;
        public bool walkable;
        int heapIndex;

        public int fCost
        {
            get
            {
                return hCost + gCost;
            }
        }

        public int HeapIndex {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }

        public Node(Position p, bool groundType)
        {
            this.position = p;
            this.walkable = groundType;

        }

        /// <summary>
        /// Gibt die Geländeart als Farbe zurück
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Color GroundTypeToColor(GroundType type)
        {
            if (type == GroundType.Water)
                return Map.WATER_COLOR;
            else if (type == GroundType.Ground)
                return Map.GROUND_COLOR;
            else if (type == GroundType.Unknown)
                return Map.UNKNOWN_COLOR;
            else if (type == GroundType.Mixed)
                return Map.MIXED_COLOR;
            else
                return Map.UNKNOWN_COLOR;
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }

            return -compare;
        }
    }


    /// <summary>
    /// Implementiert die unterschiedlichen Geländearten
    /// </summary>
    enum GroundType
    {
        Water, Ground, Unknown, Mixed
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwInf_Aufgabe3
{
    /// <summary>
    /// Implementiert einen Knoten im Quadtree
    /// </summary>
    class QuadtreeElement
    {

        public GroundType groundType;
        int size;
        Position position;
        Map map;
        public bool isEndNode
        {
            get
            {
                if (size == 2 || groundType != GroundType.Unknown)
                    return true;
                else return false;
            }
        }
        // Die vier Tochterknoten
        QuadtreeElement bottomLeft;
        QuadtreeElement bottomRight;
        QuadtreeElement topLeft;
        QuadtreeElement topRight;

        public QuadtreeElement(Position position, int size, Map map)
        {
            groundType = map.GetAreaType(position.x, position.y, size);
            this.map = map;
            this.position = position;
            this.size = size;
        }

        public QuadtreeElement GetTopLeftElement()
        {

            if(topLeft == null)
            {
                topLeft = new QuadtreeElement(position, size/2, map);
            }
            return topLeft;
        }

        public QuadtreeElement GetTopRightElement()
        {

            if (topRight == null)
            {
                topRight = new QuadtreeElement(new Position(position.x + size/2, position.y), size / 2, map);
            }
            return topRight;
        }

        public QuadtreeElement GetBottomLeft()
        {

            if (bottomLeft == null)
            {
                bottomLeft = new QuadtreeElement(new Position(position.x, position.y + size/2), size / 2, map);
            }
            return bottomLeft;
        }

        public QuadtreeElement GetBottomRight()
        {

            if (bottomRight == null)
            {
                bottomRight = new QuadtreeElement(new Position(position.x + size/2, position.y + size/2), size / 2, map);
            }
            return bottomRight;
        }

        /// <summary>
        /// Gibt für einen Punkt die Geländeart zurück
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public GroundType GetGroundTypeOfNode(Node n)
        {
            // Es handelt sich um einen Knoten ohne Tochterknoten
            if (isEndNode)
            {
                return groundType;
            } else
            {
                // Berechne in welchen hälften der Punkt sich befindet
                bool nodeInRightHalf = n.position.x >= this.position.x + size / 2;
                bool nodeInBottomHalf = n.position.y >= this.position.y + size / 2;

                if (nodeInBottomHalf)
                {
                    if (nodeInRightHalf)
                    {
                        return GetBottomRight().GetGroundTypeOfNode(n);
                    }
                    return GetBottomLeft().GetGroundTypeOfNode(n);
                } else
                {
                    if (nodeInRightHalf)
                    {
                        return GetTopRightElement().GetGroundTypeOfNode(n);
                    }
                    return GetTopLeftElement().GetGroundTypeOfNode(n);
                }
            }
        }

        /// <summary>
        /// Gibt rekursiv die Anzahl der Tochterknoten und diesem Knoten zurück
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public int Count(int count)
        {
            count++;
            if (bottomLeft != null)
                count = bottomLeft.Count(count);
            if (bottomRight != null)
                count = bottomRight.Count(count);
            if (topLeft != null)
                count = topLeft.Count(count);
            if (topRight != null)
                count = topRight.Count(count);

            return count;
        }

        /// <summary>
        /// Zeichnet für diesen und die Tochterknoten das Bild
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public Bitmap ToImage(Bitmap bitmap)
        {
            //Console.WriteLine($"Drawing: {position.x}, {position.y}: {size}");
            for(int x = 0; x < size; x++)
            {
                for(int y = 0; y < size; y++)
                {
                    bitmap.SetPixel(x + position.x, y + position.y, Node.GroundTypeToColor(this.groundType));
                }

            }
            if (!isEndNode)
            {
                //Console.WriteLine("Drawing top left");
                if (topLeft != null)
                {
                    bitmap = GetTopLeftElement().ToImage(bitmap);
                }
                //Console.WriteLine("Drawing top right");
                if (topRight != null)
                {

                    bitmap = GetTopRightElement().ToImage(bitmap);
                }
                //Console.WriteLine("Drawing bottom left");
                if (bottomLeft != null)
                {

                    bitmap = GetBottomLeft().ToImage(bitmap);
                }
                //Console.WriteLine("Drawing bottom right");
                if (bottomRight != null)
                {

                    bitmap = GetBottomRight().ToImage(bitmap);
                }

            }
            return bitmap;
        }
    }
}

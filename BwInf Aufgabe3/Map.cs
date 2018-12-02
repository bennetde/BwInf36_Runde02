using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwInf_Aufgabe3
{
    class Map
    {
        public static Color GROUND_COLOR = Color.FromArgb(255, 68, 189, 50);
        public static Color WATER_COLOR = Color.FromArgb(255, 0, 168, 255);
        public static Color UNKNOWN_COLOR = Color.FromArgb(255, 53, 59, 72);
        public static Color MIXED_COLOR = Color.FromArgb(255, 241, 196, 15);

        public Node[,] nodeMap;
        public List<Position> startPosition;
        public List<Position> targetPosition;
        public Quadtree quadtree;
        public List<Node> path;

        public Map(int size)
        {
            Console.WriteLine("Creating Map with " + size);
            int quadraticSize = 2;
            while(quadraticSize < size)
            {
                quadraticSize = quadraticSize * 2;
            }
            size = quadraticSize;
            Console.WriteLine("Quadratic Map size: " + size);
            startPosition = new List<Position>();
            targetPosition = new List<Position>();
            nodeMap = new Node[size, size];
            for(int y = 0; y < size; y++)
            {
                for(int x = 0; x < size; x++)
                {
                    nodeMap[x, y] = new Node(new Position(x, y), false);
                }
            }
        }

        public GroundType GetAreaType(int xPos, int yPos, int size)
        {
            // x = 0; y = 0 is firstValue
            bool firstValue = nodeMap[xPos, yPos].walkable;

            // Ignore first value, so start at x = 1
            for(int x = 1; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (firstValue != nodeMap[x + xPos, y + yPos].walkable)
                    {
                        if(size == 2)
                        {
                            return GroundType.Mixed;
                        }
                        return GroundType.Unknown;
                    }
                }
            }
            if (firstValue)
            {
                return GroundType.Ground;
            }
            return GroundType.Water;
        }

        public static Node PixelToNode(Bitmap image, int x, int y, Map map)
        {
            Color c = image.GetPixel(x, y);
            bool groundType;
            if (c == MapReader.GROUND_COLOR)
            {
                groundType = true;
            }
            else if (c == MapReader.WATER_COLOR)
            {
                groundType = false;
            }
            else if (c == MapReader.TARGET_COLOR)
            {
                map.targetPosition.Add(new Position(x, y));
                groundType = true;
            }
            else if (c == MapReader.START_COLOR)
            {
                map.startPosition.Add(new Position(x, y));
                groundType = true;
            }
            else
            {
                groundType = true;
            }

            return new Node(new Position(x,y), groundType);
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.position.x + x;
                    int checkY = node.position.y + y;

                    // Node is in bounds?
                    if (checkX >= 0 && checkX < nodeMap.GetLength(0) && checkY >= 0 && checkY < nodeMap.GetLength(1))
                    {
                        neighbours.Add(nodeMap[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }
    }

    struct Position
    {
        public int x, y;
        public Position(int x,int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return x + "," + y;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BwInf_Aufgabe3
{
    class Pathfinder
    {
        Map map;
        static Color PATH_COLOR = Color.FromArgb(255, 255, 255, 255);

        public List<QuadtreeElement> specialElements;


        /// <summary>
        /// Implementiert den A*-Algorithmus
        /// </summary>
        /// <param name="map">Die Karte</param>
        public Pathfinder(Map map)
        {
            this.specialElements = new List<QuadtreeElement>();
            this.map = map;
        }



        /// <summary>
        /// Findet den besten Weg zwischen zwei Punkten
        /// </summary>
        /// <param name="startNode">Startpunkt</param>
        /// <param name="targetNode">Zielpunkt</param>
        public void FindPath(Node startNode, Node targetNode)
        {
            // Kreiere Openlist und füge Startpunkt hinzu
            Heap<Node> openList = new Heap<Node>(map.nodeMap.GetLength(0) * map.nodeMap.GetLength(0));
            openList.Add(startNode);

            HashSet<Node> closedList = new HashSet<Node>();

            // Es gibt noch Punkte zu erkunden
            while (openList.Count > 0)
            {
                // Nehme den vielversprechendsten Punkt
                Node currentNode = openList.RemoveFirst();
                closedList.Add(currentNode);

                // Der jetzige Punkt ist der Zielpunkt?
                if (currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    return;
                }


                // Hole Nachbarn
                foreach(Node neighbour in map.GetNeighbours(currentNode))
                {
                    // Nachbarn sind in ClosedList oder Nachbar ist Wasser
                    GroundType neighbourGroundType = map.quadtree.GetGroundTypeOfNode(neighbour);
                    if (neighbourGroundType == GroundType.Water || closedList.Contains(neighbour)){
                        continue;
                    }

                    // Spezialfall: Es befinden sich zwei Knoten mit der Geländeart Mixed nebeneinander
                    if(neighbourGroundType == GroundType.Mixed && map.quadtree.GetGroundTypeOfNode(currentNode) == GroundType.Mixed)
                    {
                        GroundType groundTypeBetweenNeighbours = GetSpecialNodeBetweenTwoNeighbours(currentNode, neighbour);
                        // Der mittlere GroundType hat den Typ Wasser -> Es ist nicht möglich auf diesen Nachbarn zu gehen
                        if(groundTypeBetweenNeighbours == GroundType.Water)
                        {
                            continue;
                        }
                    }

                    // G-Kosten berechnen
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                    // Nachbar noch nicht in Openlist oder neuer Weg zu Nachbar ist besser
                    if(newMovementCostToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                    {
                        // G-Kosten überschreiben und H-Kosten berechnen; Parent zum aktuellen Knoten setzen
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;
                        // Nachbar in openList einfügen oder überschreiben
                        if (!openList.Contains(neighbour))
                        {
                           
                            openList.Add(neighbour);
                        } else
                        {
                            openList.UpdateItem(neighbour);
                        }
                    }
                }
            }
            // Kein Pfad wurde gefunden
            return;
        }

        /// <summary>
        /// Gibt die Geländeart zwischen zwei Nachbarn
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns></returns>
        GroundType GetSpecialNodeBetweenTwoNeighbours(Node nodeA, Node nodeB)
        {
            QuadtreeElement specialElement;
            if(nodeA.position.y == nodeB.position.y)
            {
                if(nodeA.position.x < nodeB.position.x)
                {
                    specialElement = new QuadtreeElement(nodeA.position, 2, map);
                } else
                {
                    specialElement = new QuadtreeElement(nodeB.position, 2, map);
                }
            } else if(nodeA.position.y > nodeB.position.y)
            {
                specialElement = new QuadtreeElement(nodeA.position, 2, map);
            } else
            {
                specialElement = new QuadtreeElement(nodeB.position, 2, map);
            }

            return specialElement.groundType;
        }


        /// <summary>
        /// Verfolgt den Pfad über die Parents der jeweiligen Punkte zurück 
        /// </summary>
        /// <param name="startNode">Startpunkt</param>
        /// <param name="endNode">Endpunkt</param>
        void RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;
            while(currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            map.path = path;
        }

        /// <summary>
        /// Berechnet die Entfernung zwischen zwei Punkten
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="targetNode"></param>
        /// <returns></returns>
        public int GetDistance(Node sourceNode, Node targetNode)
        {
            int dstX = Math.Abs(sourceNode.position.x - targetNode.position.x);
            int dstY = Math.Abs(sourceNode.position.y - targetNode.position.y);

            if(dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY);
            } else
            {
                return 14 * dstX + 10 * (dstY - dstX);
            }
        }


        /// <summary>
        /// Zeichnet den berechneten Pfad in ein Bild
        /// </summary>
        /// <param name="image"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public Bitmap DrawPath(Bitmap image, List<Node> path)
        {
            foreach(Node n in path)
            {
                image.SetPixel(n.position.x, n.position.y, PATH_COLOR);
            }

            return image;
        }
    }
}

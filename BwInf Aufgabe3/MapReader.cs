using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BwInf_Aufgabe3
{
    /// <summary>
    /// Liest eine Karte und fügt diese in eine Map ein
    /// </summary>
    class MapReader
    {

        // Die jeweiligen Farben die sich in einem Bild befinden können
        public static Color START_COLOR = Color.FromArgb(255, 255, 0, 0);
        public static Color TARGET_COLOR = Color.FromArgb(255, 0,255, 0);

        public static Color GROUND_COLOR = Color.FromArgb(255, 0, 0, 0);
        public static Color WATER_COLOR = Color.FromArgb(255, 255, 255, 255);

        Bitmap image;

        public MapReader(Bitmap image)
        {
            this.image = image;
        }


        /// <summary>
        /// Lädt eine Map aus der eingegebenen Karte
        /// </summary>
        /// <returns></returns>
        public Map LoadMap()
        {
            Map map;
            if(image.Size.Height > image.Size.Width)
            {
                map = new Map(image.Size.Height);
            } else
            {
                map = new Map(image.Size.Width);
            }

            for(int x = 0; x < image.Size.Width; x++){
                for (int y = 0; y < image.Size.Height; y++)
                {
                    map.nodeMap[x, y] = Map.PixelToNode(image,x,y,map);
                }
            }
            map.quadtree = new Quadtree(map);
            return map;
        }
    }
}

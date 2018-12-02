using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace BwInf_Aufgabe3
{
    class Program
    {
        // Einstiegsmethode für das Programm
        [STAThread]
        static void Main(string[] args)
        {
            // Unendliche Schleife, damit sich das Programm nicht schließt
            while (true)
            {
                // Frage den Nutzer nach einer Datei ab
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "png files (*.png)|*.png";
                dialog.FilterIndex = 2;
                string filePath;
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    //Hole den Dateinamen des Bidles
                    filePath = dialog.FileName;
                    // Starte eine Stoppuhr um die Laufzeit zu überwachen
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    // Lese die eingegebene Karte des Nutzers ein und lade die Map
                    MapReader reader = new MapReader(new Bitmap(filePath));
                    Map map = reader.LoadMap();
                    sw.Stop();

                    // Warte auf die Nutzereingabe, welcher Startpunkt benutzt werden soll
                    Console.WriteLine("Found " + map.startPosition.Count + " startpositions and " + map.targetPosition.Count + " targetpositions.");

                    Console.WriteLine("Target-Position at: " + map.targetPosition[0].ToString());

                    for (int i = 0; i < map.startPosition.Count; i++)
                    {
                        Console.WriteLine("Index: " + i + "| Position: " + map.startPosition[i].ToString());
                    }
                    Console.Write("Wähle die Startposition per Index aus: ");
                    string input = Console.ReadLine();
                    int parsedValue = -1;
                    bool numberEntered = false;

                    // Warte solange bis der Nutzer eine richtige Zahl eingegeben hat
                    while (numberEntered == false)
                    {
                        if (!Int32.TryParse(input, out parsedValue))
                        {
                            continue;
                        }

                        if(parsedValue >= 0 && parsedValue < map.startPosition.Count)
                        {
                            numberEntered = true;
                        }
                    }

                    // Suche den Pfad
                    Console.WriteLine("Trying to find path...");
                    Pathfinder finder = new Pathfinder(map);
                    sw.Start();
                    finder.FindPath(map.nodeMap[map.startPosition[parsedValue].x, map.startPosition[parsedValue].y], map.nodeMap[map.targetPosition[0].x, map.targetPosition[0].y]);
                    sw.Stop();
                    // Gebe dem Nutzer Daten über die Berechnung zurück
                    Console.WriteLine("Path found in " + sw.ElapsedMilliseconds + "ms | Count: " + map.quadtree.Count() + finder.specialElements.Count);
                    // Speichere den Quadtree und den Pfad in einem neuen Bild
                    Bitmap image = map.quadtree.ToImage();
                    if (map.path != null)
                    {
                        image = finder.DrawPath(image, map.path);
                    }
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    image.Save(fileName + "_" + parsedValue.ToString() + ".png");
                    // Algorithmus beendet.
                    Console.WriteLine("Finished");
                    sw.Reset();





                }
                // Der Nutzer hat während des Bildladens abbrechen gedrückt -> Programm schließen
                else if (result == DialogResult.Cancel)
                {
                    break;
                }
            }

        }
    }
}

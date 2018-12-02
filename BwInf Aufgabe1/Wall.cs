using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwInf_Aufgabe1
{
    class Wall
    {

        public List<Row> rows
        {
            private set;
            get;
        }
        private readonly int n;

        public int maxHeight
        {
            get
            {
                return n / 2 + 1;
            }
        }

        public int rowLength
        {
            get
            {
                return n * (n + 1) / 2;
            }
        }

        public Wall(int n)
        {
            this.n = n;
            List<int> freeGaps = new List<int>();

            for(int i = 1; i < rowLength + 1; i++)
            {
                freeGaps.Add(i);
            }

            this.rows = BuildWall(new Row(n), freeGaps, 1);
        }


        /// <summary>
        /// Baut die Mauer rekursiv auf
        /// </summary>
        /// <param name="row">Die erste geteste Anordnung für diese Reihe</param>
        /// <param name="currentFreeGaps">Eien Liste mit den verfügbaren Spalten, die benutzt werden können.</param>
        /// <param name="currentRow">Die zurzeitige Reihe</param>
        /// <returns>Die gebaute Mauer als Liste oder null bei einem Fehlschlag</returns>
        private List<Row> BuildWall(Row row, List<int> currentFreeGaps, int currentRow)
        {
            // Wenn eine Kombination schon probiert wurde, wird ein Schritt zurückgegangen
            List<Row> triedCombinations = new List<Row>();
            while (!triedCombinations.Contains(row))
            {
                // Speichert welche Fuge gerade berechnet wird
                int gap = 0;
                // Speichert ab welchem Block der Algorithmus einen Fehler gefunden hat
                int notWorkingIndex = 0;
                // Speichert die neuen freien Fugen ab
                List<int> newCurrentFreeGaps = new List<int>();
                newCurrentFreeGaps.AddRange(currentFreeGaps);
                // Gehe jedes Element in der zur zeitigen Reihe durch
                foreach(int n in row.blocks)
                {
                    // Berechne die zur zeitige Spaltenposition
                    gap += n;

                    // Schau ob diese Spalte schon belegt ist
                    if (!newCurrentFreeGaps.Contains(gap))
                    {
                        // Spalte belegt -> Schritt zurück und notWorkingIndex berechnen
                        notWorkingIndex = row.blocks.IndexOf(n);
                        gap -= n;
                        break;
                    }

                    // Reihe vollständig analysiert
                    if(gap == rowLength)
                    {

                        // Schau ob bei maximaler Höhe
                        if(currentRow != maxHeight)
                        {
                            // Rekursiv nächste Reihe berechnen
                            List<Row> wall = BuildWall(row.RotateDown(0), newCurrentFreeGaps, currentRow + 1);
                            if(wall != null)
                            {
                                // Die nächste Mauer hatte einen Fehler
                                wall.Add(row);
                                return wall;
                            }
                        } else
                        {
                            // Letzte Reihe berechnet -> Alle vorherigen Reihen in einer Liste zusammenfassen
                            List<Row> wall = new List<Row>();
                            wall.Add(row);
                            return wall;
                        }
                    }

                    //Die Spalte aus den verfügbaren Spalten entfernen, da diese nun benutzt wird
                    if(gap != rowLength)
                    {
                        newCurrentFreeGaps.Remove(gap);
                    }
                }

                // Kombination hat nicht geklappt -> nächste Position ausprobieren
                triedCombinations.Add(new Row(row.blocks));
                row = row.RotateDown(notWorkingIndex);
            }
            // Keine Kombination war möglich -> vorherige Reihe neu sortieren
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwInf_Aufgabe1
{
    class Row
    {
        public List<int> blocks;
        
        public Row(List<int> blocks)
        {
            this.blocks = blocks;
        }

        public Row(int n)
        {
            blocks = new List<int>();
            for(int i = 1; i <= n; i++)
            {
                blocks.Add(i);
            }
        }

        
        /// <summary>
        /// Rotates a list down.
        /// </summary>
        /// <param name="startIndex">The first object that should be rotated down</param>
        public Row RotateDown(int startIndex)
        {
            int[] newBlocks = new int[blocks.Count];
            
            for(int i = 0; i < blocks.Count; i++)
            {
                newBlocks[i] = blocks[i];
            }

            int firstValue = blocks[startIndex];

            for(int i = startIndex; i < blocks.Count - 1; i++)
            {
                newBlocks[i] = blocks[i + 1];
            }

            newBlocks[blocks.Count -1] = firstValue;

            return new Row(newBlocks.ToList());
        }

        public override bool Equals(object obj)
        {
            var item = obj as Row;

            if(item == null)
            {
                return false;
            }

            for(int i = 0; i < item.blocks.Count; i++)
            {
                if(item.blocks[i] != this.blocks[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return GetHashCode();
        }

        public override string ToString()
        {
            string output = "/ ";
            foreach(int n in blocks)
            {
                output += n + " / ";
            }

            return output;
        }
    }
}

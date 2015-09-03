using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualNet
{
    public class Node
    {
        public Node(short x, short y, short z, int offset)
        {
            X = x;
            Y = y;
            Z = z;
            Offset = offset;
            Children = new List<int>();
            ChildrenLines = new List<RenderLine>();
            Threshold =.51f;
        }
        public float Activation { get; set; }
        public float Threshold { get; set; }
        public bool IsLoaded { get; set; }
        public short X { get; private set; }
        public short Y { get; private set; }
        public short Z { get; private set; }
        public int Offset { get; private set; }
        public List<int> Children { get; private set; }
        public List<RenderLine> ChildrenLines { get; private set; }
    }
}

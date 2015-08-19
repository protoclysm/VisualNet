using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualNet
{
    public abstract class Node
    {
        public Node(int x, int y, int z,int offset)
        {
            X = x;
            Y = y;
            Z = z;
            Offset = offset;
            Children = new List<int>();
            ChildrenLines = new List<RenderLine>();
        }
        public float Activation { get; set; }
        public bool IsLoaded { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        public int Offset { get; private set; }
        public List<int> Children { get; private set; }
        public List<RenderLine> ChildrenLines { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualNet
{
    public class NormalNode : Node
    {
        public NormalNode(int x, int y, int z, int offset) : base(x, y, z, offset) { }
        //public override List<int> Fire()
        //{
        //    this.Activation = 0;

        //    return Children;
        //}
    }
}

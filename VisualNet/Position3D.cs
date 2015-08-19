using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualNet
{
    public class Position3D
    {
        public byte x { get; set; }
        public byte y { get; set; }
        public byte z { get; set; }
        public Position3D() { }
        public Position3D(byte x,byte y, byte z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    public class Vector3D : Position3D
    {

        public byte xT { get; set; }
        public byte yT { get; set; }
        public byte zT { get; set; }
        public float w { get; set; }
        public Vector3D() { }
        public Vector3D(byte x, byte y, byte z, byte xT, byte yT, byte zT):base(x,y,z)
        {
            this.xT = xT;
            this.yT = yT;
            this.zT = zT;
        }
    }
}

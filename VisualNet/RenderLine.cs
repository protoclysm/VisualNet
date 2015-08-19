using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualNet
{
    public class RenderLine
    {
        public float xS { get; set; }
        public float yS { get; set; }
        public float zS { get; set; }
        public float xV { get; set; }
        public float yV { get; set; }
        public float zV { get; set; }
        public float strength { get; set; }
        public float actionR { get; set; }
        public float actionG { get; set; }
        public float actionB { get; set; }
        public RenderLine(float bx, float by, float bz, float bxv, float byv, float bzv, float act1, float act2, float act3, float str)
        {
            xS = bx;
            yS = by;
            zS = bz;
            xV = bxv;
            yV = byv;
            zV = bzv;
            actionR = act1;
            actionG = act2;
            actionB = act3;
            strength = str;
        }
    }
}

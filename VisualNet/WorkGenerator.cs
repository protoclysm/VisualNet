using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualNet
{
    public abstract class WorkGenerator
    {
        protected ConcurrentQueue<Node> _clientWorkQueue;
        public ConcurrentQueue<RenderLine> _clientRenderQueue;
        protected Tuple<int, int, int> _blockOffset;
        protected GLManager _glmanager;
        protected Node[] _block;


        public WorkGenerator(ref ConcurrentQueue<Node> queueToAddWorkTo, ref ConcurrentQueue<RenderLine> renderLines, ref Tuple<int, int, int> blockOffset, Node[] block)
        {
            _block = block;
            _blockOffset = blockOffset;
            _clientWorkQueue = queueToAddWorkTo;
            _clientRenderQueue = renderLines;
        }

        public abstract void Generate(Node workNode);
    }
}

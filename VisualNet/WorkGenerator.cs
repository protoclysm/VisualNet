using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualNet
{
    public abstract class WorkGenerator
    {
        protected ThreadSafeQueue<Node> _clientWorkQueue;
        protected ThreadSafeQueue<RenderLine> _clientRenderQueue;
        protected Tuple<int, int, int> _blockOffset;
        protected GLManager _glmanager;
        protected Node[] _block;


        public WorkGenerator(ref ThreadSafeQueue<Node> queueToAddWorkTo, ref ThreadSafeQueue<RenderLine> renderLines, ref Tuple<int, int, int> blockOffset, Node[] block)
        {
            _block = block;
            _blockOffset = blockOffset;
            _clientWorkQueue = queueToAddWorkTo;
            _clientRenderQueue = renderLines;
        }

        public abstract ThreadSafeQueue<RenderLine> Generate(Node workNode);
    }
}

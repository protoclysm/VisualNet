using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VisualNet
{
    public class FiringWorkGenerator : WorkGenerator
    {
        public FiringWorkGenerator(ref ThreadSafeQueue<Node> workQueue, ref ThreadSafeQueue<RenderLine> renderQueue, Tuple<int, int, int> blockOffset, Node[] block)
            : base(ref workQueue, ref renderQueue, ref blockOffset, block)
        {

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override ThreadSafeQueue<RenderLine> Generate(Node workNode)
        {
            Node child;
            int index = 0;
            foreach (int tt in workNode.Children)
            {
                child = _block[tt];
                child.Activation += workNode.ChildrenLines[index].strength;
                //don't need to reload nodes that are already in queue
                if (!child.IsLoaded)
                {
                    //experimental learning with child weights, doesn't work
                    //workNode.ChildrenLines[count].strength = Math.Min(1, (float)Math.Tan(workNode.ChildrenLines[count].strength));

                    if (child.Activation >= 1)
                    {
                        child.IsLoaded = true;
                        //add activated children to work queue
                        _clientWorkQueue.Enqueue(child);
                        //add lines to be rendered
                        _clientRenderQueue.Enqueue(workNode.ChildrenLines[index]);
                    }
                }
                index++;
            }
            workNode.Activation = 0;
            workNode.IsLoaded = false;
            return _clientRenderQueue;
        }
    }
}

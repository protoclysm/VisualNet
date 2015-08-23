using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VisualNet
{
    public class FiringWorkGenerator : WorkGenerator
    {
        public FiringWorkGenerator(ref ConcurrentQueue<Node> workQueue, ref ConcurrentQueue<RenderLine> renderQueue, Tuple<int, int, int> blockOffset, Node[] block)
            : base(ref workQueue, ref renderQueue, ref blockOffset, block)
        {

        }
        Node child;
        Random rr = new Random();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Generate(Node workNode)
        {            
            int index = 0;
            foreach (int tt in workNode.Children)
            {
                child = _block[tt];
                child.Activation += workNode.ChildrenLines[index].strength;
                //don't need to reload nodes that are already in queue
                if ( rr.Next(10)==1)
                {
                    //experimental learning with child weights, doesn't work


                    
                    if (child.Activation >= 1)
                    {
                        workNode.ChildrenLines[index].strength += workNode.Activation;

                        workNode.ChildrenLines[index].actionG = child.Activation;
                        _clientRenderQueue.Enqueue(workNode.ChildrenLines[index]);
                        //Thread.Sleep(1);

                        child.IsLoaded = true;
                        //add activated children to work queue
                        _clientWorkQueue.Enqueue(child);
                        //add lines to be rendered
                        
                    }
                }
                index++;
            }
        }
    }
}

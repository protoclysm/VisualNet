using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace VisualNet
{
    public class WorkManager
    {
        Node[] nodeCube;//each work manager owns its cube
        ConcurrentQueue<Node> _fireQueue = new ConcurrentQueue<Node>();
        public static ConcurrentQueue<RenderLine> _renderQueue;
        Random rr;
        Tuple<int, int, int> blockOffset;
        GLManager _glmanager;
        int blockWidth,
            blockHeight,
            blockDepth;
        protected float blockw, blockh, blockd;
        public bool working = false;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();
        [DllImport("kernel32.dll")]
        static extern IntPtr SetThreadAffinityMask(IntPtr hThread, IntPtr dwThreadAffinityMask);

        public WorkManager(int bwidth, int bheight, int bdepth, int offsetX, int offsetY, int offsetZ, GLManager glmanager)
        {
            rr = new Random(offsetX + offsetY + offsetZ);
            _glmanager = glmanager;
            _renderQueue = glmanager._renderQueue;
            blockOffset = new Tuple<int, int, int>(offsetX, offsetY, offsetZ);
            blockWidth = bwidth;
            blockHeight = bheight;
            blockDepth = bdepth;
            blockw = 1 / (float)blockWidth;
            blockh = 1 / (float)blockHeight;
            blockd = 1 / (float)blockDepth;
            nodeCube = new Node[blockWidth * blockHeight * blockDepth];

            //populate the cube with nodes
            for (short d = 0; d < blockDepth; d++)
            {
                for (short w = 0; w < blockWidth; w++)
                {
                    for (short h = 0; h < blockHeight; h++)
                    {
                        nodeCube[w + blockWidth * (h + blockHeight * d)] = new Node(w, h, d, (w + blockWidth * (h + blockHeight * d)));
                    }
                }
            }

            List<jsonConnection> jsonConnections;
            var serializer = new JsonSerializer();
            using (var re = File.OpenText("nodes.json"))
            using (var reader = new JsonTextReader(re))
            {
                jsonConnections = serializer.Deserialize<List<jsonConnection>>(reader);
            }

            //cycle through every node in the cube, creating all connections from json for each of them
            for (byte d = 0; d < blockDepth - 1; d++)
            {
                for (byte w = 0; w < blockWidth; w++)
                {
                    for (byte h = 0; h < blockHeight; h++)
                    {
                        Node p = nodeCube[(w) + blockWidth * ((h) + blockHeight * (d))];
                        foreach (jsonConnection conn in jsonConnections)
                        {
                            //check that the child of the connection is inside the cube
                            if (d + conn.z >= 0 && d + conn.z < blockDepth)
                            {
                                if (w + conn.x >= 0 && w + conn.x < blockWidth)
                                {
                                    if (h + conn.y >= 0 && h + conn.y < blockHeight)
                                    {
                                        Connect(p, nodeCube[(w + conn.x) + blockWidth * ((h + conn.y) + blockHeight * (d + conn.z))], conn.weight);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Connect(Node n, Node p, float weight)
        {
            n.Children.Add(p.Offset);
            n.ChildrenLines.Add(
                new RenderLine(
                    (n.X * blockw) + (blockOffset.Item1),
                    (n.Y * blockh) + (blockOffset.Item2),
                    (n.Z * blockd) + (blockOffset.Item3),
                    (p.X * blockw) + (blockOffset.Item1),
                    (p.Y * blockh) + (blockOffset.Item2),
                    (p.Z * blockd) + (blockOffset.Item3),
                    1 / (blockOffset.Item1 + 1),
                    1 / (blockOffset.Item2 + 1),
                    1 / (blockOffset.Item3 + 1),
                    weight
                    )
                );
        }
        //orchestrates
        public int WorkCycle(int procnum)
        {
            //not really sure if this does anything
            int processor = (int)procnum;
            Thread tr = Thread.CurrentThread;
            if (Environment.ProcessorCount > 1)
            {
                SetThreadAffinityMask(GetCurrentThread(),
                    new IntPtr(1 << processor));
            }
            working = true;

            //basic input for demo
            for (int w = blockWidth / 2; w < (blockWidth / 2) + 1; w++)
            {
                for (int h = blockHeight / 2; h < (blockHeight / 2) + 1; h++)
                {
                    //int vary =10;
                    //for (int w = 0; w < blockWidth - vary; w += vary)
                    //{
                    //    for (int h = 0; h < blockHeight - vary; h += vary)
                    //    {
                    //activate nodes on the first layer
                    nodeCube[(w) + blockWidth * ((h) + blockHeight * (0))].Activation = 1;
                    nodeCube[(w) + blockWidth * ((h) + blockHeight * (0))].IsLoaded = true;
                    //add them to the work queue
                    _fireQueue.Enqueue(nodeCube[(w) + blockWidth * ((h) + blockHeight * (0))]);
                }
            }
            for (int w = 0; w < blockWidth; w++)
            {

                nodeCube[(w) + blockWidth * ((blockWidth/2) + blockHeight * (blockDepth - 1))].Activation = 1;
                nodeCube[(w) + blockWidth * ((blockWidth / 2) + blockHeight * (blockDepth - 1))].IsLoaded = true;
                //add them to the work queue
                _fireQueue.Enqueue(nodeCube[(w) + blockWidth * ((blockWidth / 2) + blockHeight * (blockDepth - 1))]);

            }
            for (int h = 0; h < blockHeight; h++)
            {
                nodeCube[(blockHeight / 2) + blockWidth * ((h) + blockHeight * (blockDepth - 1))].Activation = 1;
                nodeCube[(blockHeight / 2) + blockWidth * ((h) + blockHeight * (blockDepth - 1))].IsLoaded = true;
                //add them to the work queue
                _fireQueue.Enqueue(nodeCube[(blockHeight / 2) + blockWidth * ((h) + blockHeight * (blockDepth - 1))]);
            }
            //This does the work. Cycles through the queue.
            //For each node, add connection weight to connection child,
            //return list of lines to be rendered, add them to glmananger queue
            int counter = 0;

            while (_fireQueue.Count > 0 && counter<1500000)
            {
                //counter = 0;
                Node workNode;
                //maybe add a threshold to node, and use that instead of 1
                if (_fireQueue.TryDequeue(out workNode) && workNode.IsLoaded && workNode.Activation >= 1)
                {
                    Generate(workNode);
                    workNode.IsLoaded = false;
                    workNode.Activation = 0;
                    counter++;
                } 
            }


            foreach (Node n in nodeCube)
            {
                n.Threshold += .0025f;
            }
            working = false;
            return counter;
        }
        Node child;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Generate(Node workNode)
        {
            int index = 0;
            foreach (int tt in workNode.Children)
            {
                child = nodeCube[tt];
                child.Activation += workNode.ChildrenLines[index].strength;
                //don't need to reload nodes that are already in queue

                //experimental learning with child weights, doesn't work
                if (child.Activation >= child.Threshold && !child.IsLoaded)
                {
                    //if (rr.Next(9) == 0)
                    {
                        workNode.ChildrenLines[index].strength += .025f;
                    } 
                    //if (rr.Next(5) == 0)
                    {
                        _renderQueue.Enqueue(workNode.ChildrenLines[index]);
                        
                    } 

                    workNode.ChildrenLines[index].actionG = child.Activation;

                    child.IsLoaded = true;
                    //add activated children to work queue
                    _fireQueue.Enqueue(child);
                    //add lines to be rendered
                }
                index++;
            }
        }
    }

    public class jsonConnection
    {
        [JsonProperty("x")]
        public int x { get; set; }
        [JsonProperty("y")]
        public int y { get; set; }
        [JsonProperty("z")]
        public int z { get; set; }
        [JsonProperty("weight")]
        public float weight { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
namespace VisualNet
{
    public class GLManager
    {
        public int blockWidth = int.Parse(ConfigurationManager.AppSettings["gcBlockWidth"]),
            blockHeight = int.Parse(ConfigurationManager.AppSettings["gcBlockHeight"]),
            blockDepth = int.Parse(ConfigurationManager.AppSettings["gcBlockDepth"]);
        float bx0, bx1, by0, by1, bz0, bz1, delta = 0;
        float blockw, blockh, blockd;
        int deltaX = -400, deltaY = -500;
        bool isdown = false;
        bool doRender = true;
        public OpenGL gl;
        ThreadSafeQueue<Queue<RenderLine>> lineQueues = new ThreadSafeQueue<Queue<RenderLine>>();
        ThreadSafeQueue<RenderLine> lineQueue = new ThreadSafeQueue<RenderLine>();
        public GLManager()
        {
            
            blockw = 1 / (float)blockWidth;
            blockh = 1 / (float)blockHeight;
            blockd = 1 / (float)blockDepth;
        }
        public void SetGL(OpenGL gl)
        {
            this.gl = gl;
        }
        public void AddQueue(ThreadSafeQueue<RenderLine> workQueue)
        {
                using (var locked = workQueue.Lock())
                {
                    while (locked.Count > 0)
                    {
                        lineQueue.Enqueue(locked.Dequeue());
                    }
                }
        }

        public void AllowRender()
        {
            doRender = true;
        }
        public void openGLCtrl1_OpenGLDraw(object sender, RenderEventArgs e)
        {
            if (doRender)
            {
                gl.ClearColor(.25f, .2f, .2f, 1);

                gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

                gl.LoadIdentity();
                gl.Translate(-.2f, -.2f, -3.25f + delta);

                gl.Rotate(deltaY, .5f, 0, 0);
                gl.Rotate(deltaX, 0f, .5f, 0);

                try
                {
                    gl.Color(1f, 1f, 1f);

                    gl.Enable(OpenGL.GL_LINE_SMOOTH);
                    gl.Enable(OpenGL.GL_BLEND);
                    gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                    gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_DONT_CARE);
                    gl.LineWidth(.25f);

                    //draw the containing box
                    gl.Begin(OpenGL.GL_LINES);

                    gl.Vertex(0, 0, 0);
                    gl.Vertex(0, 0, 1);
                    gl.Vertex(0, 0, 0);
                    gl.Vertex(1, 0, 0);
                    gl.Vertex(0, 0, 0);
                    gl.Vertex(0, 1, 0);
                    gl.Vertex(0, 1, 1);
                    gl.Vertex(0, 0, 1);
                    gl.Vertex(0, 1, 1);
                    gl.Vertex(0, 1, 0);
                    gl.Vertex(0, 1, 1);
                    gl.Vertex(1, 1, 1);
                    gl.Vertex(1, 1, 0);
                    gl.Vertex(1, 0, 0);
                    gl.Vertex(1, 1, 0);
                    gl.Vertex(0, 1, 0);
                    gl.Vertex(1, 1, 0);
                    gl.Vertex(1, 1, 1);
                    gl.Vertex(1, 0, 1);
                    gl.Vertex(0, 0, 1);
                    gl.Vertex(1, 0, 1);
                    gl.Vertex(1, 0, 0);
                    gl.Vertex(1, 0, 1);
                    gl.Vertex(1, 1, 1);
                    gl.End();


                    gl.Color(0f, .9f, (float)1);
                    gl.Begin(OpenGL.GL_LINES);
                    gl.Vertex(0, 0, 0);
                    gl.Vertex(0, 0, 100);
                    gl.Vertex(0, 0, 0);
                    gl.Vertex(0, 100, 0);
                    gl.Vertex(0, 0, 0);
                    gl.Vertex(100, 0, 0);
                    gl.End();

                    gl.LineWidth(.15f);

                    gl.Begin(OpenGL.GL_LINES);
                    int renderCounterL = 0;

                    //draw the lines for each firing path
                    RenderLine cq;
                    using (var locked = lineQueue.Lock())
                    {
                        while (locked.Count > 0)
                        {
                            cq = locked.Dequeue();

                            if (cq != null)
                            {
                                bx0 = cq.xS;
                                bx1 = cq.xV;
                                by0 = cq.yS;
                                by1 = cq.yV;
                                bz0 = cq.zS;
                                bz1 = cq.zV;

                                gl.Color(cq.actionR, cq.actionG, cq.actionB);

                                gl.Vertex(bx0, by0, bz0);
                                gl.Vertex(bx1, by1, bz1);
                            }
                            renderCounterL++;
                        }
                        lineQueue.Clear();
                    }
                    gl.End();
                }
                catch { }
                doRender = false;
            }
        }
        public void openGLCtrl1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                delta = delta + .31f;
            else
                delta = delta - .31f;
        }
        public void openGLCtrl1_MouseDown(object sender, MouseEventArgs e)
        {
            isdown = true;
        }

        public void openGLCtrl1_MouseUp(object sender, MouseEventArgs e)
        {
            isdown = false;
        }
        public void openGLCtrl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isdown)
            {
                deltaX = -e.X;
                deltaY = -e.Y;
            }
        }
        public void openGLCtrl1_OpenGLDblClick(object sender, MouseEventArgs e)
        {

        }
    }
}

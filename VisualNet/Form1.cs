using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace VisualNet
{
    public partial class Form1 : Form
    {
        public System.Timers.Timer cycleTimer = new System.Timers.Timer();
        public System.Timers.Timer opTimer = new System.Timers.Timer();
        List<WorkManager> managers = new List<WorkManager>();
        Task[] taskArray;
        int OpCounter = 0;
        public static bool isThreading = false;
        public GLManager glmanager;

        int blockWidth = int.Parse(ConfigurationManager.AppSettings["gcBlockWidth"]),
                        blockHeight = int.Parse(ConfigurationManager.AppSettings["gcBlockHeight"]),
                        blockDepth = int.Parse(ConfigurationManager.AppSettings["gcBlockDepth"]),
                        blockCountX = int.Parse(ConfigurationManager.AppSettings["blockCountX"]),
                        blockCountY = int.Parse(ConfigurationManager.AppSettings["blockCountY"]),
                        blockCountZ = int.Parse(ConfigurationManager.AppSettings["blockCountZ"]);

        public Form1()
        {
            glmanager = new GLManager();

            InitializeComponent();
            glmanager.SetGL(this.openGLCtrl1.OpenGL);
            cycleTimer.Elapsed += new ElapsedEventHandler(DisplayTimeEvent);
            cycleTimer.Interval = 1;
            opTimer.Elapsed += new ElapsedEventHandler(OpTimeEvent);
            opTimer.Interval = 1000;

            for (int d = 0; d < blockCountZ; d++)
            {
                for (int w = 0; w < blockCountX; w++)
                {
                    for (int h = 0; h < blockCountY; h++)
                    {
                        managers.Add(new WorkManager(blockWidth, blockHeight, blockDepth, w, h, d, glmanager));
                    }
                }
            }

            taskArray = new Task[managers.Count];

            cycleTimer.Start();
            opTimer.Start();
        }

        public void DisplayTimeEvent(object source, ElapsedEventArgs e)
        {
            int returnValue = 0;
            if (!isThreading)
            {
                isThreading = true;
                int count = 0;
                int procnum = 0;

                foreach (WorkManager man in managers)
                {
                    taskArray[count] = Task.Factory.StartNew(() => { returnValue += man.WorkCycle(procnum); });
                    //for thread affinity, which I never got to work quite right
                    procnum++;
                    count++;
                }
                Task.WaitAll(taskArray);
                glmanager.AllowRender();
                OpCounter += returnValue;
                isThreading = false;
            }
        }
        public void OpTimeEvent(object source, ElapsedEventArgs e)
        {
            //display how many nodes were processed this second
            try
            {
                textBox1.Invoke(new MethodInvoker(delegate
                {
                    textBox1.Text = string.Format("{0:n0}", OpCounter);
                    textBox2.Text = DateTime.Now.ToLongTimeString();
                    OpCounter = 0;
                }));
            }
            catch { }
        }

    }
}

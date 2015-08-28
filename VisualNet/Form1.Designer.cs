namespace VisualNet
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.openGLCtrl1 = new SharpGL.OpenGLControl();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.openGLCtrl1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1126, 58);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(153, 20);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1126, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(153, 20);
            this.textBox2.TabIndex = 1;
            // 
            // openGLCtrl1
            // 

            this.openGLCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openGLCtrl1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.openGLCtrl1.DrawFPS = false;
            this.openGLCtrl1.FrameRate = 999;
            this.openGLCtrl1.Location = new System.Drawing.Point(12, 12);
            this.openGLCtrl1.Name = "openGLCtrl1";
            this.openGLCtrl1.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL4_4;
            this.openGLCtrl1.RenderContextType = SharpGL.RenderContextType.DIBSection;
            //ignore and continue
            this.openGLCtrl1.OpenGLDraw += new SharpGL.RenderEventHandler(glmanager.openGLCtrl1_OpenGLDraw);
            this.openGLCtrl1.MouseDown += new System.Windows.Forms.MouseEventHandler(glmanager.openGLCtrl1_MouseDown);
            this.openGLCtrl1.MouseMove += new System.Windows.Forms.MouseEventHandler(glmanager.openGLCtrl1_MouseMove);
            this.openGLCtrl1.MouseUp += new System.Windows.Forms.MouseEventHandler(glmanager.openGLCtrl1_MouseUp);
            this.openGLCtrl1.MouseWheel += new System.Windows.Forms.MouseEventHandler(glmanager.openGLCtrl1_MouseWheel);
            this.openGLCtrl1.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLCtrl1.Size = new System.Drawing.Size(1101, 702);
            this.openGLCtrl1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1126, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ops per second";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1304, 726);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.openGLCtrl1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.openGLCtrl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        public SharpGL.OpenGLControl openGLCtrl1;

        private System.Windows.Forms.Label label1;
    }
}


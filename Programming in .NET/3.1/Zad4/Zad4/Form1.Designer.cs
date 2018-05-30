namespace Zad4
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
            this.tbxValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxMin = new System.Windows.Forms.TextBox();
            this.tbxMax = new System.Windows.Forms.TextBox();
            this.Refresh = new System.Windows.Forms.Button();
            this.btnAnimate = new System.Windows.Forms.Button();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.customProgressBar = new Zad4.CustomProgressBar();
            this.SuspendLayout();
            // 
            // tbxValue
            // 
            this.tbxValue.Location = new System.Drawing.Point(59, 107);
            this.tbxValue.Name = "tbxValue";
            this.tbxValue.Size = new System.Drawing.Size(113, 20);
            this.tbxValue.TabIndex = 2;
            this.tbxValue.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Wartosc:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Min:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Max:";
            // 
            // tbxMin
            // 
            this.tbxMin.Location = new System.Drawing.Point(59, 136);
            this.tbxMin.Name = "tbxMin";
            this.tbxMin.Size = new System.Drawing.Size(113, 20);
            this.tbxMin.TabIndex = 6;
            this.tbxMin.Text = "0";
            // 
            // tbxMax
            // 
            this.tbxMax.Location = new System.Drawing.Point(59, 167);
            this.tbxMax.Name = "tbxMax";
            this.tbxMax.Size = new System.Drawing.Size(113, 20);
            this.tbxMax.TabIndex = 7;
            this.tbxMax.Text = "100";
            // 
            // Refresh
            // 
            this.helpProvider1.SetHelpKeyword(this.Refresh, "1");
            this.helpProvider1.SetHelpNavigator(this.Refresh, System.Windows.Forms.HelpNavigator.TopicId);
            this.Refresh.Location = new System.Drawing.Point(97, 193);
            this.Refresh.Name = "Refresh";
            this.helpProvider1.SetShowHelp(this.Refresh, true);
            this.Refresh.Size = new System.Drawing.Size(75, 23);
            this.Refresh.TabIndex = 8;
            this.Refresh.Text = "Refresh";
            this.Refresh.UseVisualStyleBackColor = true;
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // btnAnimate
            // 
            this.helpProvider1.SetHelpKeyword(this.btnAnimate, "2");
            this.helpProvider1.SetHelpNavigator(this.btnAnimate, System.Windows.Forms.HelpNavigator.TopicId);
            this.btnAnimate.Location = new System.Drawing.Point(97, 222);
            this.btnAnimate.Name = "btnAnimate";
            this.helpProvider1.SetShowHelp(this.btnAnimate, true);
            this.btnAnimate.Size = new System.Drawing.Size(75, 23);
            this.btnAnimate.TabIndex = 9;
            this.btnAnimate.Text = "Animacja";
            this.btnAnimate.UseVisualStyleBackColor = true;
            this.btnAnimate.Click += new System.EventHandler(this.button1_Click);
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "C:\\Users\\Satyr\\Documents\\Visual Studio 2015\\Projects\\Win32 - uczelnia\\3.1\\Zad3\\Za" +
    "d3\\bin\\Debug\\3.chm";
            // 
            // customProgressBar
            // 
            this.customProgressBar.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.customProgressBar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.customProgressBar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.customProgressBar.Location = new System.Drawing.Point(12, 27);
            this.customProgressBar.Max = 100;
            this.customProgressBar.Min = 0;
            this.customProgressBar.Name = "customProgressBar";
            this.customProgressBar.Size = new System.Drawing.Size(260, 32);
            this.customProgressBar.TabIndex = 0;
            this.customProgressBar.Value = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnAnimate);
            this.Controls.Add(this.Refresh);
            this.Controls.Add(this.tbxMax);
            this.Controls.Add(this.tbxMin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxValue);
            this.Controls.Add(this.customProgressBar);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomProgressBar customProgressBar;
        private System.Windows.Forms.TextBox tbxValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxMin;
        private System.Windows.Forms.TextBox tbxMax;
        private System.Windows.Forms.Button Refresh;
        private System.Windows.Forms.Button btnAnimate;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}


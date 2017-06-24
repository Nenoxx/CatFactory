namespace Cat_Factory
{
    partial class HelpWindow
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
            this.GotItFam = new System.Windows.Forms.PictureBox();
            this.Instructions = new System.Windows.Forms.PictureBox();
            this.Background = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.GotItFam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Instructions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Background)).BeginInit();
            this.SuspendLayout();
            // 
            // GotItFam
            // 
            this.GotItFam.Image = global::Cat_Factory.Properties.Resources.OKButton;
            this.GotItFam.Location = new System.Drawing.Point(218, 369);
            this.GotItFam.Name = "GotItFam";
            this.GotItFam.Size = new System.Drawing.Size(167, 105);
            this.GotItFam.TabIndex = 2;
            this.GotItFam.TabStop = false;
            this.GotItFam.Click += new System.EventHandler(this.GotItFam_Click);
            // 
            // Instructions
            // 
            this.Instructions.Image = global::Cat_Factory.Properties.Resources.Instructions;
            this.Instructions.Location = new System.Drawing.Point(104, 36);
            this.Instructions.Name = "Instructions";
            this.Instructions.Size = new System.Drawing.Size(405, 327);
            this.Instructions.TabIndex = 1;
            this.Instructions.TabStop = false;
            // 
            // Background
            // 
            this.Background.Image = global::Cat_Factory.Properties.Resources.Background;
            this.Background.Location = new System.Drawing.Point(2, 1);
            this.Background.Name = "Background";
            this.Background.Size = new System.Drawing.Size(651, 484);
            this.Background.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Background.TabIndex = 0;
            this.Background.TabStop = false;
            // 
            // HelpWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 486);
            this.Controls.Add(this.GotItFam);
            this.Controls.Add(this.Instructions);
            this.Controls.Add(this.Background);
            this.Name = "HelpWindow";
            this.Text = "Need halp with teh kitties?";
            ((System.ComponentModel.ISupportInitialize)(this.GotItFam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Instructions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Background)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Background;
        private System.Windows.Forms.PictureBox Instructions;
        private System.Windows.Forms.PictureBox GotItFam;
    }
}
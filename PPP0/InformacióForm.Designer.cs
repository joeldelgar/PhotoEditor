namespace ImagenLib
{
    partial class InformacióForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformacióForm));
            this.FotoPB = new System.Windows.Forms.PictureBox();
            this.InformacióTB = new System.Windows.Forms.RichTextBox();
            this.NomLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.FotoPB)).BeginInit();
            this.SuspendLayout();
            // 
            // FotoPB
            // 
            this.FotoPB.Image = ((System.Drawing.Image)(resources.GetObject("FotoPB.Image")));
            this.FotoPB.Location = new System.Drawing.Point(13, 13);
            this.FotoPB.Margin = new System.Windows.Forms.Padding(4);
            this.FotoPB.Name = "FotoPB";
            this.FotoPB.Size = new System.Drawing.Size(586, 457);
            this.FotoPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.FotoPB.TabIndex = 0;
            this.FotoPB.TabStop = false;
            // 
            // InformacióTB
            // 
            this.InformacióTB.Location = new System.Drawing.Point(607, 13);
            this.InformacióTB.Margin = new System.Windows.Forms.Padding(4);
            this.InformacióTB.Name = "InformacióTB";
            this.InformacióTB.ReadOnly = true;
            this.InformacióTB.Size = new System.Drawing.Size(633, 564);
            this.InformacióTB.TabIndex = 1;
            this.InformacióTB.Text = resources.GetString("InformacióTB.Text");
            // 
            // NomLbl
            // 
            this.NomLbl.AutoSize = true;
            this.NomLbl.Location = new System.Drawing.Point(13, 485);
            this.NomLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NomLbl.Name = "NomLbl";
            this.NomLbl.Size = new System.Drawing.Size(230, 92);
            this.NomLbl.TabIndex = 2;
            this.NomLbl.Text = "Membres:\r\nAlbert Compte Prades\r\nJoel Compte Prades\r\nJoel Delgado Garrido\r\n";
            // 
            // InformacióForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1261, 596);
            this.Controls.Add(this.NomLbl);
            this.Controls.Add(this.InformacióTB);
            this.Controls.Add(this.FotoPB);
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "InformacióForm";
            this.Text = "InformacióForm";
            this.Load += new System.EventHandler(this.InformacióForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FotoPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox FotoPB;
        private System.Windows.Forms.RichTextBox InformacióTB;
        private System.Windows.Forms.Label NomLbl;
    }
}
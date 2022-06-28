namespace Samurai
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileOpenVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileOpenASS = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdVideo = new System.Windows.Forms.OpenFileDialog();
            this.ofdASS = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileOpenVideo,
            this.mnuFileOpenASS});
            this.mnuFile.Name = "mnuFile";
            resources.ApplyResources(this.mnuFile, "mnuFile");
            // 
            // mnuFileOpenVideo
            // 
            this.mnuFileOpenVideo.Name = "mnuFileOpenVideo";
            resources.ApplyResources(this.mnuFileOpenVideo, "mnuFileOpenVideo");
            this.mnuFileOpenVideo.Click += new System.EventHandler(this.mnuFileOpenVideo_Click);
            // 
            // mnuFileOpenASS
            // 
            this.mnuFileOpenASS.Name = "mnuFileOpenASS";
            resources.ApplyResources(this.mnuFileOpenASS, "mnuFileOpenASS");
            this.mnuFileOpenASS.Click += new System.EventHandler(this.mnuFileOpenASS_Click);
            // 
            // ofdVideo
            // 
            this.ofdVideo.FileName = "openFileDialog1";
            // 
            // ofdASS
            // 
            this.ofdASS.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuFileOpenVideo;
        private System.Windows.Forms.OpenFileDialog ofdVideo;
        private System.Windows.Forms.ToolStripMenuItem mnuFileOpenASS;
        private System.Windows.Forms.OpenFileDialog ofdASS;
    }
}


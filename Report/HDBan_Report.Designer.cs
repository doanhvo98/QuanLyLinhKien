namespace QuanLyLinhKien.Report
{
    partial class HDBan_Report
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
            this.crvHDBan = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crvHDBan
            // 
            this.crvHDBan.ActiveViewIndex = -1;
            this.crvHDBan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crvHDBan.Cursor = System.Windows.Forms.Cursors.Default;
            this.crvHDBan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crvHDBan.Location = new System.Drawing.Point(0, 0);
            this.crvHDBan.Name = "crvHDBan";
            this.crvHDBan.Size = new System.Drawing.Size(854, 544);
            this.crvHDBan.TabIndex = 0;
            // 
            // HDBan_Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 544);
            this.Controls.Add(this.crvHDBan);
            this.Name = "HDBan_Report";
            this.Text = "HDBan_Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.HDBan_Report_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crvHDBan;
    }
}
namespace QuanLyLinhKien.Report
{
    partial class HDNhap_Report
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
            this.crv_HDNhap = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crv_HDNhap
            // 
            this.crv_HDNhap.ActiveViewIndex = -1;
            this.crv_HDNhap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crv_HDNhap.Cursor = System.Windows.Forms.Cursors.Default;
            this.crv_HDNhap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crv_HDNhap.Location = new System.Drawing.Point(0, 0);
            this.crv_HDNhap.Name = "crv_HDNhap";
            this.crv_HDNhap.Size = new System.Drawing.Size(1049, 564);
            this.crv_HDNhap.TabIndex = 0;
            // 
            // HDNhap_Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 564);
            this.Controls.Add(this.crv_HDNhap);
            this.Name = "HDNhap_Report";
            this.Text = "HDNhap_Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.HDNhap_Report_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crv_HDNhap;
    }
}
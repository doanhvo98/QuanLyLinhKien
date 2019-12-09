using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyLinhKien.Report
{
    public partial class HDBan_Report : Form
    {
        public HDBan_Report()
        {
            InitializeComponent();
            Report.HDBan hdb = new Report.HDBan();
            crvHDBan.ReportSource = hdb;
            crvHDBan.Refresh();
        }

        private void HDBan_Report_Load(object sender, EventArgs e)
        {
            
        }
    }
}

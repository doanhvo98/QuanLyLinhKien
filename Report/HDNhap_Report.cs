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
    public partial class HDNhap_Report : Form
    {
        public HDNhap_Report()
        {
            InitializeComponent();
            HDNhap hdn = new HDNhap();
            crv_HDNhap.ReportSource = hdn;
            crv_HDNhap.Refresh();
            crv_HDNhap.ResetText();
        }

        private void HDNhap_Report_Load(object sender, EventArgs e)
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyLinhKien.Report
{
    public partial class HoaDonBan_Report : Form
    {
        SqlConnection conn;
        DataSet ds = new DataSet();
        DataTable dtKHChon;
        DataTable dtSoHD;
        String soHD;
        public HoaDonBan_Report(String soHD)
        {
            InitializeComponent();
            this.soHD = soHD;

            ReadFileConn f = new ReadFileConn();
            String s = f.LoadConn();
            conn = new SqlConnection(s);
        }
        public HoaDonBan_Report()
        {
            InitializeComponent();
            ReadFileConn f = new ReadFileConn();
            String s = f.LoadConn();
            conn = new SqlConnection(s);
        }

        private void HoaDonBan_Report_Load(object sender, EventArgs e)
        {
            if (soHD == null)
            {
                groupInHD.Visible = true;
                loadSoHDNULL();
               
            }
            else
            {
                chbTim.Checked = false;
                groupInHD.Visible = false;

                CrystalReport_HoaDonBan crp = new CrystalReport_HoaDonBan();
                crp.SetParameterValue("LocSHD", soHD);
                crvHDB.ReportSource = crp;
                crvHDB.Refresh();
            }
            
        }
        private void loadSoHDNULL()
        {
            //------------------------------ Bảng khách hàng -----------------------------------
            String selectKH = "select * from KH";
            SqlDataAdapter daKH = new SqlDataAdapter(selectKH, conn);
            daKH.Fill(ds, "KH");

            DataColumn[] key = new DataColumn[1];
            key[0] = ds.Tables["KH"].Columns[0];
            ds.Tables["KH"].PrimaryKey = key;

            //------------------------------ Bảng Hóa Đơn Bán ----------------------------------
            String selectHDB = "select * from HDBan";
            SqlDataAdapter daHDB = new SqlDataAdapter(selectHDB, conn);
            daHDB.Fill(ds, "HDBan");

            //----------------------------- Bảng Khách hàng chon -----------------------------
            dtKHChon = new DataTable();
            DataColumn col1 = new DataColumn("MaKH");
            DataColumn col2 = new DataColumn("TenKH");
            dtKHChon.Columns.Add(col1);
            dtKHChon.Columns.Add(col2);

            dtSoHD = new DataTable();
        }

        private void chbTim_CheckedChanged(object sender, EventArgs e)
        {
            if (chbTim.Checked)
            {
                groupInHD.Visible = true;
                if(soHD != null)
                {
                    loadSoHDNULL();
                }
                
            }
            else
                groupInHD.Visible = false;
            
        }

        

        private void txtTimTen_KeyUp(object sender, KeyEventArgs e)
        {
            if (dtKHChon.Rows.Count > 0)
                dtKHChon.Rows.Clear();
            string tenTim = txtTimTen.Text.Trim().ToLower();

            for (int i = 0; i < ds.Tables["KH"].Rows.Count; i++)
            {
                if (ds.Tables["KH"].Rows[i][1].ToString().Trim().ToLower().Contains(tenTim))
                {
                    DataRow drKHChon = dtKHChon.NewRow();
                    drKHChon[0] = ds.Tables["KH"].Rows[i]["MaKH"].ToString();
                    drKHChon[1] = ds.Tables["KH"].Rows[i]["TenKH"].ToString();

                    dtKHChon.Rows.Add(drKHChon);
                }
            }

            cboTenKH.DataSource = dtKHChon;
            cboTenKH.DisplayMember = "TenKH";
            cboTenKH.ValueMember = "MaKH";

            loadSDT();
            loadSoHD();

        }
        private void loadSDT()
        {
            if (dtKHChon.Rows.Count > 0)
            {
                DataRow drSDT = ds.Tables["KH"].Rows.Find(cboTenKH.SelectedValue.ToString());
                if (drSDT != null)
                {
                    txtSDT.Text = drSDT["SDT"].ToString();
                }
            }
        }
        private void cboTenKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadSDT();
            loadSoHD();

        }
        private void loadSoHD()
        { 
            if(dtKHChon.Rows.Count >0)
            {
                if(dtSoHD.Rows.Count > 0)
                   dtSoHD.Rows.Clear();
                String selectSoHD = "select * from HDBan where MaKH = '" + cboTenKH.SelectedValue.ToString() + "'";
                SqlDataAdapter daSoHD = new SqlDataAdapter(selectSoHD, conn);
                daSoHD.Fill(dtSoHD);

                cboSoHD.DataSource = dtSoHD;
                cboSoHD.DisplayMember = "MaHDBan";
                cboSoHD.ValueMember = "MaHDBan";
            }
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            if (dtSoHD.Rows.Count > 0)
            {
                CrystalReport_HoaDonBan crp = new CrystalReport_HoaDonBan();
                crp.SetParameterValue("LocSHD", cboSoHD.SelectedValue.ToString());
                crvHDB.ReportSource = crp;
                crvHDB.Refresh();
            }
            else
            {
                MessageBox.Show("Bạn Chưa Chọn Số Hóa Đơn", "Thông Báo");
                return;
            }
        }

    }
}

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

namespace QuanLyLinhKien
{
    public partial class QLKho : Form
    {
        DataTable dt;
        DataSet ds;
        SqlDataAdapter da;
        SqlConnection conn = new SqlConnection(@"data source = KMA0WZ8TM93EHCQ\SQLEXPRESS; Initial Catalog = QL_ThietBiMayTinh; Integrated Security = True");
        DataView dv;

        public QLKho()
        {
            InitializeComponent();
        }

        private void QLKho_Load(object sender, EventArgs e)
        {
            load_cboDanhMucSP();
            cboDanhMucSP.SelectedIndex = 0;
            dgvKho_DanhMuc.ReadOnly = true;
            panel3.Enabled = false;

            dgvKho_DanhMuc.Columns["MaSP"].Width = 120;
            dgvKho_DanhMuc.Columns["TenSP"].Width = 400;
            dgvKho_DanhMuc.Columns["MaNCC"].Width = 100;
            dgvKho_DanhMuc.Columns["MaDanhMuc"].Width = 150;
            dgvKho_DanhMuc.Columns["GiaNhap"].Width = 180;
            dgvKho_DanhMuc.Columns["GiaBan"].Width = 180;
            dgvKho_DanhMuc.Columns["SoLuong"].Width = 120;

        }

        private void QLKho_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conn.State.ToString() == "Open")
            {
                conn.Close();
            }
        }
        //----------------------- cboDanhMucSP ------------------
        //Load danh muc sản phẩm
        void load_cboDanhMucSP()
        {
            dt = new DataTable();
            string srtSelect = "Select * from DanhMucSP";
            da = new SqlDataAdapter(srtSelect, conn);
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr[0] = "0";
            dr[1] = "Tất cả";
            dt.Rows.InsertAt(dr, 0);
            cboDanhMucSP.DataSource = dt;
            cboDanhMucSP.DisplayMember = "TenDanhMuc";
            cboDanhMucSP.ValueMember = "MaDanhMuc";
        }

        //Khi click "Tất cả danh mục" sẽ hiện tất cả sản phẩm có trong kho xuống dataGirdView
        void load_dgvAllSP_DanhMuc()
        {
            DataTable dt = new DataTable();
            da = new SqlDataAdapter("select MaSP, TenSP, SP.MaNCC, MaDanhMuc, GiaNhap, GiaBan, SoLuong from SP", conn);
            da.Fill(dt);
            dv = new DataView(dt);
            dgvKho_DanhMuc.DataSource = dv;
        }

        private void cboDanhMucSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDanhMucSP.SelectedIndex == 0)
            {

                load_dgvAllSP_DanhMuc();
                cboTenSP.Enabled = false;
                cboTenSP.SelectedIndex = -1;
            }
            else
            {
                load_cboTenSP();
                cboTenSP.SelectedIndex = 0;
                cboTenSP.Enabled = true;
            }
        }
        //----------------------- cboTenSP -------------------------
        //Load tên sản phẩm
        void load_cboTenSP()
        {
            dt = new DataTable();
            string strSelect = "Select MaSP, TenSP, MaNCC, MaDanhMuc, GiaNhap, GiaBan, SoLuong from SP where MaDanhMuc = '" + cboDanhMucSP.SelectedValue.ToString() + "'";
            da = new SqlDataAdapter(strSelect, conn);
            da.Fill(dt);

            //dgvKho_DanhMuc.DataSource = dt;
            DataRow dr = dt.NewRow();
            dr[0] = "0";
            dr[1] = "Tất cả sản phẩm";
            dt.Rows.InsertAt(dr, 0);
            cboTenSP.DataSource = dt;
            cboTenSP.DisplayMember = "TenSP";
            cboTenSP.ValueMember = "MaSP";
            cboTenSP.SelectedIndex = 0;
        }

        //Khi click "Tất cả sản phẩm" sẽ hiện tất cả sản phẩm thuộc danh mục được chọn xuống dataGridView
        void load_dgvAllSP_TenSP()
        {
            ds = new DataSet();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter("select * from DanhMucSP", conn);
            da.Fill(ds, "DanhMucSP");
            string s = cboDanhMucSP.Text;
            string ma = "";
            for (int i = 0; i < ds.Tables["DanhMucSP"].Rows.Count; i++)
            {
                if (s == ds.Tables["DanhMucSP"].Rows[i][1].ToString())
                {
                    ma = ds.Tables["DanhMucSP"].Rows[i][0].ToString().Trim();
                }
            }
            string str = "select MaSP, TenSP, SP.MaNCC, MaDanhMuc, GiaNhap, GiaBan, SoLuong from SP where MaDanhMuc = '" + ma + "'";
            SqlDataAdapter da_danhMuc = new SqlDataAdapter(str, conn);
            da_danhMuc.Fill(dt);
            dgvKho_DanhMuc.DataSource = dt;
        }

        private void cboTenSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDanhMucSP.SelectedIndex > 0)
            {
                if (cboTenSP.SelectedIndex == 0)
                {
                    load_dgvAllSP_TenSP();
                }
                else
                {
                    dt = new DataTable();
                    string strSelect = "Select MaSP, TenSP, SP.MaNCC, MaDanhMuc, GiaNhap, GiaBan, SoLuong from SP where MaSP = '" + cboTenSP.SelectedValue.ToString() + "'";
                    da = new SqlDataAdapter(strSelect, conn);
                    da.Fill(dt);

                    dgvKho_DanhMuc.DataSource = dt;
                }
            }
        }
        //======================= Tìm SP theo Nhà cung cấp ======================
        private void ckbTimTheoTenNCC_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbTimTheoTenNCC.Checked)
            {
                load_cboTenNCC();
                panel1.Enabled = false;
                panel3.Enabled = true;
                dgvKho_DanhMuc.ClearSelection();
            }
            else
            {
                panel1.Enabled = true;
                panel3.Enabled = false;
                dgvKho_NCC.ClearSelection();
            }
        }
        void load_cboTenNCC()
        {
            dt = new DataTable();
            string str = "select * from NCC";
            da = new SqlDataAdapter(str, conn);
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr[0] = "0";
            dr[1] = "Tất cả nhà cung cấp";
            dt.Rows.InsertAt(dr, 0);
            cboTenNCC.DataSource = dt;
            cboTenNCC.DisplayMember = "TenNCC";
            cboTenNCC.ValueMember = "MaNCC";
        }

        private void cboTenNCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt = new DataTable();
            string str = "select MaSP, TenSP, NCC.MaNCC, MaDanhMuc, GiaNhap, GiaBan, SoLuong from SP, NCC where NCC.MaNCC = SP.MaNCC";
            da = new SqlDataAdapter(str, conn);
            da.Fill(dt);
            dv = new DataView(dt);
            dgvKho_NCC.DataSource = dv;
            if (cboTenNCC.SelectedIndex > 0)
            {
                dv.RowFilter = "MaNCC = '" + cboTenNCC.SelectedValue.ToString() + "'";
            }
            dgvKho_NCC.Columns["MaSP"].Width = 120;
            dgvKho_NCC.Columns["TenSP"].Width = 400;
            dgvKho_NCC.Columns["MaNCC"].Width = 100;
            dgvKho_NCC.Columns["MaDanhMuc"].Width = 150;
            dgvKho_NCC.Columns["GiaNhap"].Width = 180;
            dgvKho_NCC.Columns["GiaBan"].Width = 180;
            dgvKho_NCC.Columns["SoLuong"].Width = 120;
        }
        



    }
}

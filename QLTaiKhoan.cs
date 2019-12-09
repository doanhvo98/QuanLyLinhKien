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
    
    public partial class QLTaiKhoan : Form
    {
        SqlConnection conn;
        DataSet ds;

        DataTable dtTaiKhoan;

        SqlCommandBuilder cmb;

        SqlDataAdapter daTK;

        public QLTaiKhoan()
        {
            InitializeComponent();

            ReadFileConn f = new ReadFileConn();
            String s = f.LoadConn();
            conn = new SqlConnection(s);
        }

        private void QLTaiKhoan_Load(object sender, EventArgs e)
        {
            
            ds = new DataSet();

            

            // --------------------------------- Bảng tài khoản ------------------------------------
            String selectTK = "select * from TaiKhoan";
            
            daTK = new SqlDataAdapter(selectTK, conn);
            daTK.Fill(ds, "TaiKhoan");

           

            DataColumn[] keyTK = new DataColumn[2];
            keyTK[0] = ds.Tables["TaiKhoan"].Columns[0];
            keyTK[1] = ds.Tables["TaiKhoan"].Columns[2];
            ds.Tables["TaiKhoan"].PrimaryKey = keyTK;

            // -------------------------------- Bảng Nhân Viên --------------------------------
            String selectNV = "select * from NV";
            SqlDataAdapter daNV = new SqlDataAdapter(selectNV, conn);
            daNV.Fill(ds, "NV");

            DataColumn[] keyNV = new DataColumn[1];
            keyNV[0] = ds.Tables["NV"].Columns[0];
            ds.Tables["NV"].PrimaryKey = keyNV;

            cboNhanVien.DataSource = ds.Tables["NV"];
            cboNhanVien.DisplayMember = "TenNV";
            cboNhanVien.ValueMember = "MaNV";
            // -------------------------------- Bảng dtTaiKhoan --------------------------------

            dtTaiKhoan = new DataTable();
            DataColumn col1 = new DataColumn("Tên Đăng Nhập");
            DataColumn col2 = new DataColumn("Mật Khẩu");
            DataColumn col3 = new DataColumn("Tên Nhân Viên");
            DataColumn col4 = new DataColumn("Chức Vụ");
            DataColumn col5 = new DataColumn("Quyền");

            dtTaiKhoan.Columns.Add(col1);
            dtTaiKhoan.Columns.Add(col2);
            dtTaiKhoan.Columns.Add(col3);
            dtTaiKhoan.Columns.Add(col4);
            dtTaiKhoan.Columns.Add(col5);

            DataColumn[] keyTaiKhoan = new DataColumn[1];
            keyTaiKhoan[0] = dtTaiKhoan.Columns[0];
            dtTaiKhoan.PrimaryKey = keyTaiKhoan;

            // load tài khoản vào datagridview
            loadDagridView();

            dtgTaiKhoan.DataSource = dtTaiKhoan;
            dtgTaiKhoan.Columns["Tên Đăng Nhập"].Width = 150;
            dtgTaiKhoan.Columns["Mật Khẩu"].Width = 150;
            dtgTaiKhoan.Columns["Tên Nhân Viên"].Width = 250;
            dtgTaiKhoan.Columns["Chức Vụ"].Width = 170;
            dtgTaiKhoan.Columns["Quyền"].Width = 170;

            dtgTaiKhoan.AllowUserToAddRows = false;
            dtgTaiKhoan.ReadOnly = true;
           

        }
        private void loadDagridView()
        {
            for (int i = 0; i < ds.Tables["TaiKhoan"].Rows.Count; i++)
            {
                DataRow drTaiKhoan = dtTaiKhoan.NewRow();
                drTaiKhoan["Tên Đăng Nhập"] = ds.Tables["TaiKhoan"].Rows[i][0].ToString();
                drTaiKhoan["Mật Khẩu"] = ds.Tables["TaiKhoan"].Rows[i][1].ToString();

                DataRow drNV = ds.Tables["NV"].Rows.Find(ds.Tables["TaiKhoan"].Rows[i][2].ToString());
                drTaiKhoan["Tên Nhân Viên"] = drNV[1];
                drTaiKhoan["Chức Vụ"] = drNV[2];
                drTaiKhoan["Quyền"] = ds.Tables["TaiKhoan"].Rows[i][3].ToString();

                dtTaiKhoan.Rows.Add(drTaiKhoan);
            }
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            chbTaiKhoan.Checked = false;
            dtTaiKhoan.Rows.Clear();
            
                for (int i = 0; i < ds.Tables["TaiKhoan"].Rows.Count; i++)
                {
                    string s1 = txtTuKhoa.Text.Trim().ToLower();
                    String s2 = ds.Tables["TaiKhoan"].Rows[i][0].ToString().Trim().ToLower();
                    if (s2.Contains(s1))
                    {
                        DataRow drTaiKhoan = dtTaiKhoan.NewRow();
                        drTaiKhoan["Tên Đăng Nhập"] = ds.Tables["TaiKhoan"].Rows[i][0].ToString();
                        drTaiKhoan["Mật Khẩu"] = ds.Tables["TaiKhoan"].Rows[i][1].ToString();

                        DataRow drNV = ds.Tables["NV"].Rows.Find(ds.Tables["TaiKhoan"].Rows[i][2].ToString());
                        drTaiKhoan["Tên Nhân Viên"] = drNV[1];
                        drTaiKhoan["Chức Vụ"] = drNV[2];
                        drTaiKhoan["Quyền"] = ds.Tables["TaiKhoan"].Rows[i][3].ToString();

                        dtTaiKhoan.Rows.Add(drTaiKhoan);
                    }
                }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Clear();
            chbTaiKhoan.Checked = true;
            // Kiểm tra nhân viên đã có tài khoản hay chưa
            for (int i = 0; i < ds.Tables["TaiKhoan"].Rows.Count; i++)
            {
                if (ds.Tables["TaiKhoan"].Rows[i][2].ToString() == cboNhanVien.SelectedValue.ToString())
                {
                    MessageBox.Show("Tài Khoản Nhân Viên Này Đã Tồn Tại");
                    txtTaiKhoan.Text = ds.Tables["TaiKhoan"].Rows[i][0].ToString();
                    txtMatKhau.Text = ds.Tables["TaiKhoan"].Rows[i][1].ToString();
                    txtQuyen.Text = ds.Tables["TaiKhoan"].Rows[i][3].ToString();
                    return;
                }
            }
            // kiểm tra tên tài khoản đã tồn tại hay chưa
            for (int i = 0; i < ds.Tables["TaiKhoan"].Rows.Count; i++)
            {
                if (ds.Tables["TaiKhoan"].Rows[i][0].ToString() == txtTaiKhoan.Text.Trim())
                {
                    MessageBox.Show("Tên Tài Khoản Đã Tồn Tại");
                    txtTaiKhoan.Focus();
                    return;
                }
            }
            // kiểm tra rỗng
            if (txtTaiKhoan.Text.Trim() == "")
            {
                MessageBox.Show("Chưa Nhập Thông Tin Tên Tài Khoản");
                txtTaiKhoan.Focus();
                return;
            }
            if (txtMatKhau.Text.Trim() == "")
            {
                MessageBox.Show("Chưa Nhập Thông Tin Mật Khẩu");
                txtMatKhau.Focus();
                return;
            }
            if (txtQuyen.Text.Trim() == "")
            {
                MessageBox.Show("Chưa Nhập Thông Tin Quyền Tài Khoản");
                txtQuyen.Focus();
                return;
            }

            // Thêm Tài Khoản
            DataRow drTaiKhoan = ds.Tables["TaiKhoan"].NewRow();
            drTaiKhoan[0] = txtTaiKhoan.Text;
            drTaiKhoan[1] = txtMatKhau.Text;
            drTaiKhoan[2] = cboNhanVien.SelectedValue.ToString();
            drTaiKhoan[3] = txtQuyen.Text;

            ds.Tables["TaiKhoan"].Rows.Add(drTaiKhoan);
            cmb = new SqlCommandBuilder(daTK);
            daTK.Update(ds, "TaiKhoan");

            dtTaiKhoan.Rows.Clear();
            loadDagridView();
            MessageBox.Show("Thêm Tài Khoản Thành Công");
            reset();
               
            
        }
        private void reset()
        {
            txtTaiKhoan.Clear();
            txtMatKhau.Clear();
            txtQuyen.Clear();
        }
        private void cboNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            reset();
        }

        private void chbTaiKhoan_CheckedChanged(object sender, EventArgs e)
        {
            if (chbTaiKhoan.Checked == true)
            {
                dtTaiKhoan.Rows.Clear();
                loadDagridView();
                txtTuKhoa.Clear();
            }
        }

        private void dtgTaiKhoan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dtgTaiKhoan.CurrentRow.Index;
            string ma = string.Empty;// mã Nhân viên của tên tài khoản được chọn để cập nhật cboNhanVien
            for (int i = 0; i < ds.Tables["TaiKhoan"].Rows.Count; i++)
            {
                if (dtTaiKhoan.Rows[index][0].ToString().Trim() == ds.Tables["TaiKhoan"].Rows[i][0].ToString().Trim()) // nếu tên tài khoản trong dtTaiKhoan == Tài khoản trong ds.Table["TaiKhoan"]
                {
                    ma = ds.Tables["TaiKhoan"].Rows[i][2].ToString();
                    break;
                }
            }
            // tìm vị trí của mã nhân viên được chọn
            int vt = -1;
            for (int i = 0; i < ds.Tables["NV"].Rows.Count; i++)
            {
                if (ma.Trim() == ds.Tables["NV"].Rows[i][0].ToString().Trim())
                {
                    vt = i;
                    break;
                }
            }
            // cập nhật cboNhanVien
            cboNhanVien.SelectedIndex = vt;
            // cập nhật thông tin còn lại
            txtTaiKhoan.Text = dtTaiKhoan.Rows[index]["Tên Đăng Nhập"].ToString();
            txtMatKhau.Text = dtTaiKhoan.Rows[index]["Mật Khẩu"].ToString();
            txtQuyen.Text = dtTaiKhoan.Rows[index]["Quyền"].ToString();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int index = dtgTaiKhoan.CurrentRow.Index;
            String maNV = cboNhanVien.SelectedValue.ToString();
            
            
            // xóa trong Bảng TaiKhoan
            String TenDN = string.Empty;
            for (int i = 0; i < ds.Tables["TaiKhoan"].Rows.Count; i++)
            {
                if (maNV.Trim() == ds.Tables["TaiKhoan"].Rows[i][2].ToString().Trim())
                {
                    TenDN = ds.Tables["TaiKhoan"].Rows[i][0].ToString().Trim();
                    ds.Tables["TaiKhoan"].Rows[i].Delete();
                    break;
                }
            }
            if (TenDN == string.Empty)
            {
                MessageBox.Show("Nhân viên này Chưa có tài khoản");
                return;
            }
            // xóa trong dtTaiKhoan (datagridview)
            for (int i = 0; i < dtTaiKhoan.Rows.Count; i++)
            {
                if (dtTaiKhoan.Rows[i][0].ToString().Trim() == TenDN)
                {
                    dtTaiKhoan.Rows[i].Delete();
                }
            }
            
            // cập nhật bảng TaiKhoan
            cmb = new SqlCommandBuilder(daTK);
            daTK.Update(ds, "TaiKhoan");

            MessageBox.Show("Xóa Thành Công");
            reset();
            
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            String maNV = cboNhanVien.SelectedValue.ToString();


            // sửa trong Bảng TaiKhoan
            String TenDN = string.Empty;
            for (int i = 0; i < ds.Tables["TaiKhoan"].Rows.Count; i++)
            {
                if (maNV.Trim() == ds.Tables["TaiKhoan"].Rows[i][2].ToString().Trim())
                {
                    TenDN = ds.Tables["TaiKhoan"].Rows[i][0].ToString().Trim();

                    ds.Tables["TaiKhoan"].Rows[i][0] = txtTaiKhoan.Text;
                    ds.Tables["TaiKhoan"].Rows[i][1] = txtMatKhau.Text;
                    ds.Tables["TaiKhoan"].Rows[i][3] = txtQuyen.Text;
                    break;
                }
            }
            if (TenDN == string.Empty)
            {
                MessageBox.Show("Nhân Viên Này Chưa Có Tài Khoản");
                return;
            }
            // sửa trong dtTaiKhoan(datagridview)
            for (int i = 0; i < dtTaiKhoan.Rows.Count; i++)
            {
                if (dtTaiKhoan.Rows[i][0].ToString().Trim() == TenDN)
                {
                    dtTaiKhoan.Rows[i]["Tên Đăng Nhập"] = txtTaiKhoan.Text;
                    dtTaiKhoan.Rows[i]["Mật Khẩu"] = txtMatKhau.Text;
                    dtTaiKhoan.Rows[i]["Quyền"] = txtQuyen.Text;
                }
            }
            // cập nhật bảng TaiKhoan
            cmb = new SqlCommandBuilder(daTK);
            daTK.Update(ds, "TaiKhoan");

            MessageBox.Show("Sửa Thành Công");
            reset();
            
        }
        
    }
}
  
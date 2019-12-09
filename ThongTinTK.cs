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
    public partial class ThongTinTK : Form
    {
        SqlConnection conn;
        DataTable dtTK;
        SqlDataAdapter daTK;
        String maNV;
        String tenDangNhap;
        public ThongTinTK()
        {
            InitializeComponent();

            ReadFileConn f = new ReadFileConn();
            String s = f.LoadConn();
            conn = new SqlConnection(s);
        }
        public ThongTinTK( String maNV, String tenDangNhap)
        {
            InitializeComponent();
            this.maNV = maNV;
            this.tenDangNhap = tenDangNhap;

            ReadFileConn f = new ReadFileConn();
            String s = f.LoadConn();
            conn = new SqlConnection(s);
        }
        private void ThongTinTK_Load(object sender, EventArgs e)
        {
            // load thông tin tài khoản
            DataTable dtNV = new DataTable();
            String selectNV = "select * from NV where MaNV = '"+maNV+"'";
            SqlDataAdapter daNV = new SqlDataAdapter(selectNV, conn);
            daNV.Fill(dtNV);

            txtHoTen.Text = dtNV.Rows[0][1].ToString();
            txtChucVu.Text = dtNV.Rows[0][2].ToString();
            txtGioiTinh.Text = dtNV.Rows[0][3].ToString();
            txtDiaChi.Text = dtNV.Rows[0][4].ToString();
            txtSDT.Text = dtNV.Rows[0][5].ToString();

            // Bảng Tài Khoản
            dtTK = new DataTable();
            String selectTK = "select * from TaiKhoan";
            daTK = new SqlDataAdapter(selectTK, conn);
            daTK.Fill(dtTK);

            DataColumn[] keyTK = new DataColumn[1];
            keyTK[0] = dtTK.Columns[0];
            dtTK.PrimaryKey = keyTK;
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            // kiểm tra rỗng
            if (txtMKCu.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn Chưa Nhập Mật Khẩu cũ", "Thông Báo");
                txtMKCu.Focus();
                return;
            }
            if (txtMKMoi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn chưa Nhập Mật Khẩu Mới", "Thông Báo");
                txtMKMoi.Focus();
                return;
            }
            // 2 mật khẩu trùng nhau
            if (txtMKCu.Text.Trim() == txtMKMoi.Text.Trim())
            {
                MessageBox.Show("Mật khẩu mới trùng với mật khẩu cũ", "Thông Báo");
                return;
            }
            DataRow drTK = dtTK.Rows.Find(tenDangNhap);
            // kiểm tra mật khẩu cũ
            if (drTK != null)
            {
                if (drTK["MatKhau"].ToString().Trim() != txtMKCu.Text.Trim())
                {
                    MessageBox.Show("Mật Khẩu Cũ Không đúng");
                    txtMKCu.Focus();
                    return;
                }
                else
                    drTK["MatKhau"] = txtMKMoi.Text;
            }
            SqlCommandBuilder cmb = new SqlCommandBuilder(daTK);
            daTK.Update(dtTK);
            MessageBox.Show("Cập Nhật Mật Khẩu Thành Công");
        }

    }
}

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
    public partial class DangNhap : Form
    {
        public SqlConnection conn;
        public DangNhap()
        {
            InitializeComponent();
      
            ReadFileConn f = new ReadFileConn();
            String s = f.LoadConn();
            conn = new SqlConnection(s);
        }
        
        private void DangNhap_Load(object sender, EventArgs e)
        {
            
        }
        private void txtLogin_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            String strSelect = "Select * from TaiKhoan where TenDangNhap='" + txtUser.Text.Trim() + "' and MatKhau='" + txtPassword.Text.Trim() + "'";
            SqlDataAdapter da = new SqlDataAdapter(strSelect, conn);
            da.Fill(dt);
            
            if (dt.Rows.Count > 0)
            {
                FrmMain fr = new FrmMain(dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), dt.Rows[0][2].ToString(), dt.Rows[0][3].ToString());
                this.Hide();
                fr.Show();
            }
            else
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng");
        }

        private void txtExit_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn có muốn thoát", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (r == DialogResult.Yes)
                Application.Exit();
        }

        private void DangNhap_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        
    }
}

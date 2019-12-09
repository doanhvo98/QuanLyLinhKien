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
using System.IO;

namespace QuanLyLinhKien
{
    public partial class KetNoi : Form
    {
        SqlConnection conn;

        public KetNoi()
        {
            InitializeComponent();
        }
        private void KetNoi_Load(object sender, EventArgs e)
        {
            //String filePath = "D:\\DH\\đồ án lập trình window\\QuanlyLinhKien\\QuanLyLinhKien\\QuanLyLinhKien\\data.txt";
            String filePath = "data.txt";
            FileStream fs = new FileStream(filePath, FileMode.Open);
            StreamReader sReader = new StreamReader(fs, Encoding.UTF8);
          
            String server = sReader.ReadLine();
            String database = sReader.ReadLine();
            String user = sReader.ReadLine();
            String pass = sReader.ReadLine();
            if (database.Trim().Length != 0)
            {
                txtServer.Text = server;
                txtDatabase.Text = database;
                txtUser.Text = user;
                txtPassword.Text = pass;
            }
            if (user.Trim().Length == 0)
                chb_integrate.Checked = true;
            fs.Close();
        }
        
        private void btnKetNoi_Click(object sender, EventArgs e)
        {
            // ghi vào file data
            String Data = txtServer.Text;
            String Database = txtDatabase.Text;
            String User = txtUser.Text;
            String pass = txtPassword.Text;
            //String filePath = "D:\\DH\\đồ án lập trình window\\QuanlyLinhKien\\QuanLyLinhKien\\QuanLyLinhKien\\data.txt";
            String filePath = "data.txt";
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);
            sWriter.WriteLine(Data);
            sWriter.WriteLine(Database);
            sWriter.WriteLine(User);
            sWriter.WriteLine(pass);
            sWriter.Flush();
            fs.Close();

            if (chb_integrate.Checked == true)
            {
                try
                {
                    String s = @"Data source = " + txtServer.Text + ";Initial catalog=" + txtDatabase.Text + ";integrated security= true";
                    
                    conn = new SqlConnection(s);
                    conn.Open();
                    if (conn.State.ToString() == "Open")
                    {
                        MessageBox.Show("Kết nối thành công");
                        DangNhap dn = new DangNhap();
                        this.Hide();
                        dn.Show();
                    }
                    else
                        MessageBox.Show("Kết nối thất bại");
                }
                catch
                {
                    MessageBox.Show("Kết nối thất bại");
                }
            }
            else
            {
                try
                {
                    String s = @"Data source = " + txtServer.Text + ";Initial catalog=" + txtDatabase.Text + "; User id=" + txtUser.Text + "; password=" + txtPassword.Text;
                    conn = new SqlConnection(s);
                    conn.Open();
                    if (conn.State.ToString() == "Open")
                    {
                        MessageBox.Show("Kết nối thành công");
                        DangNhap dn = new DangNhap();
                        dn.conn = conn;
                        this.Close();
                        dn.Show();
                    }
                    else
                        MessageBox.Show("Kết nối thất bại", "Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                catch
                {
                    MessageBox.Show("Kết nối thất bại");
                }
            }
            
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn có muốn thoát","Thoát",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1);
            if (r == DialogResult.Yes)
                this.Close();
        }

        private void chb_integrate_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_integrate.Checked == true)
            {
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
            }
            else
            {
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        
    }
}

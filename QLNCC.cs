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
    public partial class QLNCC : Form
    {
        DataTable dt;
        DataSet ds = new DataSet();
        SqlDataAdapter da;
        SqlConnection conn;
        public QLNCC()
        {
            InitializeComponent();
            ReadFileConn f = new ReadFileConn();
            String s = f.LoadConn();
            conn = new SqlConnection(s);
        }
        private void QLNCC_Load(object sender, EventArgs e)
        {
            load_dgvNCC();

            txtMaNCC.Enabled = txtTenNCC.Enabled = false;
            btnXoa.Enabled = btnSua.Enabled = btnLuu.Enabled = false;
        }

        //Load dữ liệu cho dataGridView
        void load_dgvNCC()
        {
            string strSelect = "Select * from NCC";
            da = new SqlDataAdapter(strSelect, conn);
            da.Fill(ds, "NCC");
            dgvNCC.DataSource = ds.Tables["NCC"];
            DataColumn[] key = new DataColumn[1];
            key[0] = ds.Tables["NCC"].Columns[0];
            ds.Tables["NCC"].PrimaryKey = key;
        }

        private void dgvNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int numRow = e.RowIndex;
                txtMaNCC.Text = dgvNCC.Rows[numRow].Cells[0].Value.ToString();
                txtTenNCC.Text = dgvNCC.Rows[numRow].Cells[1].Value.ToString();
                btnXoa.Enabled = btnSua.Enabled = true;
                btnThem.Text = "Thêm";
                txtMaNCC.Focus();

                if (btnSua.Text == "Hủy")
                    btnXoa.Enabled = false;
                if (btnThem.Text == "Hủy")
                    btnSua.Enabled = btnXoa.Enabled = false;
            }
            catch
            {
                MessageBox.Show("^^");
            }
        }
        //================  btnThem  ==================
        //Sự kiện của btnThem khi có text là "Thêm"
        void btnThem_TextIsThem()
        {
            txtMaNCC.Enabled = txtTenNCC.Enabled = true;
            btnLuu.Enabled = true;
            btnXoa.Enabled = btnSua.Enabled = false;

            txtMaNCC.Focus();
            txtMaNCC.Clear();
            txtTenNCC.Clear();
        }

        //Sự kiện của btnThem khi có text là "Hủy"
        void btnThem_TextIsHuy()
        {
            txtMaNCC.Enabled = txtTenNCC.Enabled = false;
            btnXoa.Enabled = btnSua.Enabled = btnLuu.Enabled = false;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (btnThem.Text == "Thêm")
            {
                btnThem_TextIsThem();
                btnThem.Text = "Hủy";
            }
            else
            {
                btnThem_TextIsHuy();
                btnThem.Text = "Thêm";
            }
        }

        //=================  btnSua  ====================
        //Sự kiện của btnSua khi có text là "Sửa"
        void btnSua_TextIsSua()
        {
            btnThem.Enabled = btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            txtMaNCC.Enabled = false;
            txtTenNCC.Enabled = true;
            txtTenNCC.Focus();
            btnThem.Text = "Thêm";
        }

        //Sự kiện của btnSua khi có text là "Hủy"
        void btnSua_TextIsHuy()
        {
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnThem.Enabled = true;
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtTenNCC.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (btnSua.Text == "Sửa")
            {
                btnSua_TextIsSua();
                btnSua.Text = "Hủy";
            }
            else
            {
                btnSua_TextIsHuy();
                btnSua.Text = "Sửa";
            }
        }
        //===============  btnLuu  ======================
        //Lưu cho hành động Thêm
        void btnLuu_Them()
        {
            DataRow insert = ds.Tables["NCC"].NewRow();
            insert["MaNCC"] = txtMaNCC.Text;
            insert["TenNCC"] = txtTenNCC.Text;
            ds.Tables["NCC"].Rows.Add(insert);
            SqlCommandBuilder cmb = new SqlCommandBuilder(da);
            da.Update(ds, "NCC");
        }

        //Lưu cho hành động Sửa
        void btnLuu_Sua()
        {
            DataRow dr = ds.Tables["NCC"].Rows.Find(txtMaNCC.Text);
            if (dr != null)
            {
                dr["TenNCC"] = txtTenNCC.Text;
            }
            SqlCommandBuilder cmb = new SqlCommandBuilder(da);
            da.Update(ds, "NCC");
        }

        //true: không trùng     false: trùng
        bool kt_MaNCC(string ma)
        {
            string strSelect = "Select Count(*) from NCC where MaNCC = '" + ma + "'";
            SqlCommand cmd = new SqlCommand(strSelect, conn);
            int count = (int)cmd.ExecuteScalar();
            if (count >= 1)
                return false;
            return true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            //Kiểm tra hợp lệ
            if (txtMaNCC.Text == string.Empty)
            {
                MessageBox.Show("Chưa nhập mã NCC");
                txtMaNCC.Focus();
                return;
            }
            if (txtTenNCC.Text == string.Empty)
            {
                MessageBox.Show("Chưa nhập tên NCC");
                txtTenNCC.Focus();
                return;
            }

            //-------------------
            if (MessageBox.Show("Bạn có chắc muốn lưu", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                if (txtMaNCC.Enabled) //Lưu cho hành động Thêm
                {
                    if (kt_MaNCC(txtMaNCC.Text))
                    {
                        btnLuu_Them();
                        MessageBox.Show("Thành công");
                        btnLuu.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Trùng mã NCC");
                    }
                    btnThem.Text = "Thêm";
                }
                else //Lưu cho hành động Sửa
                {
                    btnLuu_Sua();
                    btnSua.Text = "Sửa";
                    btnThem.Enabled = true;
                    btnLuu.Enabled = btnSua.Enabled = false;
                }
                txtMaNCC.Clear();
                txtTenNCC.Clear();
                txtMaNCC.Enabled = txtTenNCC.Enabled = false;
            }
        }

        //==================  btnXoa  ====================
        //Kiểm tra khóa ngoại trong bảng SP
        bool Kt_KhoaNgoaiSP_NCC() // nếu tồn tại MaNCC trong bảng SP -> false
        {
            DataTable dt_SP = new DataTable();
            string str = " Select * from SP where MaNCC='" + txtMaNCC.Text + "' ";
            SqlDataAdapter da_SP = new SqlDataAdapter(str, conn);
            da_SP.Fill(dt_SP);
            if (dt_SP.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        //Kiểm tra khóa ngoại trong bảng HDNhap
        bool Kt_KhoaNgoaiHDNhap_NCC() // nếu tồn tại MaNCC trong bảng HDNhap -> false
        {
            DataTable dt_HDNhap = new DataTable();
            string str = " Select * from HDNhap where MaNCC='" + txtMaNCC.Text + "' ";
            SqlDataAdapter da_HDNhap = new SqlDataAdapter(str, conn);
            da_HDNhap.Fill(dt_HDNhap);
            if (dt_HDNhap.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                if (Kt_KhoaNgoaiSP_NCC() && Kt_KhoaNgoaiHDNhap_NCC())
                {
                    DataRow upd = ds.Tables["NCC"].Rows.Find(txtMaNCC.Text);
                    if (upd != null)
                    {
                        upd.Delete();
                    }
                    SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                    da.Update(ds, "NCC");
                    MessageBox.Show("Xóa thành công");
                }
                else
                {
                    MessageBox.Show("Không thể xóa");
                }
                txtMaNCC.Clear();
                txtTenNCC.Clear();
                txtMaNCC.Enabled = txtTenNCC.Enabled = false;
                btnXoa.Enabled = btnSua.Enabled = btnLuu.Enabled = false;
                btnThem.Text = "Thêm";
            }
        }
        



        
    }
}

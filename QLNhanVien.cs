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
    public partial class QLNhanVien : Form
    {
        DataTable dt;
        DataSet ds = new DataSet();
        SqlDataAdapter da;
        SqlConnection conn;
        public QLNhanVien()
        {
            InitializeComponent();
            ReadFileConn f = new ReadFileConn();
            String s = f.LoadConn();
            conn = new SqlConnection(s);
        }

        private void QLNhanVien_Load(object sender, EventArgs e)
        {
            load_dgvNV();
            load_cboChucVu();
            load_cboGioiTinh();

            txtMaNV.Enabled = txtTenNV.Enabled = cboChucVu.Enabled = cboGioiTinh.Enabled = txtDiaChi.Enabled = txtSDT.Enabled = false;
            btnSua.Enabled = btnXoa.Enabled = btnLuu.Enabled = false;
        }
        //Load dữ liệu cho dataGridView
        void load_dgvNV()
        {
            string strSelect = "Select * from NV";
            da = new SqlDataAdapter(strSelect, conn);
            da.Fill(ds, "NV");
            dgvNV.DataSource = ds.Tables["NV"];
            DataColumn[] key = new DataColumn[1];
            key[0] = ds.Tables["NV"].Columns[0];
            ds.Tables["NV"].PrimaryKey = key;
        }

        private void dgvNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int numRow = e.RowIndex;
                if (btnThem.Text == "Thêm")
                {
                    txtMaNV.Text = dgvNV.Rows[numRow].Cells[0].Value.ToString();
                    txtTenNV.Text = dgvNV.Rows[numRow].Cells[1].Value.ToString();
                    cboChucVu.Text = dgvNV.Rows[numRow].Cells[2].Value.ToString();
                    cboGioiTinh.Text = dgvNV.Rows[numRow].Cells[3].Value.ToString();
                    txtDiaChi.Text = dgvNV.Rows[numRow].Cells[4].Value.ToString();
                    txtSDT.Text = dgvNV.Rows[numRow].Cells[5].Value.ToString();
                }
                else
                {
                    txtTenNV.Text = dgvNV.Rows[numRow].Cells[1].Value.ToString();
                    cboChucVu.Text = dgvNV.Rows[numRow].Cells[2].Value.ToString();
                    cboGioiTinh.Text = dgvNV.Rows[numRow].Cells[3].Value.ToString();
                    txtDiaChi.Text = dgvNV.Rows[numRow].Cells[4].Value.ToString();
                    txtSDT.Text = dgvNV.Rows[numRow].Cells[5].Value.ToString();
                }

                btnXoa.Enabled = btnSua.Enabled = true;
                txtMaNV.Focus();

                if (btnSua.Text == "Hủy")
                    btnXoa.Enabled = false;
                if (btnThem.Text == "Hủy")
                    btnSua.Enabled = btnXoa.Enabled = false;
            }
            catch { }
        }
        //Load dữ liệu của cboChucVu
        void load_cboChucVu()
        {
            cboChucVu.Items.Add("Bán Hàng");
            cboChucVu.Items.Add("Kinh Doanh");
        }

        //Load dữ liệu của cboGioiTinh
        void load_cboGioiTinh()
        {
            cboGioiTinh.Items.Add("Nam");
            cboGioiTinh.Items.Add("Nữ");
            cboGioiTinh.Items.Add("Khác");
        }

        private void txtTenNV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            if (txtSDT.Text.Length > 9 && (Keys)e.KeyChar != Keys.Back)
            {
                e.Handled = true;
                MessageBox.Show("Tối đa chỉ 10 số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //================  btnThem  ==================
        //Sự kiện của btnThem khi có text là "Thêm"
        void btnThem_TextIsThem()
        {
            txtMaNV.Text = sinhMaAuto();
            txtTenNV.Enabled = cboChucVu.Enabled = cboGioiTinh.Enabled = txtDiaChi.Enabled = txtSDT.Enabled = true;
            btnLuu.Enabled = true;
            btnXoa.Enabled = btnSua.Enabled = false;

            txtTenNV.Focus();
            txtTenNV.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            cboChucVu.SelectedIndex = -1;
            cboGioiTinh.SelectedIndex = -1;
        }

        //Sự kiện của btnThem khi có text là "Hủy"
        void btnThem_TextIsHuy()
        {
            txtMaNV.Enabled = txtTenNV.Enabled = cboChucVu.Enabled = cboGioiTinh.Enabled = txtDiaChi.Enabled = txtSDT.Enabled = false;
            btnXoa.Enabled = btnSua.Enabled = btnLuu.Enabled = false;
            cboChucVu.SelectedIndex = -1;
            cboGioiTinh.SelectedIndex = -1;
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
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
            txtMaNV.Enabled = false;
            txtTenNV.Enabled = cboChucVu.Enabled = cboGioiTinh.Enabled = txtDiaChi.Enabled = txtSDT.Enabled = true;
            txtTenNV.Focus();
            btnThem.Text = "Thêm";
        }

        //Sự kiện của btnSua khi có text là "Hủy"
        void btnSua_TextIsHuy()
        {
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnThem.Enabled = true;
            txtTenNV.Enabled = cboChucVu.Enabled = cboGioiTinh.Enabled = txtDiaChi.Enabled = txtSDT.Enabled = false;

            txtTenNV.Clear();
            txtMaNV.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            cboChucVu.SelectedIndex = -1;
            cboGioiTinh.SelectedIndex = -1;
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
            try
            {
                DataRow insert = ds.Tables["NV"].NewRow();
                insert["MaNV"] = txtMaNV.Text;
                insert["TenNV"] = txtTenNV.Text;
                insert["ChucVu"] = cboChucVu.Text;
                insert["GioiTinh"] = cboGioiTinh.Text;
                insert["DiaChi"] = txtDiaChi.Text;
                insert["SDT"] = txtSDT.Text;
                ds.Tables["NV"].Rows.Add(insert);
                SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                da.Update(ds, "NV");
            }
            catch { }
        }

        //Lưu cho hành động Sửa
        void btnLuu_Sua()
        {
            DataRow dr = ds.Tables["NV"].Rows.Find(txtMaNV.Text);
            if (dr != null)
            {
                dr["TenNV"] = txtTenNV.Text;
                dr["ChucVu"] = cboChucVu.Text;
                dr["GioiTinh"] = cboGioiTinh.Text;
                dr["DiaChi"] = txtDiaChi.Text;
                dr["SDT"] = txtSDT.Text;
            }
            SqlCommandBuilder cmb = new SqlCommandBuilder(da);
            da.Update(ds, "NV");
        }

        //true: không trùng     false: trùng
        bool kt_MaNV(string ma)
        {
            string strSelect = "Select Count(*) from NV where MaNV = '" + ma + "'";
            SqlCommand cmd = new SqlCommand(strSelect, conn);
            int count = (int)cmd.ExecuteScalar();
            if (count >= 1)
                return false;
            return true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            //Kiểm tra hợp lệ
            if (txtMaNV.Text == string.Empty)
            {
                MessageBox.Show("Chưa nhập mã NCC");
                txtMaNV.Focus();
                return;
            }
            if (txtTenNV.Text == string.Empty)
            {
                MessageBox.Show("Chưa nhập tên NCC");
                txtTenNV.Focus();
                return;
            }
            if (cboChucVu.Text == string.Empty)
            {
                MessageBox.Show("Chưa chọn chức vụ");
                cboChucVu.Focus();
                return;
            }
            if (cboGioiTinh.Text == string.Empty)
            {
                MessageBox.Show("Chưa chọn giới tính");
                cboGioiTinh.Focus();
                return;
            }
            if (txtDiaChi.Text == string.Empty)
            {
                MessageBox.Show("Chưa nhập địa chỉ");
                txtDiaChi.Focus();
                return;
            }
            if (txtSDT.Text == string.Empty)
            {
                MessageBox.Show("Chưa nhập số điện thoại");
                txtSDT.Focus();
                return;
            }

            //-------------------
            if (MessageBox.Show("Bạn có chắc muốn lưu", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                if (kt_MaNV(txtMaNV.Text)) //Lưu cho hành động Thêm
                {
                    btnLuu_Them();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    btnLuu.Enabled = false;
                    btnThem.Text = "Thêm";
                }
                else //Lưu cho hành động Sửa
                {
                    btnLuu_Sua();
                    MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    btnSua.Text = "Sửa";
                    btnThem.Enabled = true;
                }
                txtMaNV.Clear();
                txtTenNV.Clear();
                txtDiaChi.Clear();
                txtSDT.Clear();
                cboChucVu.SelectedIndex = -1;
                cboGioiTinh.SelectedIndex = -1;
                txtMaNV.Enabled = txtTenNV.Enabled = cboChucVu.Enabled = cboGioiTinh.Enabled = txtDiaChi.Enabled = txtSDT.Enabled = false;
                btnLuu.Enabled = btnSua.Enabled = btnXoa.Enabled = false;
            }
        }
        //==================  btnXoa  ====================
        //Kiểm tra khóa ngoại trong bảng TK
        bool Kt_KhoaNgoaiTK_NV() // nếu tồn tại MaNV trong bảng TaiKhoan -> false
        {
            DataTable dt_TK = new DataTable();
            string str = " Select * from TaiKhoan where MaNV='" + txtMaNV.Text + "' ";
            SqlDataAdapter da_TK = new SqlDataAdapter(str, conn);
            da_TK.Fill(dt_TK);
            if (dt_TK.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        //Kiểm tra khóa ngoại trong bảng HDNhap
        bool Kt_KhoaNgoaiHDNhap_NV() // nếu tồn tại MaNV trong bảng HDNhap -> false
        {
            DataTable dt_HDNhap = new DataTable();
            string str = " Select * from HDNhap where MaNV='" + txtMaNV.Text + "' ";
            SqlDataAdapter da_HDNhap = new SqlDataAdapter(str, conn);
            da_HDNhap.Fill(dt_HDNhap);
            if (dt_HDNhap.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        //Kiểm tra khóa ngoại trong bảng HDBan
        bool Kt_KhoaNgoaiHDBan_NV() // nếu tồn tại MaNV trong bảng HDBan -> false
        {
            DataTable dt_HDBan = new DataTable();
            string str = " Select * from HDBan where MaNV='" + txtMaNV.Text + "' ";
            SqlDataAdapter da_HDBan = new SqlDataAdapter(str, conn);
            da_HDBan.Fill(dt_HDBan);
            if (dt_HDBan.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                if (Kt_KhoaNgoaiHDBan_NV() && Kt_KhoaNgoaiHDNhap_NV() && Kt_KhoaNgoaiTK_NV())
                {
                    DataRow upd = ds.Tables["NV"].Rows.Find(txtMaNV.Text);
                    if (upd != null)
                    {
                        upd.Delete();
                    }
                    SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                    da.Update(ds, "NV");
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("Không thể xóa");
                }
                txtTenNV.Clear();
                txtMaNV.Clear();
                txtDiaChi.Clear();
                txtSDT.Clear();
                cboChucVu.SelectedIndex = -1;
                cboGioiTinh.SelectedIndex = -1;
                txtTenNV.Enabled = cboChucVu.Enabled = cboGioiTinh.Enabled = txtDiaChi.Enabled = txtSDT.Enabled = false;
                btnXoa.Enabled = btnSua.Enabled = btnLuu.Enabled = false;
                btnThem.Text = "Thêm";
            }
        }
        //Sinh mã tự động
        string sinhMaAuto()
        {
            int coso = 0;
            DataTable dt_NV = new DataTable();
            string str = "select * from NV";
            SqlDataAdapter da_NV = new SqlDataAdapter(str, conn);
            da_NV.Fill(dt_NV);
            if (dt_NV.Rows.Count == 0)// nếu danh sách nhân viên rỗng
            {
                coso = 1;
            }
            else if (dt_NV.Rows.Count == 1 && int.Parse(dt_NV.Rows[0][0].ToString().Substring(2, 3)) == 1) // nếu danh sách nhân viên có 1 nhân viên và mã người này là NV001
            {
                coso = 2;
            }
            else if (dt_NV.Rows.Count == 1 && int.Parse(dt_NV.Rows[0][0].ToString().Substring(2, 3)) > 1) // nếu danh sách có 1 nhân viên mà mã người này khác NV001
            {
                coso = 1;
            }
            else // nếu danh sách có hơn 1 nhân viên
            {
                for (int i = 0; i < dt_NV.Rows.Count - 1; i++)
                {
                    if (int.Parse(dt_NV.Rows[i + 1][0].ToString().Substring(2, 3)) - int.Parse(dt_NV.Rows[i][0].ToString().Substring(2, 3)) > 1)
                    {
                        coso = int.Parse(dt_NV.Rows[i][0].ToString().Substring(2, 3)) + 1;
                        break;
                    }
                }
                coso = int.Parse(dt_NV.Rows[dt_NV.Rows.Count - 1][0].ToString().Substring(2, 3)) + 1;
            }
            if (coso < 10)
                return "NV00" + coso.ToString();
            if (coso < 100)
                return "NV0" + coso.ToString();
            return "NV" + coso.ToString();
        }
    }
}

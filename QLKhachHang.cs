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
    public partial class QLKhachHang : Form
    {
        SqlConnection conn;
        DataTable KH;

        public QLKhachHang()
        {
            InitializeComponent();
        }

        private void QLKhachHang_Load(object sender, EventArgs e)
        {
            Class.Functions.Connect();
            string sql;
            sql = "SELECT * FROM KH";
            KH = Class.Functions.GetDataToTable(sql);
            dtgv_KhachHang.DataSource = KH;
        }
        private void ResetValue()
        {
            txt_MaKH.Text = "";
            txt_TenKH.Text = "";
            txt_DiaChi.Text = "";
            txt_GhiChu.Text = "";
            txt_SDT.Text = "";
        }
        private void LoadDataGridView()
        {

            /*dtgv_KhachHang.Columns[0].HeaderText = "Mã khách";
            dtgv_KhachHang.Columns[1].HeaderText = "Tên khách";
            dtgv_KhachHang.Columns[2].HeaderText = "Số điện thoại";
            dtgv_KhachHang.Columns[3].HeaderText = "Địa chỉ";
            dtgv_KhachHang.Columns[4].HeaderText = "Ghi chú";*/
            string sql;
            sql = "SELECT * FROM KH";
            KH = Class.Functions.GetDataToTable(sql);
            dtgv_KhachHang.DataSource = KH;
            dtgv_KhachHang.AllowUserToAddRows = false;
            dtgv_KhachHang.EditMode = DataGridViewEditMode.EditProgrammatically;
            dtgv_KhachHang.Columns["MaKH"].Width = 500;
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValue();
            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void txt_TenKH_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ResetValue();
            txt_MaKH.Enabled = true;
            txt_MaKH.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txt_MaKH.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã khách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_MaKH.Focus();
                return;
            }
            if (txt_TenKH.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên khách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_TenKH.Focus();
                return;
            }
            if (txt_DiaChi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_DiaChi.Focus();
                return;
            }
            if (txt_SDT.Text == "")
            {
                MessageBox.Show("Bạn phải nhập số điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_SDT.Focus();
                return;
            }
            sql = "SELECT MaKH FROM KH WHERE MaKH=N'" + txt_MaKH.Text.Trim() + "'";
            if (Class.Functions.kiemtra(sql))
            {
                MessageBox.Show("Mã khách đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_MaKH.Focus();
                return;
            }
            sql = "INSERT INTO KH VALUES(N'" + txt_MaKH.Text.Trim() + "',N'" + txt_TenKH.Text.Trim() + "',N'" + txt_SDT.Text + "',N'" + txt_DiaChi.Text.Trim() + "',N'" + txt_GhiChu.Text.Trim() + "')";
            Class.Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValue();

            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txt_MaKH.Enabled = false;
        }

        private void dtgv_KhachHang_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                txt_MaKH.Focus();
                return;
            }

            txt_MaKH.Text = dtgv_KhachHang.CurrentRow.Cells["MaKH"].Value.ToString();
            txt_TenKH.Text = dtgv_KhachHang.CurrentRow.Cells["TenKH"].Value.ToString();
            txt_SDT.Text = dtgv_KhachHang.CurrentRow.Cells["SDT"].Value.ToString();
            txt_DiaChi.Text = dtgv_KhachHang.CurrentRow.Cells["DiaChi"].Value.ToString();
            txt_GhiChu.Text = dtgv_KhachHang.CurrentRow.Cells["GhiChu"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
        }

        private void QLKhachHang_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dl = MessageBox.Show("Bạn có muốn thoát không?", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (dl == DialogResult.No)
                e.Cancel = true;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql;
            if (KH.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txt_MaKH.Text == "")
            {
                MessageBox.Show("Bạn phải chọn khách hàng cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_MaKH.Focus();
                return;
            }
            if (txt_TenKH.Text.Trim().Length == 0)
            {
                MessageBox.Show("Chưa nhập tên khách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_TenKH.Focus();
                return;
            }
            if (txt_DiaChi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Chưa nhập địa chỉ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_DiaChi.Focus();
                return;
            }
            if (txt_SDT.Text == "")
            {
                MessageBox.Show("Chưa nhập số điện thoại khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_SDT.Focus();
                return;
            }
            sql = "UPDATE KH SET TenKH=N'" + txt_TenKH.Text.Trim().ToString() + "',DiaChi=N'" + txt_DiaChi.Text.Trim().ToString() + "',SDT=N'" + txt_SDT.Text.ToString() + "',GhiChu=N'" + txt_GhiChu.Text.Trim().ToString() + "'WHERE MaKH=N'" + txt_MaKH.Text + "'";
            Class.Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValue();
            btnBoQua.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (KH.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txt_MaKH.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn khách hàng muốn xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa khách hàng này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE FROM KH WHERE MaKH='" + txt_MaKH.Text + "'";
                Class.Functions.RunSQLDel(sql);
                LoadDataGridView();
                ResetValue();
            }
        }





        






    }
}

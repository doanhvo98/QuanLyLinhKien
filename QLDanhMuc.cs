using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyLinhKien
{
    public partial class QLDanhMuc : Form
    {
        DataTable DanhMucSP;
        public QLDanhMuc()
        {
            InitializeComponent();
        }

        private void QLDanhMuc_Load(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            Class.Functions.Connect();
            LoadDataGridView();
        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT * FROM DanhMucSP";
            DanhMucSP = Class.Functions.GetDataToTable(sql);
            dtgv_DanhMuc.DataSource = DanhMucSP;
            dtgv_DanhMuc.AllowUserToAddRows = false;
            dtgv_DanhMuc.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dtgv_DanhMuc_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                txt_MaDanhMuc.Focus();
                return;
            }

            txt_MaDanhMuc.Text = dtgv_DanhMuc.CurrentRow.Cells["MaDanhMuc"].Value.ToString();
            txtTenDanhMuc.Text = dtgv_DanhMuc.CurrentRow.Cells["TenDanhMuc"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;   
        }
        private void ResetValue()
        {
            txt_MaDanhMuc.Text = string.Empty;
            txtTenDanhMuc.Text = string.Empty;
        }

        private void QLDanhMuc_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dl = MessageBox.Show("Bạn có muốn thoát không?", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (dl == DialogResult.No)
                e.Cancel = true;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (DanhMucSP.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txt_MaDanhMuc.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn danh mục muốn xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE FROM DanhMucSP WHERE MaDanhMuc=N'" + txt_MaDanhMuc.Text + "'";
                Class.Functions.RunSQLDel(sql);
                LoadDataGridView();
                ResetValue();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txt_MaDanhMuc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã danh mục", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_MaDanhMuc.Focus();
                return;
            }
            if (txtTenDanhMuc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên danh mục", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenDanhMuc.Focus();
                return;
            }

            sql = "SELECT MaDanhMuc FROM DanhMucSP WHERE MaDanhMuc=N'" + txt_MaDanhMuc.Text.Trim() + "'";
            if (Class.Functions.kiemtra(sql))
            {
                MessageBox.Show("Mã danh mục đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_MaDanhMuc.Focus();
                return;
            }
            sql = "INSERT INTO DanhMucSP VALUES(N'" + txt_MaDanhMuc.Text.Trim() + "',N'" + txtTenDanhMuc.Text.Trim() + "')";
            Class.Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValue();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql;
            if (DanhMucSP.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txt_MaDanhMuc.Text == "")
            {
                MessageBox.Show("Bạn phải chọn danh mục cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_MaDanhMuc.Focus();
                return;
            }
            if (txtTenDanhMuc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Chưa nhập tên danh mục", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenDanhMuc.Focus();
                return;
            }

            sql = "UPDATE DanhMucSP SET TenDanhMuc=N'" + txtTenDanhMuc.Text.Trim().ToString() + "' WHERE MaDanhMuc=N'" + txt_MaDanhMuc.Text + "'";
            Class.Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValue();
        }







    }
}

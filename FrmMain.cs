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
    public partial class FrmMain : DevComponents.DotNetBar.Office2007RibbonForm
    {
        String tenDangNhap, matKhau, maNV, quyen;


        public FrmMain()
        {
            InitializeComponent();
        }
        public FrmMain(String tenDangNhap, String matKhau, String maNV, String quyen)
        {
            InitializeComponent();
            this.tenDangNhap = tenDangNhap;
            this.matKhau = matKhau;
            this.maNV = maNV;
            this.quyen = quyen;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            txtTaiKhoan.Text = tenDangNhap;
            if (quyen == "User")
            {
                btnQLTaiKhoan.Enabled = false;
                
            }
        }


        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Hide();
            DangNhap dn = new DangNhap();
            dn.Show();
        }

        private void btnQLTaiKhoan_Click(object sender, EventArgs e)
        {
            QLTaiKhoan tk = new QLTaiKhoan();
            
            
            tk.Show();
        }

        private void buttonItem4_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn có muốn thoát", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (r == DialogResult.Yes)
                Application.Exit();
        }

        private void btnQLBanHang_Click(object sender, EventArgs e)
        {
            QLBanHang bh = new QLBanHang();
            bh.Show();
        }

        private void btnThayDoiMK_Click(object sender, EventArgs e)
        {
            ThongTinTK mk = new ThongTinTK(maNV, tenDangNhap);
            mk.Show();
        }

        private void btnInHDB_Click(object sender, EventArgs e)
        {
            Report.HoaDonBan_Report crpHDB = new Report.HoaDonBan_Report();
            crpHDB.Show();
        }

        private void btnThoatPM_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn có muốn thoát", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (r == DialogResult.Yes)
                Application.Exit();
        }

        private void btnQLNCC_Click(object sender, EventArgs e)
        {
            QLNCC ncc = new QLNCC();
            ncc.Show();
        }

        private void btnHDNReport_Click(object sender, EventArgs e)
        {
            Report.HDNhap_Report hdn = new Report.HDNhap_Report();
            hdn.Show();
        }

        private void btnHDBReport_Click(object sender, EventArgs e)
        {
            Report.HDBan_Report hdb = new Report.HDBan_Report();
            hdb.Show();
        }

        private void btnQLKhachHang_Click(object sender, EventArgs e)
        {
            QLKhachHang kh = new QLKhachHang();
            kh.Show();
        }

        private void QLDanhMuc_Click(object sender, EventArgs e)
        {
            QLDanhMuc dm = new QLDanhMuc();
            dm.Show();
        }

        private void btnNhapSP_Click(object sender, EventArgs e)
        {
            NhapSP nsp = new NhapSP();
            nsp.Show();
        }

        private void btnQLKho_Click(object sender, EventArgs e)
        {
            QLKho qlk = new QLKho();
            qlk.Show();
        }

        private void btnQLNhanVien_Click(object sender, EventArgs e)
        {
            QLNhanVien nv = new QLNhanVien();
            nv.Show();
        }
    }
}

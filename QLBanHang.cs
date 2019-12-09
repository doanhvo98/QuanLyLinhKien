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
    public partial class QLBanHang : Form
    {
        SqlConnection conn;
        DataSet ds;
        DataTable dtSPChon;
        DataTable dtMucChon;

        SqlDataAdapter daKH;
        SqlDataAdapter daHDB;
        SqlDataAdapter daSP;
        SqlDataAdapter daCTHD;

        SqlCommandBuilder cmb;

        String soHD = "";
        public QLBanHang()
        {
            InitializeComponent();

            ReadFileConn f = new ReadFileConn();
            String s = f.LoadConn();
            conn = new SqlConnection(s);
        }
        private void QLBanHang_Load(object sender, EventArgs e)
        {
            ds = new DataSet();

            String selectDM = "select * from DanhMucSP";
            String selectSPDM = "select MaSP as N'Mã Sản Phẩm', TenSP as N'Tên Sản Phẩm', GiaBan as 'Giá Bán' from SP";
            

            SqlDataAdapter daDM = new SqlDataAdapter(selectDM, conn);
            SqlDataAdapter daSPDanhMuc = new SqlDataAdapter(selectSPDM, conn);

            daDM.Fill(ds,"DanhMuc");
            daSPDanhMuc.Fill(ds, "SPDanhMuc");

            trvDanhMuc.Nodes.Add("Tất Cả");
            for (int i = 0; i < ds.Tables["DanhMuc"].Rows.Count; i++)
            {
                trvDanhMuc.Nodes.Add(ds.Tables["DanhMuc"].Rows[i][1].ToString());
            }

            // không cho phép gõ thêm các dòng mới
            dtgSanPham.AllowUserToAddRows = false;
            dtgSanPham.ReadOnly = true;

            // load dữ liệu combobox nhân viên
            string selectNV = "select * from NV";
            SqlDataAdapter daNV = new SqlDataAdapter(selectNV, conn);
            daNV.Fill(ds, "NV");
            cboNV.DataSource = ds.Tables["NV"];
            cboNV.DisplayMember = "TenNV";
            cboNV.ValueMember = "MaNV";

            //----------------------------------------------- Bảng Khách Hàng ----------------------------------------
            String selectKH = "select * from KH";
            daKH = new SqlDataAdapter(selectKH, conn);
            daKH.Fill(ds, "KH");
            DataColumn[] keyKH = new DataColumn[1];
            keyKH[0] = ds.Tables["KH"].Columns[0];
            ds.Tables["KH"].PrimaryKey = keyKH;

            //----------------------------------------------- Bảng Hóa đơn bán ---------------------------------
            String selectHDB = "select * from HDBan";
            daHDB = new SqlDataAdapter(selectHDB, conn);
            daHDB.Fill(ds, "HDBan");
            DataColumn[] keyHDB = new DataColumn[1];
            keyHDB[0] = ds.Tables["HDBan"].Columns[0];
            ds.Tables["HDBan"].PrimaryKey = keyHDB;

            // ----------------------------------------------- Bảng Sản phẩm ---------------------------------------
            string selectSP = "select * from SP";
            daSP = new SqlDataAdapter(selectSP, conn);
            daSP.Fill(ds, "SP");
            DataColumn[] keySP = new DataColumn[1];
            keySP[0] = ds.Tables["SP"].Columns[0];
            ds.Tables["SP"].PrimaryKey = keySP;
            
            //------------------------------------------------- Bảng Chi Tiết Hóa Đơn ----------------------------
            string selectCTHD = "select * from ChiTietHDBan";
            daCTHD = new SqlDataAdapter(selectCTHD, conn);
            daCTHD.Fill(ds, "ChiTietHDBan");
            DataColumn[] keyCTHD = new DataColumn[2];
            keyCTHD[0] = ds.Tables["ChiTietHDBan"].Columns[0];
            keyCTHD[1] = ds.Tables["ChiTietHDBan"].Columns[1];
            ds.Tables["ChiTietHDBan"].PrimaryKey = keyCTHD;
            
           
            //----------------------------------- Bảng dtSPChon -----------------------------------
            dtSPChon = new DataTable();
            DataColumn col1 = new DataColumn("Mã Sản Phẩm");
            DataColumn col2 = new DataColumn("Tên Sản Phẩm");
            DataColumn col3 = new DataColumn("Số Lượng");
            DataColumn col4 = new DataColumn("Đơn Giá");
            DataColumn col5 = new DataColumn("Thành Tiền");
            dtSPChon.Columns.Add(col1);
            dtSPChon.Columns.Add(col2);
            dtSPChon.Columns.Add(col3);
            dtSPChon.Columns.Add(col4);
            dtSPChon.Columns.Add(col5);

            DataColumn[] keySPChon = new DataColumn[1];
            keySPChon[0] = dtSPChon.Columns[0];
            dtSPChon.PrimaryKey = keySPChon;

            // DatagridView sản phẩm mua:

            dtgSPMua.DataSource = dtSPChon;
            
            dtgSPMua.Columns["Mã Sản Phẩm"].Width = 100;
            dtgSPMua.Columns["Tên Sản Phẩm"].Width = 345;
            dtgSPMua.Columns["Số Lượng"].Width = 80;
            dtgSPMua.Columns["Đơn Giá"].Width = 130;
            dtgSPMua.Columns["Thành Tiền"].Width = 130;

            dtgSPMua.AllowUserToAddRows = false;

            // cột số lượng được thay đổi
            dtgSPMua.Columns["Mã Sản Phẩm"].ReadOnly = true;
            dtgSPMua.Columns["Tên Sản Phẩm"].ReadOnly = true;
            dtgSPMua.Columns["Đơn Giá"].ReadOnly = true;
            dtgSPMua.Columns["Thành Tiền"].ReadOnly = true;

            // sinh mã hóa đơn bán
            sinhMaHDBan();

            soHD = txtSoHoaDon.Text;
            
        }
        private void sinhMaHDBan()
        {
            if (ds.Tables["HDBan"].Rows.Count == 0)
                txtSoHoaDon.Text = "HDB00001";
            else
            {
                string MaHDBstring = ds.Tables["HDBan"].Rows[ds.Tables["HDBan"].Rows.Count - 1][0].ToString().Trim();// lấy mã hóa đơn bán cuối cùng
                MaHDBstring = MaHDBstring.Substring(3);// cắt chuỗi từ vị trí số 3
                int sl = MaHDBstring.Length;// số lượng chuỗi vừa cắt
                int MaHDBSo = Int32.Parse(MaHDBstring) + 1;// tăng lên 1
                string string0 = "";// string chứa số 0
                for (int i = 0; i < sl - MaHDBSo.ToString().Length; i++)
                    string0 += "0";
                txtSoHoaDon.Text = "HDB" + string0 + MaHDBSo.ToString();
            }
        }
        private void trvDanhMuc_AfterSelect(object sender, TreeViewEventArgs e)
        {
            dtMucChon = new DataTable();
            String s = trvDanhMuc.SelectedNode.Text;// tên danh mục của danh mục được chọn
            if (trvDanhMuc.SelectedNode.Index == 0) // nếu danh mục được chọn là phần tử đầu tiên trong treeview
                dtMucChon = ds.Tables["SPDanhMuc"]; //add tất cả sản phẩm vào datagridview
            else
            {
                string ma = "";
                for (int i = 0; i < ds.Tables["DanhMuc"].Rows.Count; i++)
                {
                    if (s == ds.Tables["DanhMuc"].Rows[i][1].ToString())//  nếu tên danh mục được chọn trong DanhMuc = tên danh mục trong sản phẩm
                    {
                        ma = ds.Tables["DanhMuc"].Rows[i][0].ToString().Trim(); // lấy mã danh mục của Danh mục chọn 
                    }
                }
                
                String selectMucChon = "select MaSP as N'Mã Sản Phẩm', TenSP as N'Tên Sản Phẩm', GiaBan as 'Giá Bán' from SP where MADANHMUC = '" +ma+"'";
                SqlDataAdapter daMucChon = new SqlDataAdapter(selectMucChon, conn);
                daMucChon.Fill(dtMucChon);
            }
            dtgSanPham.DataSource = dtMucChon;

            
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // khi chưa chọn sp nao
            if (dtSPChon.Rows.Count == 0)
            {
                MessageBox.Show("Bạn Chưa chọn sản phẩm", "Thông báo");
                return;
            }
            if (dtgSPMua.Rows.Count >=1)
            {
                dtSPChon.Rows.RemoveAt(dtgSPMua.CurrentRow.Index);
                CapNhatTong();
            }
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtgSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // khi chưa chọn sp nao
            if (dtMucChon.Rows.Count == 0)
            {
                MessageBox.Show("Bạn Chưa chọn sản phẩm", "Thông báo");
                return;
            }
            int index = dtgSanPham.CurrentRow.Index; //lấy ra chỉ số của row đang đc chọn
            string maChon = dtgSanPham.Rows[index].Cells[0].Value.ToString().Trim(); // mã sản phẩm được chọn
            // kiểm tra trùng mã sản phẩm
            if (!KTMaSPChon(maChon))
            {
                MessageBox.Show("Sản phẩm đã được chọn");
                return;
            }
           
            DataRow drSPChon = dtSPChon.NewRow();
            DataRow drSP = ds.Tables["SP"].Rows.Find(maChon);
            if (Int32.Parse(drSP[3].ToString()) == 0) // if so luong san pham = 0
            {
                MessageBox.Show("Sản Phẩm đã hết");
                return;
            }

            drSPChon["Mã Sản Phẩm"] = maChon;
            drSPChon["Tên Sản Phẩm"] = drSP[2];
            drSPChon["Số Lượng"] = 1;
            drSPChon["Đơn Giá"] = drSP[5];
            drSPChon["Thành Tiền"] = Double.Parse(drSPChon["Đơn Giá"].ToString()) * Int32.Parse(drSPChon["Số Lượng"].ToString());
            dtSPChon.Rows.Add(drSPChon);

            // cập nhật tổng tiền
            CapNhatTong();
        }
        // kiểm tra mã trùng khi thêm vào danh sách sản phẩm mua dtSpChon
        private Boolean KTMaSPChon(string s)
        {
            DataRow drSPChon = dtSPChon.Rows.Find(s);
            if (drSPChon != null)
                return false;
            return true;
        }
        // Lấy số lượng sản phẩm trong kho của mã hàng chọn
        private int LaySoLuong(int index)
        {
            int sl = 0;
            string ma = dtSPChon.Rows[index][0].ToString().Trim(); // mã sản phẩm đang chọn
            for (int i = 0; i < ds.Tables["SP"].Rows.Count; i++)
            {
                if (ma == ds.Tables["SP"].Rows[i][0].ToString().Trim())
                    sl = Int32.Parse(ds.Tables["SP"].Rows[i][3].ToString());
            }
            return sl;
        }
        private void btnTang1_Click(object sender, EventArgs e)
        {
            
            
            // khi chưa chọn sp nao
            if (dtSPChon.Rows.Count == 0)
            {
                MessageBox.Show("Bạn Chưa chọn sản phẩm", "Thông báo");
                return;
            }
            int index = dtgSPMua.CurrentRow.Index; //lấy ra chỉ số của row đang đc chọn
            if (Int32.Parse(dtSPChon.Rows[index][2].ToString()) < LaySoLuong(index)) // nếu số lượng cập nhật nhỏ hơn số lượng sp trong kho
            {
                dtSPChon.Rows[index][2] = Int32.Parse(dtSPChon.Rows[index][2].ToString()) + 1; // tăng lên 1
                dtSPChon.Rows[index][4] = Int32.Parse(dtSPChon.Rows[index][2].ToString()) * Double.Parse(dtSPChon.Rows[index][3].ToString()); // cập nhật thành tiền 
            }
            else
                MessageBox.Show("Sản Phẩm đã hết");
             // cập nhật tổng tiền
            CapNhatTong();
        }
        private void btnGiam1_Click(object sender, EventArgs e)
        {
            // khi chưa chọn sp nao
            if (dtSPChon.Rows.Count == 0)
            {
                MessageBox.Show("Bạn Chưa chọn sản phẩm", "Thông báo");
                return;
            }
            int index = dtgSPMua.CurrentRow.Index; //lấy ra chỉ số của row đang đc chọn
            if (Int32.Parse(dtSPChon.Rows[index][2].ToString()) > 1)
            {
                dtSPChon.Rows[index][2] = Int32.Parse(dtSPChon.Rows[index][2].ToString()) - 1; // tăng lên 1
                dtSPChon.Rows[index][4] = Int32.Parse(dtSPChon.Rows[index][2].ToString()) * Double.Parse(dtSPChon.Rows[index][3].ToString()); // cập nhật thành tiền
                // cập nhật tổng tiền
                CapNhatTong();
            }
        }
        private void dtgSPMua_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int index = dtgSPMua.CurrentRow.Index;
            if (Int32.Parse(dtSPChon.Rows[index][2].ToString()) <= LaySoLuong(index))
            {
                dtSPChon.Rows[index][4] = Int32.Parse(dtSPChon.Rows[index][2].ToString()) * Double.Parse(dtSPChon.Rows[index][3].ToString()); // cập nhật thành tiền
               
            }
            else
            {
                MessageBox.Show("Vượt quá số lượng sản phẩm trong kho! ( số lượng: "+LaySoLuong(index)+")");
                dtSPChon.Rows[index][2] = LaySoLuong(index); // số lượng max
                dtSPChon.Rows[index][4] = Int32.Parse(dtSPChon.Rows[index][2].ToString()) * Double.Parse(dtSPChon.Rows[index][3].ToString()); // cập nhật thành tiền
                
            }
            CapNhatTong();
        }
        // cập nhật tổng tiền
        private void CapNhatTong()
        {
            String s = "0";
            if (dtSPChon.Rows.Count == 0)
                txtTong.Text = "0";
            else
            {
                for (int i = 0; i < dtSPChon.Rows.Count; i++)
                {
                    s = (Double.Parse(s) + Double.Parse(dtSPChon.Rows[i][4].ToString())).ToString();
                    txtTong.Text = s;
                }
            }
        }

        private void dtgSPMua_DataSourceChanged(object sender, EventArgs e)
        {
            CapNhatTong();
        }
        private void reset()
        {
            txtTenKH.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            txtGhiChu.Clear();
        }
        private void chbKhachHangMoi_CheckedChanged(object sender, EventArgs e)
        {
            if (chbKhachHangMoi.Checked)
            {
                txtTenKH.Enabled = true;
                txtDiaChi.Enabled = true;
                txtGhiChu.Enabled = true;
                txtSDT.Enabled = true;

                reset();
              

                txtTenKH.Focus();
            }
            else
            {
                txtTenKH.Enabled = false;
                txtDiaChi.Enabled = false;
                txtGhiChu.Enabled = false;

                reset();
            }
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {
            if (chbKhachHangMoi.Checked == false)
            {
                for (int i = 0; i < ds.Tables["KH"].Rows.Count; i++)
                {
                    if (txtSDT.Text.Trim() == ds.Tables["KH"].Rows[i][2].ToString().Trim())
                    {
                        MessageBox.Show(ds.Tables["KH"].Rows[i][1].ToString());
                        txtTenKH.Text = ds.Tables["KH"].Rows[i][1].ToString();
                        txtDiaChi.Text = ds.Tables["KH"].Rows[i][3].ToString();
                        txtGhiChu.Text = ds.Tables["KH"].Rows[i][4].ToString();
                        return;
                    }
                    else
                    {
                        txtTenKH.Clear();
                        txtDiaChi.Clear();
                        txtGhiChu.Clear();
                    
                    }
                }
            }
        }
        // tự sinh mã khách hàng
        private string sinhMaKH()
        { 
            string kq;
            if (ds.Tables["KH"].Rows.Count == 0)
                kq = "KH00001";
            else
            {
                string KHstring = ds.Tables["KH"].Rows[ds.Tables["KH"].Rows.Count - 1][0].ToString().Trim();// lấy mã hóa đơn bán cuối cùng
                KHstring = KHstring.Substring(2);// cắt chuỗi từ vị trí số 3
                int sl = KHstring.Length;// số lượng chuỗi vừa cắt
                int MaKHSo = Int32.Parse(KHstring) + 1;// tăng lên 1
                string string0 = "";// string chứa số 0
                for (int i = 0; i < sl - MaKHSo.ToString().Length; i++)
                    string0 += "0";
                kq = "KH" + string0 + MaKHSo.ToString();
            }
            return kq;

        }
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            
            // Kiểm tra rỗng
            if (chbKhachHangMoi.Checked == true)
            {
                if (txtTenKH.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Bạn Chưa Nhập tên Khách hàng !", "Xin Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTenKH.Focus();
                    return;
                }
                if (txtSDT.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Bạn Chưa Nhập Số Điện Thoại !", "Xin Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSDT.Focus();
                    return;
                }
                if (txtDiaChi.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Bạn Chưa Nhập Địa Chỉ Khách hàng !", "Xin Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDiaChi.Focus();
                    return;
                }
            }

            // kiểm tra có sản phẩm chọn hay chưa
            if (dtSPChon.Rows.Count == 0)
            {
                MessageBox.Show("Bạn Chưa Chọn Sản Phẩm");
                return;
            }
            // kiểm tra trùng số điện thoại
            if (chbKhachHangMoi.Checked == true)
            {
                for (int i = 0; i < ds.Tables["KH"].Rows.Count; i++)
                {
                    if (ds.Tables["KH"].Rows[i]["SDT"].ToString().Trim() == txtSDT.Text.Trim())
                    {
                        DialogResult r;
                        r = MessageBox.Show("Số Điện thoại đã tồn tại !\n Mã Khách hàng: " + ds.Tables["KH"].Rows[i]["MaKH"].ToString() + "\n Tên Khách hàng: " + ds.Tables["KH"].Rows[i]["TenKH"].ToString() + "\n Bạn có muốn tiếp tục không?", "Rất tiếc", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (r == DialogResult.No)
                        {
                            txtSDT.Focus();
                            return;
                        }
                        else
                        {
                            ds.Tables["KH"].Rows[i]["SDT"] = null;
                            break;
                        }
                    }

                }
            }

            // Thêm khách hàng
            string maKH = sinhMaKH();
            if (chbKhachHangMoi.Checked == true)
            {
                DataRow drKhachHang = ds.Tables["KH"].NewRow();
                drKhachHang["MaKH"] = maKH;
                drKhachHang["TenKH"] = txtTenKH.Text;
                drKhachHang["DiaChi"] = txtDiaChi.Text;
                drKhachHang["SDT"] = txtSDT.Text;
                drKhachHang["GhiChu"] = txtGhiChu.Text;
                ds.Tables["KH"].Rows.Add(drKhachHang);

                cmb = new SqlCommandBuilder(daKH);
                daKH.Update(ds, "KH");

            }
            // thêm hóa đơn bán
            DataRow drHDB = ds.Tables["HDBan"].NewRow();

            drHDB["MaHDBan"] = txtSoHoaDon.Text;
            drHDB["MaNV"] = cboNV.SelectedValue.ToString();
            drHDB["NgayBan"] = dtpNgay.Value.ToString("MM/dd/yyyy");
            if (chbKhachHangMoi.Checked == false)
            {
                for (int i = 0; i < ds.Tables["KH"].Rows.Count; i++)
                {
                    if (ds.Tables["KH"].Rows[i]["SDT"].ToString().Trim() == txtSDT.Text)
                        drHDB["MaKH"] = ds.Tables["KH"].Rows[i]["MaKH"];
                }
            }
            else
                drHDB["MaKH"] = maKH;


            drHDB["TongTien"] = Double.Parse(txtTong.Text);
            ds.Tables["HDBan"].Rows.Add(drHDB);

            cmb = new SqlCommandBuilder(daHDB);
            daHDB.Update(ds, "HDBan");

            // cập nhật số lượng sản phẩm trong bảng sản phẩm   
            for(int i=0; i< dtSPChon.Rows.Count; i++)
            {
                string ma = dtSPChon.Rows[i][0].ToString();
                int soLuong = Int32.Parse(dtSPChon.Rows[i][2].ToString());
                DataRow drSP = ds.Tables["SP"].Rows.Find(ma);
                if (drSP != null)
                {
                    drSP[3] = Int32.Parse(drSP[3].ToString()) - soLuong;
                }
                cmb = new SqlCommandBuilder(daSP);
                daSP.Update(ds, "SP");

            }
            
            // thêm chi tiết hóa đơn
            
            for (int i = 0; i < dtSPChon.Rows.Count; i++)
            {
                DataRow drCTHD = ds.Tables["ChiTietHDBan"].NewRow();
                drCTHD["MaHD"] = txtSoHoaDon.Text;
                drCTHD["MaSP"] = dtSPChon.Rows[i]["Mã Sản Phẩm"];
                drCTHD["SoLuong"] = dtSPChon.Rows[i]["Số Lượng"];
                drCTHD["DonGia"] = dtSPChon.Rows[i]["Đơn Giá"];
                drCTHD["ThanhTien"] = dtSPChon.Rows[i]["Thành Tiền"];

                ds.Tables["ChiTietHDBan"].Rows.Add(drCTHD);
               
            }
            cmb = new SqlCommandBuilder(daCTHD);
            daCTHD.Update(ds, "ChiTietHDBan");

            MessageBox.Show("Thêm Hóa Đơn Thành Công");

           
        }

        private void btnInHD_Click(object sender, EventArgs e)
        {
            DataRow drHDBan = ds.Tables["HDBan"].Rows.Find(txtSoHoaDon.Text);
            if (drHDBan == null)
            {
                MessageBox.Show("Cần Thanh Toán Hóa Đơn Trước", "Thông Báo");
                return;
            }

            Report.HoaDonBan_Report rp = new Report.HoaDonBan_Report(soHD);
            rp.Show();
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            
            // reset
            
            reset();
            sinhMaHDBan();
            soHD = txtSoHoaDon.Text;
            dtSPChon.Clear();
            txtTenKH.Focus();
            CapNhatTong();
        }

        

        
    }
}

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
    public partial class NhapSP : Form
    {
        double tt = 0;
        SqlConnection conn;
        SqlDataAdapter da_SP;
        DataSet ds;
        DataTable dtSP;
        SqlDataAdapter daCTHD;
        public NhapSP()
        {
            InitializeComponent();
            ReadFileConn f = new ReadFileConn();
            String s = f.LoadConn();
            conn = new SqlConnection(s);
        }

        private void NhapSP_Load(object sender, EventArgs e)
        {
            ds = new DataSet();
            //Cbo_NCC
            string sqlncc = "Select * From NCC";
            SqlDataAdapter da_ncc = new SqlDataAdapter(sqlncc, conn);
            da_ncc.Fill(ds, "NCC");
            DataRow r = ds.Tables["NCC"].NewRow();
            r[0] = "0";
            r[1] = "Tất cả nhà cung cấp";
            ds.Tables["NCC"].Rows.InsertAt(r, 0);
            DataColumn[] keyNCC = new DataColumn[1];
            keyNCC[0] = ds.Tables["NCC"].Columns[0];
            ds.Tables["NCC"].PrimaryKey = keyNCC;

            cbo_NCC.DataSource = ds.Tables["NCC"];
            cbo_NCC.DisplayMember = "TenNCC";
            cbo_NCC.ValueMember = "MaNCC";
            //Cbo_MDanhMuc
            string sqldm = "select*from DanhMucSP";
            SqlDataAdapter da_dm = new SqlDataAdapter(sqldm, conn);
            da_dm.Fill(ds, "DanhMucSP");
            DataColumn[] keyDM = new DataColumn[1];
            keyDM[0] = ds.Tables["DanhMucSP"].Columns[0];
            ds.Tables["DanhMucSP"].PrimaryKey = keyDM;
            cbo_DanhMuc.DataSource = ds.Tables["DanhMucSP"];
            cbo_DanhMuc.DisplayMember = "TenDanhMuc";
            cbo_DanhMuc.ValueMember = "MaDanhMuc";
            //Cbo_NV
            string sqlnv = "select MaNV, TenNV from NV";
            SqlDataAdapter da_nv = new SqlDataAdapter(sqlnv, conn);
            da_nv.Fill(ds, "NV");
            DataColumn[] keyNV = new DataColumn[1];
            keyNV[0] = ds.Tables["NV"].Columns[0];
            ds.Tables["NV"].PrimaryKey = keyNV;
            cbo_NV.DataSource = ds.Tables["NV"];
            cbo_NV.DisplayMember = "TenNV";
            cbo_NV.ValueMember = "MaNV";
            //Loaddtgv
            LoadDtgvSP();
            if (chk_SP.Checked == true)
            {
                autoCode();
            }
            sinhMaHD();
            //Load HD Nhap
            String selectHDN = "select * from HDNhap";
            daHDN = new SqlDataAdapter(selectHDN, conn);
            daHDN.Fill(ds, "HDNhap");
            DataColumn[] keyHDB = new DataColumn[1];
            keyHDB[0] = ds.Tables["HDNhap"].Columns[0];
            ds.Tables["HDNhap"].PrimaryKey = keyHDB;
            //Load SP
            string selectSP = "select * from SP";
            da_SP = new SqlDataAdapter(selectSP, conn);
            da_SP.Fill(ds, "SP");
            DataColumn[] keySP = new DataColumn[1];
            keySP[0] = ds.Tables["SP"].Columns[0];
            ds.Tables["SP"].PrimaryKey = keySP;

            //Load CTHD
            string selectCTHD = "select * from ChiTietHDNhap";
            daCTHD = new SqlDataAdapter(selectCTHD, conn);
            daCTHD.Fill(ds, "ChiTietHDNhap");
            DataColumn[] keyCTHD = new DataColumn[2];
            keyCTHD[0] = ds.Tables["ChiTietHDNhap"].Columns[0];
            keyCTHD[1] = ds.Tables["ChiTietHDNhap"].Columns[1];
            ds.Tables["ChiTietHDNhap"].PrimaryKey = keyCTHD;
            txt_MaSP.Enabled = false;
        }
        private void LoadDtgvSP()
        {
            DataColumn[] keySP = new DataColumn[1];
            string sql = "select*from SP";
            da_SP = new SqlDataAdapter(sql, conn);
            ds = new DataSet();
            da_SP.Fill(ds, "SP");
            keySP[0] = ds.Tables["SP"].Columns[0];

            ds.Tables["SP"].PrimaryKey = keySP;
            dtgv_SP.DataSource = ds.Tables["SP"]; ;
            dtgv_SP.AllowUserToAddRows = false;
            dtgv_SP.ReadOnly = true;
        }
        private void LoadDtgvSPTheoNCC()
        {
            DataColumn[] keySP = new DataColumn[1];
            string sql = "select*from SP where MaNCC='" + cbo_NCC.SelectedValue.ToString() + "'";
            da_SP = new SqlDataAdapter(sql, conn);
            ds = new DataSet();
            da_SP.Fill(ds, "SP");
            keySP[0] = ds.Tables["SP"].Columns[0];
            ds.Tables["SP"].PrimaryKey = keySP;
            dtgv_SP.DataSource = ds.Tables["SP"]; ;
            dtgv_SP.AllowUserToAddRows = false;
            dtgv_SP.ReadOnly = true;
        }
        SqlDataAdapter daHDN;
        private void sinhMaHD()
        {
            dt = new DataTable();
            string sql = "select * from HDNhap";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);
            DataColumn[] keyHD = new DataColumn[1];
            keyHD[0] = dt.Columns[0];
            dt.PrimaryKey = keyHD;
            if (dt.Rows.Count == 0)
                txt_MaHD.Text = "HDN00001";
            else
            {
                string MaHDNstring = dt.Rows[dt.Rows.Count - 1][0].ToString().Trim();// lấy mã hóa đơn bán cuối cùng
                MaHDNstring = MaHDNstring.Substring(3);// cắt chuỗi từ vị trí số 3
                int sl = MaHDNstring.Length;// số lượng chuỗi vừa cắt
                int MaHDNSo = Int32.Parse(MaHDNstring) + 1;// tăng lên 1
                string string0 = "";// string chứa số 0
                for (int i = 0; i < sl - MaHDNSo.ToString().Length; i++)
                    string0 += "0";
                txt_MaHD.Text = "HDN" + string0 + MaHDNSo.ToString();
            }
            txt_MaHD.Enabled = false;
        }

        private void NhapSP_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dl = MessageBox.Show("Bạn có muốn thoát không?", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (dl == DialogResult.No)
                e.Cancel = true;
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_SoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
        DataTable dt;
        private void autoCode()
        {
            dt = new DataTable();
            String maDM = cbo_DanhMuc.SelectedValue.ToString().Trim();
            //MessageBox.Show(maDM);
            int maDMLenght = maDM.Length;
            //MessageBox.Show(maDMLenght.ToString());
            //string ma;
            String selectSPDM = "select * from SP where MaDanhMuc = '" + maDM + "'";
            //dtSPDM = Class.Functions.GetDataToTable(selectSPDM);
            SqlDataAdapter da_dmsp = new SqlDataAdapter(selectSPDM, conn);
            da_dmsp.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                txt_MaSP.Text = maDM + "001";
            }
            else
            {
                String maCuoi = dt.Rows[dt.Rows.Count - 1][0].ToString();// lay ma sp danh muc cuoi
                String maCuoiSoString = maCuoi.Substring(maDMLenght).Trim();
                int maCuoiSoStringLengt = maCuoiSoString.Length - 1;
                int maCuoiSo = Int32.Parse(maCuoiSoString);

                String string0 = string.Empty;
                for (int i = 0; i < maCuoiSoStringLengt; i++)
                    string0 += "0";
                txt_MaSP.Text = maDM + string0 + (maCuoiSo + 1).ToString().Trim();
            }

        }

        private void cbo_DanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chk_SP.Checked == true)
                autoCode();
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if (dtgv_NhapMoi.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                if (dtgv_NhapMoi.SelectedRows == null)
                {
                    MessageBox.Show("Bạn chưa chọn sản phẩm muốn xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    String maSP = string.Empty;
                    for (int i = dtgv_NhapMoi.SelectedRows.Count - 1; i >= 0; i--)
                    {
                        dtgv_NhapMoi.Rows.RemoveAt(dtgv_NhapMoi.SelectedRows[i].Index);
                    }
                }
            }
        }

        private void cbo_NCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbo_NCC.SelectedIndex > 0)
                LoadDtgvSPTheoNCC();
            else
                LoadDtgvSP();
        }

        private void dtgv_SP_Click(object sender, EventArgs e)
        {
            if (chk_SP.Checked == false)
            {
                if (dtgv_SP.SelectedRows.Count > 0)
                {
                    DataGridViewRow r = dtgv_SP.SelectedRows[0];
                    txt_MaSP.Text = r.Cells["MaSP"].Value.ToString();
                    txt_TenSP.Text = r.Cells["TenSP"].Value.ToString();
                    txt_SoLuong.Text = string.Empty;
                    txt_GiaBan.Text = r.Cells["GiaBan"].Value.ToString();
                    txt_GiaNhap.Text = r.Cells["GiaNhap"].Value.ToString();
                    txt_GhiChu.Text = r.Cells["GhiChu"].Value.ToString();
                    //cbo_NCC.SelectedValue = r.Cells["MaNCC"].Value.ToString();
                    cbo_DanhMuc.SelectedValue = r.Cells["MaDanhMuc"].Value.ToString();
                }
            }
        }
        private void tien()
        {
            double sl = double.Parse(txt_SoLuong.Text);
            double gnhap = double.Parse(txt_GiaNhap.Text);
            tt += sl * gnhap;
            txt_TongTien.Text = tt.ToString();
            tt = 0;

        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            if (txt_MaSP.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mã sản phẩm", "Thông báo");
                return;
            }
            if (txt_TenSP.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tên sản phẩm", "Thông báo");
                return;
            }
            if (txt_SoLuong.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập số lượng", "Thông báo");
                return;
            }
            if (txt_GiaNhap.Text == "")
            {
                MessageBox.Show("Bạn chưa điền giá nhập", "Thông báo");
                return;
            }
            if (txt_GiaBan.Text == "")
            {
                MessageBox.Show("Bạn chưa điền giá bán", "Thông báo");
                return;
            }


            if (cbo_NCC.SelectedIndex == 0)
            {
                MessageBox.Show("Bạn chưa chọn nhà cung cấp", "Thông báo");
                dtgv_NhapMoi.AllowUserToAddRows = false;
                return;
            }
            else
            {
                for (int i = 0; i < dtgv_NhapMoi.Rows.Count; i++)
                {
                    if (dtgv_NhapMoi.Rows[i].Cells[0].Value.ToString().Trim() == txt_MaSP.Text.Trim())
                    {
                        MessageBox.Show("Sản phẩm đã được chọn");
                        return;
                    }
                }
                dtgv_NhapMoi.Rows.Add(1);
                int indexRow = dtgv_NhapMoi.Rows.Count - 1;
                dtgv_NhapMoi[0, indexRow].Value = txt_MaSP.Text;
                dtgv_NhapMoi[1, indexRow].Value = cbo_NCC.SelectedValue.ToString();
                dtgv_NhapMoi[2, indexRow].Value = txt_TenSP.Text;
                dtgv_NhapMoi[3, indexRow].Value = txt_SoLuong.Text;
                dtgv_NhapMoi[4, indexRow].Value = txt_GiaNhap.Text;
                dtgv_NhapMoi[5, indexRow].Value = txt_GiaBan.Text;

                dtgv_NhapMoi[6, indexRow].Value = cbo_DanhMuc.SelectedValue.ToString();

                dtgv_NhapMoi[7, indexRow].Value = txt_GhiChu.Text;

                tien();
                txt_SoLuong.Clear();
                cbo_NCC.Enabled = false;
            }
        }
        DataTable dtCTHD;
        SqlCommandBuilder cmb;
        DataTable dtSPM;
        SqlDataAdapter daSPM;

        private void btn_ThanhToan_Click(object sender, EventArgs e)
        {
            if (dtgv_NhapMoi.Rows.Count <= 0)
            {
                MessageBox.Show("Bạn chưa nhập sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ///////////////////////////////////////////////////////////////////////Them HD
            dt = new DataTable();
            string sql = "select * from HDNhap";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);
            DataRow drHDN = dt.NewRow();
            drHDN["MaHDNhap"] = txt_MaHD.Text;
            drHDN["MaNV"] = cbo_NV.SelectedValue.ToString();
            drHDN["NgayNhap"] = dtp_NgayNhap.Value.ToString("dd/MM/yyyy");
            drHDN["MaNCC"] = cbo_NCC.SelectedValue.ToString();
            drHDN["TongTien"] = double.Parse(txt_TongTien.Text);
            dt.Rows.Add(drHDN);
            cmb = new SqlCommandBuilder(daHDN);
            daHDN.Update(dt);

            /////////////////////////////////////////////////////////////////////Them SP moi
            dtSPM = new DataTable();
            string sp = "select * from SP";
            daSPM = new SqlDataAdapter(sp, conn);
            daSPM.Fill(dtSPM);
            DataColumn[] keySP = new DataColumn[1];
            keySP[0] = dtSPM.Columns[0];
            dtSPM.PrimaryKey = keySP;
            for (int i = 0; i < dtgv_NhapMoi.Rows.Count; i++)
            {
                string ma = dtgv_NhapMoi.Rows[i].Cells[0].Value.ToString();

                DataRow drSP = dtSPM.Rows.Find(ma);
                if (drSP != null)
                {
                    int soLuong = Int32.Parse(dtgv_NhapMoi.Rows[i].Cells[3].Value.ToString());
                    drSP[3] = Int32.Parse(drSP[3].ToString()) + soLuong;
                }
                else
                {
                    DataRow dr_SP = dtSPM.NewRow();

                    string msp = dtgv_NhapMoi.Rows[i].Cells[0].Value.ToString();
                    string mcc = dtgv_NhapMoi.Rows[i].Cells[1].Value.ToString();
                    string tensp = dtgv_NhapMoi.Rows[i].Cells[2].Value.ToString();
                    string solg = dtgv_NhapMoi.Rows[i].Cells[3].Value.ToString();
                    string gn = dtgv_NhapMoi.Rows[i].Cells[4].Value.ToString();
                    string gb = dtgv_NhapMoi.Rows[i].Cells[5].Value.ToString();
                    string mdm = dtgv_NhapMoi.Rows[i].Cells[6].Value.ToString();
                    string gc = dtgv_NhapMoi.Rows[i].Cells[7].Value.ToString();
                    dr_SP["MaSP"] = msp;
                    dr_SP["MaNCC"] = mcc;
                    dr_SP["TenSP"] = tensp;
                    dr_SP["SoLuong"] = int.Parse(solg);
                    dr_SP["GiaNhap"] = double.Parse(gn);
                    dr_SP["GiaBan"] = double.Parse(gb);
                    dr_SP["MaDanhMuc"] = mdm;
                    dr_SP["GhiChu"] = gc;


                    dtSPM.Rows.Add(dr_SP);
                }


            }
            cmb = new SqlCommandBuilder(daSPM);
            daSPM.Update(dtSPM);

            //////////////////////////////////////////////////////////////////////Them ChiTietHD

            dtCTHD = new DataTable();
            string hd = "select * from ChiTietHDNhap";
            daCTHD = new SqlDataAdapter(hd, conn);
            daCTHD.Fill(dtCTHD);

            DataColumn[] keyCTHD = new DataColumn[2];
            keyCTHD[0] = dtCTHD.Columns[0];
            keyCTHD[1] = dtCTHD.Columns[1];
            dtCTHD.PrimaryKey = keyCTHD;

            for (int i = 0; i < dtgv_NhapMoi.Rows.Count; i++)
            {

                DataRow drCTHD = dtCTHD.NewRow();
                int sl = Int32.Parse(dtgv_NhapMoi.Rows[i].Cells[3].Value.ToString());
                double gia = double.Parse(dtgv_NhapMoi.Rows[i].Cells[4].Value.ToString());

                drCTHD["MaHDNhap"] = txt_MaHD.Text;
                drCTHD["MaSP"] = dtgv_NhapMoi.Rows[i].Cells[0].Value.ToString();
                drCTHD["SoLuong"] = Int32.Parse(dtgv_NhapMoi.Rows[i].Cells[3].Value.ToString());
                drCTHD["DonGia"] = Double.Parse(dtgv_NhapMoi.Rows[i].Cells[4].Value.ToString());
                drCTHD["ThanhTien"] = sl * gia;
                // MessageBox.Show((sl * gia).ToString());

                dtCTHD.Rows.Add(drCTHD);

            }
            cmb = new SqlCommandBuilder(daCTHD);
            daCTHD.Update(dtCTHD);

            MessageBox.Show("Thanh Toán Thành Công");


            //MessageBox.Show("Thêm Hóa Đơn Thành Công");

            sinhMaHD();
            LoadDtgvSP();
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            txt_MaSP.Clear();
            txt_TenSP.Clear();
            txt_SoLuong.Clear();
            //cbo_NCC.SelectedIndex = 0;
            txt_GiaNhap.Clear();
            txt_GiaBan.Clear();
            txt_GhiChu.Clear();
        }

        private void chk_SP_CheckedChanged(object sender, EventArgs e)
        {
            txt_MaSP.Clear();
            txt_TenSP.Clear();
            txt_SoLuong.Clear();
            txt_MaSP.Enabled = false;
            autoCode();
            txt_GiaNhap.Clear();
            txt_GiaBan.Clear();
            txt_GhiChu.Clear();
        }

        private void btn_CapNhat_Click(object sender, EventArgs e)
        {
            dtgv_NhapMoi.Rows.Clear();
            txt_TongTien.Clear();
            cbo_NCC.Enabled = true;
        }
    }
}

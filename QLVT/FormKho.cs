using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormKho : Form
    {
        string macn = "";
        int vitri = 0;
        public FormKho()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // current
            txtMaKho.Enabled = true;
            vitri = bds_Kho.Position;
            panelControl2.Enabled = true;
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void khoBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bds_Kho.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DSKHO);

        }

        private void FormKho_Load(object sender, EventArgs e)
        {
            DSKHO.EnforceConstraints = false;
            this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
            this.khoTableAdapter.Fill(this.DSKHO.Kho);
            
            this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.datHangTableAdapter.Fill(this.DSKHO.DatHang);

            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.DSKHO.PhieuNhap);

            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuXuatTableAdapter.Fill(this.DSKHO.PhieuXuat);

            /*            macn = ((DataRowView)bds_Kho[0])["MaCN"].ToString();*/
            macn = "CN" + Program.mChinhNhanh.ToString();
            Console.WriteLine(macn);
            // Van con loi, tu xu li, thay k sua, thay se check khi thi
            cbChiNhanh.DataSource = Program.bds_dspm;
            cbChiNhanh.DisplayMember = "TENCN";
            cbChiNhanh.ValueMember = "TENSERVER";
            cbChiNhanh.SelectedIndex = Program.mChinhNhanh;

            if (Program.mGroup == "CONGTY")
            {
                cbChiNhanh.Enabled = true;
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = false;

            }
            else
            {
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = true;
                cbChiNhanh.Enabled = false;
            }
            btnReload.Enabled = true;
            btnGhi.Enabled = false;
            btnHuy.Enabled = false;
        }

        private void mACNTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void mAKHOLabel_Click(object sender, EventArgs e)
        {

        }

        private void mACNLabel_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace QLVT
{
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FormMain()
        {
            InitializeComponent();

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void barEditItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }


        private void DangXuat()
        {
            foreach (Form f in this.MdiChildren)
            {
                f.Close();
                f.Dispose();
            }
            MANV.Text = "MANV";
            HOTEN.Text = "HOTEN";
            NHOM.Text = "NHOM";
            Program.conn.Close();
            GC.Collect();
            GC.WaitForFullGCApproach();
            GC.WaitForFullGCComplete();
            GC.WaitForPendingFinalizers();
            GC.Collect();

        }


        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form form = CheckExists(typeof(FormDangNhap));
            {
                if (form != null) form.Activate();
                else
                {

                    FormDangNhap f = new FormDangNhap();
                    f.MdiParent = this;
                    f.Show();
                }
            }
        }

        public void HienThiMenu()
        {
            MANV.Text = "Mã NV: " + Program.username;
            HOTEN.Text = "Họ tên nhân viên: " + Program.mHoTen;
            NHOM.Text = "Nhóm: " + Program.mGroup;
            // Phân quyền
            ribbonPageBaoCao.Visible = ribbonPageNhapXuat.Visible= btnTaoTaiKhoan.Enabled = ribbonPageNghiepVu.Visible = btnTaoTaiKhoan.Enabled = true;
            barButtonItemDangXuat.Enabled = true;
            btnDangNhap.Enabled = false;
            /// tiep tuc phan quuyen ....
            if (Program.mGroup == "USER" )
            {
                btnTaoTaiKhoan.Enabled = false;
            } else {
                btnTaoTaiKhoan.Enabled = true;

            }
        }
 

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form form = this.CheckExists(typeof(FormTaoTaiKhoan));
            if (Program.mlogin != Program.mloginDN)
            {
                MessageBox.Show("Bạn không có quyền tạo tài khoản ở site khác, vui lòng chọn lại site của bạn", "", MessageBoxButtons.OK);
                if (form != null) form.Close();
            }
            else
            {
                if (form != null) form.Activate();
                else
                {
                    FormTaoTaiKhoan f = new FormTaoTaiKhoan();
                    f.MdiParent = this;
                    f.Show();
                }
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DangXuat();
            ribbonPageBaoCao.Visible = ribbonPageNhapXuat.Visible = ribbonPageNghiepVu.Visible = false;
            barButtonItemDangXuat.Enabled = btnTaoTaiKhoan.Enabled = false;
            btnDangNhap.Enabled = true;
           

        }

        private void strip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        
        private void barButtonItem3_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form form = this.CheckExists(typeof(FormNhanVien));
            {
                if (form != null) form.Activate();
                else
                {
                    FormNhanVien f = new FormNhanVien();
                    f.MdiParent = this;
                    f.Show();
                }
            }
        }

        private void barButtonItem7_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form form = this.CheckExists(typeof(FormKho));
            {
                if (form != null) form.Activate();
                else
                {
                    FormKho f = new FormKho();
                    f.MdiParent = this;
                    f.Show();
                }
            }
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form form = this.CheckExists(typeof(FormPhieuXuat));
            {
                if (form != null) form.Activate();
                else
                {
                    FormPhieuXuat f = new FormPhieuXuat();
                    f.MdiParent = this;
                    f.Show();
                }
            }
        }
    }
}

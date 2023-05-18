using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Data.Helpers.FindSearchRichParser;

namespace QLVT
{
    public partial class FormPhieuXuat : Form
    {
        string macn = "";
        int vitri = 0;

        bool check_them = true;

        String mapx;
        DateTime ngay;
        String hotenKH;
        int manv;
        String makho;
        String mavattu;
        int soluong;
        float dongia;

        Stack<String> stack = new Stack<String>();
        Stack<String> stack2 = new Stack<String>();

        public FormPhieuXuat()
        {
            InitializeComponent();
        }

        private void phieuXuatBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.phieuXuatBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DSPHIEUXUAT);

        }

        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }
        private void FormPhieuXuat_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'DSPHIEUXUAT.NhanVien' table. You can move, or remove it, as needed.
            
            // TODO: This line of code loads data into the 'DSPHIEUXUAT.vw_DSNV' table. You can move, or remove it, as needed.

            DSPHIEUXUAT.EnforceConstraints = false;
            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuXuatTableAdapter.Fill(this.DSPHIEUXUAT.PhieuXuat);
            this.CTPXTableAdapter.Connection.ConnectionString = Program.connstr;
            this.CTPXTableAdapter.Fill(this.DSPHIEUXUAT.CTPX);
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.DSPHIEUXUAT.NhanVien);
            macn = ((DataRowView)nhanVienBindingSource[0])["MaCN"].ToString();

            // Van con loi, tu xu li, thay k sua, thay se check khi thi
            cbChiNhanh.DataSource = Program.bds_dspm;
            cbChiNhanh.DisplayMember = "TENCN";
            cbChiNhanh.ValueMember = "TENSERVER";
            cbChiNhanh.SelectedIndex = Program.mChinhNhanh;

            cbChiNhanh.SelectedIndex = Program.mChinhNhanh;
            if (Program.mGroup == "CONG TY")
            {
                cbChiNhanh.Enabled = true;
                btnThemPX.Enabled = btnXoaPX.Enabled = btnSuaPX.Enabled = false;

            }
            else
            {
                btnThemPX.Enabled = btnXoaPX.Enabled = btnSuaPX.Enabled = true;
                cbChiNhanh.Enabled = false;
            }
            btnReloadPX.Enabled = true;
            btnGhiPX.Enabled = false;
            btnHuyPX.Enabled = false;

            // bat tat phan quyen - chua phan quyen cho nhom khác


        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Form form = this.CheckExists(typeof(FormCTPhieuXuat));
            //{
            //    if (form != null) form.Activate();
            //    else
            //    {
            //        FormCTPhieuXuat f = new FormCTPhieuXuat();
            //        f.MdiParent = (FormMain)this.ParentForm;
            //        f.Show();
            //    }
            //}

            NGAY.Visible = HOTEN.Visible = MANV.Visible = MAKHO.Visible = phieuXuatGridControl.Enabled = !MAKHO.Visible;
            lbNGAY.Visible = lbHOTEN.Visible = lbMANV.Visible = lbMAKHO.Visible  = !lbMAKHO.Visible;
            MAVT.Visible = SOLUONG.Visible = DONGIA.Visible = cTPXGridControl.Enabled = !DONGIA.Visible;
            lbMAVT.Visible = lbSOLUONG.Visible = lbDONGIA.Visible = !lbDONGIA.Visible;
            barButtonPX.Caption = MANV.Visible == true ? "Chi tiết phiếu xuất" : "Phiếu xuất";
            barButtonPX.ItemAppearance.Normal.BackColor = MANV.Visible == true ? Color.Pink : Color.Tan;
            panelControl2.Enabled = false;

        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void phieuXuatGridControl_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private string tuDongTangMa(string ma, int sochucaidau)
        {

            string m = ma.Substring(0, sochucaidau);
            int so = int.Parse(ma.Substring(sochucaidau)) + 1;
            if (so <= 9)
                return m + '0' + so.ToString();
            return m + so.ToString();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barButtonPX.Caption == "Chi tiết phiếu xuất")
            {
                btnThemPX.Enabled = btnXoaPX.Enabled = btnSuaPX.Enabled = btnReloadPX.Enabled = btnThoatPX.Enabled = btnPhucHoiPX.Enabled = false;
                btnGhiPX.Enabled = btnHuyPX.Enabled = true;
                phieuXuatGridControl.Enabled = false;
                check_them = true;
                panelControl2.Enabled = true;
                vitri = phieuXuatBindingSource.Position;
                panelControl2.Enabled = true;
                phieuXuatBindingSource.AddNew();
                string getMaxIdQuery = "EXEC [dbo].[sp_Get_Max_Id_Char] 'PHIEUXUAT', 'MAPX'";
                string maphieuxxuat = "";
                MANV.Text = Program.username.ToString();
                Console.WriteLine(getMaxIdQuery);
                try
                {
                    Program.myReader = Program.ExecSqlDataReader(getMaxIdQuery);
                    if (Program.myReader == null) { return; }
                    Program.myReader.Read();
                    if (Program.myReader.GetString(0) == "NULL")
                    {
                        MAPX.Text = "PX01";
                        Program.myReader.Close();
                    }
                    else
                    {
                        maphieuxxuat = tuDongTangMa(Program.myReader.GetString(0), 2);
                        MAPX.Text = maphieuxxuat;
                        Program.myReader.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối! " + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    return;
                }

            }
            else
            {
                btnThemPX.Enabled = btnXoaPX.Enabled = btnSuaPX.Enabled = btnReloadPX.Enabled = btnThoatPX.Enabled = btnPhucHoiPX.Enabled = false;
                btnGhiPX.Enabled = btnHuyPX.Enabled = true;
                check_them = true;
                vitri = CTPXBindingSource.Position;
                panelControl2.Enabled = true;
                cTPXGridControl.Enabled = false;
                CTPXBindingSource.AddNew();
            } 
                
            
         }    
        
 

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
            this.Close();
        }

        private void panelControl4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barButtonPX.Caption == "Chi tiết phiếu xuất")
            {
                phieuXuatBindingSource.CancelEdit();
                if (btnThemPX.Enabled == false)
                {
                    phieuXuatBindingSource.Position = vitri;
                }

                btnThemPX.Enabled = btnXoaPX.Enabled = btnSuaPX.Enabled = btnReloadPX.Enabled = btnThoatPX.Enabled = true;
                btnGhiPX.Enabled = btnHuyPX.Enabled = false;
                phieuXuatGridControl.Enabled = true;
                this.phieuXuatTableAdapter.Fill(this.DSPHIEUXUAT.PhieuXuat);
            }
            else
            {
                CTPXBindingSource.CancelEdit();
                if (btnThemPX.Enabled == false)
                {
                    CTPXBindingSource.Position = vitri;
                }

                btnThemPX.Enabled = btnXoaPX.Enabled = btnSuaPX.Enabled = btnReloadPX.Enabled = btnThoatPX.Enabled = true;
                btnGhiPX.Enabled = btnHuyPX.Enabled = false;
                cTPXGridControl.Enabled = true;
                panelControl2.Enabled = false;
                this.CTPXTableAdapter.Fill(this.DSPHIEUXUAT.CTPX);
            }    
            
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.phieuXuatTableAdapter.Fill(this.DSPHIEUXUAT.PhieuXuat);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload !" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barButtonPX.Caption == "Chi tiết phiếu xuất")
            {
                vitri = phieuXuatBindingSource.Position;
                DataRowView dt = ((DataRowView)phieuXuatBindingSource[phieuXuatBindingSource.Position]);
                mapx = dt["MAPX"].ToString();
                ngay = (DateTime)dt["NGAY"];
                hotenKH = dt["HOTENKH"].ToString();
                manv = (int)dt["MANV"];
                makho = dt["MAKHO"].ToString();
                panelControl2.Enabled = true;
                btnThemPX.Enabled = btnXoaPX.Enabled = btnSuaPX.Enabled = btnReloadPX.Enabled = btnThoatPX.Enabled = btnPhucHoiPX.Enabled = false;
                btnGhiPX.Enabled = btnHuyPX.Enabled = true;
                phieuXuatGridControl.Enabled = false;
                check_them = false;
            }
            else
            {
                vitri = CTPXBindingSource.Position;
                DataRowView dt = ((DataRowView)CTPXBindingSource[CTPXBindingSource.Position]);
                mapx = dt["MAPX"].ToString();
                mavattu = dt["MAVT"].ToString();
                soluong = int.Parse(dt["SOLUONG"].ToString());
                dongia = float.Parse(dt["DONGIA"].ToString());
                panelControl2.Enabled = true;
                btnThemPX.Enabled = btnXoaPX.Enabled = btnSuaPX.Enabled = btnReloadPX.Enabled = btnThoatPX.Enabled = btnPhucHoiPX.Enabled = false;
                btnGhiPX.Enabled = btnHuyPX.Enabled = true;
                cTPXGridControl.Enabled = false;
                check_them = false;
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barButtonPX.Caption == "Chi tiết phiếu xuất")
            {
                if (CTPXBindingSource.Count > 0)

                {
                    MessageBox.Show("Không thể xóa phiếu xuất này vì đã có chi tiết phiếu xuất!", "", MessageBoxButtons.OK);
                    return;
                }

                if (MessageBox.Show("Bạn có thực sự muốn xóa phiếu xuất này!", "Xác nhận", MessageBoxButtons.OKCancel)
                   == DialogResult.OK)
                {
                    try
                    {
                        DataRowView dt = ((DataRowView)phieuXuatBindingSource[phieuXuatBindingSource.Position]);
                        mapx = dt["MAPX"].ToString();
                        ngay = (DateTime)dt["NGAY"];
                        hotenKH = dt["HOTENKH"].ToString();
                        manv = (int)dt["MANV"];
                        makho = dt["MAKHO"].ToString();

                        phieuXuatBindingSource.RemoveCurrent();
                        this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.phieuXuatTableAdapter.Update(this.DSPHIEUXUAT.PhieuXuat);
                        String query = string.Format("INSERT INTO DBO.PHIEUXUAT(MAPX,NGAY,HOTENKH,MANV,MAKHO) " +
                                                    " VALUES('{0}','{1}',N'{2}',{3}, '{4}')", mapx, ngay, hotenKH, manv, makho);
                        Console.WriteLine(query);
                        stack.Push(query);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa phiếu xuất. Bạn hãy xóa lại \n" + ex.Message, "", MessageBoxButtons.OK);
                        this.phieuXuatTableAdapter.Fill(this.DSPHIEUXUAT.PhieuXuat);
                        phieuXuatBindingSource.Position = phieuXuatBindingSource.Find("MAPX", mapx);
                        return;
                    }
                }
                if (phieuXuatBindingSource.Count == 0)
                {
                    btnXoa.Enabled = false;
                }
                btnGhiPX.Enabled = btnHuyPX.Enabled = false;
            }
        else
            {

                if (MessageBox.Show("Bạn có thực sự muốn xóa chi tiết phiếu xuất này!", "Xác nhận", MessageBoxButtons.OKCancel)
                   == DialogResult.OK)
                {
                    try
                    {
                        DataRowView dt = ((DataRowView)CTPXBindingSource[CTPXBindingSource.Position]);
                        mapx = dt["MAPX"].ToString();
                        mavattu = dt["MAVT"].ToString();
                        soluong = int.Parse(dt["SOLUONG"].ToString());
                        dongia = float.Parse(dt["SOLUONG"].ToString());

                        CTPXBindingSource.RemoveCurrent();
                        this.CTPXTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.CTPXTableAdapter.Update(this.DSPHIEUXUAT.CTPX);
                        String query = string.Format("INSERT INTO DBO.CTPX(MAPX,MAVT,SOLUONG,DONGIA) " +
                                                    " VALUES('{0}','{1}',{2},{3})", mapx, mavattu, soluong, dongia);
                        Console.WriteLine(query);
                        stack2.Push(query);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa chi tiết phiếu xuất. Bạn hãy xóa lại \n" + ex.Message, "", MessageBoxButtons.OK);
                        this.CTPXTableAdapter.Fill(this.DSPHIEUXUAT.CTPX);
                        this.CTPXTableAdapter.Fill(this.DSPHIEUXUAT.CTPX);
                        return;
                    }
                }
                if (CTPXBindingSource.Count == 0)
                {
                    btnXoa.Enabled = false;
                }
                btnGhiPX.Enabled = btnHuyPX.Enabled = false;
            }    
     }

        private void validatePhieuXuat()
        {
            if (NGAY.Text.Trim() == "")
            {
                MessageBox.Show("Ngày tạp phiếu xuất không được để trống!", "", MessageBoxButtons.OK);
                NGAY.Focus();
                return;
            }
            if (HOTEN.Text.Trim() == "")
            {
                MessageBox.Show("Họ tên khách hàng không được để trống!", "", MessageBoxButtons.OK);
                HOTEN.Focus();
                return;
            }
            if (HOTEN.Text.Length > 100)
            {
                MessageBox.Show("Họ tên khách hàng không thể lớn hơn 100 kí tự", "Thông báo", MessageBoxButtons.OK);
                HOTEN.Focus();
                return;
            }

            if (Regex.IsMatch(HOTEN.Text, @"^[a-zA-Z ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễếệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+$") == false)
            {
                MessageBox.Show("Họ chỉ có chữ cái và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                HOTEN.Focus();
                return;
            }

            if (MAKHO.Text.Trim() == "")
            {
                MessageBox.Show("Mã kho không được để trống!", "", MessageBoxButtons.OK);
                MAKHO.Focus();
                return;
            }


            if (MAKHO.Text.Length > 4)
            {
                MessageBox.Show("Mã kho không thể lớn hơn 4 kí tự", "Thông báo", MessageBoxButtons.OK);
                MAKHO.Focus();
                return;
            }

        }
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barButtonPX.Caption == "Chi tiết phiếu xuất")
            {

                validatePhieuXuat();

                if (check_them == true)
                {
                    String checkpx =
                      "EXEC [dbo].[sp_Check_Exists_Id_Char] 'PHIEUXUAT', 'MAPX' ,'" + MAPX.Text.Trim() + "'";
                    // string getMaxIdQuery = "EXEC [dbo].[sp_Get_Max_Id_Char] 'PHIEUXUAT', 'MAPX'";
                    Console.WriteLine(checkpx);
                    try
                    {
                        Program.myReader = Program.ExecSqlDataReader(checkpx);
                        if (Program.myReader == null) { return; }
                        Program.myReader.Read();
                        if (Program.myReader.GetInt32(0) == 1)
                        {

                            MAPX.Text = tuDongTangMa(MAPX.Text.Trim(), 2);
                            MessageBox.Show("Mã đã tồn tại! Hệ thống đã tự động đổi mã cho bạn rồi, vui lòng xác nhận lại!", "Thông báo", MessageBoxButtons.OK);
                            Program.myReader.Close();
                            return;
                        }
                        else
                        {
                            Program.myReader.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi kết nối! " + ex.Message, "Thông báo", MessageBoxButtons.OK);
                        return;
                    }

                }

                try
                {
                    String query = "";
                    phieuXuatBindingSource.EndEdit();
                    phieuXuatBindingSource.ResetCurrentItem();
                    this.phieuXuatTableAdapter.Update(this.DSPHIEUXUAT.PhieuXuat);

                    if (check_them)
                    {

                        query = "DELETE DBO.PHIEUXUAT WHERE MAPX = '" + MAPX.Text.Trim() + "'";

                    }
                    else
                    {
                        query = "UPDATE DBO.PHIEUXUAT " +
                               "SET " +
                               "MAPX = '" + mapx + "'," +
                               "NGAY = '" + ngay.ToString() + "'," +
                               "HOTENKH = N'" + hotenKH + "'," +
                               "MANV = " + manv + "," +
                               "MAKHO = '" + makho + "'" +
                               "WHERE MAPX = '" + mapx + "'";

                    }
                    Console.WriteLine(query);

                    stack.Push(query);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi ghi phiếu xuất \n" + ex.Message, "", MessageBoxButtons.OK);
                    return;
                }
                btnThemPX.Enabled = btnSuaPX.Enabled = btnXoaPX.Enabled = btnThoatPX.Enabled = btnReloadPX.Enabled = btnPhucHoiPX.Enabled = true;
                btnGhiPX.Enabled = btnHuyPX.Enabled = false;
                panelControl2.Enabled = false;
                phieuXuatGridControl.Enabled = true;
            }
            else
            {
                if (check_them == true)
                {
                    String checkvt =
                      "EXEC [dbo].[sp_Check_Exists_Id_Char] 'VATTU', 'MAVT' ,'" + MAVT.Text.Trim() + "'";
                    // string getMaxIdQuery = "EXEC [dbo].[sp_Get_Max_Id_Char] 'PHIEUXUAT', 'MAPX'";
                    Console.WriteLine(checkvt);
                    try
                    {
                        Program.myReader = Program.ExecSqlDataReader(checkvt);
                        if (Program.myReader == null) { return; }
                        Program.myReader.Read();
                        if (Program.myReader.GetInt32(0) == 0)
                        {

                            MessageBox.Show("Mã vật tư không tồn tại ", "Thông báo", MessageBoxButtons.OK);
                            Program.myReader.Close();
                            return;
                        }
                        else
                        {
                            Program.myReader.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi kết nối! " + ex.Message, "Thông báo", MessageBoxButtons.OK);
                        return;
                    }


                }

                try
                {
                    String query = "";
                    CTPXBindingSource.EndEdit();
                    CTPXBindingSource.ResetCurrentItem();
                    this.CTPXTableAdapter.Update(this.DSPHIEUXUAT.CTPX);

                    if (check_them)
                    {

                        query = "DELETE DBO.CTPX WHERE MAPX = '" + MAPX.Text.Trim() + "' AND MAVT = '" + MAVT.Text.ToString() + "'";

                    }
                    else
                    {
                        query = "UPDATE DBO.CTPX " +
                               "SET " +
                               "MAPX = '" + mapx + "'," +
                               "MAVT = '" + mavattu + "'," +
                               "SOLUONG = " + soluong + "," +
                               "DONGIA = " + dongia + " " +
                               "WHERE MAPX = '" + MAPX.Text.Trim() + "' AND MAVT = '" + MAVT.Text.ToString() + "'";

                    }
                    Console.WriteLine(query);

                    stack2.Push(query);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi ghi chi tiết phiếu xuất \n" + ex.Message, "", MessageBoxButtons.OK);
                    return;
                }
                btnThemPX.Enabled = btnSuaPX.Enabled = btnXoaPX.Enabled = btnThoatPX.Enabled = btnReloadPX.Enabled = btnPhucHoiPX.Enabled = true;
                btnGhiPX.Enabled = btnHuyPX.Enabled = false;
                panelControl2.Enabled = false;
                cTPXGridControl.Enabled = true;
            }    

            
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barButtonPX.Caption == "Chi tiết phiếu xuất")
            {
                if (stack.Count == 0)
                {
                    MessageBox.Show("Không có gì để phục hồi!", "", MessageBoxButtons.OK);
                    ; return;
                }


                string query = stack.Pop();
                Program.ExecSqlNonQuery(query);

                this.phieuXuatTableAdapter.Fill(this.DSPHIEUXUAT.PhieuXuat);

                phieuXuatGridControl.Enabled = true;
                btnThemPX.Enabled = btnXoaPX.Enabled = btnSuaPX.Enabled = btnReloadPX.Enabled = btnThoatPX.Enabled = btnPhucHoiPX.Enabled = true;
                btnGhiPX.Enabled = btnHuyPX.Enabled = false;
                phieuXuatGridControl.Enabled = true;
            }
            else
            {
                if (stack2.Count == 0)
                {
                    MessageBox.Show("Không có gì để phục hồi!", "", MessageBoxButtons.OK);
                    ; return;
                }


                string query = stack2.Pop();
                Program.ExecSqlNonQuery(query);

                this.CTPXTableAdapter.Fill(this.DSPHIEUXUAT.CTPX);

                btnThemPX.Enabled = btnXoaPX.Enabled = btnSuaPX.Enabled = btnReloadPX.Enabled = btnThoatPX.Enabled = btnPhucHoiPX.Enabled = true;
                btnGhiPX.Enabled = btnHuyPX.Enabled = false;
                cTPXGridControl.Enabled = true;
            }    
        }
    }
}

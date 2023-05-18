using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormNhanVien : Form

    {
        string macn = "";
        int vitri = 0;

        bool check_them = true;

        Stack<string> stack = new Stack<string>();

        int manv;
        String ho;
        String ten;
        String cmnd;
        String diachi;
        DateTime? ngaysinh;
        float luong;
        int trangthaixoa = 0;


        public FormNhanVien()
        {
            InitializeComponent();
        }
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        private string ToString(DateTime? dt, string format)
        {
            return dt == null ? "NULL" : "'" +((DateTime)dt).ToString(format) +"'";
        }

       
        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            
            DS1.EnforceConstraints = false;
       
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.DS1.NhanVien);

            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuXuatTableAdapter.Fill(this.DS1.PhieuXuat);

            this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.datHangTableAdapter.Fill(this.DS1.DatHang);

            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.DS1.PhieuNhap);

            macn = ((DataRowView)nhanVienBindingSource[0])["MaCN"].ToString();

          

            // Van con loi, tu xu li, thay k sua, thay se check khi thi
            cbChiNhanh.DataSource = Program.bds_dspm;
            cbChiNhanh.DisplayMember = "TENCN";
            cbChiNhanh.ValueMember = "TENSERVER";
            cbChiNhanh.SelectedIndex = Program.mChinhNhanh;
            if (Program.mGroup == "CONGTY")
            {
                cbChiNhanh.Enabled = true;
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled= false;
      
            }
            else
            {
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled  = true;
                cbChiNhanh.Enabled = false;
            }
            btnReload.Enabled = true;
            btnGhi.Enabled = false;
            btnHuy.Enabled = false;

            // bat tat phan quyen - chua phan quyen cho nhom khác
        }


        private void nhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.nhanVienBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS1);

        }

        private void nhanVienGridControl_Click(object sender, EventArgs e)
        {

        }

        private void dIACHITextEdit_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void mACNTextEdit_EditValueChanged(object sender, EventArgs e)
        {

        }


        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            tbMaNV.Enabled = true;
            vitri = nhanVienBindingSource.Position;
            panelControl2.Enabled = true;
            nhanVienBindingSource.AddNew();
            teMaCN.Text = macn;
            deNgaySinh.EditValue = "";
            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = btnPhucHoi.Enabled= false;
            btnGhi.Enabled = btnHuy.Enabled = true;
            nhanVienGridControl.Enabled = false;
            check_them = true;
        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (stack.Count == 0)
            {
                MessageBox.Show("Không có gì để phục hồi!", "", MessageBoxButtons.OK);
            ;   return;
            }
            
  
            string query  = stack.Pop();
            Program.ExecSqlNonQuery(query);

            this.nhanVienTableAdapter.Fill(this.DS1.NhanVien);

            nhanVienGridControl.Enabled = true;
            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = btnPhucHoi.Enabled = true;
            btnGhi.Enabled = btnHuy.Enabled = false;
            nhanVienGridControl.Enabled = true;
           
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            tbMaNV.Enabled = false;
            vitri = nhanVienBindingSource.Position;
            DataRowView dt = ((DataRowView)nhanVienBindingSource[nhanVienBindingSource.Position]);
            manv = int.Parse(dt["MANV"].ToString());
            ho = dt["HO"].ToString();
            ten = dt["TEN"].ToString();
            cmnd = dt["SOCMND"].ToString();
            diachi = dt["DIACHI"].ToString();
            ngaysinh = dt["NGAYSINH"].ToString() == "" ? null : (DateTime?)dt["NGAYSINH"];
            luong = float.Parse(dt["LUONG"].ToString());
            macn = dt["MACN"].ToString();
            trangthaixoa = 0;
            if (dt["TrangThaiXoa"].ToString() == "true") trangthaixoa = 1;
            panelControl2.Enabled = true;
            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = btnPhucHoi.Enabled = false;
            btnGhi.Enabled = btnHuy.Enabled = true;
            nhanVienGridControl.Enabled = false;
            check_them = false;       
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.nhanVienTableAdapter.Fill(this.DS1.NhanVien);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi Reload !" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (datHangBindingSource.Count > 0)
       
            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập đơn hàng!" , "", MessageBoxButtons.OK);
              
                return;
            }
            if (phieuNhapBindingSource.Count > 0)

            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập phiếu nhập!", "", MessageBoxButtons.OK);
            
                return;

            }
            if (phieuXuatBindingSource.Count > 0)

            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập phiếu xuất!", "", MessageBoxButtons.OK);
               
                return;

            }
            if (MessageBox.Show("Bạn có thực sự muốn xóa nhân viên này!", "Xác nhận", MessageBoxButtons.OKCancel) 
                == DialogResult.OK)
            {
                try
                {
                    DataRowView dt = ((DataRowView)nhanVienBindingSource[nhanVienBindingSource.Position]);
                    manv = int.Parse(dt["MANV"].ToString());
                    ho = dt["HO"].ToString();
                    ten = dt["TEN"].ToString();
                    cmnd = dt["SOCMND"].ToString();
                    diachi = dt["DIACHI"].ToString();
                    ngaysinh = dt["NGAYSINH"].ToString() == "" ? null : (DateTime?)dt["NGAYSINH"];
                    luong = float.Parse(dt["LUONG"].ToString());
                    macn = dt["MACN"].ToString(); ;
                    trangthaixoa = 0;
                    if (dt["TrangThaiXoa"].ToString() == "true") trangthaixoa = 1;

                    nhanVienBindingSource.RemoveCurrent();
                    this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.nhanVienTableAdapter.Update(this.DS1.NhanVien);
                    string query = string.Format("INSERT INTO DBO.NHANVIEN(MANV,HO,TEN,SOCMND,DIACHI,NGAYSINH,LUONG,MACN, TrangThaiXoa) " +
                                                " VALUES({0},'{1}','{2}','{3}', N'{4}' ,{5}, {6},'{7}', {8})", manv, ho, ten,cmnd, diachi, ToString(ngaysinh, "yyyy-MM-dd"), luong, macn, trangthaixoa);
                    Console.WriteLine(query);
                    stack.Push(query);
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Lỗi xóa nhân viên. Bạn hãy xóa lại \n" + ex.Message, "", MessageBoxButtons.OK);
                    this.nhanVienTableAdapter.Fill(this.DS1.NhanVien);
                    nhanVienBindingSource.Position = nhanVienBindingSource.Find("MANV", manv);
                    return;
                }
            }
            if (nhanVienBindingSource.Count == 0)
            {
                btnXoa.Enabled = false;
            }
            btnGhi.Enabled = btnHuy.Enabled = false;
        }

        public int getAge(DateTime dateOfBirth)
        {
            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int dob = int.Parse(dateOfBirth.ToString("yyyyMMdd"));
            return (now - dob) / 10000;
        }
        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
          
            if (tbMaNV.Text.Trim() == "")
            {
                MessageBox.Show("Mã nhân viên không được để trống!", "", MessageBoxButtons.OK);
                tbMaNV.Focus();
                return;
            }
            if (teHo.Text.Trim() == "")
            {
                MessageBox.Show("Họ không được để trống!", "", MessageBoxButtons.OK);
                teHo.Focus();
                return;
            }

            if (Regex.IsMatch(teTen.Text, @"^[a-zA-Z ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễếệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+$") == false)
            {
                MessageBox.Show("Họ chỉ có chữ cái và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                teHo.Focus();
                return;
            }

            if (teTen.Text.Trim() == "")
            {
                MessageBox.Show("Tên nhân viên không được để trống!", "", MessageBoxButtons.OK);
                teTen.Focus();
                return;
            }

           

            if (Regex.IsMatch(teTen.Text, @"^[a-zA-Z ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễếệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+$") == false)
            {
                MessageBox.Show("Tên người chỉ có chữ cái và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                teTen.Focus();
                return;
            }

            if (teTen.Text.Length > 10)
            {
                MessageBox.Show("Tên không thể lớn hơn 10 kí tự", "Thông báo", MessageBoxButtons.OK);
                teTen.Focus();
                return;
            }
            
            if (teDiaChi.Text == "")
            {
                MessageBox.Show("Không bỏ trống địa chỉ", "Thông báo", MessageBoxButtons.OK);
                teDiaChi.Focus();
                return;
            }

            if (Regex.IsMatch(teDiaChi.Text, @"^[a-zA-Z0-9 ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễếệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+$") == false)
            {
                MessageBox.Show("Địa chỉ chỉ chấp nhận chữ cái, số và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                teDiaChi.Focus();
                return;
            }

            if (teDiaChi.Text.Length > 100)
            {
                MessageBox.Show("Độ dài tối đa địa chỉ 100 kí tự", "Thông báo", MessageBoxButtons.OK);
                teDiaChi.Focus();
                return;
            }

            if(teCMND.Text == "")
            {
                MessageBox.Show("Không bỏ trống số CMND", "Thông báo", MessageBoxButtons.OK);
                teCMND.Focus();
                return;
            }

            if (Regex.IsMatch(teCMND.Text, @"^[0-9]+$") == false)
            {
                MessageBox.Show("Địa chỉ chỉ chấp nhận số", "Thông báo", MessageBoxButtons.OK);
                teCMND.Focus();
                return;
            }

            if (teCMND.Text.Length > 20)
            {
                MessageBox.Show("Độ dài tối đa CMND 20 kí tự", "Thông báo", MessageBoxButtons.OK);
                teCMND.Focus();
                return;
            }
            if (getAge(deNgaySinh.DateTime) < 18)
            {
                MessageBox.Show("Nhân viên chưa đủ 18 tuổi", "Thông báo", MessageBoxButtons.OK);
                deNgaySinh.Focus();
                return;
            }
            if (teLuong.Text.Length == 0)
            {
                MessageBox.Show("Lương không được bỏ trống!", "Thông báo", MessageBoxButtons.OK);
                teLuong.Focus();
                return;
            }

            Console.WriteLine(teLuong.Text.ToString());
           
            
           
            if (check_them == true)
            {
                String checknv =
                  "DECLARE	@result int " +
                  "EXEC @result = [dbo].[sp_CheckNhanVienTonTai] " +
                  int.Parse(tbMaNV.Text.Trim()) +
                  "  SELECT 'result' = @result";
                Console.WriteLine(checknv);
                try
                {
                    Program.myReader = Program.ExecSqlDataReader(checknv);
                    if (Program.myReader == null) { return; }
                    Program.myReader.Read();                    
                    if (Program.myReader.GetInt32(0) == 1)
                    {
                        MessageBox.Show("Nhân viên đã tồn tại!", "Thông báo", MessageBoxButtons.OK);
                        tbMaNV.Focus();
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
                nhanVienBindingSource.EndEdit();
                nhanVienBindingSource.ResetCurrentItem();
                this.nhanVienTableAdapter.Update(this.DS1.NhanVien);
                string query = "";
                if (check_them)
                {
                    
                    query = "DELETE DBO.NHANVIEN WHERE MANV = " + tbMaNV.Text.Trim();
                    
                }
                else
                {
                    query = "UPDATE DBO.NhanVien " +
                            "SET " +
                            "HO = '" + ho + "'," +
                            "TEN = '" + ten + "'," +
                            "SOCMND = '" + cmnd + "'," +
                            "DIACHI = '" + diachi + "'," +
                            "NGAYSINH = " +ToString(ngaysinh, "yyyy-MM-dd") + ","+
                            "LUONG = '" + luong + "'," +
                            "MACN = '" + macn + "'," +
                            "TrangThaiXoa = " + trangthaixoa + " " +
                            "WHERE MANV = '" + manv + "'";
                    
                }
                stack.Push(query);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi ghi nhân viên \n" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
         
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnThoat.Enabled = btnReload.Enabled = btnPhucHoi.Enabled= true;
            btnGhi.Enabled = btnHuy.Enabled = false;
            panelControl2.Enabled = false;
            nhanVienGridControl.Enabled = true;
        }

        private void cbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
    
            Console.WriteLine(cbChiNhanh.SelectedValue.ToString());

            if (cbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView")
                return;
            Program.servername = cbChiNhanh.SelectedValue.ToString();

            if (cbChiNhanh.SelectedIndex != Program.mChinhNhanh)
            {
                Program.mlogin = Program.remotelogin;
                Program.password = Program.remotepassword;

            } 
            else
            {
                Program.mlogin = Program.mloginDN;
                Program.password = Program.passwordDN;

            }
            
            if(Program.KetNoi() == 0)
            {
                MessageBox.Show("Lỗi kết nối về chi nhánh mới", "", MessageBoxButtons.OK);

            }  
            
            else
            {
                this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.nhanVienTableAdapter.Fill(this.DS1.NhanVien);

                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuXuatTableAdapter.Fill(this.DS1.PhieuXuat);

                this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.datHangTableAdapter.Fill(this.DS1.DatHang);

                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuNhapTableAdapter.Fill(this.DS1.PhieuNhap);

                macn = ((DataRowView)nhanVienBindingSource[0])["MaCN"].ToString();

            }
        }

        private void btnHuy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            nhanVienBindingSource.CancelEdit();

            if (btnThem.Enabled == false)
            {
                nhanVienBindingSource.Position = vitri;
            }

            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = btnHuy.Enabled = false;
            nhanVienGridControl.Enabled = true;
            this.nhanVienTableAdapter.Fill(this.DS1.NhanVien);

        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

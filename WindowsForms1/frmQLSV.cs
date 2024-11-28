using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms1
{
    public partial class frmQLSV : Form
    {
        public frmQLSV()
        {
            InitializeComponent();
        }

        // Set tất cả các trường nhập liệu về giá trị mặc định
        public void SetNull()
        {
            txtMSSV.Clear();
            txtHoTen.Clear();
            txtDiemTB.Clear();
            rdNam.Checked = true;
            rdNu.Checked = false;
            cboChuyenNganh.SelectedIndex = -1;
            dtpNgaySinh.Value = DateTime.Now;
            txtMSSV.Focus();
        }

        // Lấy dữ liệu sinh viên từ các trường nhập liệu
        private (string MSSV, string HoTen, string GioiTinh, string NgaySinh, double DiemTB, string ChuyenNganh) GetSinhVienData()
        {
            string MSSV = txtMSSV.Text;
            string hoTen = txtHoTen.Text;
            string gioiTinh = rdNam.Checked ? "Nam" : "Nữ";
            string ngaySinh = dtpNgaySinh.Value.ToShortDateString();
            double diemTb;

            if (!double.TryParse(txtDiemTB.Text, out diemTb))
            {
                MessageBox.Show("Vui lòng nhập điểm trung bình hợp lệ!", "Thông báo");
                return (null, null, null, null, 0, null);
            }

            string chuyenNganh = cboChuyenNganh.SelectedItem?.ToString() ?? "";
            return (MSSV, hoTen, gioiTinh, ngaySinh, diemTb, chuyenNganh);
        }

        // Cập nhật dữ liệu sinh viên vào các trường nhập liệu
        private void SetSinhVienData(ListViewItem selectedItem)
        {
            txtMSSV.Text = selectedItem.SubItems[0].Text;
            txtHoTen.Text = selectedItem.SubItems[1].Text;
            rdNam.Checked = selectedItem.SubItems[2].Text == "Nam";
            rdNu.Checked = selectedItem.SubItems[2].Text == "Nữ";
            dtpNgaySinh.Text = selectedItem.SubItems[3].Text;
            txtDiemTB.Text = selectedItem.SubItems[4].Text;
            cboChuyenNganh.Text = selectedItem.SubItems[5].Text;
        }

        // Cập nhật ListViewItem với dữ liệu mới
        private void UpdateListViewItem(ListViewItem selectedItem, string MSSV, string hoTen, string gioiTinh, string ngaySinh, double diemTb, string chuyenNganh)
        {
            selectedItem.SubItems[0].Text = MSSV;
            selectedItem.SubItems[1].Text = hoTen;
            selectedItem.SubItems[2].Text = gioiTinh;
            selectedItem.SubItems[3].Text = ngaySinh;
            selectedItem.SubItems[4].Text = diemTb.ToString();
            selectedItem.SubItems[5].Text = chuyenNganh;
        }

        // Sự kiện khi chọn dòng trong ListView
        private void lstSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSinhVien.SelectedItems.Count > 0)
            {
                SetSinhVienData(lstSinhVien.SelectedItems[0]);
            }
        }

        // Sự kiện khi double click vào item trong ListView
        private void lstSinhVien_ItemActivate(object sender, EventArgs e)
        {
            if (lstSinhVien.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lstSinhVien.SelectedItems[0];
                SetSinhVienData(selectedItem);
            }
        }

        // Khi form load, thiết lập các giá trị mặc định
        private void QLSV_Load(object sender, EventArgs e)
        {
            SetNull();
        }

        // Thêm sinh viên vào danh sách
        private void btnThem_Click(object sender, EventArgs e)
        {
            var (MSSV, hoTen, gioiTinh, ngaySinh, diemTb, chuyenNganh) = GetSinhVienData();

            if (MSSV == null || hoTen == null || gioiTinh == null || ngaySinh == null || diemTb == 0 || chuyenNganh == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo");
                return;
            }

            if (IsStudentExists(MSSV))
            {
                MessageBox.Show("Mã số sinh viên đã tồn tại!", "Thông báo");
                return;
            }

            AddNewStudentToList(MSSV, hoTen, gioiTinh, ngaySinh, diemTb, chuyenNganh);
        }

        // Kiểm tra nếu mã số sinh viên đã tồn tại trong danh sách
        private bool IsStudentExists(string mssv)
        {
            return lstSinhVien.Items.Cast<ListViewItem>()
                                    .Any(item => item.SubItems[0].Text == mssv);
        }

        // Thêm sinh viên mới vào ListView
        private void AddNewStudentToList(string MSSV, string hoTen, string gioiTinh, string ngaySinh, double diemTb, string chuyenNganh)
        {
            ListViewItem lvi = new ListViewItem(MSSV);
            lvi.SubItems.Add(hoTen);
            lvi.SubItems.Add(gioiTinh);
            lvi.SubItems.Add(ngaySinh);
            lvi.SubItems.Add(diemTb.ToString());
            lvi.SubItems.Add(chuyenNganh);
            lstSinhVien.Items.Add(lvi);

            UpdateStatistics();

            SetNull();
            MessageBox.Show("Sinh viên đã được thêm vào danh sách!", "Thông báo");
        }

        // Cập nhật thông tin thống kê
        private void UpdateStatistics()
        {
            txtTongSoNam.Text = CountMaleStudents().ToString();
            txtTongSoNu.Text = (lstSinhVien.Items.Count - CountMaleStudents()).ToString();
        }

        // Đếm số lượng sinh viên nam
        private int CountMaleStudents()
        {
            return lstSinhVien.Items.Cast<ListViewItem>().Count(item => item.SubItems[2].Text == "Nam");
        }

        // Sửa thông tin sinh viên đã chọn
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (lstSinhVien.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa", "Thông báo");
                return;
            }

            var selectedItem = lstSinhVien.SelectedItems[0];
            var (MSSV, hoTen, gioiTinh, ngaySinh, diemTb, chuyenNganh) = GetSinhVienData();

            UpdateListViewItem(selectedItem, MSSV, hoTen, gioiTinh, ngaySinh, diemTb, chuyenNganh);

            MessageBox.Show("Thông tin sinh viên đã được cập nhật!", "Thông báo");

            UpdateStatistics();
            SetNull();
        }

        // Xóa sinh viên đã chọn
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (lstSinhVien.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Bạn có muốn xóa sinh viên này?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    lstSinhVien.Items.Remove(lstSinhVien.SelectedItems[0]);
                    SetNull();
                    MessageBox.Show("Sinh viên đã được xóa khỏi danh sách!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa!", "Thông báo");
            }

            UpdateStatistics();
        }

        // Lưu thay đổi
        private void btnLuu_Click(object sender, EventArgs e)
        {
            var (MSSV, hoTen, gioiTinh, ngaySinh, diemTb, chuyenNganh) = GetSinhVienData();

            if (MSSV == null || hoTen == null || gioiTinh == null || ngaySinh == null || diemTb == 0 || chuyenNganh == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo");
                return;
            }

            var existingItem = lstSinhVien.Items.Cast<ListViewItem>().FirstOrDefault(item => item.SubItems[0].Text == MSSV);

            if (existingItem != null)
            {
                UpdateListViewItem(existingItem, MSSV, hoTen, gioiTinh, ngaySinh, diemTb, chuyenNganh);
                MessageBox.Show("Thông tin sinh viên đã được cập nhật!", "Thông báo");
            }
            else
            {
                AddNewStudentToList(MSSV, hoTen, gioiTinh, ngaySinh, diemTb, chuyenNganh);
            }

            UpdateStatistics();
        }

        // Hủy thao tác
        private void btnHuy_Click(object sender, EventArgs e)
        {
            SetNull();
            lstSinhVien.SelectedItems.Clear();
        }

        // Thoát ứng dụng
        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        private void txtDiemTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}

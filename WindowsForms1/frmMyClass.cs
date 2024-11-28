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
    public partial class frmMyClass : Form
    {
        public frmMyClass()
        {
            InitializeComponent();
        }

        private void btnXuatTT_Click(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text;
            string ngaySinh = dtpNgaySinh.Value.ToString("dd/MM/yyyy");
            string gioiTinh = rbtnNam.Checked ? "Nam" : (rbtnNu.Checked ? "Nữ" : "Chưa chọn");
            
            List<string> soThich = new List<string>();
            if (chkTheThao.Checked) soThich.Add("Thể thao");
            if (chkPhimAnh.Checked) soThich.Add("Phim ảnh");
            if (chkDuLich.Checked) soThich.Add("Du lịch");

            string soThichText = soThich.Count > 0 ? string.Join(", ", soThich) : "Không có";

            string thongTin = $"Họ tên: {hoTen}, Giới tính: {gioiTinh}, Ngày sinh: {ngaySinh}, Sở thích: {soThichText}";

            MessageBox.Show(thongTin, "Thông tin người dùng", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmMyClass_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        }
    }
}

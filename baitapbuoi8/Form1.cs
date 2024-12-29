using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.model;

namespace baitapbuoi8
{
    public partial class Form1 : Form
    {
        student Dbsinhvien = new student();
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //nhập dữ liệu cho các thuộc tính
            fillDgvSinhvien();
            fillcbbLop();
        }
        private void fillcbbLop()
        {
            List<LOP> listLop = Dbsinhvien.LOPs.ToList();

            cbbLop.DataSource = listLop;
            cbbLop.DisplayMember = "tenlop";
            cbbLop.ValueMember = "malop";
        }
        private void fillDgvSinhvien()
        {
            List<SINHVIEN> listSinhvien = Dbsinhvien.SINHVIENs.ToList();
            foreach (SINHVIEN sinhvien in listSinhvien)
            {
                int newRow = dgvSinhvien.Rows.Add();
                dgvSinhvien.Rows[newRow].Cells[0].Value = sinhvien.masv;
                dgvSinhvien.Rows[newRow].Cells[1].Value = sinhvien.hovaten;
                dgvSinhvien.Rows[newRow].Cells[2].Value = sinhvien.ngaysinh;
                dgvSinhvien.Rows[newRow].Cells[3].Value = sinhvien.LOP.tenlop;

            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!isLuu)
            {
                MessageBox.Show("Bạn phải bật chế độ lưu trước khi thêm sinh viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                // Lấy thông tin từ các ô nhập liệu
                string maSV = txtMASV.Text.Trim();
                string hoTen = txtHOTEN.Text.Trim();
                DateTime ngaySinh = dtNgaysinh.Value;
                string maLop = string.Format(cbbLop.SelectedValue.ToString());

                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(hoTen))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra mã sinh viên đã tồn tại
                var existingSinhvien = Dbsinhvien.SINHVIENs.FirstOrDefault(s => s.masv == maSV);
                if (existingSinhvien != null)
                {
                    MessageBox.Show("Mã sinh viên đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng Sinhvien mới
                SINHVIEN sinhvien = new SINHVIEN
                {
                    masv = maSV,
                    hovaten = hoTen,
                    ngaysinh = ngaySinh,
                    malop = maLop
                };

                // Thêm vào database
                Dbsinhvien.SINHVIENs.Add(sinhvien);
                Dbsinhvien.SaveChanges();

                // Cập nhật lại DataGridView
                dgvSinhvien.Rows.Clear();
                fillDgvSinhvien();

                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có hàng nào được chọn không
            if (dgvSinhvien.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xoá không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    dgvSinhvien.Rows.RemoveAt(dgvSinhvien.CurrentRow.Index);
                    MessageBox.Show("Xoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hàng để xoá!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        
        }

        private void btnSUa_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có hàng nào được chọn không
                if (dgvSinhvien.CurrentRow != null)
                {
                    // Lấy mã sinh viên từ hàng hiện tại
                    string maSV = dgvSinhvien.CurrentRow.Cells[0].Value.ToString();

                    // Tìm sinh viên trong database
                    SINHVIEN existingSinhvien = Dbsinhvien.SINHVIENs.FirstOrDefault(s => s.masv == maSV);
                    if (existingSinhvien != null)
                    {
                        // Kiểm tra thông tin mới
                        string hoTen = txtHOTEN.Text.Trim();
                        DateTime ngaySinh = dtNgaysinh.Value;
                        string maLop = string.Format(cbbLop.SelectedValue.ToString());

                        if (string.IsNullOrEmpty(hoTen))
                        {
                            MessageBox.Show("Họ tên không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Cập nhật thông tin
                        existingSinhvien.hovaten = hoTen;
                        existingSinhvien.ngaysinh = ngaySinh;
                        existingSinhvien.malop = maLop;

                        // Lưu thay đổi vào database
                        Dbsinhvien.SaveChanges();

                        // Cập nhật lại DataGridView
                        dgvSinhvien.Rows.Clear();
                        fillDgvSinhvien();

                        MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một hàng để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private bool isLuu = false;
        private void btnluu_Click(object sender, EventArgs e)
        {
            isLuu = true;
            MessageBox.Show("Chế độ lưu đã được bật. Bạn có thể thêm sinh viên mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnkluu_Click(object sender, EventArgs e)
        {
            isLuu = false;
            MessageBox.Show("Chế độ lưu đã bị tắt. Bạn không thể thêm sinh viên mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    }



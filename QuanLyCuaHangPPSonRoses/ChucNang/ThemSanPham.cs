using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Numerics;

namespace QuanLyCuaHangPPSonRoses.ChucNang
{
    public partial class ThemSanPham : Form
    {
        NpgsqlConnection connection;
        public ThemSanPham()
        {
            InitializeComponent();
            connection = new NpgsqlConnection();
            connection.ConnectionString = @"Server=localhost;Port=5432;Username=postgres;Password=123;Database=postgres"; 
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            if (ktDienND())
            {
                MemoryStream ms = new MemoryStream();
                picSP.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] imageData = ms.GetBuffer();

                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO sanpham (masp, tensp, sl, loaisp, gia, hinhanh) VALUES (@masp, @tensp, @sl, @loaisp, @gia, @hinhanh)", connection))
                {
                    command.Parameters.AddWithValue("@masp", txtMaSP.Text);
                    command.Parameters.AddWithValue("@tensp", txtTenSP.Text);
                    command.Parameters.AddWithValue("@sl", int.Parse(txtSL.Text));
                    command.Parameters.AddWithValue("@loaisp", cmbPhanLoai.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@gia", Int64.Parse(txtGia.Text));
                    command.Parameters.AddWithValue("@hinhanh", imageData);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Thêm sản phẩm thành công!");
                }

                connection.Close();
            }
        }
       
        private void btnThemHinhAnh_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Hình ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            openFileDialog1.ShowDialog();
            picSP.Image=Image.FromFile(openFileDialog1.FileName);
        }

        private bool ktDienND()
        {
            if (string.IsNullOrEmpty(txtMaSP.Text) || string.IsNullOrEmpty(txtTenSP.Text) || string.IsNullOrEmpty(txtSL.Text) || string.IsNullOrEmpty(cmbPhanLoai.Text) || string.IsNullOrEmpty(txtGia.Text) || picSP.Image == null)
            {
                MessageBox.Show("Hãy nhập đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}

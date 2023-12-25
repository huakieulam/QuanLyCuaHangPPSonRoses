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
                picSP.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageData = ms.GetBuffer();

                connection.Open();

                string masp = txtMaSP.Text;

                // Kiểm tra xem mã sản phẩm đã tồn tại hay chưa
                string query = "SELECT COUNT(*) FROM sanpham WHERE masp = @masp";
                using (NpgsqlCommand checkCommand = new NpgsqlCommand(query, connection))
                {
                    checkCommand.Parameters.AddWithValue("@masp", masp);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count > 0)
                    {
                        query = @"UPDATE sanpham
                          SET tensp = @tensp, sl = @sl, loaisp = @loaisp, gia = @gia, hinhanh = @hinhanh
                          WHERE masp = @masp";

                        using (NpgsqlCommand updateCommand = new NpgsqlCommand(query, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@masp", masp);
                            updateCommand.Parameters.AddWithValue("@tensp", txtTenSP.Text);
                            updateCommand.Parameters.AddWithValue("@sl", int.Parse(txtSL.Text));
                            updateCommand.Parameters.AddWithValue("@loaisp", cmbPhanLoai.SelectedItem.ToString());
                            updateCommand.Parameters.AddWithValue("@gia", Int64.Parse(txtGia.Text));
                            updateCommand.Parameters.AddWithValue("@hinhanh", imageData);

                            updateCommand.ExecuteNonQuery();

                            MessageBox.Show("Cập nhật sản phẩm thành công!");
                        }
                    }
                    else
                    {
                        query = @"INSERT INTO sanpham (masp, tensp, sl, loaisp, gia, hinhanh)
                          VALUES (@masp, @tensp, @sl, @loaisp, @gia, @hinhanh)";

                        using (NpgsqlCommand insertCommand = new NpgsqlCommand(query, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@masp", masp);
                            insertCommand.Parameters.AddWithValue("@tensp", txtTenSP.Text);
                            insertCommand.Parameters.AddWithValue("@sl", int.Parse(txtSL.Text));
                            insertCommand.Parameters.AddWithValue("@loaisp", cmbPhanLoai.SelectedItem.ToString());
                            insertCommand.Parameters.AddWithValue("@gia", Int64.Parse(txtGia.Text));
                            insertCommand.Parameters.AddWithValue("@hinhanh", imageData);

                            insertCommand.ExecuteNonQuery();

                            MessageBox.Show("Thêm sản phẩm thành công!");
                        }
                    }
                }
                connection.Close();
            }
        
        }
    
       
        private void btnThemHinhAnh_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Hình ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                picSP.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private bool ktDienND()
        {
            if (string.IsNullOrEmpty(txtMaSP.Text) || string.IsNullOrEmpty(txtTenSP.Text) || string.IsNullOrEmpty(txtSL.Text) || string.IsNullOrEmpty(cmbPhanLoai.Text) || string.IsNullOrEmpty(txtGia.Text) || picSP.Image == null)
            {
                MessageBox.Show("Hãy nhập đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        /*private bool ktMaSP(string maSP)
        {
            bool trungMaSP = false;

            using (NpgsqlCommand command = new NpgsqlCommand("SELECT COUNT(*) FROM sanpham WHERE masp = @masp", connection))
            {
                command.Parameters.AddWithValue("@masp", maSP);
                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();

                if (count > 0)
                {
                    trungMaSP = true;
                }
            }

            return trungMaSP;
        }
*/


        public string MASP
        {
            get { return txtMaSP.Text; }
            set { txtMaSP.Text = value; }
        }

        public string TENSP
        {
            get { return txtTenSP.Text; }
            set { txtTenSP.Text = value; }
        }

        public int SOLUONG
        {
            get { return int.Parse(txtSL.Text); }
            set { txtSL.Text = value.ToString(); }
        }

        public string PHANLOAI
        {
            get { return cmbPhanLoai.SelectedItem.ToString(); }
            set { cmbPhanLoai.SelectedItem = value; }
        }

        public decimal GIA
        {
            get { return decimal.Parse(txtGia.Text); }
            set { txtGia.Text = value.ToString(); }
        }

        public Image HINHANH
        {
            get { return picSP.Image; }
            set { picSP.Image = value; }
        }

        public bool ReadOnlyMaSP
        {
            get { return txtMaSP.ReadOnly; }
            set { txtMaSP.ReadOnly = value; }
        }
    }
}

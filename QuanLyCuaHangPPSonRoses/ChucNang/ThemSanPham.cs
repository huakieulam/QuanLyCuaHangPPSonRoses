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
            connection = new NpgsqlConnection();
            connection.ConnectionString = @"Server=localhost;Port=5432;Username=postgres;Password=123;Database=postgres";
            InitializeComponent();
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            if (ktDienND())
            {
                Image temp = new Bitmap(picSP.Image);

                MemoryStream ms = new MemoryStream();
               
                temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                imageByteArray= ms.ToArray();

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
                            updateCommand.Parameters.AddWithValue("@hinhanh", imageByteArray);

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
                            insertCommand.Parameters.AddWithValue("@hinhanh", imageByteArray);

                            insertCommand.ExecuteNonQuery();

                            MessageBox.Show("Thêm sản phẩm thành công!");
                        }
                    }
                }
                connection.Close();
            }
        
        }

        string filepath;
        Byte[] imageByteArray;
        private void btnThemHinhAnh_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Hình ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //picSP.Image = Image.FromFile(openFileDialog1.FileName);
                filepath = openFileDialog1.FileName;
                picSP.Image = new Bitmap(filepath);
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

        private void cmbPhanLoai_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prefix = "";
            if (cmbPhanLoai.SelectedItem.ToString() == "Sơn nội thất")
            {
                prefix = "NT_";
            }
            else if (cmbPhanLoai.SelectedItem.ToString() == "Sơn ngoại thất")
            {
                prefix = "NGT_";
            }

            txtMaSP.Text = prefix;
        }

        private void txtTenSP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void txtSL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}

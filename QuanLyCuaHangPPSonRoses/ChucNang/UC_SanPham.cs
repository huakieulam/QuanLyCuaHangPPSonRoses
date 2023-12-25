using DevExpress.Xpo.DB.Helpers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangPPSonRoses.ChucNang
{
    public partial class UC_SanPham : UserControl
    {
        public UC_SanPham()
        {
            InitializeComponent();
        }

        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            ThemSanPham themSanPham = new ThemSanPham();
            themSanPham.Show();
        }

        private void UC_SanPham_Load(object sender, EventArgs e)
        {

            HienThiSP();
            
        }
        private Image HinhAnhMangByte(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            using (var stream = new System.IO.MemoryStream(byteArray))
            {
                return Image.FromStream(stream);
            }
        }
        public void HienThiSP()
        {
            flowLayoutPanel1.Controls.Clear();

            string connectionString = @"Server=localhost;Port=5432;Username=postgres;Password=123;Database=postgres";
            string query = "SELECT * FROM sanpham";
            List<UC_SP_thongtinsp> danhSachSanPham = new List<UC_SP_thongtinsp>(); 

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UC_SP_thongtinsp uc_SP_Thongtinsp = new UC_SP_thongtinsp();
                            uc_SP_Thongtinsp.MASP = reader["masp"].ToString();
                            uc_SP_Thongtinsp.TENSP = reader["tensp"].ToString();
                            uc_SP_Thongtinsp.SOLUONG = Convert.ToInt32(reader["sl"]);
                            uc_SP_Thongtinsp.PHANLOAI = reader["loaisp"].ToString();
                            uc_SP_Thongtinsp.GIA = Convert.ToDecimal(reader["gia"]);
                            uc_SP_Thongtinsp.HINHANH = HinhAnhMangByte((byte[])reader["hinhanh"]);

                            danhSachSanPham.Add(uc_SP_Thongtinsp);
                        }
                    }
                }
            }

            foreach (UC_SP_thongtinsp sanPham in danhSachSanPham)
            {
                sanPham.XoaSanPhamClicked += XoaSanPham_Clicked;

                flowLayoutPanel1.Controls.Add(sanPham);
            }
            
        }

        private void btnLamMoiDL_Click(object sender, EventArgs e)
        {
            HienThiSP();
        }
        private void XoaSanPham_Clicked(object sender, EventArgs e)
        {
            UC_SP_thongtinsp sanPham = sender as UC_SP_thongtinsp;
            string maSanPham = sanPham.MASP;

            string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=123;Database=postgres;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM sanpham WHERE masp = @masp";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@masp", maSanPham);
                    command.ExecuteNonQuery();
                }
            }
            flowLayoutPanel1.Controls.Remove(sanPham);


            HienThiSP();
        }
    }
}

using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangPPSonRoses.ChucNang
{
    public partial class DanhSachDonHang : Form
    {
        public DanhSachDonHang()
        {
            InitializeComponent();
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DanhSachDonHang_Load(object sender, EventArgs e)
        {
            HienThiDonHang();
        }
        private void HienThiDonHang()
        {
            panelHienThiDH.Controls.Clear();

            string connectionString = @"Server=localhost;Port=5432;Username=postgres;Password=123;Database=postgres";
            string query = "SELECT dh.maDH, dh.tenKH, dh.email, dh.sdt, dh.ngay, dh.tongDH, dh.trangthai, ctdh.masp, ctdh.tensp, ctdh.phanloai, ctdh.slmua, ctdh.giasp FROM donhang dh JOIN chitietdonhang ctdh ON dh.maDH = ctdh.madh;";
            List<UC_DanhSachDonHang> danhsachDH = new List<UC_DanhSachDonHang>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UC_DanhSachDonHang uc_DSDH = new UC_DanhSachDonHang();
                            uc_DSDH.TENKH = reader["tenKH"].ToString();
                            uc_DSDH.SDT = reader["sdt"].ToString();
                            uc_DSDH.EMAIL = reader["email"].ToString();

                            uc_DSDH.MASP = reader["masp"].ToString();
                            uc_DSDH.TENSP = reader["tensp"].ToString();
                            uc_DSDH.LOAI = reader["phanloai"].ToString();
                            uc_DSDH.SL = Convert.ToInt32(reader["slmua"]);
                            uc_DSDH.GIASP = Convert.ToDecimal(reader["giasp"]);
                            uc_DSDH.TONGDON = Convert.ToDecimal(reader["tongDH"]);

                            //panelHienThiDH.Controls.Add(uc_DSDH);
                            danhsachDH.Add(uc_DSDH);

                        }
                    }
                }
            }
            foreach (UC_DanhSachDonHang donHang in danhsachDH)
            {
                panelHienThiDH.Controls.Add(donHang);
            }
        }
    }
}

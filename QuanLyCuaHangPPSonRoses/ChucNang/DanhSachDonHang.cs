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
            string query = "SELECT dh.maDH, dh.tenKH, dh.email, dh.sdt, dh.ngay, dh.tongDH, dh.trangthai, ctdh.masp, ctdh.tensp, ctdh.phanloai, ctdh.slmua, ctdh.giasp, ctdh.tonggiasp FROM donhang dh JOIN chitietdonhang ctdh ON dh.maDH = ctdh.madh";
            
            Dictionary<string, List<UC_DanhSachDonHang>> danhSachDonHang = new Dictionary<string, List<UC_DanhSachDonHang>>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string orderID = reader.GetInt32(0).ToString();

                    UC_DanhSachDonHang uc_DSDH = new UC_DanhSachDonHang();
                    uc_DSDH.TENKH = reader["tenKH"].ToString();
                    uc_DSDH.SDT = reader["sdt"].ToString();
                    uc_DSDH.EMAIL = reader["email"].ToString();

                    uc_DSDH.MASP = reader["masp"].ToString();
                    uc_DSDH.TENSP = reader["tensp"].ToString();
                    uc_DSDH.LOAI = reader["phanloai"].ToString();
                    uc_DSDH.SL = Convert.ToInt32(reader["slmua"]);
                    uc_DSDH.GIA = Convert.ToDecimal(reader["giasp"]);
                    uc_DSDH.TONG = Convert.ToDecimal(reader["tonggiasp"]);
                    uc_DSDH.TONGGIA= Convert.ToDecimal(reader["tongdh"]);
                    if (danhSachDonHang.ContainsKey(orderID))
                    {
                        danhSachDonHang[orderID].Add(uc_DSDH);
                    }
                    else
                    {
                        danhSachDonHang.Add(orderID, new List<UC_DanhSachDonHang> { uc_DSDH });
                    }
                }

                reader.Close();
                connection.Close();
            }

            foreach (var order in danhSachDonHang)
            {
                UC_DanhSachDonHang donHang = new UC_DanhSachDonHang();
                donHang.TENKH = order.Value.First().TENKH;
                donHang.SDT = order.Value.First().SDT;
                donHang.EMAIL = order.Value.First().EMAIL;
                donHang.TONGGIA = order.Value.First().TONGGIA;

                foreach (UC_DanhSachDonHang sanPham in order.Value)
                {
                    // Thêm dữ liệu sản phẩm vào DataGridView của đơn hàng
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(donHang.dgvDSSPDonHang);
                    row.Cells[0].Value = sanPham.MASP;
                    row.Cells[1].Value = sanPham.TENSP;
                    row.Cells[2].Value = sanPham.LOAI;
                    row.Cells[3].Value = sanPham.SL;
                    row.Cells[4].Value = sanPham.GIA;
                    row.Cells[5].Value = sanPham.TONG;
                    donHang.dgvDSSPDonHang.Rows.Add(row);
                }

                panelHienThiDH.Controls.Add(donHang);
            }
        }
    }
}

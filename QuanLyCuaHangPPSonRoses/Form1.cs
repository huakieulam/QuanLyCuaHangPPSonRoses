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

namespace QuanLyCuaHangPPSonRoses
{
    public partial class Form1 : Form
    {
        private NpgsqlConnection connection;
        private NpgsqlDataAdapter dataAdapter;
        private DataSet dataSet;

        public Form1()
        {
            InitializeComponent();
            connection = new NpgsqlConnection("Server=localhost;Port=5432;Username=postgres;Password=123;Database=postgres");
        }
        private void LoadData()
        {
            try
            {
                connection.Open();

                // Tạo câu truy vấn SELECT để lấy dữ liệu từ bảng "sanpham"
                string query = "SELECT masp, tensp, sl, loaisp, hinhanh FROM sanpham";

                // Khởi tạo đối tượng NpgsqlDataAdapter và DataSet
                dataAdapter = new NpgsqlDataAdapter(query, connection);
                dataSet = new DataSet();

                // Đổ dữ liệu từ cơ sở dữ liệu vào DataSet
                dataAdapter.Fill(dataSet, "sanpham");

                // Thiết lập nguồn dữ liệu cho DataGridView
                dataGridView1.DataSource = dataSet.Tables["sanpham"];

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}

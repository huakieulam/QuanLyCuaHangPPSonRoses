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
        }
        private void LoadData()
        {
            string connectionString = @"Server=localhost;Port=5432;Username=postgres;Password=123;Database=postgres";
            string query = "SELECT dh.maDH, dh.tenKH, dh.email, dh.sdt, dh.ngay, dh.tongDH, dh.trangthai, ctdh.masp, ctdh.tensp, ctdh.phanloai, ctdh.slmua, ctdh.giasp FROM donhang dh JOIN chitietdonhang ctdh ON dh.maDH = ctdh.madh where dh.maDH='16'";


            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}

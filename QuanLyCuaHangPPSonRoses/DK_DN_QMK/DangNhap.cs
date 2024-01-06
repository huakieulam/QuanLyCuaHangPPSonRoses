using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static DevExpress.Data.Mask.Internal.RegExMaskMath.DraftFiniteAutomaton<T>;
using System.Xml.Linq;
using System.Security.Cryptography;
using Npgsql;
using QuanLyCuaHangPPSonRoses.ChucNang;
using QuanLyCuaHangPPSonRoses.DK_DN_QMK;
using static DevExpress.XtraEditors.Mask.MaskSettings;

namespace QuanLyCuaHangPPSonRoses.DK_DN_QMK
{
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }

        private void btnQuenMatKhau_Click(object sender, EventArgs e)
        {
            QuenMatKhau formQuenMK = new QuenMatKhau();
            formQuenMK.Show();
            this.Hide();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if (rdbKhachHang.Checked)
            {
                DangKy formDangKy = new DangKy();
                formDangKy.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Chỉ có khách hàng mới được đăng ký");
            }



        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            string hashedEnteredPassword = HashPassword(enteredPassword);
            return string.Equals(hashedEnteredPassword, storedHashedPassword, StringComparison.OrdinalIgnoreCase);
        }
        public bool Login(string email, string matkhau)
        {
            string hashPassword = HashPassword(matkhau);

            string connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=123;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT dangnhap(email, matkhau) FROM taikhoan WHERE email = @in_email AND matkhau = @in_matkhau";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@in_email", email);
                    command.Parameters.AddWithValue("@in_matkhau", hashPassword);

                    var result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        public string GetVaiTro(string email)
        {
            string connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=123;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT vaitro FROM taikhoan WHERE email = @in_email";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@in_email", email);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Lấy giá trị từ cột "vaitro"
                            string vaiTro = reader.GetString(0);
                            return vaiTro;
                        }
                    }
                }
            }

            return null;
        }
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string email = txtNhapEmail.Text;
            string matkhau = txtNhapMatKhau.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(matkhau))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
                return;
            }
            bool isAuthenticated = Login(email, matkhau);
            if (isAuthenticated)
            {
                string connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=123;";
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string vaitro = "";
                    if (rdbKhachHang.Checked)
                    {
                        vaitro = "Khách hàng";
                    }
                    else if (rdbQuanLy.Checked)
                    {
                        vaitro = "Quản lý";
                    }
                    else if (rdbNhanVien.Checked)
                    {
                        vaitro = "Nhân viên";
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn chức năng!");
                        return;
                    }
                    if (vaitro == "Khách hàng" || vaitro == "Nhân viên" || vaitro == "Quản lý")
                    {
                        string vaitroDangNhap = GetVaiTro(email);
                        if (vaitroDangNhap == vaitro)
                        {
                            MessageBox.Show($"Đăng nhập thành công với vai trò: {vaitro}");

                            TrangChu formTrangChu = new TrangChu();
                            formTrangChu.Show();
                            this.Hide();
                            //switch (vaitro)
                            //{
                            //    case "Khách hàng":
                            //        KhachHang formKhachHang = new KhachHang();
                            //        formKhachHang.Show();
                            //        break;
                            //    case "Nhân viên":
                            //        NhanVien formNhanVien = new NhanVien();
                            //        formNhanVien.Show();
                            //        break;
                            //    case "Quản lý":
                            //        QuanLy formQuanLy = new QuanLy();
                            //        formQuanLy.Show();
                            //        break;
                            //    default:
                            //        MessageBox.Show("Vai trò không hợp lệ!");
                            //        break;
                            //}

                            //this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng chọn đúng vai trò!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Vai trò không hợp lệ!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Thông tin đăng nhập không hợp lệ!");
            }
        }
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // Chuyển đổi byte thành dạng chuỗi hex
                }
                return builder.ToString();
            }
        }
    }
}


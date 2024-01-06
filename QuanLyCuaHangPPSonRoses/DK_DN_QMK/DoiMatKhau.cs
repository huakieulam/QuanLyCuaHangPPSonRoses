using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QuanLyCuaHangPPSonRoses.DK_DN_QMK
{
    public partial class DoiMatKhau : Form
    {
        private string QuenMatKhauEmail { get; set; }

        public DoiMatKhau(string email)
        {
            QuenMatKhauEmail = email;
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            QuenMatKhau formQuenMK = new QuenMatKhau();
            formQuenMK.Show();
        }
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // Chuyểnđổi byte thành dạng chuỗi hex
                }
                return builder.ToString();
            }
        }
        public bool DoiMK(string email, string password)
        {
            string hashedPassword = HashPassword(password);
            string connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=123;";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE taikhoan SET matkhau = @in_matkhau WHERE email = @in_email";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@in_email", email);
                        command.Parameters.AddWithValue("@in_matkhau", hashedPassword);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
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
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thay đổi mật khẩu: " + ex.Message);
                return false;
            }
          
        }
        private bool IsValidPassword(string password)
        {
            // Kiểm tra độ dài ít nhất 8 ký tự
            if (password.Length < 8)
            {
                return false;
            }

            // Kiểm tra không chứa khoảng trắng
            if (password.Contains(" "))
            {
                return false;
            }

            // Kiểm tra chứa ít nhất 1 chữ cái in hoa
            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                return false;
            }

            // Kiểm tra chứa ít nhất 1 chữ cái thường
            if (!Regex.IsMatch(password, "[a-z]"))
            {
                return false;
            }

            // Kiểm tra chứa ít nhất 1 chữ số
            if (!Regex.IsMatch(password, "[0-9]"))
            {
                return false;
            }

            return true;
        }
        private void txtNhapMK_Leave(object sender, EventArgs e)
        {
            string password = txtNhapMK.Text;
            if (password.Length < 8)
            {
                // Mật khẩu không hợp lệ
                // Hiển thị thông báo lỗi
                MessageBox.Show("Mật khẩu không hợp lệ! Mật khẩu phải có ít nhất 8 ký tự.");
                txtNhapMK.Text = string.Empty;
            }
            else if (!IsValidPassword(password))
            {
                // Mật khẩu không hợp lệ
                // Hiển thị thông báo lỗi
                MessageBox.Show("Mật khẩu không hợp lệ! Mật khẩu phải chứa chữ cái in hoa, chữ cái thường, chữ số.");
                txtNhapMK.Text = string.Empty;
            }
        }
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string password = txtNhapMK.Text;
            string rePassword = txtXNMatKhau.Text;
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rePassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin mật khẩu và mật khẩu xác nhận.");
            }
            else if (password.Equals(rePassword))
            {
               
                    if (DoiMK(QuenMatKhauEmail, password))
                    {
                        MessageBox.Show("Đổi mật khẩu thành công");
                        DangNhap formDangNhap = new DangNhap();
                        formDangNhap.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Không thể đổi mật khẩu");
                    }
                    
            }
            else
            {
                MessageBox.Show("Mật khẩu nhập lại không chính xác");
            }
        }
    }
}

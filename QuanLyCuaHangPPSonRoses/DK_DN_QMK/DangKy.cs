using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using Npgsql;
using System.Text.RegularExpressions;

namespace QuanLyCuaHangPPSonRoses.DK_DN_QMK
{
    public partial class DangKy : Form
    {
        public DangKy()
        {
            InitializeComponent();
        }
        Random rd = new Random();
        int otp;

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            DangNhap formDangNhap = new DangNhap();
            formDangNhap.Show();
        }
        private bool CheckMail(string email)
        {
            string connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=123;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand("SELECT count(*) FROM taikhoan WHERE email = @in_email", connection))
                {
                    command.Parameters.AddWithValue("@in_email", email);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
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

        private void btnGuiMaXN_Click(object sender, EventArgs e)
        {
            try
            {

                otp = rd.Next(100000, 1000000);
                var fromAddress = new MailAddress("kieulammm@gmail.com");
                var toAddress = new MailAddress(txtNhapEmail.Text);
                const string formPass = "wzzvfpyzcsxyoqsi";
                const string subject = "Xác nhận đăng ký";
                string body = otp.ToString();
                string email = txtNhapEmail.Text.Trim();
                if (!IsValidEmailFormat(email))
                {
                    MessageBox.Show("Định dạng email không hợp lệ!");
                    return;
                }
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, formPass),
                    Timeout = 200000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
                MessageBox.Show("Mã xác nhận đã được gửi qua email!", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private bool IsValidEmailFormat(string email)
        {
            // Kiểm tra định dạng email bằng biểu thức chính quy
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, pattern);
        }
        //private bool IsValidPhoneNumber(string phoneNumber)
        //{
        //    // Kiểm tra định dạng số điện thoại 
        //    string pattern = @"^\d{10}$";
        //    return Regex.IsMatch(phoneNumber, pattern);
        //}
        //private bool IsValidPassword(string password)
        //{
        //    // Kiểm tra mật khẩu theo các ràng buộc:
        //    // - Tối thiểu 8 ký tự
        //    // - Ít nhất một kí tự đặc biệt
        //    // - Không chứa khoảng trắng
        //    string pattern = @"^(?=.*?[^\w\s]).{8,}$";
        //    return Regex.IsMatch(password, pattern);
        //}
        //private bool IsValidName(string name)
        //{
        //    // Kiểm tra tên không chứa khoảng trắng, không có ký tự đặc biệt
        //    string pattern = @"^[^\s\p{P}]+$";
        //    return Regex.IsMatch(name, pattern);
        //}
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string email = txtNhapEmail.Text;
            string ten = txtNhapHoTen.Text;
            string passWord = txtNhapMatKhau.Text;
            string sdt = txtNhapSDT.Text;
            string maxacnhan = txtNhapMaXN.Text;
            string hashPassword = HashPassword(passWord);
            string vaitro = "Khách hàng";

            if (otp.ToString().Equals(txtNhapMaXN.Text))
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(ten) || string.IsNullOrWhiteSpace(sdt) || string.IsNullOrWhiteSpace(passWord) || string.IsNullOrWhiteSpace(maxacnhan))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                //if (!IsValidName(ten))
                //{
                //    MessageBox.Show("Tên không được chứa khoảng trắng, không để trống và không có ký tự đặc biệt!");
                //    return;
                //}

                //if (!IsValidPhoneNumber(sdt))
                //{
                //    MessageBox.Show("Số điện thoại không hợp lệ!");
                //    return;
                //}

                //if (!IsValidPassword(passWord))
                //{
                //    MessageBox.Show("Mật khẩu không hợp lệ!");
                //    return;
                //}
                

                bool emailExists = CheckMail(email);

                if (emailExists)
                {
                    MessageBox.Show("Email đã tồn tại. Vui lòng đăng nhập hoặc khôi phục mật khẩu.", "Thông báo");
                }
                string connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=123;";
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO taikhoan (email, ten, matkhau, vaitro, sdt) VALUES (@in_email, @in_ten, @in_matkhau, @in_vaitro, @in_sdt)", connection))
                    {
                        command.Parameters.AddWithValue("@in_email", email);
                        command.Parameters.AddWithValue("@in_ten", ten);
                        command.Parameters.AddWithValue("@in_matkhau", hashPassword);
                        command.Parameters.AddWithValue("@in_sdt", sdt);
                        command.Parameters.AddWithValue("@in_vaitro", vaitro);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Đăng ký thành công!");

                    }
                }
            }
            else
            {
                MessageBox.Show("Mã xác nhận không chính xác!");
            }
        }
    
    }
}

   


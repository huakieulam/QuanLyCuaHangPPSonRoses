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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using DevExpress.XtraEditors.DXErrorProvider;

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
                string email = txtNhapEmail.Text;
                string name = txtNhapHoTen.Text;
                string phoneNumber = txtNhapSDT.Text;
                string password = txtNhapMatKhau.Text;

                // Kiểm tra xem tất cả các thông tin đã được nhập hay chưa
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin trước khi gửi mã xác nhận.", "Thông báo");
                    return;
                }


                otp = rd.Next(100000, 1000000);

                bool emailExists = CheckMail(email);

                if (emailExists)
                {
                    MessageBox.Show("Email đã tồn tại. Vui lòng đăng nhập hoặc khôi phục mật khẩu.", "Thông báo");
                }
                else
                {
                    var fromAddress = new MailAddress("kieulammm@gmail.com");
                    var toAddress = new MailAddress(email);
                    const string formPass = "wzzvfpyzcsxyoqsi";
                    const string subject = "Xác nhận đăng ký";
                    string body = otp.ToString();

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
                    
        }
        private bool IsValidName(string name)
        {
            // Kiểm tra chỉ chứa chữ cái có dấu và ít nhất một chữ cái
            string pattern = @"^[\p{L} ]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(name);
        }

        private bool IsValidPassword(string password)
        {
            //Kiểm tra độ dài ít nhất 8 ký tự
            if (password.Length < 8)
            {
                return false;
            }

            // Kiểm tra không chứa khoảng trắng
            if (password.Contains(" "))
            {
                return false;
            }

            // Kiểm tra chỉ chứa chữ cái in hoa, chữ cái thường và chữ số
            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$"))
            {
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            // Kiểm tra định dạng email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(email))
            {
                return false;
            }

            // Kiểm tra cuối email là "@gmail.com"
            string gmailSuffix = "@gmail.com";
            if (!email.EndsWith(gmailSuffix, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private void txtNhapEmail_TextChanged(object sender, EventArgs e)
        {
            string email = txtNhapEmail.Text;
            if (IsValidEmail(email))
            {
                // Email không hợp lệ
                // Hiển thị thông báo lỗi
                MessageBox.Show("Email không hợp lệ! Vui lòng nhập đúng định dạng email và kết thúc bằng @gmail.com.");
                txtNhapEmail.Text = "";
            }
        }
        private void txtNhapMatKhau_TextChanged(object sender, EventArgs e)
        {
            string password = txtNhapMatKhau.Text;
            if (password.Length >= 8 && !IsValidPassword(password))
            {
                // Mật khẩu không hợp lệ
                MessageBox.Show("Mật khẩu không hợp lệ! Mật khẩu phải chứa chữ cái in hoa, chữ cái thường và chữ số.");
                txtNhapMatKhau.Text = "";
            }
        }
       
        private void txtNhapMatKhau_Leave(object sender, EventArgs e)
        {
            string password = txtNhapMatKhau.Text;
            if (password.Length < 8)
            {
                // Mật khẩu không hợp lệ
                // Hiển thị thông báo lỗi
                MessageBox.Show("Mật khẩu không hợp lệ! Mật khẩu phải có ít nhất 8 ký tự.");
                txtNhapMatKhau.Text = "";
            }
        }
       
        private void txtNhapHoTen_TextChanged(object sender, EventArgs e)
        {

            string name = txtNhapHoTen.Text;
            if (!IsValidName(name))
            {
                MessageBox.Show("Tên không hợp lệ!");
                txtNhapHoTen.Text = ""; // Xóa nội dung nhập vào
            }
        }

        private void txtNhapSDT_TextChanged(object sender, EventArgs e)
        {
            string phoneNumber = txtNhapSDT.Text;
            if (phoneNumber.Length > 10)
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Số điện thoại chỉ có độ dài là 10 số.");
                txtNhapSDT.Text = "";
            }
        }
        private void txtNhapSDT_Leave(object sender, EventArgs e)
        {
            string phoneNumber = txtNhapSDT.Text;
            if (phoneNumber.Length < 10) //  khi độ dài nhỏ hơn 10
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Số điện thoại phải có đủ 10 số.");
                txtNhapSDT.Clear();
            }
        }

        private void txtNhapSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ngăn không cho phép nhập
                MessageBox.Show("Vui lòng chỉ nhập số vào ô số điện thoại.", "Thông báo");
            }
        }
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string email = txtNhapEmail.Text;
            string ten = txtNhapHoTen.Text;
            string passWord = txtNhapMatKhau.Text;
            string sdt = txtNhapSDT.Text;
            string maxacnhan = txtNhapMaXN.Text;
            string hashPassword = HashPassword(passWord);
            string vaitro = "Khách hàng";

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(ten) || string.IsNullOrWhiteSpace(sdt) || string.IsNullOrWhiteSpace(passWord) || string.IsNullOrWhiteSpace(maxacnhan))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }
            
            if (otp.ToString().Equals(maxacnhan))
            {
                bool emailExists = CheckMail(email);

                if (emailExists)
                {
                    MessageBox.Show("Email đã tồn tại. Vui lòng đăng nhập hoặc khôi phục mật khẩu.", "Thông báo");
                }
                else
                {
                    try
                    {
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
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi khi đăng ký: " + ex.Message);
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

   


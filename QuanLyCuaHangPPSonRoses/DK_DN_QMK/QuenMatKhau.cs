using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QuanLyCuaHangPPSonRoses.DK_DN_QMK
{
    public partial class QuenMatKhau : Form
    {
        public QuenMatKhau()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            DangNhap formDangNhap = new DangNhap();
            formDangNhap.Show();
        }

        Random rd = new Random();
        int otp;

        public bool CheckMail(string email)
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
        

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string email = txtNhapEmail.Text;
            string maXacNhan = txtNhapMaXN.Text;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(maXacNhan))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin Email và Mã xác nhận.");
            }
            else if (otp.ToString().Equals(txtNhapMaXN.Text))
            {
                // Mã xác nhận nhập đúng, thực hiện các hành động tiếp theo
                MessageBox.Show("Mã xác nhận đúng");
                // Chuyển đến form DoiMatKhau
                DoiMatKhau formDoiMatKhau = new DoiMatKhau(email);
                formDoiMatKhau.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Mã xác nhận không đúng");
            }
        }

        private void btnGuiMaXN_Click(object sender, EventArgs e)
        {
            string email = txtNhapEmail.Text;
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ Email.");
            }
            else if (CheckMail(email))
            {
                try
                {
                    otp = rd.Next(100000, 1000000);
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
                    using (var mes = new MailMessage(fromAddress, toAddress))
                    {
                        mes.Subject = subject;
                        mes.Body = body;
                        smtp.Send(mes);
                    }
                    MessageBox.Show("OTP đã được gửi qua email!", "Thông báo OTP");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Email không tồn tại trong cơ sở dữ liệu!");
            }
        }
    }
}

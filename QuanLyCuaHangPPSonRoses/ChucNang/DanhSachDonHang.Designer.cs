namespace QuanLyCuaHangPPSonRoses.ChucNang
{
    partial class DanhSachDonHang
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DanhSachDonHang));
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.panelHienThiDH = new System.Windows.Forms.FlowLayoutPanel();
            this.btnThoat = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 30;
            this.guna2Elipse1.TargetControl = this;
            // 
            // panelHienThiDH
            // 
            this.panelHienThiDH.AutoScroll = true;
            this.panelHienThiDH.BackColor = System.Drawing.Color.White;
            this.panelHienThiDH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelHienThiDH.Location = new System.Drawing.Point(12, 58);
            this.panelHienThiDH.Name = "panelHienThiDH";
            this.panelHienThiDH.Size = new System.Drawing.Size(744, 564);
            this.panelHienThiDH.TabIndex = 0;
            // 
            // btnThoat
            // 
            this.btnThoat.BackColor = System.Drawing.Color.Transparent;
            this.btnThoat.BorderRadius = 10;
            this.btnThoat.BorderThickness = 1;
            this.btnThoat.CheckedState.FillColor = System.Drawing.Color.White;
            this.btnThoat.CheckedState.ForeColor = System.Drawing.Color.Black;
            this.btnThoat.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnThoat.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnThoat.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnThoat.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnThoat.FillColor = System.Drawing.Color.White;
            this.btnThoat.Font = new System.Drawing.Font("Sitka Text", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThoat.ForeColor = System.Drawing.Color.Black;
            this.btnThoat.Image = ((System.Drawing.Image)(resources.GetObject("btnThoat.Image")));
            this.btnThoat.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnThoat.ImageSize = new System.Drawing.Size(25, 25);
            this.btnThoat.Location = new System.Drawing.Point(12, 14);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(107, 38);
            this.btnThoat.TabIndex = 22;
            this.btnThoat.Text = "Trở lại";
            this.btnThoat.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnThoat.TextOffset = new System.Drawing.Point(5, -1);
            this.btnThoat.UseTransparentBackground = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // DanhSachDonHang
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(770, 631);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.panelHienThiDH);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DanhSachDonHang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DanhSachDonHang";
            this.Load += new System.EventHandler(this.DanhSachDonHang_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private System.Windows.Forms.FlowLayoutPanel panelHienThiDH;
        private Guna.UI2.WinForms.Guna2Button btnThoat;
    }
}
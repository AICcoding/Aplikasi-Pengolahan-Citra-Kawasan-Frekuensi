using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;

namespace Aplikasi_Pengolahan_Citra_Kawasan_Frekuensi
{
    public partial class Form1 : Form
    {
        Bitmap gambar_awal, gambar_hasil, gambar_tmp, gambar_cari_diameter, DFT_red, DFT_green, DFT_blue;
        Image<Bgr, Byte> gambar_awal_e;

        bool tambah_filter, menggambar;
        Point koordinat_awal, koordinat_akhir;
        int jari_jari_lingkaran;

        public Form1()
        {
            InitializeComponent();

            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            comboBox2.Enabled = false;
            comboBox4.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;

            comboBox2.Text = "Pilih canel";
            comboBox2.Items.Add("Red");
            comboBox2.Items.Add("Green");
            comboBox2.Items.Add("Blue");
            //comboBox2.Items.Add("Grayscale");

            comboBox4.Text = "Pilih canel";
            comboBox4.Items.Add("Red");
            comboBox4.Items.Add("Green");
            comboBox4.Items.Add("Blue");
            //comboBox4.Items.Add("Grayscale");

            tambah_filter = false;
            menggambar = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog pilih_gambar = new OpenFileDialog();
            pilih_gambar.Filter = "File gambar (*.BMP; *.JPG; *.PNG)|*.BMP; *.JPG; *.PNG";
            if (pilih_gambar.ShowDialog() == DialogResult.OK)
            {
                gambar_awal_e = new Image<Bgr, byte>(pilih_gambar.FileName);

                gambar_awal = new Bitmap(new Bitmap(pilih_gambar.FileName)); //gambar asli
                gambar_hasil = new Bitmap(new Bitmap(pilih_gambar.FileName)); 
                gambar_tmp = new Bitmap(new Bitmap(pilih_gambar.FileName)); //buat proses pengolahan
                gambar_cari_diameter = new Bitmap(new Bitmap(pilih_gambar.FileName)); //gambar membuat lingkaran filter

                DFT_red = new Bitmap(new Bitmap(pilih_gambar.FileName)); //simpan gambar DFT red
                DFT_green = new Bitmap(new Bitmap(pilih_gambar.FileName));
                DFT_blue = new Bitmap(new Bitmap(pilih_gambar.FileName));

                pictureBox1.Image = gambar_awal_e.ToBitmap();
                //pictureBox1.Image = gambar_awal;

                button2.Enabled = true;
                button3.Enabled = false;
                button4.Enabled = false;
                checkBox1.Enabled = true;
                checkBox2.Enabled = false;
                comboBox4.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            label2.Text = "0";
            jari_jari_lingkaran = 0;
            gambar_cari_diameter = (Bitmap)gambar_awal.Clone();
            if(checkBox1.Checked==true)
            {
                tambah_filter = true;
                pictureBox1.Image = gambar_cari_diameter;

                button4.Enabled = true;
                checkBox2.Enabled = true;
                checkBox2.Checked = true;
                label1.Enabled = true;
                label2.Enabled = true;
                MessageBox.Show("Silakan tambahkan filter (klik & drag) pada gambar awal !",
                                   "Mode tambah filter", MessageBoxButtons.OK,
                                   MessageBoxIcon.Information,
                                   0);
            }
            else if(checkBox1.Checked==false)
            {
                tambah_filter = false;
                pictureBox1.Image = gambar_awal;
                button4.Enabled = false;
                checkBox2.Enabled = false;
                checkBox2.Checked = false;
                label1.Enabled = false;
                label2.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            button4.Enabled = true;
            checkBox1.Enabled = true;

            comboBox2.Enabled = true;
            comboBox4.Enabled = false;

            comboBox2.SelectedIndex = 0;

            cari_DFT_gambar_awal();
        }

        private void cari_DFT_gambar_awal()
        {
            int N ,M, phi;
            double tampung_R, tampung_G, tampung_B;
            N=gambar_tmp.Height;
            M=gambar_tmp.Width;
            phi = 180;

            for(int u=0;u<gambar_tmp.Width;u++)
            {
                for(int v=0;v<gambar_tmp.Height;v++)
                {
                    tampung_R=0;
                    tampung_G=0;
                    tampung_B=0;
                    for (int x = 0; x < gambar_tmp.Width; x++)
                    {
                        for (int y = 0; y < gambar_tmp.Height; y++)
                        {
                            tampung_R += gambar_tmp.GetPixel(x, y).R * Math.Cos(2*phi*((u * x / N) + (v * y / M)));
                            tampung_G += gambar_tmp.GetPixel(x, y).G * Math.Cos(2*phi*((u * x / N) + (v * y / M)));
                            tampung_B += gambar_tmp.GetPixel(x, y).B * Math.Cos(2*phi*((u * x / N) + (v * y / M)));
                        }
                    }
                    if (tampung_R > 255)
                        tampung_R = 255;
                    else if (tampung_R < 0)
                        tampung_R = 0;

                    if (tampung_G > 255)
                        tampung_G = 255;
                    else if (tampung_G < 0)
                        tampung_G = 0;

                    if (tampung_B > 255)
                        tampung_B = 255;
                    else if (tampung_B < 0)
                        tampung_B = 0;

                    DFT_red.SetPixel(u, v, Color.FromArgb(Convert.ToInt16(tampung_R), 0, 0));
                    DFT_green.SetPixel(u, v, Color.FromArgb(0, Convert.ToInt16(tampung_G), 0));
                    DFT_blue.SetPixel(u, v, Color.FromArgb(0, 0, Convert.ToInt16(tampung_B)));
                }
            }
            pictureBox2.Image = DFT_red;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            button4.Enabled = true;
            checkBox1.Enabled = true;

            comboBox2.Enabled = true;
            comboBox4.Enabled = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked==true)
            {
                pictureBox1.Image = gambar_cari_diameter;
            }
            else if(checkBox2.Checked==false)
            {
                pictureBox1.Image = gambar_awal;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = gambar_awal;
            label2.Text = "0";
            jari_jari_lingkaran = 0;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex == 0)
            {
                pictureBox2.Image = DFT_red;
            }
            else if (comboBox2.SelectedIndex == 1)
            {
                pictureBox2.Image = DFT_green;
            }
            else if (comboBox2.SelectedIndex == 2)
            {
                pictureBox2.Image = DFT_blue;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {        
            if(tambah_filter==true)
            {
                gambar_cari_diameter = (Bitmap)gambar_awal.Clone();
                pictureBox1.Image = gambar_cari_diameter;
                menggambar = true;
                koordinat_awal.X = gambar_cari_diameter.Width / 2;
                koordinat_awal.Y = gambar_cari_diameter.Height / 2;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(menggambar==true)
            {
                //gambar lingkaran
                koordinat_akhir.X = e.X;
                koordinat_akhir.Y = e.Y;
                gambar_cari_diameter = (Bitmap)gambar_awal.Clone();
                gambar_lingkaran_bresenham(koordinat_awal, koordinat_akhir, Color.Black);
            }
        }

        private void gambar_lingkaran_bresenham(Point titik_pusat, Point titik_luar, Color warna)
        {
            int radius;
            Point titik;

            //mencari jari-jari lingkaran
            if (titik_pusat.Y != titik_luar.Y)
            {
                double A, B, C;
                A = Math.Abs(titik_luar.X - titik_pusat.X);
                B = Math.Abs(titik_luar.Y - titik_pusat.Y);
                A = Math.Pow(A, 2);
                B = Math.Pow(B, 2);
                C = Math.Sqrt(A + B);
                radius = (int)Math.Floor(C);
            }
            else
            {
                radius = Math.Abs(koordinat_akhir.X - koordinat_awal.X);
            }

            label2.Text = Convert.ToString(radius);
            jari_jari_lingkaran = radius;

            if (titik_luar != titik_pusat)
            {
                int f = 1 - radius;
                int ddF_x = 1;
                int ddF_y = -2 * radius;
                int x = 0;
                int y = radius;

                put_piksel(titik_pusat.X, titik_pusat.Y + radius, warna);
                put_piksel(titik_pusat.X + radius, titik_pusat.Y, warna);
                put_piksel(titik_pusat.X, titik_pusat.Y - radius, warna);
                put_piksel(titik_pusat.X - radius, titik_pusat.Y, warna);

                while (x < y)
                {
                    // ddF_x == 2 * x + 1;
                    // ddF_y == -2 * y;
                    // f == x*x + y*y - radius*radius + 2*x - y + 1;

                    if (f >= 0) //jika pk lebih besar/sama dengan 0
                    {
                        y--;
                        x++;

                        ddF_y += 2;
                        ddF_x += 2;

                        f += ddF_y;
                        f += ddF_x;
                    }
                    else
                    {
                        x++;
                        ddF_x += 2;
                        f += ddF_x;
                    }

                    put_piksel(titik_pusat.X + x, titik_pusat.Y + y, warna);
                    put_piksel(titik_pusat.X + y, titik_pusat.Y + x, warna);

                    put_piksel(titik_pusat.X + y, titik_pusat.Y - x, warna);
                    put_piksel(titik_pusat.X + x, titik_pusat.Y - y, warna);

                    put_piksel(titik_pusat.X - x, titik_pusat.Y - y, warna);
                    put_piksel(titik_pusat.X - y, titik_pusat.Y - x, warna);

                    put_piksel(titik_pusat.X - y, titik_pusat.Y + x, warna);
                    put_piksel(titik_pusat.X - x, titik_pusat.Y + y, warna);                    
                }
            }
            else
            {
                put_piksel(titik_pusat.X, titik_pusat.Y, warna);
            }
        }

        private void put_piksel(int x, int y, Color warna_kotak)
        {
            int x_awal, y_awal, x_akhir, y_akhir;

            /*x_awal x * ukuran_grid;
            y_awal = y * ukuran_grid;

            x_akhir = x_awal + ukuran_grid;
            y_akhir = y_awal + ukuran_grid;*/

            x_awal = x;
            y_awal = y;

            x_akhir = x_awal + 1;
            y_akhir = y_awal + 1;

            //if ((x_akhir <= ukuran_grid * jumlah_grid_panjang && y_akhir <= ukuran_grid * jumlah_grid_lebar) && (x >= 0 && y >= 0))
            //{
            
            try
            {
                for (int i = x_awal; i < x_akhir; i++)
                {
                    for (int j = y_awal; j < y_akhir; j++)
                    {
                        gambar_cari_diameter.SetPixel(i, j, warna_kotak);
                    }
                }
                pictureBox1.Image = gambar_cari_diameter;
            }
            catch { }
            //}
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            menggambar = false;
        }
    }
}

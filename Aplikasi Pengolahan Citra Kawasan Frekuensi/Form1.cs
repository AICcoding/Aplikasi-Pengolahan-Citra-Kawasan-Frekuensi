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
        Bitmap gambar_awal, gambar_hasil, gambar_tmp, DFT_red, DFT_green, DFT_blue;
        Image<Bgr, Byte> gambar_awal_e, DFT_red_e, DFT_green_e, DFT_blue_e, DFT_grayscale_e, gambar_akhir_e;

        public Form1()
        {
            InitializeComponent();

            button2.Enabled = false;
            button3.Enabled = false;
            comboBox2.Enabled = false;
            comboBox4.Enabled = false;

            comboBox2.Text = "Pilih canel";
            comboBox2.Items.Add("Grayscale");
            comboBox2.Items.Add("Red");
            comboBox2.Items.Add("Green");
            comboBox2.Items.Add("Blue");
            

            comboBox4.Text = "Pilih canel";
            comboBox4.Items.Add("Grayscale");
            comboBox4.Items.Add("Red");
            comboBox4.Items.Add("Green");
            comboBox4.Items.Add("Blue");
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog pilih_gambar = new OpenFileDialog();
            pilih_gambar.Filter = "File gambar (*.BMP; *.JPG; *.PNG)|*.BMP; *.JPG; *.PNG";
            if (pilih_gambar.ShowDialog() == DialogResult.OK)
            {
                gambar_awal_e = new Image<Bgr, byte>(pilih_gambar.FileName);
                DFT_grayscale_e = new Image<Bgr, byte>(pilih_gambar.FileName);

                gambar_awal = new Bitmap(new Bitmap(pilih_gambar.FileName)); //gambar asli
                gambar_hasil = new Bitmap(new Bitmap(pilih_gambar.FileName)); 
                gambar_tmp = new Bitmap(new Bitmap(pilih_gambar.FileName)); //buat proses pengolahan

                DFT_red = new Bitmap(new Bitmap(pilih_gambar.FileName)); //simpan gambar DFT red
                DFT_green = new Bitmap(new Bitmap(pilih_gambar.FileName));
                DFT_blue = new Bitmap(new Bitmap(pilih_gambar.FileName));

                pictureBox1.Image = gambar_awal_e.ToBitmap();

                button2.Enabled = true;
                button3.Enabled = false;
                comboBox4.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;

            comboBox2.Enabled = true;
            comboBox4.Enabled = false;

            comboBox2.SelectedIndex = 0;

            gambarAwal_ke_DFT_e();
        }

        private void gambarAwal_ke_DFT_e()
        {
            DFT_red_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);
            DFT_green_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);
            DFT_blue_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);
            DFT_grayscale_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);

            Byte[,,] GetPixel_e = gambar_awal_e.Data; //Mengambil warna dari gambar awal

            Byte[, ,] SetPixelR_e = DFT_red_e.Data; //Mengeset warna ke gambar akhir
            Byte[, ,] SetPixelG_e = DFT_green_e.Data; //Mengeset warna ke gambar akhir
            Byte[, ,] SetPixelB_e = DFT_blue_e.Data; //Mengeset warna ke gambar akhir
            Byte[, ,] SetPixelGS_e = DFT_grayscale_e.Data; //Mengeset warna ke gambar akhir

            int N, M, phi;
            double r, g, b, gs;
            double mag;

            N = gambar_tmp.Width;
            M = gambar_tmp.Height;
            phi = 180;

            for (int u = 0; u < M; u++)
            {
                for (int v = 0; v < N; v++)
                {
                    r = 0;
                    g = 0;
                    b = 0;
                    gs = 0;
                    for (int x = 0; x < M; x++)
                    {
                        for (int y = 0; y < N; y++)
                        {
                            gs += (((float)GetPixel_e[x, y, 0] + (float)GetPixel_e[x, y, 1] + (float)GetPixel_e[x, y, 2]) / 3F) * Math.Cos(2 * Math.PI * (((float)u * (float)x / (float)M) + ((float)v * (float)y / (float)N)));
                            r += ((float)GetPixel_e[x, y, 2]) * cos(2 * phi * (((float)u * (float)x / (float)M) + ((float)v * (float)y / (float)N)));
                            g += ((float)GetPixel_e[x, y, 1]) * cos(2 * phi * (((float)u * (float)x / (float)M) + ((float)v * (float)y / (float)N)));
                            b += ((float)GetPixel_e[x, y, 0]) * cos(2 * phi * (((float)u * (float)x / (float)M) + ((float)v * (float)y / (float)N)));
                        }
                    }

                    if (r > 255)
                        r = 255;
                    else if (r < 0)
                        r = 0;

                    if (g > 255)
                        g = 255;
                    else if (g < 0)
                        g = 0;

                    if (b > 255)
                        b = 255;
                    else if (b < 0)
                        b = 0;

                    if (gs < 0)
                        gs = 0;
                    else if (gs > 255)
                        gs = 255;

                    SetPixelR_e[u, v, 0] = 0;
                    SetPixelR_e[u, v, 1] = 0;
                    SetPixelR_e[u, v, 2] = (byte)r;

                    SetPixelG_e[u, v, 0] = 0;
                    SetPixelG_e[u, v, 1] = (byte)g;
                    SetPixelG_e[u, v, 2] = 0;

                    SetPixelB_e[u, v, 0] = (byte)b;
                    SetPixelB_e[u, v, 1] = 0;
                    SetPixelB_e[u, v, 2] = 0;

                    SetPixelGS_e[u, v, 0] = (byte)gs;
                    SetPixelGS_e[u, v, 1] = (byte)gs;
                    SetPixelGS_e[u, v, 2] = (byte)gs;             
                }
            }
            pictureBox2.Image = DFT_grayscale_e.ToBitmap();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;

            comboBox2.Enabled = true;
            comboBox4.Enabled = true;
            DFT_ke_gambarAwal_e();
        }

        private void DFT_ke_gambarAwal_e()
        {
            gambar_akhir_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);

            Byte[, ,] GetPixel_e = DFT_grayscale_e.Data; //Mengambil warna dari gambar awal

            Byte[, ,] SetPixel_e = gambar_akhir_e.Data; //Mengeset warna ke gambar akhir       

            int N, M, phi;
            double gs;

            N = gambar_tmp.Width;
            M = gambar_tmp.Height;
            phi = 180;

            for (int u = 0; u < M; u++)
            {
                for (int v = 0; v < N; v++)
                {
                    gs = 0;
                    for (int x = 0; x < M; x++)
                    {
                        for (int y = 0; y < N; y++)
                        {
                            gs += (((float)GetPixel_e[x, y, 0] + (float)GetPixel_e[x, y, 1] + (float)GetPixel_e[x, y, 2]) / 3F) * cos(2 * phi * (((float)u * (float)x / (float)M) + ((float)v * (float)y / (float)N)));
                        }
                    }

                    gs = (1F / M * N) * gs;

                    if (gs < 0)
                        gs = 0;
                    else if (gs > 255)
                        gs = 255;

                    SetPixel_e[u, v, 0] = (byte)gs;
                    SetPixel_e[u, v, 1] = (byte)gs;
                    SetPixel_e[u, v, 2] = (byte)gs;
                }
            }
            pictureBox3.Image = gambar_akhir_e.ToBitmap();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex == 0)
            {
                pictureBox2.Image = DFT_grayscale_e.ToBitmap();
            }
            else if (comboBox2.SelectedIndex == 1)
            {
                pictureBox2.Image = DFT_red_e.ToBitmap();
            }
            else if (comboBox2.SelectedIndex == 2)
            {
                pictureBox2.Image = DFT_green_e.ToBitmap();
            }
            else if (comboBox2.SelectedIndex == 3)
            {
                pictureBox2.Image = DFT_blue_e.ToBitmap();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            kalkulator a = new kalkulator();
            a.Show();
        }

        private double cos(double sudut)
        {
            double hasil, tmp;
            tmp = sudut * Math.PI / 180F;
            hasil = Math.Cos(tmp);
            hasil = Math.Round(hasil,4);
            return hasil;
        }

        private void button4_Click(object sender, EventArgs e)
        {        
            MessageBox.Show("Dibuat Oleh :\n\n1. I Wayan Ariantha Sentanu\t[1308605009]\n2. I Gede Pramarta Sedana\t[1308605027]\n3. I Putu Agus Suarya Wibawa\t[1308605034]\n4. Daniel Kurniawan\t\t[1308605039]",
                                  "Tentang Kami", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information,
                                  0);
        }
    }
}

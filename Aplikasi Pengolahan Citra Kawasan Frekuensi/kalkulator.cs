using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplikasi_Pengolahan_Citra_Kawasan_Frekuensi
{
    public partial class kalkulator : Form
    {
        public kalkulator()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            int N, M;
            double gs, fx, ux_bagi_m, vy_bagi_n;
            N = 5;
            M = dataGridView1.Rows.Count - 1;

            for (int u = 0; u < M; u++)
            {
                dataGridView2.Rows.Add(1);
                for (int v = 0; v < N; v++)
                {
                    gs = 0;
                    for (int x = 0; x < M; x++)
                    {
                        for (int y = 0; y < N; y++)
                        {
                            fx=float.Parse(dataGridView1.Rows[x].Cells[y].Value.ToString());
                            ux_bagi_m=(float)u * (float)x / (float)M;
                            vy_bagi_n=(float)v * (float)y / (float)N;

                            gs += fx * cos(360F * (ux_bagi_m + vy_bagi_n));
                        }
                    }                                   
                    dataGridView2.Rows[u].Cells[v].Value = gs;                
                }
            }
        }
        private double cos(double sudut)
        {
            double hasil, tmp;
            tmp = sudut * Math.PI / 180F;
            hasil = Math.Cos(tmp);
            hasil = Math.Round(hasil, 4);
            return hasil;
        }
    }
}

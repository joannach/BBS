using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.IO;
//using System.

namespace BBS
{
    // ZAPIS KLUCZA DO PLIKU ZROBIC!!! 
    // ODCZYT KLUCZA Z PLIKU!!! 
    public partial class Form1 : Form
    {
        int n = 0;
        int p, q, x = 0;
        int x0 = 0;
        int[] xo;
        int t = 32;
        string klucz;

        bool czy_pierwsza(int a)
        {
            if (a < 2)
                return false; //gdy liczba jest mniejsza niż 2 to nie jest pierwszą

            for (int i = 2; i * i <= a; i++)
                if (a % i == 0)
                    return false; //gdy znajdziemy dzielnik, to dana liczba nie jest pierwsza
            return true;
        }

        int nwd(int a, int b)
        {
            while (a != b)
                if (a > b)
                    a -= b; //lub a = a - b;
                else
                    b -= a; //lub b = b-a
            return a; // lub b - obie zmienne przechowują wynik NWD(a,b)
        }


        private void button1_Click(object sender, EventArgs e)              //wczytywanie tekstu z pliku
        {
            OpenFileDialog D = new OpenFileDialog();
            if (D.ShowDialog() == DialogResult.OK)
            {
                string Fn = D.FileName;

                FileStream Fs = new FileStream(Fn, FileMode.Open, FileAccess.Read);
                StreamReader F = new StreamReader(Fs);

                string buffer = F.ReadToEnd();

                textBox2.Text = buffer;

                F.Close();
                Fs.Close();
            }
        }


        public string zamien_na_bity(string tekst)
        {
            string wynik = "";
            for (int i = 0; i < tekst.Length; i++)
            {
                char c = tekst[i];
                for (int j = 15; j >= 0; j--)
                {
                    if ((c & (1 << j)) == 0)
                        wynik += "1";

                    else
                        wynik += "0";
                }
            }
            return wynik;
        }


        private void button4_Click(object sender, EventArgs e)
        {

            
        }

        int power(int a, int b)
        {
            int wynik=0;

            for (int i = 0; i < b; i++)
            {
                wynik += a * a;
            }
            return wynik;
        }

        public void generatorklucza()
        {
            xo = new int[int.Parse(textBox6.Text)];

            for (int i = 0; i < xo.Length; i++)           // ZALEZNE OD DLUGOSCI TEKSTU
            {
                x0 = (x0*x0) % n;
                xo[i] = x0;                                         //zapisuje kolejne wartosci x0 do tablicy 
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            if (!czy_pierwsza(p))
                MessageBox.Show("Podana liczba p nie jest liczba pierwsza!");
            if (!czy_pierwsza(q))
                MessageBox.Show("Podana liczba q nie jest liczba pierwsza!");

            n = p * q;

            // znalezienie liczby x - wzglednie pierwszej (jesli nwd(n, x) = 1)


            while (true)
            {
                x = r.Next(1, n);
                if (x % p != 0 && x % q != 0)
                    break;
            }

            if (n == 0)
                MessageBox.Show("Prosze podac p i q");
            else
            {
                x0 = (x * x) % n;

                generatorklucza();
                klucz = "";

                for (int i = 0; i < xo.Length; i++)
                {
                    klucz += xo[i] & 1;
                }


                textBox3.Text = klucz;
                System.IO.File.WriteAllText("moj_klucz.txt", klucz);
            }

        }



        public Form1()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            t = int.Parse(textBox6.Text)+1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            p = Convert.ToInt32(textBox8.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            q = Convert.ToInt32(textBox7.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox9.Text = n.ToString();
        }

        private void button_zaszyfruj_Click(object sender, EventArgs e)
        {
            string tekst_bitowo = zamien_na_bity(textBox1.Text);
            textBox4.Text = tekst_bitowo;

            string zaszyfrowany = "";
            for (int i = 0; i < tekst_bitowo.Length; i++)
            {
                char c1 = tekst_bitowo[i];
                char c2 = klucz[i % klucz.Length];
                if (c1 == c2)
                    zaszyfrowany += '0';
                else
                    zaszyfrowany += '1';
            }
            textBox11.Text = zaszyfrowany;
        }

        private void button_szyfruj_Click(object sender, EventArgs e)
        {
            string tekst_bitowo = zamien_na_bity(textBox1.Text);
            textBox_bitowo.Text = tekst_bitowo;

            string zaszyfrowany = "";
            for (int i = 0; i < tekst_bitowo.Length; i++)
            {
                char c1 = tekst_bitowo[i];
                char c2 = klucz[i%klucz.Length];
                if (c1 == c2)
                    zaszyfrowany += '0';
                else
                    zaszyfrowany += '1';
            }
            textBox10.Text = zaszyfrowany;
        }

        public string zamien_na_znaki(string bity)
        {
            string wynik = "";
            if (bity.Length % 16 == 0)
            {
                for (int i = 0; i < bity.Length; i += 16)
                {
                    int znak = 0;
                    for (int j = 0; j < 16; j++)
                    {
                        if (bity[i + j] == '1')
                            znak |= 1 << j;
                    }
                    wynik += (char)znak;
                }
            }
            else
            {
                MessageBox.Show("Błąd!");
            }

            return wynik;
        }


        private void button_testy_Click(object sender, EventArgs e)         //  POJEDYNCZYCH BITOW
        {
            textBox12.Text = klucz;
            double licznik = 0;
            for (int i = 0; i < textBox12.TextLength; i++)
            {
                if (klucz[i] == '1')
                {
                    licznik++;
                }
            }
            string tekst = licznik.ToString();
            if (licznik > (textBox12.TextLength) * (0.475) && licznik < (textBox12.TextLength) * (0.975))
                textBox13.Text = "Test zdany - ilość wystąpień liczby 1 jest w normie i wynosi " + tekst;
            else
                textBox13.Text = "Test niezdany - ilość wystąpień liczby 1 nie mieści się w normie i wynosi" + tekst;
        }

        private void button9_Click(object sender, EventArgs e)          //  KLUCZ Z PLIKU
        {
            OpenFileDialog D = new OpenFileDialog();
            if (D.ShowDialog() == DialogResult.OK)
            {
                string Fn = D.FileName;

                FileStream Fs = new FileStream(Fn, FileMode.Open, FileAccess.Read);
                StreamReader F = new StreamReader(Fs);

                string buffer = F.ReadToEnd();

                textBox16.Text = buffer;

                F.Close();
                Fs.Close();
            }
            klucz = textBox16.Text;
        }

        private void button10_Click(object sender, EventArgs e)         //  TEST SERII
        {
            textBox17.Text = klucz;
            int seria = 0;
            String tekst = textBox17.Text;
            char porownaj = tekst[0];
            int s1 = 0, s2=0, s3=0, s4=0, s5=0, s6=0;

            foreach (char z in tekst)
            {
                if (porownaj == z)
                    seria++;
                else
                {
                    porownaj = z;
                    if (seria == 1)
                        s1++;
                    if (seria == 2)
                        s2++;
                    if (seria == 3)
                        s3++;
                    if (seria == 4)
                        s4++;
                    if (seria == 5)
                        s5++;
                    if (seria >= 6)
                        s6++;
                    seria = 0;
                }
            }
            double z1p = (textBox17.TextLength * 0.11575);
            double z1k = (textBox17.TextLength * 0.13425);
            double z2p = (textBox17.TextLength * 0.0557);
            double z2k = (textBox17.TextLength * 0.0693);
            double z3p = (textBox17.TextLength * 0.02635);
            double z3k = (textBox17.TextLength * 0.03615);
            double z4p = (textBox17.TextLength * 0.012);
            double z4k = (textBox17.TextLength * 0.0192);
            double z5p = (textBox17.TextLength * 0.00515);
            double z5k = (textBox17.TextLength * 0.01045);
            double z6p = (textBox17.TextLength * 0.00515);
            double z6k = (textBox17.TextLength * 0.01045);
             

            if ( s1 >= z1p && s1 <= z1k
                && s2 >= z2p && s2 <= z2k
                && s3 >= z3p && s3 <= z3k
                && s4 >= z4p && s4 <= z4k
                && s5 >= z5p && s5 <= z5k
                && s6 >= z6p && s6 <= z6k)
            {
                textBox18.Text = "Test zdany";
            }
            else
            {
                textBox18.Text = "Test niezdany";
            }
        }

        double[] pojedyncze()
        {
            int n0 = 0, n1 = 0;
            double[] t = new double[3];

            for (int i = 0; i < klucz.Length; i++)
            {
                if (klucz[i] == 0)
                    n0++;
                else
                    n1++;
            }

            double res;
            res = (double)((n0 - n1) * (n0 - n1)) / (double)klucz.Length;


            t[0] = n0;
            t[1] = n1;
            t[2] = res;
            return t;
        }

        private void button11_Click(object sender, EventArgs e)         //  PAR BITOW
        {
            textBox19.Text = klucz;
            double[] t = pojedyncze();
            double n0 = t[0], n1 = t[1];

            int[,] tabk = new int[,] { { 00, 0 }, { 01, 0 }, { 10, 0 }, { 11, 0 }, };

            int number = 0;

            for (int i = 0; klucz.Length - i > 1; i++)
            {
                number = klucz[i] * 10 + klucz[i + 1];
                for (int j = 0; j < 4; j++)
                    if (number == tabk[j, 0])
                    {
                        tabk[j, 1]++;
                        break;
                    }
            }
            double suma = 0;

            for (int i = 0; i < 4; i++)
                suma += tabk[i, 1] * tabk[i, 1];

            double a = (double)4 / (double)(klucz.Length - 1);
            double b = (double)2 / (double)klucz.Length;

            double res;
            res = (a * suma) - (b * (n0 * n0 + n1 * n1)) + 1;

            textBox20.Text = " S2 równe " + Math.Round(res, 4) + Environment.NewLine;

        }

        private void button4_Click_1(object sender, EventArgs e)            //  POKEROWY
        {
            textBox15.Text = klucz;
            int[,] tabk = new int[,] { 
            { 0000, 0 }, { 0001, 0 }, { 0010, 0 }, { 0011, 0 },
            { 0100, 0 }, { 0101, 0 }, { 0110, 0 }, { 0111, 0 }, 
            { 1000, 0 }, { 1001, 0 }, { 1010, 0 }, { 1011, 0 }, 
            { 1100, 0 }, { 1101, 0 }, { 1110, 0 }, { 1111, 0 },};
            int number = 0;

            for (int i = 0; klucz.Length - i > 3; i += 4)
            {
                number = klucz[i] * 1000 + klucz[i + 1] * 100 + klucz[i + 2] * 10 + klucz[i + 3];
                for (int j = 0; j < 16; j++)
                    if (number == tabk[j, 0])
                    {
                        tabk[j, 1]++;
                        break;
                    }
            }

            double temp, si = 0;

            for (int i = 0; i < 16; i++)
            {
                temp = (double)tabk[i, 1] * (double)tabk[i, 1];
                si += temp;
            }

            double res;
            res = (double)16 / ((double)klucz.Length / 4) * si - klucz.Length;

            if (res > 2.16 && res < 46.17)
                textBox14.Text = "Zaliczony X=" + Math.Round(res, 4) + Environment.NewLine;
            else
                textBox14.Text = "Niezaliczony X=" + Math.Round(res, 4) + Environment.NewLine;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string deszyfrowany = "";
            for (int i = 0; i < textBox10.Text.Length; i++)
            {
                char c1 = textBox10.Text[i];
                char c2 = klucz[i % klucz.Length];
                if (c1 == c2)
                    deszyfrowany += '0';
                else
                    deszyfrowany += '1';
            }
            textBox5.Text = zamien_na_znaki(deszyfrowany);
        }

        }
    }


//deszyfrowanie xor zaszyfrowanego z kluczem, wykorzystac funkcje na koncuzamien na znaki

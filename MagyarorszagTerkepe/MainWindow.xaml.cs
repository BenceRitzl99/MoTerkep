using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace MagyarorszagTerkepe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double[] x_ek = new double[3137];
        //List<double> x_ek2 = new List<double>(); //amennyiben nem tudnánk előre hogy hány település is van, ez egy szerencsésebb tároló
        double[] y_ok = new double[3137];
        //List<double> y_ok2 = new List<double>();
        public MainWindow()
        {
            InitializeComponent();
            StreamReader file = new StreamReader("tables.helyseg_hu.csv");
            int db = 0;
            file.ReadLine();
            while(!file.EndOfStream)
            {
                string sor = file.ReadLine();
                string[] reszek = sor.Split(';'); //név;x;y
                string[] fpszp = reszek[1].Split(new char[] {':','.'});
                double x = int.Parse(fpszp[0]); //ez a fok egész értéke
                x += (double.Parse(fpszp[1]) / 60);
                x += (double.Parse(fpszp[2]) / 6000);
                x_ek[db] = x;
                //x_ek2.Add(x);
                fpszp = reszek[2].Split(new char[] {':','.'});
                double y = int.Parse(fpszp[0]); //ez a fok egész értéke
                y += (double.Parse(fpszp[1]) / 60);
                y += (double.Parse(fpszp[2]) / 6000);
                y_ok[db] = y;
                //y_ok2.Add(y);
                db++;
            }
            file.Close();
            // MessageBox.Show($"Beolvasott sorok száma: {db}");
        }

        private void Rajzlap_MouseUp(object sender, MouseButtonEventArgs e)
              
        {
            double xmeret = Rajzlap.Width / 7;
            double ymeret = Rajzlap.Height / 3;
            for (int i = 0; i < x_ek.Length; i++)
            {
                //MessageBox.Show("Kattintás");
                Line vonal = new Line();
                vonal.X1 = 16/x_ek[i] * xmeret*3;
                vonal.Y1 = 45.7/y_ok[i] * ymeret*5;
                vonal.X2 = vonal.X1 + 1;
                vonal.Y2 = vonal.Y1 + 1;
                vonal.Stroke = Brushes.Black;
                vonal.StrokeThickness = 5;
                Rajzlap.Children.Add(vonal);
            }
        }
       
    }
}

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

        private void Terkep(object sender, MouseButtonEventArgs e)
              
        {
            Rajzolas();
        }

        private void Vonal_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show($"({((Line)sender).X1};{((Line)sender).Y1}"); //jó lenne ha minél nagyobb egy település, annál nagyobb legyen a pont.
            MessageBox.Show($"{((Rectangle)sender).Margin}");
        }

        private void Atmeretezes(object sender, SizeChangedEventArgs e)
        {
            //MessageBox.Show("Lényegében itt kellene lebonyolítani a rajzolást!");
            Rajzolas();

        }

        void Rajzolas()
        {
            // Jobb volna Line helyett Rectangle vagy Ellipse, ami nagyobb kiterjedésű
            Rajzlap.Children.Clear(); // reszponzívvá vált.
            //MessageBox.Show("Méret: " + this.ActualWidth + " x " + this.ActualHeight); // kiírja az ablak aktuális méretét.
            Rajzlap.Width = this.ActualWidth - 20;
            Rajzlap.Height = this.ActualHeight - 20;
            double xmeret = Rajzlap.Width / 7;
            double ymeret = Rajzlap.Height / 3;
            for (int i = 0; i < x_ek.Length; i++)
            {
                //MessageBox.Show("Kattintás");
                Rectangle vonal = new Rectangle();
                vonal.Width = 7;
                vonal.Height = 7;
                vonal.Margin = new Thickness((x_ek[i] - 16) * xmeret + 3, (48.7 - y_ok[i]) * ymeret + 3,0,0);
                // Mivel a Rectangle-nek és Ellipse-nek is van kiterjedése: szélessége, magassága, érdemes a pozícionálást,
                // amennyiben 7x7-es, ennek a négyszögnek a középpontjába orientálni.
                // Alaphelyzetben a pozíció a bal felső sarok lenne, de ha ahhoz képest,
                // mind vízszintesen, mind függőlegesen a +3 eltolást alkalmazzuk a 7x7-es közepéhez jutunk.

                /*vonal.X1 = (x_ek[i] - 16) * xmeret;
                vonal.Y1 = (48.7 - y_ok[i]) * ymeret; // ezek csinálták meg a pontokkal kijelölt M.o-t.
                vonal.X2 = vonal.X1 + 1;
                vonal.Y2 = vonal.Y1 + 1; */
                
                //vonal.Stroke = Brushes.Black;
                if (y_ok[i] > 47.5)
                {
                    vonal.Stroke = Brushes.Red;
                    vonal.Fill = Brushes.Red;
                }
                else if (y_ok[i] < 46.3)
                {
                    vonal.Stroke = Brushes.Green;
                    vonal.Fill = Brushes.Green;
                }
                else
                {
                    vonal.Stroke = Brushes.White;
                    vonal.Fill = Brushes.White;
                }

                vonal.StrokeThickness = 2;
                vonal.MouseUp += Vonal_MouseUp;
                Rajzlap.Children.Add(vonal);
            }
        }
    }
}

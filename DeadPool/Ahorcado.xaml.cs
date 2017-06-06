using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace DeadPool
{
    /// <summary>
    /// Lógica de interacción para Ahorcado.xaml
    /// </summary>
    public partial class Ahorcado : Window
    {

        String superhero;
        int fallos;
        int segundos;
        System.Windows.Threading.DispatcherTimer myDispatcherTimer;

        public Ahorcado()
        {
            InitializeComponent();
            textBox.Focus();
            segundos = 1;
            fallos = 0;
            randomSuperHero();
            rayasNombre();

            myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 100 Milliseconds 
            myDispatcherTimer.Tick += new EventHandler(Each_Tick);
            myDispatcherTimer.Start();

        }

        private void randomSuperHero()
        {
            XmlTextReader myXMLreader = new XmlTextReader("Superheroes.xml");
            int cantidadtotal = -1;
            int aleatorio = 0;
            while (myXMLreader.Read() && (cantidadtotal != 0))
            {
                if (myXMLreader.NodeType == XmlNodeType.Element)
                {
                    if (myXMLreader.Name == "Cantidad")
                    {
                        myXMLreader.Read();
                        int cantidad = myXMLreader.ReadContentAsInt();
                        cantidadtotal=1;
                        Random rnd = new Random();
                        aleatorio = rnd.Next(1, (cantidad+1));
                        //textBox.Text = "" + aleatorio;
                    }else if (cantidadtotal == aleatorio)
                    {
                        myXMLreader.Read();
                        superhero = myXMLreader.ReadContentAsString();
                        //textBox.Text = textBox.Text + "   " + cadena;
                        //textBox.Text = superhero;
                        cantidadtotal = -1;
                    }else if (aleatorio != 0)
                    {
                        myXMLreader.Read();
                        cantidadtotal++;
                    }
                    
                }
            }
        }

        private void rayasNombre()
        {
            String aux = "";
            foreach (char c in superhero)
            {
                if (c == ' ')
                {
                    aux += "\t";
                }else
                {
                    aux = aux +  "_ ";
                }
            }

            textBlock.Text = aux;
        }

        private void cambiarCaracteres(char car)
        {
            char[] arrNombre = superhero.ToCharArray();
            char[] arrLineas = textBlock.Text.ToCharArray();
            Boolean coincide = false;

            for (int i = 0; i< superhero.Length; i++) {
                if (arrNombre[i].ToString().Equals(car.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    if (arrLineas[i*2] == ' ')
                    {
                        arrLineas[(i * 2)-1] = arrNombre[i];
                    }
                    else
                    {
                        arrLineas[i * 2] = arrNombre[i];
                    }
                    coincide = true;
                }
            }

            if (!coincide)
            {
                fallos++;
                textBlockfail.Text += car.ToString().ToUpper()+" ";
                switch (fallos)
                {
                    case 1:
                        image.Source = new BitmapImage(new Uri("ahorcado_Cabeza.png", UriKind.Relative));
                        textBlock_Intentos.Text = "4";
                        break;
                    case 2:
                        image.Source = new BitmapImage(new Uri("ahorcado_Cuerpo.png", UriKind.Relative));
                        textBlock_Intentos.Text = "3";
                        break;
                    case 3:
                        image.Source = new BitmapImage(new Uri("ahorcado_BrazoDer.png", UriKind.Relative));
                        textBlock_Intentos.Text = "2";
                        break;
                    case 4:
                        image.Source = new BitmapImage(new Uri("ahorcado_Completo.png", UriKind.Relative));
                        textBlock_Intentos.Text = "1";
                        break;
                    case 5:
                        image.Source = new BitmapImage(new Uri("cal_deadpoolsf.png", UriKind.Relative));
                        textBlock_Intentos.Text = "GAME OVER";
                        textBox.IsEnabled = false;
                        myDispatcherTimer.Stop();
                        textBlock_Intentos.Foreground = Brushes.DarkRed;
                        break;
                }
            }

            textBlock.Text = new string (arrLineas);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length != 0)
            {
                char car = textBox.Text[0];
                cambiarCaracteres(car);
                textBox.Text = "";
                if (completo())
                {
                    calculapuntos();
                    image.Source = new BitmapImage(new Uri("deadpool_approbes.png", UriKind.Relative));
                    textBox.IsEnabled = false;
                }
            }


            textBox.Focus();

        }

        private void EnterClicked(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (textBox.Text.Length != 0)
                {
                    char car = textBox.Text[0];
                    cambiarCaracteres(car);
                    textBox.Text = "";
                    if (completo())
                    {
                        calculapuntos();
                        image.Source = new BitmapImage(new Uri("deadpool_approbes.png", UriKind.Relative));
                        textBox.IsEnabled = false;
                    }
                }
                e.Handled = true;
            }
        }

        private Boolean completo()
        {
            foreach (char c in textBlock.Text)
            {
                if (c == '_')
                {
                    return false;
                }
            }

            return true;
        }

        private void Each_Tick(object o, EventArgs sender)
        {
            label1.Content = "Segundos pasados: " + segundos++.ToString();
        }

        private int calculapuntos()
        {
            int puntos = 0;

            if (segundos <= 6)
            {
                puntos = 100;
            } else if ((6 < segundos) && (segundos <= 12))
            {
                puntos = 90;
            }
            else if ((12 < segundos) && (segundos <= 18))
            {
                puntos = 80;
            } else if ((18 < segundos) && (segundos <= 24))
            {
                puntos = 70;            }
            else if ((24 < segundos) && (segundos <= 30))
            {
                puntos = 60;
            }
            else if ((30 < segundos) && (segundos <= 36))
            {
                puntos = 50;
            } else if ((36 < segundos) && (segundos <= 42))
            {
                puntos = 40;
            } else if ((42 < segundos) && (segundos <= 48))
            {
                puntos = 30;
            }
            else if ((48 < segundos) && (segundos <= 54))
            {
                puntos = 20;
            }
            else if ((54 < segundos) && (segundos <= 60))
            {
                puntos = 10;
            }
            else if (60 < segundos)
            {
                puntos = 0;
            }

            myDispatcherTimer.Stop();
            label1.Content = "Has tardado "+segundos+" segundos y has ganado "+puntos+" puntos";
            return puntos;

        }       


    }
    
}

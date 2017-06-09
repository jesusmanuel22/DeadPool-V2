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
using System.Windows.Threading;
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
        Button   comer, dormir, jugar;
        DispatcherTimer t2,t3;
        ProgressBar pb;
        MediaPlayer instrumental;
        MediaPlayer cancion_juego;
        Boolean musica;

        public Ahorcado(Button b1, Button b2, Button b3, DispatcherTimer t1, ProgressBar pb, MediaPlayer instrumentales,Boolean sonido)
        {
            InitializeComponent();
            this.musica = sonido;
            t2 = new DispatcherTimer();
            t2.Interval = TimeSpan.FromSeconds(1.0);
            t2.Tick += new EventHandler(reloj);
            t2.Start();

            textBox.Focus();
            segundos = 1;
            fallos = 0;
            randomSuperHero();
            rayasNombre();
            this.pb = pb;
            this.instrumental = instrumentales;
            instrumental.Volume=0;
            cancion_juego = new MediaPlayer();
            cargarCancionAleatoriaJuego();
            if (musica) {
                cancion_juego.Volume = 0.05;

            }else if (!musica)
            {
                cancion_juego.Volume = 0;
            }
            
            cancion_juego.Play();
            //myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            //myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 100 Milliseconds 
            //myDispatcherTimer.Tick += new EventHandler(Each_Tick);
           // myDispatcherTimer.Start();
            comer = b1;
            dormir = b2;
            jugar = b3;
            this.t3 = t1;

        }
       int seg = 0;
        int min = 0;
        int hora = 0;  
        private void reloj(object sender, EventArgs e)
        {

            seg++;

            if (seg == 60)
            {
                min++;
                seg = 0;
            }
            else if (min == 60)
            {
                hora++;
                min = 0;
            }

            lbl_SegundosPasados.Content = "Tiempo transcurrido: " + min.ToString().PadLeft(2, '0') +
                ":" + seg.ToString().PadLeft(2, '0');


        }
        private void cargarCancionAleatoriaJuego()
        {
            String[] canciones = {
                "instrumental.wav",
            "introMusic.wav"};
            Random num = new Random();
            int numero = num.Next(0, canciones.Length);
            //img_introDeadPool.Source = new BitmapImage(new Uri(imagenes[numero], UriKind.Relative));
            cancion_juego.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\"+canciones[numero]));

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
                        textBlock_Intentos.Foreground = Brushes.DarkRed;
                        t2.Stop();
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

        private void btn_informacion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("¡¡Bienvenido al minijuego!!\n En este minijuego te encontrarás una"+
                "versión mejorada del ahorcado, ya que tendrás que evitar que yo, DeadPool,"+
                " me pegue un tiro porque no te sepas los superhéroes...\n Introduce una sola letra,"+
                " si pones más sólo cogerá la primera. Además, no repitas letras, ¡noob!.\n "+
                "Hazlo en poco tiempo y me aburriré menos.\n Ahora a conseguir mi aprobado, "+
                "¡¡Good Luck & Chimichangas!! ", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            comer.IsHitTestVisible = true;
            dormir.IsHitTestVisible = true;
            jugar.IsHitTestVisible = true;
            if (musica)
            {
                instrumental.Volume = 0.1;

            }
            else if (!musica)
            {
                instrumental.Volume = 0; 
            }
            
            cancion_juego.Stop();
            t3.Start();
        }

        /*private void Each_Tick(object o, EventArgs sender)
        {
            //lbl_SegundosPasados.Content = "Segundos pasados: " + segundos++.ToString("mm:ss");
            lbl_SegundosPasados.Content = "Tiempo total" + hora.ToString().PadLeft(2, '0') + ":"
                + min.ToString().PadLeft(2, '0') +
                ":" + seg.ToString().PadLeft(2, '0');
        }
        */
        private int calculapuntos()
        {
            int puntos = 0;

            if (min==0 && segundos <= 6)
            {
                puntos = 100;
            } else if ((min == 0 && 6 < segundos) && (min == 0 && segundos <= 12))
            {
                puntos = 90;
            }
            else if ((min == 0 &&12 < segundos) && (min == 0 && segundos <= 18))
            {
                puntos = 80;
            } else if ((min == 0 && 18 < segundos) && (min == 0 && segundos <= 24))
            {
                puntos = 70;            }
            else if ((min == 0 && 24 < segundos) && (min == 0 && segundos <= 30))
            {
                puntos = 60;
            }
            else if ((min == 0 && 30 < segundos) && (min == 0 && segundos <= 36))
            {
                puntos = 50;
            } else if ((min == 0 && 36 < segundos) && (min == 0 && segundos <= 42))
            {
                puntos = 40;
            } else if ((min == 0 && 42 < segundos) && (min == 0 && segundos <= 48))
            {
                puntos = 30;
            }
            else if ((min == 0 && 48 < segundos) && (min == 0 && segundos <= 54))
            {
                puntos = 20;
            }
            else if ((min == 0 && 54 < segundos) && (min == 0 && segundos <= 60))
            {
                puntos = 10;
            }
            else if (1 < min)
            {
                puntos = 0;
            }
            t2.Stop();
            //myDispatcherTimer.Stop();
            /*myDispatcherTimer.Stop();*/
            lbl_SegundosPasados.Content = "Has tardado "+min+ " con  " + seg +" segundos y has ganado "+puntos+" puntos";
            pb.Value+=puntos;
            
            return puntos;

        }


    }
  

}

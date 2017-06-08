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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Media;
using System.Xml;
using System.Threading;
using System.IO;

namespace DeadPool
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer t1;
        Ahorcado ahorcado;
        private MediaPlayer sonido;
        private MediaPlayer efectos;
        private MediaPlayer disparos;
        private MediaPlayer katanas;
        private MediaPlayer instrumentales;
        public void IntroducirNombre() {
            
        }
        public MainWindow()
        {
           
            InitializeComponent();
            MessageBox.Show("AVISO, esta aplicación contiene música, para proteger su integridad auditiva le rogamos que baje el volumen y suba progresivamente hasta donde usted vea conveniente\n Muchas gracias\n ¡¡CHIMICHANGAS!!", "ALERT!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            cargarImagenAleatoriaInicio();
            inic_App();
            cv_AboutUs.Visibility = Visibility.Collapsed;
            instrumentales = new MediaPlayer();
        }
        private void inic_App() {
            sonido = new MediaPlayer();
            sonido.Volume = 0.01;
            efectos = new MediaPlayer();
            sonido.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\introMusic.wav"));
            sonido.MediaEnded += new EventHandler(Media_Ended);
            sonido.Play();
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            sonido.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\introMusic.wav"));
            sonido.Play();
        }

        private void reloj(object sender, EventArgs e)
        {
            pgb_Diversion.Value -= 5.0;
            pgb_Energia.Value -= 1.0;
            pgb_Hambre.Value -= 5.0;
            
            Storyboard hambriento;
            hambriento = (Storyboard)this.Resources["pgb_Hambre"];
            if (pgb_Hambre.Value < 50 && pgb_Hambre.Value >= 10)
            {
                Pizza_icon50_png.Visibility = Visibility.Visible;
                Pizza_icon10_png.Visibility = Visibility.Hidden;
                Pizza_icon_png.Visibility = Visibility.Hidden;
            }
            else if (pgb_Hambre.Value < 10 && pgb_Hambre.Value >= 0)
            {
                Pizza_icon50_png.Visibility = Visibility.Hidden;
                Pizza_icon10_png.Visibility = Visibility.Visible;
                Pizza_icon_png.Visibility = Visibility.Hidden;
            }
            else
            {
                Pizza_icon50_png.Visibility = Visibility.Hidden;
                Pizza_icon10_png.Visibility = Visibility.Hidden;
                Pizza_icon_png.Visibility = Visibility.Visible;
            }
            Storyboard diversion;
            diversion = (Storyboard)this.Resources["pgb_Diversion"];
            if (pgb_Diversion.Value < 50 && pgb_Diversion.Value >= 10)
            {
                balon_10_png.Visibility = Visibility.Hidden;
                balon_50_png.Visibility = Visibility.Visible;
                balon_png.Visibility = Visibility.Hidden;
            }
            else if (pgb_Diversion.Value < 10 && pgb_Diversion.Value >= 0)
            {
                balon_10_png.Visibility = Visibility.Visible;
                balon_50_png.Visibility = Visibility.Hidden;
                balon_png.Visibility = Visibility.Hidden;
            }
            else
            {
                balon_10_png.Visibility = Visibility.Hidden;
                balon_50_png.Visibility = Visibility.Hidden;
                balon_png.Visibility = Visibility.Visible;
            }
            Storyboard cansado;
            cansado = (Storyboard)this.Resources["pgb_Cansado"];
            if (pgb_Energia.Value < 50 && pgb_Energia.Value >= 10)
            {
                dormir10_png.Visibility = Visibility.Hidden;
                dormir50_png.Visibility = Visibility.Visible;
                dormir_png.Visibility = Visibility.Hidden;
            }
            else if (pgb_Energia.Value < 10 && pgb_Energia.Value >= 0)
            {
                dormir10_png.Visibility = Visibility.Hidden;
                dormir50_png.Visibility = Visibility.Hidden;
                dormir_png.Visibility = Visibility.Visible;
            }
            else
            {
                dormir10_png.Visibility = Visibility.Visible;
                dormir50_png.Visibility = Visibility.Hidden;
                dormir_png.Visibility = Visibility.Hidden;            }




        }

        private void btnComer_Click(object sender, RoutedEventArgs e)
        {
            if (pgb_Hambre.Value <= 50) {
                pgb_Hambre.Value += 100.0;
                
                Accion_comer();
            }

           
        }

        private void btnJugar_Click(object sender, RoutedEventArgs e)
        {
            btnComer.IsHitTestVisible = false;
            btnJugar.IsHitTestVisible = false;
            btnDormir.IsHitTestVisible = false;
            t1.Stop();
            ahorcado = new Ahorcado(this.btnComer,this.btnDormir,this.btnJugar,this.t1, this.pgb_Diversion);
            ahorcado.Show();
            

        }

        private void btnDormir_Click(object sender, RoutedEventArgs e)
        {
            pgb_Energia.Value += 35.0;
            Accion_Dormir();
        }

        

        

        private void el_cabeza_MouseUp(object sender, MouseButtonEventArgs e)
        {
      
                Mascara_png.Visibility = Visibility.Visible;
        }
        

      

        private void Mascara_png_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mascara_png.Visibility = Visibility.Hidden;
        }

        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            btnComer.IsHitTestVisible = false;
            btnJugar.IsHitTestVisible = false;
            btnDormir.IsHitTestVisible = false;
            Storyboard espada;
            espada = (Storyboard)this.Resources["Espada"];
            
            espada.Completed += Animacion_Completed;
            
            espada.Begin();

        }
        private void Accion_comer()
        {
            btnComer.IsHitTestVisible = false;
            btnJugar.IsHitTestVisible = false;
            btnDormir.IsHitTestVisible = false;
            Storyboard comiendo;
            comiendo = (Storyboard)this.Resources["Comiendo"];
            comiendo.Completed += Animacion_Completed;
            comiendo.Begin();
            efectos.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\eat.wav"));
            efectos.Volume = 0.5;
            efectos.Play();
        }
        

        private void Accion_Dormir()
        {
            btnComer.IsHitTestVisible = false;
            btnJugar.IsHitTestVisible = false;
            btnDormir.IsHitTestVisible = false;
            Storyboard durmiendo;
            durmiendo = (Storyboard)this.Resources["Dormir"];
            durmiendo.Completed += Animacion_Completed;
            efectos.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\snore.wav"));
            efectos.Volume = 0.5;
            efectos.Play();
            durmiendo.Begin();
           

        }
        private void cargarNuevaProgressBar() {
            pgb_Diversion.Value = 100;
            pgb_Hambre.Value = 100;
            pgb_Energia.Value = 100;
        }
        private void cargarProgressBar()
        {
            XmlTextReader myXMLreader = new XmlTextReader("Persistencia.xml");
            while (myXMLreader.Read())
            {
                if (myXMLreader.NodeType == XmlNodeType.Element)
                {
                    if (myXMLreader.Name == "Diversion")
                    {
                        myXMLreader.Read();
                        pgb_Diversion.Value = myXMLreader.ReadContentAsDouble();
                    }
                    if (myXMLreader.Name == "Comida")
                    {
                        myXMLreader.Read();
                        pgb_Hambre.Value = myXMLreader.ReadContentAsDouble();
                    }
                    if (myXMLreader.Name == "Energia")
                    {
                        myXMLreader.Read();
                        pgb_Energia.Value = myXMLreader.ReadContentAsDouble();
                    }
                }
            }
            myXMLreader.Close();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");
            using (XmlWriter writer = XmlWriter.Create("Persistencia.xml", settings))
            {
                writer.WriteStartElement("Atributos");
                writer.WriteElementString("Comida", pgb_Hambre.Value + "");
                writer.WriteElementString("Energia", pgb_Energia.Value + "");
                writer.WriteElementString("Diversion", pgb_Diversion.Value + "");
                writer.WriteEndElement();
                writer.Flush();
                // writer.Close();
            }

            try
            {
                ahorcado.Close();
            }
            catch (Exception exc)
            {

            }
           
        }
        private void Animacion_Completed(object sender, EventArgs e)
        {
            btnComer.IsHitTestVisible = true;
            btnJugar.IsHitTestVisible = true;
            btnDormir.IsHitTestVisible = true;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Storyboard disparo;
            disparo = (Storyboard)this.Resources["DisparoDer"];
            
            disparo.Completed += DisparoDer_Completed;
            disparos = new MediaPlayer();
            disparos.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\disparo.wav"));
            disparos.Volume = 0.01;
            disparos.Play();
            sonido.Stop();
            disparo.Begin();
            
        }
        private void DisparoDer_Completed(object sender, EventArgs e)
        {
            Canvas_Inicio.Visibility = Visibility.Collapsed;
            cargarProgressBar();


            instrumentales.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\instrumental.wav"));
            instrumentales.MediaEnded += new EventHandler(Media_Ended2);
            instrumentales.Play();
        

        
            t1 = new DispatcherTimer();
            t1.Interval = TimeSpan.FromSeconds(3.0);
            t1.Tick += new EventHandler(reloj);


            t1.Start();
            Pizza_icon50_png.Visibility = Visibility.Hidden;
            Pizza_icon10_png.Visibility = Visibility.Hidden;
            Pizza_icon_png.Visibility = Visibility.Visible;
            balon_10_png.Visibility = Visibility.Hidden;
            balon_50_png.Visibility = Visibility.Hidden;
            balon_png.Visibility = Visibility.Visible;
            dormir10_png.Visibility = Visibility.Hidden;
            dormir50_png.Visibility = Visibility.Hidden;
            dormir_png.Visibility = Visibility.Hidden;
        }
    private void Media_Ended2(object sender, EventArgs e)
    {
        instrumentales.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\instrumental.wav"));
        instrumentales.Play();
    }
    private void disparo_NuevaPartida(object sender, RoutedEventArgs e)
        {
            instrumentales = new MediaPlayer();
            instrumentales.Volume = 0.1;


            instrumentales.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\instrumental.wav"));
            instrumentales.MediaEnded += new EventHandler(Media_Ended2);
            instrumentales.Play();
            Storyboard disparo = (Storyboard)this.Resources["DisparoIzq"];

            disparo.Completed += DisparoIzq_Completed;
            disparos = new MediaPlayer();
            disparos.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\disparo.wav"));
            disparos.Volume = 0.01;
            disparos.Play();
            sonido.Stop();
            disparo.Begin();
            

        }
        private void iniciar_Nuevo() {
            Canvas_Inicio.Visibility = Visibility.Collapsed;
            cargarNuevaProgressBar();
            t1 = new DispatcherTimer();
            t1.Interval = TimeSpan.FromSeconds(3.0);
            t1.Tick += new EventHandler(reloj);


            t1.Start();
            Pizza_icon50_png.Visibility = Visibility.Hidden;
            Pizza_icon10_png.Visibility = Visibility.Hidden;
            Pizza_icon_png.Visibility = Visibility.Visible;
            balon_10_png.Visibility = Visibility.Hidden;
            balon_50_png.Visibility = Visibility.Hidden;
            balon_png.Visibility = Visibility.Visible;
            dormir10_png.Visibility = Visibility.Hidden;
            dormir50_png.Visibility = Visibility.Hidden;
            dormir_png.Visibility = Visibility.Hidden;
        }
        private void DisparoIzq_Completed(object sender, EventArgs e) {
            iniciar_Nuevo();
        }

        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            
            Storyboard katana;
            katana = (Storyboard)this.Resources["katana"];

            katana.Completed += katana_Completed;
            katanas = new MediaPlayer();
            katanas.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\katana.wav"));
            katanas.Volume = 0.03;
            katanas.Play();
            katana.Begin();

        }
        private void katana_Completed(object sender, EventArgs e) {
            cv_AboutUs.Visibility = Visibility.Visible;
            Canvas_Inicio.Visibility = Visibility.Collapsed;
        }

        private void AboutUsAccept_Click(object sender, RoutedEventArgs e)
        {
            cv_AboutUs.Visibility = Visibility.Collapsed;
            Canvas_Inicio.Visibility = Visibility.Visible;
            cargarImagenAleatoriaInicio();
        }
        private void cargarImagenAleatoriaInicio()
        {
            String[] imagenes = {
                "ahorcado_Cabeza.png",
                "ahorcado_Completo.png",
                "ahorcado_Cuerpo.png",};
            Random num = new Random();
            int numero = num.Next(0, imagenes.Length);
            img_introDeadPool.Source = new BitmapImage(new Uri(imagenes[numero], UriKind.Relative));

        }

        private void btn_Sonido_Click(object sender, RoutedEventArgs e)
        {
            if (sonido.Volume != 0 || instrumentales.Volume !=0)
            {
                sonido.Volume = 0;
                instrumentales.Volume = 0;
            }
            else {
                sonido.Volume = 0.01;
                instrumentales.Volume = 0.01;

            }
        }
    }
}
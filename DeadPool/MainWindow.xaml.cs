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
using System.Speech.Synthesis;

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
        private MediaPlayer gameOverSound;
        Boolean musica = true;
        Boolean gameOvervar = false;
        int numaburrimiento = 0;
        int numhambre = 0;
        int numCansado = 0;
        private SpeechSynthesizer synthesizer;
        private String nombre;
        private Thread t;

        public void IntroducirNombre() {
            
        }

        public MainWindow()
        {
            InitializeComponent();

            synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Volume = 100;
            synthesizer.Rate = 1;


            MessageBox.Show("AVISO, esta aplicación contiene música, para proteger su integridad auditiva le rogamos que baje el volumen y suba progresivamente hasta donde usted vea conveniente\n Muchas gracias\n ¡¡CHIMICHANGAS!!", "ALERT!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            cargarImagenAleatoriaInicio();
            inic_App();
            btn_Pausa.Visibility = Visibility.Hidden;
            cv_AboutUs.Visibility = Visibility.Collapsed;
            cv_fondopausa.Visibility = Visibility.Collapsed;
            cv_brpausa.Visibility = Visibility.Collapsed;
            br_pausa.Visibility = Visibility.Collapsed;
            instrumentales = new MediaPlayer();
            instrumentales.Volume = 0.3;
            btn_musicaoff.Content = "Desactivar música de fondo";
        }

        private void ejecutarVoz(string frase)
        {
            synthesizer.Speak(frase);
        }

        private void inic_App() {
            sonido = new MediaPlayer();
            sonido.Volume = 0.1;
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
            if ((pgb_Hambre.Value < 10 && pgb_Diversion.Value < 10 || pgb_Hambre.Value < 10 && pgb_Energia.Value < 10
                || pgb_Diversion.Value < 10 && pgb_Energia.Value < 10) && !gameOvervar) {

                game_Over();
            }
            if ((pgb_Hambre.Value ==0  || pgb_Hambre.Value ==0 || pgb_Diversion.Value ==0) && !gameOvervar)
            {

                game_Over();

            }
            if ((pgb_Hambre.Value < 50 && pgb_Hambre.Value >= 20) && !gameOvervar)
            {
                Pizza_icon50_png.Visibility = Visibility.Visible;
                Pizza_icon10_png.Visibility = Visibility.Hidden;
                Pizza_icon_png.Visibility = Visibility.Hidden;
                if (numhambre == 0)
                {
                    t = new Thread(() => ejecutarVoz("Tengo hambre"));
                    t.Start();
                    Accion_hambre();
                    numhambre++;
                }
                
            }
            else if ((pgb_Hambre.Value < 20 && pgb_Hambre.Value >= 0)&&!gameOvervar)
            {
                
                Pizza_icon50_png.Visibility = Visibility.Hidden;
                Pizza_icon10_png.Visibility = Visibility.Visible;
                Pizza_icon_png.Visibility = Visibility.Hidden;
                if (numhambre == 0)
                {
                    t = new Thread(() => ejecutarVoz("¿En serio tengo que repetirte que tengo hambre? CHIMICHANGAS EVRYWHERE"));
                    t.Start();
                    Accion_hambre();
                    numhambre++;
                }
            }
            else if(!gameOvervar)
            {
                numhambre = 0;
                Pizza_icon50_png.Visibility = Visibility.Hidden;
                Pizza_icon10_png.Visibility = Visibility.Hidden;
                Pizza_icon_png.Visibility = Visibility.Visible;
            }
            Storyboard diversion;
            diversion = (Storyboard)this.Resources["pgb_Diversion"];
            
            if ((pgb_Diversion.Value < 50 && pgb_Diversion.Value >= 20)&& !gameOvervar)
            {
                numaburrimiento = 0;
                balon_10_png.Visibility = Visibility.Hidden;
                balon_50_png.Visibility = Visibility.Visible;
                balon_png.Visibility = Visibility.Hidden;
            }
            else if ((pgb_Diversion.Value < 20 && pgb_Diversion.Value >= 0) && !gameOvervar)
            {
                
                balon_10_png.Visibility = Visibility.Visible;
                balon_50_png.Visibility = Visibility.Hidden;
                balon_png.Visibility = Visibility.Hidden;
                if (numaburrimiento == 0) {
                    t = new Thread(() => ejecutarVoz("Me estoy empezando a aburrir..."));
                    t.Start();
                    Accion_aburrido();
                    numaburrimiento++;
                }
            }
            else if(!gameOvervar)
            {
                numaburrimiento = 0;
                balon_10_png.Visibility = Visibility.Hidden;
                balon_50_png.Visibility = Visibility.Hidden;
                balon_png.Visibility = Visibility.Visible;
            }
            Storyboard cansado;
            cansado = (Storyboard)this.Resources["pgb_Cansado"];
            numCansado = 0;
            if ((pgb_Energia.Value < 50 && pgb_Energia.Value >= 20) && !gameOvervar)
            {
                numCansado = 0;
                dormir10_png.Visibility = Visibility.Hidden;
                dormir50_png.Visibility = Visibility.Visible;
                dormir_png.Visibility = Visibility.Hidden;
            }
            else if ((pgb_Energia.Value < 20 && pgb_Energia.Value >= 0) && !gameOvervar)
            {
                dormir10_png.Visibility = Visibility.Hidden;
                dormir50_png.Visibility = Visibility.Hidden;
                dormir_png.Visibility = Visibility.Visible;
                if (numCansado == 0) {
                    AccionCansado();
                    numCansado++;
                }
            }
            else if (!gameOvervar)
            {
                numCansado = 0;
            
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
            ahorcado = new Ahorcado(this.btnComer,this.btnDormir,this.btnJugar,this.t1, this.pgb_Diversion,this.instrumentales,musica);
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
            t1.Stop();
            comiendo.Completed += Animacion_Completed;
            comiendo.Begin();
            efectos.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\eat.wav"));
            efectos.Volume = 0.5;
            efectos.Play();
        }

        private void Accion_hambre()
        {
            btnComer.IsHitTestVisible = false;
            btnJugar.IsHitTestVisible = false;
            btnDormir.IsHitTestVisible = false;
            String[] mvto = {
                "Hambre_Barriga",
                "Hambre_Barra",};
            t1.Stop();
            Random num = new Random();
            int numero = num.Next(0, mvto.Length);
            //img_introDeadPool.Source = new BitmapImage(new Uri(imagenes[numero], UriKind.Relative));
            Storyboard hambriento;
            hambriento = (Storyboard)this.Resources[mvto[numero]];
            hambriento.Completed += Animacion_Completed;
            hambriento.Begin();
        }

        private void Accion_aburrido()
        {
            btnComer.IsHitTestVisible = false;
            btnJugar.IsHitTestVisible = false;
            btnDormir.IsHitTestVisible = false;
            t1.Stop();
            String[] mvto = {
                "Aburrido_Pancarta",
                "Aburrido_Barra",};
            Random num = new Random();
            int numero = num.Next(0, mvto.Length);
            Storyboard hambriento;
            hambriento = (Storyboard)this.Resources[mvto[numero]];
            hambriento.Completed += Animacion_Completed;
            hambriento.Begin();
        }

        private void Accion_Dormir()
        {
            btnComer.IsHitTestVisible = false;
            btnJugar.IsHitTestVisible = false;
            btnDormir.IsHitTestVisible = false;
            Storyboard durmiendo;
            durmiendo = (Storyboard)this.Resources["Dormir"];
            t1.Stop();
            durmiendo.Completed += Animacion_Completed;
            efectos.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\snore.wav"));
            efectos.Volume = 0.2;
            efectos.Play();
            durmiendo.Begin();
           

        }

        private void AccionCansado()
        {
            btnComer.IsHitTestVisible = false;
            btnJugar.IsHitTestVisible = false;
            btnDormir.IsHitTestVisible = false;
            t1.Stop();
            
            Storyboard sueno;
            sueno = (Storyboard)this.Resources["Sueno_Cara"];
            sueno.Completed += Animacion_Completed;
            sueno.Begin();
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

            t = new Thread(() => ejecutarVoz("ADIOOOOOOOS"));
            t.Start();
            t.Abort();
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

            t1.Start();
            btnComer.IsHitTestVisible = true;
            btnJugar.IsHitTestVisible = true;
            btnDormir.IsHitTestVisible = true;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (Comprueba_Nick())
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
             t = new Thread(() => ejecutarVoz("Hola "+ nombre + ", espero que disfrutes de este juego y no te veas seducido por mi voz... "));
            t.Start();
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

            if (Comprueba_Nick())
            {
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

            
            

        }

        private void iniciar_Nuevo() {
            Canvas_Inicio.Visibility = Visibility.Collapsed;
            cargarNuevaProgressBar();
            t1 = new DispatcherTimer();
            t1.Interval = TimeSpan.FromSeconds(3.0);
            t1.Tick += new EventHandler(reloj);
            
            t1.Start();
            t = new Thread(() => ejecutarVoz("Hola " + nombre + ", espero que disfrutes de este juego y no te veas seducido por mi voz..."));
            t.Start();
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
                "1.jpg",
                "2.jpg",
                "3.jpg",
            "4.jpg",
            "5.jpg",
            "6.jpg",
            "7.jpg",
            "8.jpg",
            "9.jpg",
            "10.png",};
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
                // btn_Sonido.Background = ImageBrush.(@"/../../Resources/mute.png");
                ImageBrush brush1 = new ImageBrush();
                BitmapImage image = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\sonido.png"));
                brush1.ImageSource = image;
                btn_Sonido.Background = brush1;
                musica = false;
                btn_musicaoff.Content = "Activar música de fondo";
            }
            else {
                sonido.Volume = 0.1;
                instrumentales.Volume = 0.3;
                ImageBrush brush1 = new ImageBrush();
                BitmapImage image = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\mute.png"));
                brush1.ImageSource = image;
                btn_Sonido.Background = brush1;
                musica = true;
                btn_musicaoff.Content = "Desactivar música de fondo";
            }
        }

        private void abrir_menuPausa(object sender, RoutedEventArgs e)
        {
            cv_fondopausa.Visibility = Visibility.Visible;
            cv_brpausa.Visibility = Visibility.Visible;
            br_pausa.Visibility = Visibility.Visible;
            if (gameOvervar == true)
            {
                cv_Muerte.Visibility = Visibility.Visible;
                btn_Pausa.Visibility = Visibility.Hidden;
            }
            else
            {
                cv_Muerte.Visibility = Visibility.Collapsed;

            }
        }

        private void Cancelar_Option(object sender, RoutedEventArgs e)
        {
            cv_fondopausa.Visibility = Visibility.Collapsed;
            cv_brpausa.Visibility = Visibility.Collapsed;
            br_pausa.Visibility = Visibility.Collapsed;

        }

        private void Volver_Inicio(object sender, RoutedEventArgs e)
        {
            t1.Stop();
            sonido.Play();
            cv_AboutUs.Visibility = Visibility.Collapsed;
            cv_fondopausa.Visibility = Visibility.Collapsed;
            cv_brpausa.Visibility = Visibility.Collapsed;
            br_pausa.Visibility = Visibility.Collapsed;
            Canvas_Inicio.Visibility = Visibility.Visible;
            cargarImagenAleatoriaInicio();
            instrumentales.Stop();
            if (gameOvervar == true)
            {
                cv_Muerte.Visibility = Visibility.Collapsed;
            }
        
            
        }

        private Boolean Comprueba_Nick()
        {
            if (txtNick.Text == "")
            {
                MessageBox.Show("No puedes dejar el Nick vacio, por favor introduce tu nombre", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);

                return false;
            }
            else
            {
                nombre = txtNick.Text;
                return true;
            }
                
        }

        public void game_Over()
        {
            gameOvervar = true;
            t1.Stop();
            instrumentales.Stop();
            cv_Muerte.Visibility = Visibility.Visible;
            Storyboard muerte;
            muerte = (Storyboard)this.Resources["GameOver"];
           // muerte.Completed += katana_Completed;
            gameOverSound = new MediaPlayer();
            gameOverSound.Open(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\gameOver.wav"));
            gameOverSound.Volume = 0.1;
            gameOverSound.Play();
            muerte.Begin();
            btn_Pausa.Visibility = Visibility.Visible;
        }
    }
     
}
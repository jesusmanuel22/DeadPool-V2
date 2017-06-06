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

namespace DeadPool
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer t1;
        public MainWindow()
        {
            InitializeComponent();

            cargarProgressBar();

            t1 = new DispatcherTimer();
            t1.Interval = TimeSpan.FromSeconds(3.0);
            t1.Tick += new EventHandler(reloj);
            
            
            t1.Start();
            Pizza_icon50_png.Visibility = Visibility.Hidden;
            Pizza_icon10_png.Visibility = Visibility.Hidden;
            Pizza_icon_png.Visibility = Visibility.Hidden;
            balon_10_png.Visibility = Visibility.Hidden;
            balon_50_png.Visibility = Visibility.Hidden;
            balon_png.Visibility = Visibility.Hidden;
            dormir10_png.Visibility = Visibility.Hidden;
            dormir50_png.Visibility = Visibility.Hidden;
            dormir_png.Visibility = Visibility.Hidden;

            
        }

        private void reloj(object sender, EventArgs e)
        {
            pgb_Diversion.Value -= 5.0;
            pgb_Energia.Value -= 5.0;
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
            pgb_Hambre.Value += 20.0;
            Accion_comer();
        }

        private void btnJugar_Click(object sender, RoutedEventArgs e)
        {
            pgb_Diversion.Value += 20.0;
            Ahorcado ahorcado = new Ahorcado();
            ahorcado.Show();
        }

        private void btnDormir_Click(object sender, RoutedEventArgs e)
        {
            pgb_Energia.Value += 20.0;
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
            Storyboard espada;
            espada = (Storyboard)this.Resources["Espada"];
            Storyboard enfadado;
            enfadado = (Storyboard)this.Resources["EspadaEnfado"];
           //espada.Begin();
            enfadado.Begin();
        }
        SoundPlayer Comer;
        private void Accion_comer()
        {
            
            Storyboard comiendo;
            comiendo = (Storyboard)this.Resources["Comiendo"];
            Comer = new SoundPlayer(@"..\..\eat.wav");
            //espada.Begin();
            comiendo.Begin();
            Comer.Play();
            /*try
            {
                Comer = new SoundPlayer(@"C:\Users\Jesus\Documents\DeadPool\DeadPool\eat.wav");
            }
            catch (Exception e) {
                MessageBox.Show("Error: " + e);
            }*/
        }
        SoundPlayer Dormir;
        private void Accion_Dormir()
        {

            Storyboard durmiendo;
            durmiendo = (Storyboard)this.Resources["Dormir"];
            Dormir = new SoundPlayer(@"..\..\snore.wav");
            //espada.Begin();
            durmiendo.Begin();
            Dormir.Play();
            /*try
            {
                Comer = new SoundPlayer(@"C:\Users\Jesus\Documents\DeadPool\DeadPool\eat.wav");
            }
            catch (Exception e) {
                MessageBox.Show("Error: " + e);
            }*/
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
        }

    }
}

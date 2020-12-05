using Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Drawing;
using Brushes = System.Windows.Media.Brushes;

namespace CCR_songs
{
    /// <summary>
    /// Interaction logic for DodajPesmu.xaml
    /// </summary>
    public partial class DodajPesmu : Window
    {

        //public static BindingList<int> font_sizes = new BindingList<int>() 
        
        public static ObservableCollection<int> font_sizes = new ObservableCollection<int>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        public static ObservableCollection<int> Font_sizes { get; set; }


        public DodajPesmu()
        {
            InitializeComponent();
            comboColors.ItemsSource = typeof(Colors).GetProperties();
            comboSize.ItemsSource = font_sizes;

            /*
            if (image.Source == null) 
            {
                textBoxNaziv.Text = "aaaaaaaaa";
            }
            // za proveru
            */

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void xButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void minButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void buttonSlika_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            string appPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) ;

            //textBoxNaziv.Text = System.IO.Path.Combine(appPath  + "\\..\\..") ;

            dlg.InitialDirectory = appPath ; // dodaj da bude dva direktorijuma unazad
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Pictures (.jpg)|*.jpg|(.png)|*.png";

            bool result = (bool)dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                textBoxNaziv.Text = filename;
                
                BitmapImage bimage = new BitmapImage();
                bimage.BeginInit();
                bimage.UriSource = new Uri(filename, UriKind.RelativeOrAbsolute);
                bimage.EndInit();
                image.Source = bimage;
            }
        }

        private bool validate() 
        {
            bool result = true;

            int garbage;
            DateTime garb;
            RichTextBox rtb = richTextBox;
            string rtb_text = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text.Trim();

            textBoxNaziv.BorderThickness = new Thickness(1);
            textBoxPregledi.BorderThickness = new Thickness(1);
            dateDatumObjave.BorderThickness = new Thickness(1);
            imageBorder.BorderThickness = new Thickness(1);
            richTextBox.BorderThickness = new Thickness(1);

            if (textBoxNaziv.Text.Trim() == "") 
            {
                labelNazivGreska.Content = "Morate uneti naziv!";
                textBoxNaziv.BorderBrush = Brushes.Red;
                result = false;
            }
            else 
            {
                labelNazivGreska.Content = "";
                textBoxNaziv.BorderBrush = Brushes.Green;
            }

            if (textBoxPregledi.Text.Trim() == "" || !(Int32.TryParse(textBoxPregledi.Text.Trim(), out garbage))) 
            {
                labelPreglediGreska.Content = "Morate uneti broj pregleda!";
                textBoxPregledi.BorderBrush = Brushes.Red;
                result = false;

            }
            else
            {
                labelPreglediGreska.Content = "";
                textBoxPregledi.BorderBrush = Brushes.Green;
            }

            if (!DateTime.TryParse(dateDatumObjave.Text, out garb)) 
            {
                labelDatumGreska.Content = "Morate uneti validan datum!";
                dateDatumObjave.BorderBrush = Brushes.Red;
                result = false;

            }
            else
            {
                labelDatumGreska.Content = "";
                dateDatumObjave.BorderBrush = Brushes.Green;
            }

            if (image.Source == null) 
            {
                labelSlikaGreska.Content = "Morate izabrati sliku!";
                imageBorder.BorderBrush = Brushes.Red; 
                result = false;
            
            }
            else 
            {
                labelSlikaGreska.Content = "";

            }

            if (rtb_text == "" || rtb_text == "Unesite tekst pesme") 
            {
                labelTekstGreska.Content = "Morate uneti tekst!";
                richTextBox.BorderBrush = Brushes.Red;
                result = false;

            }
            else 
            {
                labelTekstGreska.Content = "";
            }

            return result;
        }

        private void buttonDodajte_Click(object sender, RoutedEventArgs e)
        {
            if (validate()) 
            {
                Song nova = new Song(Int32.Parse(textBoxPregledi.Text.Trim()), textBoxNaziv.Text.Trim(), DateTime.Parse(dateDatumObjave.Text.Trim()), (BitmapImage)image.Source, textBoxNaziv.Text.Trim() + ".rtf");
                MainWindow.Pesme.Add(nova);
            }
        }

        private void richTextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            

            textBoxNaziv.Text = "asadasdsdad";
            /*
            richTextBox.Document.Blocks.Clear();

            richTextBox.Document.Blocks.Add(new Paragraph(new Run("")));
            */
            //var rtb_text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            //rtb_text.Text = "";
        }

    }
}

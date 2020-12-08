using Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using Brushes = System.Windows.Media.Brushes;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Controls;

namespace CCR_songs
{
    /// <summary>
    /// Interaction logic for DodajPesmu.xaml
    /// </summary>
    public partial class DodajPesmu : Window
    {

        
        public static List<int> font_sizes = new List<int>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        public static List<int> Font_sizes { get; set; }

        // for colors
        private static readonly int COLUMS = 3;
        private readonly int song_indx = -1; // -1 je po defaultu ako je null song

        public System.Windows.Forms.DrawMode DrawMode { get; set; }

        public DodajPesmu(Classes.Song s)
        {
            InitializeComponent();

            if (s != null) 
            {
                textBoxNaziv.Text = s.Naziv_pesme;
                image.Source = s.Cover_image;
                textBoxPregledi.Text = s.Br_pregleda.ToString();
                dateDatumObjave.Text = s.Datum_objave.ToShortDateString();
                song_indx = MainWindow.Pesme.IndexOf(s);
                buttonDodajte.Content = "Izmenite pesmu";

                // rtf 
                string filename = textBoxNaziv.Text.Trim() + ".rtf";

                FileStream fs = new FileStream(filename, FileMode.Open);
                TextRange txt = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                txt.Load(fs, System.Windows.DataFormats.Rtf);

                fs.Close();
            }

            comboColors.ItemsSource = typeof(Colors).GetProperties();

            comboFontStyles.ItemsSource = Fonts.SystemFontFamilies;

            comboSize.ItemsSource = font_sizes;
            comboSize.SelectedItem = font_sizes[4]; // 12 font size
         
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
 
            string path_s = "";

            string[] temp = appPath.Split('\\');

            for ( int i = 0; i < temp.Length - 2; i++ )
            {
                path_s += temp[i] + '\\';
            }

            dlg.InitialDirectory = path_s ; 
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Pictures (.jpg)|*.jpg|(.png)|*.png";

            bool result = (bool)dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                
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
            System.Windows.Controls.RichTextBox rtb = richTextBox;
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
                imageBorder.BorderBrush = Brushes.Green;
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
                richTextBox.BorderBrush = Brushes.Green;
            }

            return result;
        }

        private void buttonDodajte_Click(object sender, RoutedEventArgs e)
        {
            if (validate()) 
            {
                Song nova = new Song(Int32.Parse(textBoxPregledi.Text.Trim()), textBoxNaziv.Text.Trim(), DateTime.Parse(dateDatumObjave.Text.Trim()), (BitmapImage)image.Source, textBoxNaziv.Text.Trim() + ".rtf");
                
                if (song_indx == -1) 
                {
                    MainWindow.Pesme.Add(nova);
                    labelDodato.Content = "Dodali ste pesmu!";

                    string filename = textBoxNaziv.Text.Trim() + ".rtf";

                    FileStream fs = new FileStream(filename, FileMode.OpenOrCreate);
                    TextRange txt = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    txt.Save(fs, System.Windows.DataFormats.Rtf);

                    fs.Close();

                    buttonIsprazni_Click(null, null);
                    labelDodato.Content = "Dodali ste pesmu!";

                }
                else 
                {
                    MainWindow.Pesme[song_indx] = nova;
                    labelDodato.Content = "Izmenili ste pesmu!";

                    string filename = textBoxNaziv.Text.Trim() + ".rtf";

                    FileStream fs = new FileStream(filename, FileMode.OpenOrCreate);
                    TextRange txt = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    txt.Save(fs, System.Windows.DataFormats.Rtf);

                    fs.Close();
                }
            }
        }


        private void richTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // bold
            object temp = richTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

            // italic
            object temp2 = richTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp2 != DependencyProperty.UnsetValue) && (temp2.Equals(FontStyles.Italic));

            // underline
            object temp3 = richTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp3 != DependencyProperty.UnsetValue) && (temp3.Equals(TextDecorations.Underline));

            // font size
            if (comboSize.SelectedItem != null)
            {
                richTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, comboSize.SelectedItem.ToString());
            }

            if (comboColors.SelectedItem != null) 
            {
                richTextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, comboColors.SelectedItem.ToString().Split(' ')[1]);
            }

        }

        private void richTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            string rtb_text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text.Trim();

            if (rtb_text == "Unesite tekst pesme")
                richTextBox.Document.Blocks.Clear();
        }


        private void table_Loaded(object sender, RoutedEventArgs e)
        {
            // for showing colors

            Grid grid = (Grid)sender;
            if (grid != null)
            {
                if (grid.RowDefinitions.Count == 0)
                {
                    for (int r = 0; r <= comboColors.Items.Count / COLUMS; r++)
                    {
                        grid.RowDefinitions.Add(new RowDefinition());
                    }
                }
                if (grid.ColumnDefinitions.Count == 0)
                {
                    for (int c = 0; c < Math.Min(comboColors.Items.Count, COLUMS); c++)
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition());
                    }
                }
                for (int i = 0; i < grid.Children.Count; i++)
                {
                    Grid.SetColumn(grid.Children[i], i % COLUMS);
                    Grid.SetRow(grid.Children[i], i / COLUMS);
                }
            }
        }

        private void comboSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboSize.SelectedItem != null && richTextBox.Selection.Text != "") 
            {
                richTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, comboSize.SelectedItem.ToString());
            }
        }

        private void comboColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboColors.SelectedItem != null && richTextBox.Selection.Text != "") 
            {
                richTextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, comboColors.SelectedItem.ToString().Split(' ')[1]);
            }
        }

        private void comboFontStyles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboFontStyles.SelectedItem != null)
            {
                richTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty,
               comboFontStyles.SelectedItem);
            }
        }

        private void richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string rtb_text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text.Trim();

            Regex r = new Regex("^[`~!@#$%^&*()_+-={}\"|[\\];:/.,<>?]*$"); // vecina znakova interpunkcije

            int count = 0;
            foreach  (var word in rtb_text.Split(' ') )
            {
                if (!r.IsMatch(word))
                    count++;
            }

            if (textBoxBrReci != null)
            {
                textBoxBrReci.Text = "Broj reci: " + count.ToString();
            }
        }

        private void buttonIsprazni_Click(object sender, RoutedEventArgs e)
        {
            textBoxNaziv.Text = "";
            textBoxPregledi.Text = "";
            dateDatumObjave.Text = "";
            image.Source = null;
            richTextBox.Document.Blocks.Clear();

            textBoxNaziv.ClearValue(System.Windows.Controls.TextBox.BorderBrushProperty);
            textBoxPregledi.ClearValue(System.Windows.Controls.TextBox.BorderBrushProperty);
            dateDatumObjave.ClearValue(DatePicker.BorderBrushProperty);

            var converter = new System.Windows.Media.BrushConverter();
            var brush = (System.Windows.Media.Brush)converter.ConvertFromString("#212121");
            imageBorder.BorderBrush = brush;

            richTextBox.ClearValue(System.Windows.Controls.RichTextBox.BorderBrushProperty);
            labelDodato.Content = "";
            labelNazivGreska.Content = "";
            labelPreglediGreska.Content = "";
            labelDatumGreska.Content = "";
            labelSlikaGreska.Content = "";
            labelTekstGreska.Content = "";
        }
    }
}

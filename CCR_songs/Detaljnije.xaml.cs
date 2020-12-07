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
using System.Windows.Shapes;

namespace CCR_songs
{
    /// <summary>
    /// Interaction logic for Detaljnije.xaml
    /// </summary>
    public partial class Detaljnije : Window
    {
        public Detaljnije(Classes.Song s)
        {
            
            InitializeComponent();

            labelNaslov.Content = s.Naziv_pesme;
            image.Source = s.Cover_image;
            labelPregledi.Content += " " + s.Br_pregleda;
            labelDatum.Content += " " + s.Datum_objave.ToShortDateString();

        }

        private void xButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void minButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}

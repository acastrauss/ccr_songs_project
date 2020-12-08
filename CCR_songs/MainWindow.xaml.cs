using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Classes;

namespace CCR_songs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static BindingList<Song> Pesme { get; set; }

        public MainWindow()
        {
            Pesme = new BindingList<Song>(); 
            
            DataContext = this; 

            InitializeComponent();
        }

        private void xButton_Click(object sender, RoutedEventArgs e)
        {
            // ovaj deo brise sve rtf fajlove koje je program generisao, opciono se moze izbaciti ali mi nije imalo logike da fajlovi ostanu na disku
            // kada aplikacija ne perzistira informacije nakon gasenja

            string appPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

            string[] files = System.IO.Directory.GetFiles(appPath, "*.rtf");

            foreach (string file in files)
            {
                System.IO.File.Delete(file);
            }
            //

            this.Close();    
        }

        private void minButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void dodajButton_Click(object sender, RoutedEventArgs e)
        {
            DodajPesmu dodajPesmu = new DodajPesmu(null);
            dodajPesmu.Show();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void buttonIzbrisi_Click(object sender, RoutedEventArgs e)
        {
            Pesme.Remove((Classes.Song)dataGridPesme.SelectedItem);
            dataGridPesme.Items.Refresh();
        }

        private void buttonDetaljnije_Click(object sender, RoutedEventArgs e)
        {
            Detaljnije win_det =  new Detaljnije(((Classes.Song)dataGridPesme.SelectedItem));
            win_det.Show();
        }

        private void buttonIzmeni_Click(object sender, RoutedEventArgs e)
        {

            DodajPesmu dodajPesmu = new DodajPesmu(((Classes.Song)dataGridPesme.SelectedItem));
            dodajPesmu.ShowDialog();

            dataGridPesme.Items.Refresh();
        }
    }
}

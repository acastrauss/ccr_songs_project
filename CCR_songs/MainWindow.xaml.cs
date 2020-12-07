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
        private DataIO serializer = new DataIO();
        public static BindingList<Song> Pesme { get; set; }

        public MainWindow()
        {

            Pesme = serializer.DeSerializeObject<BindingList<Song>>("pesme.xml");
            if (Pesme == null) //U slucaju da nista nije ucitano
            {
                Pesme = new BindingList<Song>(); //nova lista pa cemo u nju dodavati
            }
            DataContext = this; //okidac Data Bindinga

            InitializeComponent();
        }

        private void xButton_Click(object sender, RoutedEventArgs e)
        {
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
            dodajPesmu.Show();
        }
    }
}

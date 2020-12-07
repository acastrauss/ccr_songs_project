using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Classes
{
    [Serializable]
    public class Song
    {
        private int br_pregleda;
        private string naziv_pesme;
        private DateTime datum_objave;
        private DateTime samo_datum;
        private BitmapImage cover_image;
        private string rtf_file;

        public Song()
        {
            this.br_pregleda = 0;
            this.naziv_pesme = "";
            
        }

        public Song(int br_pregleda, string naziv_pesme, DateTime datum_objave, BitmapImage cover_image, string rtf_file)
        {
            this.br_pregleda = br_pregleda;
            this.naziv_pesme = naziv_pesme;
            this.datum_objave = datum_objave;
            this.cover_image = cover_image;
            this.rtf_file = rtf_file;
            this.samo_datum = datum_objave.Date;
        }

        public int Br_pregleda { get => br_pregleda; set => br_pregleda = value; }
        public string Naziv_pesme { get => naziv_pesme; set => naziv_pesme = value; }
        public DateTime Datum_objave { get => datum_objave; set => datum_objave = value; }
        public BitmapImage Cover_image { get => cover_image; set => cover_image = value; }
        public string Rtf_file { get => rtf_file; set => rtf_file = value; }
        public DateTime Samo_datum { get => samo_datum; set => samo_datum = value; }
    }
}

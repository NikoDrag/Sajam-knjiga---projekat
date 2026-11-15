using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Izdavac
    {
        private Autor _sef_izdavaca;
        //odma proveravamo da li je manji od 5 god i izbacujemo exception
        public string Sifra { get; set; }
        public string Naziv { get; set; }

        public Autor Sef_izdavaca
        { get; set; }
        public List<Autor> Spisak_autora { get; set; }
        public List<Knjiga> Spisak_knjiga { get; set; }

        public Izdavac()
        {
            Spisak_autora = new List<Autor>();
            Spisak_knjiga = new List<Knjiga>();
        }

        public Izdavac(string sifra, string naziv, Autor sef_izdavaca)
        {
            Sifra = sifra;
            Naziv = naziv;
            Sef_izdavaca = sef_izdavaca;
            Spisak_autora = new List<Autor>();
            Spisak_knjiga = new List<Knjiga>();
        }

        public override string ToString()
        {
            return Sifra + ", " + Naziv + ", " + Sef_izdavaca;
        }
    }
}

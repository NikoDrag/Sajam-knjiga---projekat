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
            string sefTekst = "nema";
            if (Sef_izdavaca != null)
            {
                if (!string.IsNullOrEmpty(Sef_izdavaca.Ime) || !string.IsNullOrEmpty(Sef_izdavaca.Prezime))
                {
                    sefTekst = (Sef_izdavaca.Ime + " " + Sef_izdavaca.Prezime).Trim();
                }
                else if (!string.IsNullOrEmpty(Sef_izdavaca.Broj_licne_karte))
                {
                    sefTekst = Sef_izdavaca.Broj_licne_karte;
                }
            }

            string knjigeTekst = "nema";
            if (Spisak_knjiga != null && Spisak_knjiga.Count > 0)
            {
                List<string> nazivi = new List<string>();
                foreach (Knjiga k in Spisak_knjiga)
                {
                    if (!string.IsNullOrEmpty(k.Naziv))
                        nazivi.Add(k.Naziv + " (" + k.ISBN + ")");
                    else
                        nazivi.Add(k.ISBN);
                }
                knjigeTekst = string.Join("; ", nazivi);
            }

            return Sifra + ", " + Naziv + ", Sef: " + sefTekst + " | Knjige: [" + knjigeTekst + "]";
        }
    }
}

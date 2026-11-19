using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Autor
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Datum_rodjenja { get; set; }
        public Adresa Adresa { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Broj_licne_karte { get; set; }
        public int Godine_iskustva { get; set; }
        public List<Knjiga> Knjige { get; set; }

        public Autor()
        {
            Knjige = new List<Knjiga>();
        }

        public Autor(string ime, string prezime, string datum_rodjenja, Adresa adresa, string telefon, string email, string broj_licne_karte, int godine_iskustva, List<Knjiga> knjige)
        {
            Ime = ime;
            Prezime = prezime;
            Datum_rodjenja = datum_rodjenja;
            Adresa = adresa;
            Telefon = telefon;
            Email = email;
            Broj_licne_karte = broj_licne_karte;
            Godine_iskustva = godine_iskustva;
            Knjige = knjige ?? new List<Knjiga>();
        }

        public override string ToString()
        {
            string knjigeTekst = "nema";
            if (Knjige != null && Knjige.Count > 0)
            {
                List<string> nazivi = new List<string>();
                foreach (Knjiga k in Knjige)
                {
                    if (!string.IsNullOrEmpty(k.Naziv))
                        nazivi.Add(k.Naziv + " (" + k.ISBN + ")");
                    else
                        nazivi.Add(k.ISBN);
                }
                knjigeTekst = string.Join("; ", nazivi);
            }

            return Ime + ", " + Prezime + ", " + Datum_rodjenja + ", " + Adresa + ", " +
                   Telefon + ", " + Email + ", " + Broj_licne_karte + ", " + Godine_iskustva +
                   " | Knjige: [" + knjigeTekst + "]";
        }
    }
}
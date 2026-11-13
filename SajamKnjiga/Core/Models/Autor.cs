using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Autor
    {
        public string Ime {  get; set; }
        public string Prezime { get; set; }
        public string Datum_rodjenja { get; set; }
        public string Adresa {  get; set; }
        public string Telefon {  get; set; }
        public string Email { get; set; }
        public string Broj_licne_karte { get; set; }
        public int Godine_iskustva {  get; set; }
        public List<Knjiga> Knjige { get; set; }


        public Autor() { }

        public Autor(string ime, string prezime, string datum_rodjenja, string adresa, string telefon, string email, string broj_licne_karte, int godine_iskustva, List<Knjiga> knjige)
        {
            Ime = ime;
            Prezime = prezime;
            Datum_rodjenja = datum_rodjenja;
            Adresa = adresa;
            Telefon = telefon;
            Email = email;
            Broj_licne_karte = broj_licne_karte;
            Godine_iskustva = godine_iskustva;
            Knjige = knjige;
        }

        public override string ToString()
        {
            return Ime + " " + Prezime + ", " + Datum_rodjenja + ", " + Adresa + ", " +
                   Telefon + ", " + Email + ", " + Broj_licne_karte + ", " + Godine_iskustva;
        }
    }
}

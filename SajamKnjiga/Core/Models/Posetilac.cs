using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public enum Status_posetioca {Redovan, VIP}
    public class Posetilac
    {
        public string Ime
        { get; set; }
        public string Prezime
        { get; set; }
        public string Datum_rodjenja
        { get; set; }
        public Adresa Adresa
        { get; set; }
        public string Kontakt_telefon
        { get; set; }
        public string Email
        { get; set; }
        public string Broj_clanske_karte
        { get; set; }
        public int Trenutna_godina_clanstva 
        { get; set; }
        public Status_posetioca Status_Posetioca
        {get; set; }
        public double Prosecna_ocena_recenzija
        { get; set; }
        public List<Kupovina> Spisak_kupljenih_knjiga 
        { get; set; }
        public List<Knjiga> Lista_zelja
        { get; set; }

        public Posetilac()
        {
            Spisak_kupljenih_knjiga = new List<Kupovina>();
            Lista_zelja = new List<Knjiga>();
        }

        public Posetilac(string ime, string prezime, string datum_rodjenja, Adresa adresa,
                  string kontakt_telefon, string email, string broj_clanske_karte,
                  int trenutna_godina_clanstva, Status_posetioca status_posetioca,
                  double prosecna_ocena_recenzija)
        {
            Ime = ime;
            Prezime = prezime;
            Datum_rodjenja = datum_rodjenja;
            Adresa = adresa;
            Kontakt_telefon = kontakt_telefon;
            Email = email;
            Broj_clanske_karte = broj_clanske_karte;
            Trenutna_godina_clanstva = trenutna_godina_clanstva;
            Status_Posetioca = status_posetioca;
            Prosecna_ocena_recenzija = prosecna_ocena_recenzija;
            Spisak_kupljenih_knjiga = new List<Kupovina>();
            Lista_zelja = new List<Knjiga>();
        }
        public override string ToString()
        {
            return Ime + " " + Prezime + ", " + Datum_rodjenja + ", " + Adresa + ", " +
                   Kontakt_telefon + ", " + Email + ", " + Broj_clanske_karte + ", " +
                   Trenutna_godina_clanstva + ", " + Status_Posetioca + ", " + Prosecna_ocena_recenzija;
        }

    }
}

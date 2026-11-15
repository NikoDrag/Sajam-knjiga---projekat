using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Knjiga
    {
        public string ISBN { get; set; } //medjunarodni kod
        public string Naziv {  get; set; }
        public string Zanra { get; set; }
        public string Godina_izdanja { get; set; }
        public int Cena {  get; set; }
        public int Broj_strana {  get; set; }
        public List<Autor> Autori { get; set; }
        public Izdavac Izdavac {  get; set; }
        public List<Posetilac> Posetilci_kupili { get; set; }
        public List<Posetilac> Posetilci_lista_zelja { get; set; }

        public Knjiga() { 
            Autori = new List<Autor>();
            Posetilci_kupili = new List<Posetilac>();
            Posetilci_lista_zelja = new List<Posetilac>();
        }

        public Knjiga(string isbn, string naziv, string zanra, string godina_izdanja,
            int cena, int broj_strana, List<Autor> autori, Izdavac izdavac, List<Posetilac> posetilci_kupili, List<Posetilac> posetilci_lista_zelja)
        {
            ISBN = isbn;
            Naziv = naziv;
            Zanra = zanra;
            Godina_izdanja = godina_izdanja;
            Cena = cena;
            Broj_strana = broj_strana;
            Autori = autori;
            Izdavac = izdavac;
            Posetilci_kupili = posetilci_kupili;
            Posetilci_lista_zelja = posetilci_lista_zelja;
        }

        public override string ToString()
        {
            return ISBN + ", " + Naziv + ", " + Zanra + ", " + Godina_izdanja + ", " + Cena + ", "
                + Broj_strana + ", " + Izdavac;
        }
    }
}

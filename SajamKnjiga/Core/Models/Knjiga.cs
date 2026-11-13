using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Knjiga
    {
        public string ISBN { get; set; } //међународни стандардни број књиге
        public string Naziv {  get; set; }
        public string Zanra { get; set; }
        public string Godina_izdanja { get; set; }
        public int Cena {  get; set; }
        public int Broj_strana {  get; set; }
        public List<Autor> Autori { get; set; }
        public Izdavac Izdavac {  get; set; }
        public List<Posetilac> Posetilici_kupili { get; set; }
        public List<Posetilac> Posetilac_lista_zelja { get; set; }

        public Knjiga() { }

        public Knjiga(string ISBN, string Naziv, string Zanra, string Godina_izdanja,
            int Cena, int Broj_strana, List<Autor> Autori, Izdavac Izdavac, List<Posetilac> Posetilici_kupili, List<Posetilac> Posetilac_lista_zelja)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ISBN: " + ISBN + ", ");
            sb.Append("Naziv: " + Naziv + ", ");
            sb.Append("Zanra: " + Zanra + ", ");
            sb.Append("E-mejl:" + Email + ", ");
            sb.Append("Broj licne karte: " + Broj_licne_karte + ", ");
            sb.Append("Godine iskustva: " + Godine_iskustva + ", ");
        }
    }
}

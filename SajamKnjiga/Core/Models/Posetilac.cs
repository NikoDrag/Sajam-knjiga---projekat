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
        public List<Kupovina> SpisakKupljenihKnjiga { get; set; }
        public List<Knjiga> ListaZelja { get; set; }


    }
}

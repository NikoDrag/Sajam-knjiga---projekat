using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Core.DAO
{
    public class KupovinaDAO
    {
        private readonly string Putanja = "Podaci" + Path.DirectorySeparatorChar
        + "kupovine.txt";
        private List<Kupovina> Kupovine;

        public KupovinaDAO()
        {
            Kupovine = new List<Kupovina>();
            Ucitaj_iz_fajla();
        }

        public void Ucitaj_iz_fajla()
        {
            Kupovine.Clear();

            if (!File.Exists(Putanja))
                return;

            string[] linije = File.ReadAllLines(Putanja);
            foreach (string linija in linije)
            {
                if (string.IsNullOrWhiteSpace(linija))
                    continue;

                string[] delovi = linija.Split('|');

                Posetilac posetilac = new Posetilac { Broj_clanske_karte = delovi[0] };
                Knjiga knjiga = new Knjiga { ISBN = delovi[1] };

                Kupovina K = new Kupovina(
                    posetilac,
                    knjiga,
                    delovi[2],
                    int.Parse(delovi[3]),
                    delovi[4]
                );
                Kupovine.Add(K);
            }
                
        }
        

       public void Sacuvaj_u_fajl()
       {
            string folder = Path.GetDirectoryName(Putanja);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder); 
            }

            List<string> linije = new List<string>();
            foreach (Kupovina k in Kupovine)
            {
                string clanska_karta = "";
                if (k.Posetilac != null)
                {
                    clanska_karta = k.Posetilac.Broj_clanske_karte;
                }

                string isbn_knjige = "";
                if (k.Knjiga != null)
                {
                    isbn_knjige = k.Knjiga.ISBN;
                }

                string linija = clanska_karta + "|" + isbn_knjige + "|" +
                k.Datum_kupovine + "|" + k.Ocena + "|" + k.Komentar;

                linije.Add(linija);
            }

            File.WriteAllLines(Putanja, linije);
       }

        public void Dodavanje_kupovine(Kupovina kupovina)
        {
            Kupovine.Add(kupovina);
            Sacuvaj_u_fajl();
        }

        public List<Kupovina> Prikaz_kupovina()
        {
            return Kupovine;
        }

        public Kupovina Pronadji_kupovinu(string clanska_karta, string isbn_knjige, string datum)
        {
            foreach (Kupovina k in Kupovine)
            {
                if (k.Posetilac != null && k.Knjiga != null &&
                    k.Posetilac.Broj_clanske_karte == clanska_karta &&
                    k.Knjiga.ISBN == isbn_knjige &&
                    k.Datum_kupovine == datum)
                {
                    return k;
                }
            }
            return null;
        }


        public void Ponistavanje_kupovine(string clanska_karta, string isbn_knjige, string datum)
        {
            //ova funkcija se koristi za brisanje instance kupovina
            Kupovina postojeca = Pronadji_kupovinu(clanska_karta, isbn_knjige, datum);
            if (postojeca != null)
            {
                Kupovine.Remove(postojeca);
                Sacuvaj_u_fajl();
            }
        }

        public void Izmena_kupovine(Kupovina izmenjena)
        {
            Kupovina postojeca = Pronadji_kupovinu(izmenjena.Posetilac.Broj_clanske_karte, izmenjena.Knjiga.ISBN, izmenjena.Datum_kupovine);
            if (postojeca != null)
            {
                postojeca.Ocena = izmenjena.Ocena;
                postojeca.Komentar = izmenjena.Komentar;
                Sacuvaj_u_fajl();
            }
        }
    }
}

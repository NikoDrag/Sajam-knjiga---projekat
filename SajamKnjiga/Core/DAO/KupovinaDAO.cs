using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO
{
    internal class KupovinaDAO
    {
        private readonly string Putanja = "Podaci" + Path.DirectorySeparatorChar
        + "kupovine.txt";
        private List<Kupovina> Kupovine;

        public KupovinaDAO()
        {
            Kupovine = new List<Kupovina>();
            Ucitaj_iz_fajla();
        }

        public void Ucitaj_iz_fajla(List<Posetilac> svi_posetioci = null, List<Knjiga> sve_knjige = null)
        {
            Kupovine.Clear();

            if (!File.Exists(Putanja))
            {
                return;
            }

            string[] linije = File.ReadAllLines(Putanja);
            foreach (string linija in linije)
            {
                if (string.IsNullOrWhiteSpace(linija)) continue;

                string[] delovi = linija.Split('|');
                // Redosled u fajlu:
                // 0:Broj_clanske_karte | 1:ISBN_knjige | 2:Datum_kupovine | 3:Ocena | 4:Komentar

                string clanska_karta = delovi[0];
                string isbn_knjige = delovi[1];
                string datum = delovi[2];
                int ocena = int.Parse(delovi[3]);
                string komentar = delovi[4];

                Posetilac posetilac = Pronadji_posetioca_u_listi(clanska_karta, svi_posetioci);
                Knjiga knjiga = Pronadji_knjigu_u_listi(isbn_knjige, sve_knjige);

                Kupovina kupovina = new Kupovina(posetilac, knjiga, datum, ocena, komentar);
                Kupovine.Add(kupovina);
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
                string clanska_karta = k.Posetilac != null ? k.Posetilac.Broj_clanske_karte : "";
                string isbn_knjige = k.Knjiga != null ? k.Knjiga.ISBN : "";

                string linija = clanska_karta + "|" +
                                isbn_knjige + "|" +
                                k.Datum_kupovine + "|" +
                                k.Ocena + "|" +
                                k.Komentar;

                linije.Add(linija);
            }

            File.WriteAllLines(Putanja, linije);
        }

        public void Dodaj_kupovinu(Kupovina kupovina)
        {
            Kupovine.Add(kupovina);
            Sacuvaj_u_fajl();
        }

        public void Izmeni_kupovinu(Kupovina izmenjena)
        {
            Kupovina postojeca = Pronadji_kupovinu(
                izmenjena.Posetilac.Broj_clanske_karte,
                izmenjena.Knjiga.ISBN,
                izmenjena.Datum_kupovine
            );

            if (postojeca != null)
            {
                postojeca.Ocena = izmenjena.Ocena;
                postojeca.Komentar = izmenjena.Komentar;
                Sacuvaj_u_fajl();
            }
        }

        public void Obrisi_kupovinu(string clanska_karta, string isbn_knjige, string datum)
        {
            Kupovina postojeca = Pronadji_kupovinu(clanska_karta, isbn_knjige, datum);
            if (postojeca != null)
            {
                Kupovine.Remove(postojeca);
                Sacuvaj_u_fajl();
            }
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

        public List<Kupovina> Vrati_sve_kupovine()
        {
            return Kupovine;
        }

        // Pomoćne metode za povezivanje po šifri pri učitavanju iz fajla
        private Posetilac Pronadji_posetioca_u_listi(string clanska_karta, List<Posetilac> lista)
        {
            if (lista != null)
            {
                foreach (Posetilac p in lista)
                {
                    if (p.Broj_clanske_karte == clanska_karta) return p;
                }
            }
            return new Posetilac { Broj_clanske_karte = clanska_karta };
        }

        private Knjiga Pronadji_knjigu_u_listi(string isbn, List<Knjiga> lista)
        {
            if (lista != null)
            {
                foreach (Knjiga k in lista)
                {
                    if (k.ISBN == isbn) return k;
                }
            }
            return new Knjiga { ISBN = isbn };
        }
    }
}

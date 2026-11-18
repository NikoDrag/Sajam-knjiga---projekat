using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Core.Models;

namespace Core.DAO
{
    public class KnjigaDAO
    {
        private readonly string Putanja = "Podaci" + Path.DirectorySeparatorChar + "knjige.txt";
        private List<Knjiga> Knjige;

        public KnjigaDAO()
        {
            Knjige = new List<Knjiga>();
            Ucitaj_iz_fajla();
        }

        public void Ucitaj_iz_fajla()
        {
            Knjige.Clear();

            if (!File.Exists(Putanja))
                return;

            string[] linije = File.ReadAllLines(Putanja);
            foreach (string linija in linije)
            {
                if (string.IsNullOrWhiteSpace(linija))
                    continue;

                string[] delovi = linija.Split('|');

                Izdavac izdavac = new Izdavac { Sifra = delovi[6] };

                Knjiga K = new Knjiga(
                    delovi[0],
                    delovi[1],
                    delovi[2],
                    delovi[3],
                    int.Parse(delovi[4]),
                    int.Parse(delovi[5]),
                    new List<Autor>(),
                    izdavac,
                    new List<Posetilac>(),
                    new List<Posetilac>()
                );

                Knjige.Add(K);
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

            foreach (Knjiga k in Knjige)
            {
                string sifra_izdavaca = "";
                if (k.Izdavac != null)
                {
                    sifra_izdavaca = k.Izdavac.Sifra;
                }

                string linija = k.ISBN + "|" + k.Naziv + "|" + k.Zanra + "|" +
                k.Godina_izdanja + "|" + k.Cena + "|" + k.Broj_strana + "|" +sifra_izdavaca;

                linije.Add(linija);
            }

            File.WriteAllLines(Putanja, linije);
        }

       
        public void Dodavanje_knjige(Knjiga knjiga)
        {
            Knjige.Add(knjiga);
            Sacuvaj_u_fajl();
        }

        public Knjiga Pronadji_knjigu(string isbn)
        {
            foreach (Knjiga k in Knjige)
            {
                if (k.ISBN == isbn)
                {
                    return k;
                }
            }
            return null;
        }

       
        public void Izmena_knjige(Knjiga izmenjena)
        {
            Knjiga postojeca = Pronadji_knjigu(izmenjena.ISBN);
            if (postojeca != null)
            {
                postojeca.Naziv = izmenjena.Naziv;
                postojeca.Zanra = izmenjena.Zanra;
                postojeca.Godina_izdanja = izmenjena.Godina_izdanja;
                postojeca.Cena = izmenjena.Cena;
                postojeca.Broj_strana = izmenjena.Broj_strana;
                
                if (postojeca.Izdavac != null && postojeca.Izdavac != izmenjena.Izdavac)
                {
                    postojeca.Izdavac.Spisak_knjiga.Remove(postojeca);
                }
                    
                if (izmenjena.Izdavac != null && !izmenjena.Izdavac.Spisak_knjiga.Contains(postojeca))
                {
                    izmenjena.Izdavac.Spisak_knjiga.Add(postojeca);
                }
              
                postojeca.Izdavac = izmenjena.Izdavac;
                Sacuvaj_u_fajl();
            }
        }

        public void Brisanje_knjige(string isbn)
        {
            Knjiga postojeca = Pronadji_knjigu(isbn);
            if (postojeca != null)
            {
                Knjige.Remove(postojeca);
                Sacuvaj_u_fajl();
            }
        }

        public List<Knjiga> Prikaz_knjiga()
        {
            return Knjige;
        }
    }
}
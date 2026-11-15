using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Core.Models;

namespace Core.DAO
{
    public class IzdavacDAO
    {
        private readonly string Putanja = "Podaci" + Path.DirectorySeparatorChar + "izdavaci.txt";
        private List<Izdavac> Izdavaci;

        public IzdavacDAO()
        {
            Izdavaci = new List<Izdavac>();
            Ucitaj_iz_fajla();
        }

        public void Ucitaj_iz_fajla()
        {
            Izdavaci.Clear();

            if (!File.Exists(Putanja))
                return;

            string[] linije = File.ReadAllLines(Putanja);
            foreach (string linija in linije)
            {
                if (string.IsNullOrWhiteSpace(linija))
                    continue;

                string[] delovi = linija.Split('|');

                Autor sef = null;
                if (delovi.Length > 2 && !string.IsNullOrEmpty(delovi[2]))
                {
                    sef = new Autor { Broj_licne_karte = delovi[2]};
                }

                Izdavac I = new Izdavac(
                    delovi[0],
                    delovi[1],
                    sef
                );

                Izdavaci.Add(I);
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
            foreach (Izdavac i in Izdavaci)
            {
                string licna_sefa = "";
                if (i.Sef_izdavaca != null)
                {
                    licna_sefa = i.Sef_izdavaca.Broj_licne_karte;
                }

                string linija = i.Sifra + "|" +i.Naziv + "|" + licna_sefa;

                linije.Add(linija);
            }

            File.WriteAllLines(Putanja, linije);
        }

        public void Dodavanje_izdavaca(Izdavac izdavac)
        {
            Izdavaci.Add(izdavac);
            Sacuvaj_u_fajl();
        }

        public Izdavac Pronadji_izdavaca(string sifra)
        {
            foreach (Izdavac i in Izdavaci)
            {
                if (i.Sifra == sifra)
                {
                    return i;
                }
            }
            return null;
        }

        public void Izmena_izdavaca(Izdavac izmenjeni)
        {
            Izdavac postojeci = Pronadji_izdavaca(izmenjeni.Sifra);
            if (postojeci != null)
            {
                postojeci.Naziv = izmenjeni.Naziv;
                postojeci.Sef_izdavaca = izmenjeni.Sef_izdavaca;
                Sacuvaj_u_fajl();
            }
        }

        public void Brisanje_izdavaca(string sifra)
        {
            Izdavac postojeci = Pronadji_izdavaca(sifra);
            if (postojeci != null)
            {
                Izdavaci.Remove(postojeci);
                Sacuvaj_u_fajl();
            }
        }

        public List<Izdavac> Prikaz_izdavaca()
        {
            return Izdavaci;
        }
        public bool Postavljanje_sefa_izdavaca(string sifra_izdavaca, Autor novi_sef)
        {
            Izdavac izdavac = Pronadji_izdavaca(sifra_izdavaca);
            if (izdavac != null && novi_sef != null)
            {
                if (novi_sef.Godine_iskustva >= 5)
                {
                    izdavac.Sef_izdavaca = novi_sef;
                    Sacuvaj_u_fajl();
                    return true;
                }
            }
            return false;
        }
    }
}
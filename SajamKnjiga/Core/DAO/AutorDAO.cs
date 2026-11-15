using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Core.Models;

namespace Core.DAO
{
    public class AutorDAO
    {
        private readonly string Putanja = "Podaci" + Path.DirectorySeparatorChar + "autori.txt";
        private List<Autor> Autori;

        public AutorDAO()
        {
            Autori = new List<Autor>();
            Ucitaj_iz_fajla();
        }

        public void Ucitaj_iz_fajla()
        {
            Autori.Clear();

            if (!File.Exists(Putanja))
                return;

            string[] linije = File.ReadAllLines(Putanja);
            foreach (string linija in linije)
            {
                if (string.IsNullOrWhiteSpace(linija))
                    continue;

                string[] delovi = linija.Split('|');

                Autor A = new Autor(
                    delovi[1],
                    delovi[2],
                    delovi[3],
                    delovi[4],
                    delovi[5],
                    delovi[6],
                    delovi[0],
                    int.Parse(delovi[7]),
                    new List<Knjiga>()
                );

                
                if (delovi.Length > 8 && !string.IsNullOrEmpty(delovi[8]))
                {
                    string[] isbn_niz = delovi[8].Split(';');//radi konzistentnosti sa ; razdvajamo
                    foreach (string isbn in isbn_niz)
                    {
                        Knjiga knjiga = new Knjiga { ISBN = isbn };
                        A.Knjige.Add(knjiga);
                    }
                }

                Autori.Add(A);
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
            foreach (Autor a in Autori)
            {
                string knjige_tekst = "";
                if (a.Knjige != null)
                {
                    List<string> isbn_lista = new List<string>();
                    foreach (Knjiga k in a.Knjige)
                    {
                        isbn_lista.Add(k.ISBN);
                    }
                    knjige_tekst = string.Join(";", isbn_lista);
                }

                string linija = a.Broj_licne_karte + "|" + a.Ime + "|" + a.Prezime + "|" +
                a.Datum_rodjenja + "|" + a.Adresa + "|" + a.Telefon + "|" +
                a.Email + "|" + a.Godine_iskustva + "|" + knjige_tekst;

                linije.Add(linija);
            }

            File.WriteAllLines(Putanja, linije);
        }

        public void Dodavanje_autora(Autor autor)
        {
            if (autor.Knjige == null) //zato sto mozda nista nije napisao 
            {
                autor.Knjige = new List<Knjiga>();
            }
            Autori.Add(autor);
            Sacuvaj_u_fajl();
        }

        public Autor Pronadji_autora(string broj_licne_karte)
        {
            foreach (Autor a in Autori)
            {
                if (a.Broj_licne_karte == broj_licne_karte)
                {
                    return a;
                }
            }
            return null;
        }

        public void Izmena_autora(Autor izmenjeni)
        {
            Autor postojeci = Pronadji_autora(izmenjeni.Broj_licne_karte);
            if (postojeci != null)
            {
                postojeci.Ime = izmenjeni.Ime;
                postojeci.Prezime = izmenjeni.Prezime;
                postojeci.Datum_rodjenja = izmenjeni.Datum_rodjenja;
                postojeci.Adresa = izmenjeni.Adresa;
                postojeci.Telefon = izmenjeni.Telefon;
                postojeci.Email = izmenjeni.Email;
                postojeci.Godine_iskustva = izmenjeni.Godine_iskustva;
                Sacuvaj_u_fajl();
            }
        }

        public void Brisanje_autora(string broj_licne_karte)
        {
            Autor postojeci = Pronadji_autora(broj_licne_karte);
            if (postojeci != null)
            {
                Autori.Remove(postojeci);
                Sacuvaj_u_fajl();
            }
        }

        public List<Autor> Prikaz_autora()
        {
            return Autori;
        }

        public List<Knjiga> Knjige_autora(string broj_licne_karte)
        {
            Autor a = Pronadji_autora(broj_licne_karte);
            if (a != null && a.Knjige != null)
            {
                return a.Knjige;
            }
            return new List<Knjiga>();
        }
   
        public void Dodavanje_knjige_autoru(string broj_licne_karte, Knjiga knjiga)
        {
            Autor a = Pronadji_autora(broj_licne_karte);
            if (a != null && knjiga != null)
            {
                if (a.Knjige == null)//ako nema nist opet pravimo novu praznu listu
                {
                    a.Knjige = new List<Knjiga>();
                }
                a.Knjige.Add(knjiga);
                knjiga.Autori.Add(a);
                Sacuvaj_u_fajl();
            }
        }

        public void Uklanjanje_knjige_sa_autora(string broj_licne_karte, string isbn)
        {
            Autor a = Pronadji_autora(broj_licne_karte);
            if (a != null && a.Knjige != null)
            {
                Knjiga za_brisanje = null;
                foreach (Knjiga k in a.Knjige)
                {
                    if (k.ISBN == isbn)
                    {
                        za_brisanje = k;
                        break;
                    }
                }

                if (za_brisanje != null)
                {
                    a.Knjige.Remove(za_brisanje);
                    za_brisanje.Autori.Remove(a);
                    Sacuvaj_u_fajl();
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Core.Models;

namespace Core.DAO
{
    public class PosetilacDAO
    {
        private readonly string Putanja = "Podaci" + Path.DirectorySeparatorChar + "posetioci.txt";
        private List<Posetilac> Posetioci;

        public PosetilacDAO()
        {
            Posetioci = new List<Posetilac>();
            Ucitaj_iz_fajla();
        }

        public void Ucitaj_iz_fajla(List<Knjiga> sve_knjige = null)
        { 
            Posetioci.Clear();

            if(!File.Exists(Putanja))
                return;

            string[] linije = File.ReadAllLines(Putanja);
            foreach (string linija in linija)
	        {
                if(string.IsNullOrWhiteSpace(linija))
                    continue;

                string[] delovi = linija.Split('|');
                Adresa adresa = new Adresa(delovi[4], delovi[5], delovi[6], delovi[7]);
                Status_posetioca status = (Status_posetioca)Enum.Parse(typeof(Status_posetioca), delovi[11]);
                //ovo je kod koji prilagodjava informaciju za nas postojeci enum

                Posetilac P = new Posetilac(
                    delovi[1],
                    delovi[2],
                    delovi[3],
                    adresa,
                    delovi[8],
                    delovi[9],
                    delovi[0],
                    int.Parse(delovi[10]),
                    status,
                    double.Parse(delovi[12])
                    
                    );

                //sve za listu zelja nadole
                if (delovi.Length > 13 && !string.IsNullOrEmpty(delovi[13]))
                {
                    string[] isbn_niz = delovi[13].Split(';'); //isto kao za string gore nabrajamo isbn knjiga
                    foreach (string isbn in isbn_niz)
                    {
                        Knjiga pronadjena = null;
                        if (sve_knjige != null)
                        {
                            foreach (Knjiga k in sve_knjige)
                            {
                                if (k.ISBN == isbn)
                                {
                                    pronadjena = k;
                                    break;
                                }
                            }
                        }

                        if (pronadjena == null)
                        {
                            pronadjena = new Knjiga { ISBN = isbn };
                        }
                        //rezerva ako ne nadje
                        p.Lista_zelja.Add(pronadjena);
                        pronadjena.Posetilci_lista_zelja.Add(p);
                    }
                }
                Posetioci.Add(P);
	        }
        }

        public void Sacuvaj_u_fajl()
        {
            string folder = Path.GetDirectoryName(Putanja);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);//pravimo folder za cuvanje ako vec ne postoji
            }

            List<string> linije = new List<string>();
            foreach (Posetilac p in Posetioci)
            {
                string ulica = "";
                string broj = "";
                string grad = "";
                string drzava = "";

                if (p.Adresa != null)
                {
                    ulica = p.Adresa.Ulica;
                    broj = p.Adresa.Broj;
                    grad = p.Adresa.Grad;
                    drzava = p.Adresa.Drzava;
                }

                string zelje_tekst = "";
                if (p.Lista_zelja != null)
                {
                    List<string> isbn_lista = new List<string>();
                    foreach (Knjiga k in p.Lista_zelja)
                    {
                        isbn_lista.Add(k.ISBN);
                    }
                    zelje_tekst = string.Join(";", isbn_lista);//elem;elem;elem...
                }

                string linija = p.Broj_clanske_karte + "|" + p.Ime + "|" +
                p.Prezime + "|" + p.Datum_rodjenja + "|" + ulica + "|" + broj + "|" +
                grad + "|" + drzava + "|" + p.Kontakt_telefon + "|" +
                p.Email + "|" + p.Trenutna_godina_clanstva + "|" +
                p.Status_Posetioca + "|" + p.Prosecna_ocena_recenzija + "|" + zelje_tekst;

                linije.Add(linija);
            }

            File.WriteAllLines(Putanja, linije);
        }

        public void Dodavanje_posetioca(Posetilac posetilac)
        {
            Posetioci.Add(posetilac);
            Sacuvaj_u_fajl();
        }

        public Posetilac Pronadji_posetioca(string broj_clanske_karte)
        {
            foreach (Posetilac p in Posetioci)
            {
                if (p.Broj_clanske_karte == broj_clanske_karte)
                {
                    return p;
                }
            }
            return null;
        }

        public List<Posetilac> Pretraga_posetilaca(string upit)
        {
            List<Posetilac> rezultat = new List<Posetilac>();
            string tekst = upit.ToLower();

            foreach (Posetilac p in Posetioci)
            {
                if (p.Prezime.ToLower().Contains(tekst) ||
                    p.Ime.ToLower().Contains(tekst) ||
                    p.Broj_clanske_karte.ToLower().Contains(tekst))
                {//ako je nesto od ova 3 ispunjeno odnosno sadrzi string ubacujemo ga u listu
                    rezultat.Add(p);
                }
            }

            return rezultat;
        }

        public void Izmena_posetioca(Posetilac izmenjeni)
        {
            Posetilac postojeci = Pronadji_posetioca(izmenjeni.Broj_clanske_karte);
            //trazimo ga po br clanske karte
        if (postojeci != null)
        {
            postojeci.Ime = izmenjeni.Ime;
            postojeci.Prezime = izmenjeni.Prezime;
            postojeci.Datum_rodjenja = izmenjeni.Datum_rodjenja;
            postojeci.Adresa = izmenjeni.Adresa;
            postojeci.Kontakt_telefon = izmenjeni.Kontakt_telefon;
            postojeci.Email = izmenjeni.Email;
            postojeci.Trenutna_godina_clanstva = izmenjeni.Trenutna_godina_clanstva;
            postojeci.Status_Posetioca = izmenjeni.Status_Posetioca;
            postojeci.Prosecna_ocena_recenzija = izmenjeni.Prosecna_ocena_recenzija;
            Sacuvaj_u_fajl();
            }
        }

        public void Brisanje_posetioca(string broj_clanske_karte)
        {
            Posetilac postojeci = Pronadji_posetioca(broj_clanske_karte);
            if (postojeci != null)
            {
                Posetioci.Remove(postojeci);
                Sacuvaj_u_fajl();
            }
        }

        public List<Posetilac> Prikaz_posetilaca()
        {
            return Posetioci;
        }

        public void Dodavanje_knjige_na_listu_zelja(string broj_clanske_karte, Knjiga knjiga)
        {
            Posetilac p = Pronadji_posetioca(broj_clanske_karte);
            if (p != null && knjiga != null)
            {
                p.Lista_zelja.Add(knjiga);
                knjiga.Posetilci_lista_zelja.Add(p);
                Sacuvaj_u_fajl();
            }
        }

        public List<Posetilac> Sortiranje_posetilaca()
        {
            List<Posetilac> sortirani = new List<Posetilac>(Posetioci);
            sortirani.Sort((p1, p2) => string.Compare(p1.Ime, p2.Ime));
            return sortirani; //po imenu sort
        }

        public List<Kupovina> Prikaz_kupljenih_knjiga(string broj_clanske_karte)
        {
            Posetilac p = Pronadji_posetioca(broj_clanske_karte);
            if (p != null)
            {
                return p.Spisak_kupljenih_knjiga;
            }

            return new List<Kupovina>(); //samo prazna lista jer nista nije uzeo
        }
    }
}

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

        public Posetilac_dao()
        {
            Posetioci = new List<Posetilac>();
            Ucitaj_iz_fajla();
        }

        public void Ucitaj_iz_fajla()
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

                string linija = p.Broj_clanske_karte + "|" + p.Ime + "|" +
                p.Prezime + "|" + p.Datum_rodjenja + "|" + ulica + "|" + broj + "|" +
                grad + "|" + drzava + "|" + p.Kontakt_telefon + "|" +
                p.Email + "|" + p.Trenutna_godina_clanstva + "|" +
                p.Status_Posetioca + "|" + p.Prosecna_ocena_recenzija;

                linije.Add(linija);
            }

            File.WriteAllLines(Putanja, linije);
        }




    }
}

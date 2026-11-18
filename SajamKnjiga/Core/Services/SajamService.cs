using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.DAO;


namespace Core.Services
{
    public class SajamService
    {
        public PosetilacDAO PosetilacDAO
        { get; set; }
        public KnjigaDAO KnjigaDAO 
        { get; set; }
        public AutorDAO AutorDAO 
        { get; set; }
        public IzdavacDAO IzdavacDAO 
        { get; set; }
        public KupovinaDAO KupovinaDAO 
        { get; set; }

        public SajamService()
        {
            PosetilacDAO = new PosetilacDAO();
            KnjigaDAO = new KnjigaDAO();
            AutorDAO = new AutorDAO();
            IzdavacDAO = new IzdavacDAO();
            KupovinaDAO = new KupovinaDAO();

            Povezi_podatke();
        }

        private void Povezi_podatke()
        {
            //povezivanje autora sa knjigama
            foreach (Autor a in AutorDAO.Prikaz_autora())
            {
                List<Knjiga> prave_knjige = new List<Knjiga>();
                if (a.Knjige != null)
                {
                    foreach (Knjiga k in a.Knjige)
                    {
                        Knjiga pronadjena = KnjigaDAO.Pronadji_knjigu(k.ISBN);
                        if (pronadjena != null)
                        {
                            prave_knjige.Add(pronadjena);
                            if (!pronadjena.Autori.Contains(a))
                            {
                                pronadjena.Autori.Add(a);
                            }
                        }
                    }
                }
                a.Knjige = prave_knjige;
            }
            //povezivanje izdavaca sa knjigama
            foreach (Knjiga k in KnjigaDAO.Prikaz_knjiga())
            {
                if (k.Izdavac != null)
                {
                    Izdavac pravi_izdavac = IzdavacDAO.Pronadji_izdavaca(k.Izdavac.Sifra);
                    if (pravi_izdavac != null)
                    {
                        k.Izdavac = pravi_izdavac;
                        if (!pravi_izdavac.Spisak_knjiga.Contains(k))
                        {
                            pravi_izdavac.Spisak_knjiga.Add(k);
                        }
                    }
                }
            }

            //povezivanje sefa sa izdavacima
            foreach (Izdavac i in IzdavacDAO.Prikaz_izdavaca())
            {
                if (i.Sef_izdavaca != null)
                {
                    Autor pravi_sef = AutorDAO.Pronadji_autora(i.Sef_izdavaca.Broj_licne_karte);
                    if (pravi_sef != null)
                    {
                        i.Sef_izdavaca = pravi_sef;
                    }
                }
            }

            //povezivanje liste zelja kod posetilaca
            foreach (Posetilac p in PosetilacDAO.Prikaz_posetilaca())
            {
                List<Knjiga> prava_lista_zelja = new List<Knjiga>();
                if (p.Lista_zelja != null)
                {
                    foreach (Knjiga k in p.Lista_zelja)
                    {
                        Knjiga pronadjena = KnjigaDAO.Pronadji_knjigu(k.ISBN);
                        if (pronadjena != null)
                        {
                            prava_lista_zelja.Add(pronadjena);
                            if (!pronadjena.Posetilci_lista_zelja.Contains(p))
                            {
                                pronadjena.Posetilci_lista_zelja.Add(p);
                            }
                        }
                    }
                }
                p.Lista_zelja = prava_lista_zelja;
            }

            foreach (Kupovina kup in KupovinaDAO.Prikaz_kupovina())
            {
                Posetilac p = PosetilacDAO.Pronadji_posetioca(kup.Posetilac.Broj_clanske_karte);
                Knjiga k = KnjigaDAO.Pronadji_knjigu(kup.Knjiga.ISBN);
                
                if (p != null && k != null)
                {
                    kup.Posetilac = p;
                    kup.Knjiga = k;
                    p.Spisak_kupljenih_knjiga.Add(kup);

                    if (!k.Posetilci_kupili.Contains(p))
                    {
                        k.Posetilci_kupili.Add(p);
                    }
                }
            }
        }

        public Kupovina Kupi_knjigu(string broj_clanske_karte, string isbn, string datum, int ocena, string komentar)
        {
            Posetilac p = PosetilacDAO.Pronadji_posetioca(broj_clanske_karte);
            Knjiga k = KnjigaDAO.Pronadji_knjigu(isbn);
            if (p == null || k == null)
            {
                return null;
            }

            if (KupovinaDAO.Pronadji_kupovinu(broj_clanske_karte, isbn, datum) != null)
            {
                return null;
            }

            Kupovina nova = new Kupovina(p, k, datum, ocena, komentar);
            KupovinaDAO.Dodavanje_kupovine(nova);
            p.Spisak_kupljenih_knjiga.Add(nova);
            k.Posetilci_kupili.Add(p);

            p.Prosecna_ocena_recenzija = p.Spisak_kupljenih_knjiga.Average(x => x.Ocena);
            PosetilacDAO.Sacuvaj_u_fajl();
            return nova;
        }

        public bool Izmeni_kupovinu(string karta, string isbn, string datum, int ocena, string komentar)
        {
            Kupovina kup = KupovinaDAO.Pronadji_kupovinu(karta, isbn, datum);
            if (kup == null)
            {
                return false;
            }

            kup.Ocena = ocena;
            kup.Komentar = komentar;
            KupovinaDAO.Izmena_kupovine(kup);

            Posetilac p = kup.Posetilac;
            if (p != null && p.Spisak_kupljenih_knjiga.Count > 0)
            {
                p.Prosecna_ocena_recenzija = p.Spisak_kupljenih_knjiga.Average(x => x.Ocena);
                PosetilacDAO.Sacuvaj_u_fajl();
            }
            return true;
        }

        private void Izracunaj_prosek(Posetilac p)
        {
            p.Prosecna_ocena_recenzija = p.Spisak_kupljenih_knjiga.Count > 0 
                ? p.Spisak_kupljenih_knjiga.Average(x => x.Ocena): 0;
        }

        public bool Obrisi_autora(string broj_licne_karte)
        {
            Autor autor = AutorDAO.Pronadji_autora(broj_licne_karte);
            if (autor == null)
            {
                return false;
            }

            // uklanja autora sa svih knjiga
            foreach (Knjiga k in KnjigaDAO.Prikaz_knjiga())
            {
                k.Autori.Remove(autor);
            }

            // ako je sef nekog izdavaca, izdavac ostaje bez sefa
            bool izdavac_izmenjen = false;
            foreach (Izdavac i in IzdavacDAO.Prikaz_izdavaca())
            {
                if (i.Sef_izdavaca != null && i.Sef_izdavaca.Broj_licne_karte == broj_licne_karte)
                {
                    i.Sef_izdavaca = null;
                    izdavac_izmenjen = true;
                }
                i.Spisak_autora.Remove(autor);
            }
            if (izdavac_izmenjen)
            {
                IzdavacDAO.Sacuvaj_u_fajl();
            }

            AutorDAO.Brisanje_autora(broj_licne_karte);
            return true;
        }

        public bool Obrisi_izdavaca(string sifra, out string poruka)
        {
            poruka = "";
            Izdavac izdavac = IzdavacDAO.Pronadji_izdavaca(sifra);
            if (izdavac == null)
            {
                poruka = "Izdavac nije pronadjen.";
                return false;
            }
            //Any proverava da li postoji bilo koji element u listi da isupnjava uslov
            if (KnjigaDAO.Prikaz_knjiga().Any(k => k.Izdavac != null && k.Izdavac.Sifra == sifra))
            {
                poruka = "Izdavac ima knjige. Prvo obrisi knjige ili promeni izdavaca.";
                return false;
            }

            IzdavacDAO.Brisanje_izdavaca(sifra);
            return true;
        }

        public bool Obrisi_knjigu(string isbn)
        {
            Knjiga knjiga = KnjigaDAO.Pronadji_knjigu(isbn);
            if (knjiga == null)
            {
                return false;
            }

            // Uklanja knjigu sa liste kupovine
            foreach (Kupovina kup in KupovinaDAO.Prikaz_kupovina().Where(x => x.Knjiga != null && x.Knjiga.ISBN == isbn).ToList())
            {
                if (kup.Posetilac != null)
                {
                    kup.Posetilac.Spisak_kupljenih_knjiga.Remove(kup);
                    Izracunaj_prosek(kup.Posetilac);
                }
            }
            KupovinaDAO.Prikaz_kupovina().RemoveAll(x => x.Knjiga != null && x.Knjiga.ISBN == isbn);
            KupovinaDAO.Sacuvaj_u_fajl();

            // Uklanja knjigu sa liste zelja
            foreach (Posetilac p in PosetilacDAO.Prikaz_posetilaca())
            {
                p.Lista_zelja.RemoveAll(x => x.ISBN == isbn);
            }
            PosetilacDAO.Sacuvaj_u_fajl();

            // Uklanja knjigu iz autorove liste knjiga
            foreach (Autor a in AutorDAO.Prikaz_autora())
            {
                if (a.Knjige != null)
                {
                    a.Knjige.RemoveAll(x => x.ISBN == isbn);
                }
            }
            AutorDAO.Sacuvaj_u_fajl();

            // Uklanja knjigu sa izdavacovog spiska knjiga
            if (knjiga.Izdavac != null)
            {
                knjiga.Izdavac.Spisak_knjiga.Remove(knjiga);
            }

            KnjigaDAO.Brisanje_knjige(isbn);
            return true;
        }

        public bool Obrisi_kupovinu(string karta, string isbn, string datum)
        {
            Kupovina kup = KupovinaDAO.Pronadji_kupovinu(karta, isbn, datum);
            if (kup == null)
            {
                return false;
            }

            Posetilac p = kup.Posetilac;
            Knjiga k = kup.Knjiga;

            KupovinaDAO.Ponistavanje_kupovine(karta, isbn, datum);

            if (p != null)
            {
                p.Spisak_kupljenih_knjiga.Remove(kup);
                Izracunaj_prosek(p); //mora opet da racuna prosek

                // ako posetilac nema nijednu kupovinu te knjige, uklanjamo ga iz liste kupaca
                if (k != null && !p.Spisak_kupljenih_knjiga.Any(x => x.Knjiga != null && x.Knjiga.ISBN == k.ISBN))
                {
                    k.Posetilci_kupili.Remove(p);
                }
                PosetilacDAO.Sacuvaj_u_fajl();
            }
            return true;
        }

        public bool Obrisi_posetioca(string broj_clanske_karte)
        {
            Posetilac p = PosetilacDAO.Pronadji_posetioca(broj_clanske_karte);
            if (p == null)
            {
                return false;
            }

            // Brise posetioca sa svih kupovine posetioca
            foreach (Kupovina kup in KupovinaDAO.Prikaz_kupovina())
            {
                if (kup.Posetilac != null && kup.Posetilac.Broj_clanske_karte == broj_clanske_karte && kup.Knjiga != null)
                {
                    kup.Knjiga.Posetilci_kupili.Remove(p);
                }
            }
            KupovinaDAO.Prikaz_kupovina().RemoveAll(x => x.Posetilac != null && x.Posetilac.Broj_clanske_karte == broj_clanske_karte);
            KupovinaDAO.Sacuvaj_u_fajl();

            // Brise posetioca sa lista zelja
            foreach (Knjiga k in p.Lista_zelja)
            {
                k.Posetilci_lista_zelja.Remove(p);
            }

            PosetilacDAO.Brisanje_posetioca(broj_clanske_karte);
            return true;
        }

        //#autor_posetioci
        public List<Posetilac> Posetioci_za_autora(string broj_licne_karte_autora)
        {
            List<Posetilac> rezultat = new List<Posetilac>();
            Autor izabrani_autor = AutorDAO.Pronadji_autora(broj_licne_karte_autora);

            if (izabrani_autor == null || izabrani_autor.Knjige == null)
            {
                return rezultat;
            }

            foreach (Kupovina kupovina in KupovinaDAO.Prikaz_kupovina())
            {
                if (kupovina.Posetilac == null || kupovina.Knjiga == null)
                {
                    continue;
                }

                bool knjiga_od_autora = false;
                foreach (Knjiga k in izabrani_autor.Knjige)
                {
                    if (k.ISBN == kupovina.Knjiga.ISBN)
                    {
                        knjiga_od_autora = true;
                        break;
                    }
                }

                if (knjiga_od_autora == true) 
                {
                    Posetilac pravi_posetilac = PosetilacDAO.Pronadji_posetioca(kupovina.Posetilac.Broj_clanske_karte);
                    if (pravi_posetilac != null && !rezultat.Contains(pravi_posetilac))
                    {
                        rezultat.Add(pravi_posetilac);
                    }
                }
            }

            return rezultat;
        }

        //#posetilac_uslov_knjiga
        public List<Posetilac> Posetioci_uslov_knjige(string prvi_isbn, string drugi_isbn)
        {
            List<Posetilac> rezultat = new List<Posetilac>();

            foreach (Posetilac p in PosetilacDAO.Prikaz_posetilaca())
            {
                if (p.Lista_zelja == null)
                {
                    continue;
                }

                bool ima_prvu = false;
                bool ima_drugu = false;

                foreach (Knjiga k in p.Lista_zelja)
                {
                    if (k.ISBN == prvi_isbn)
                    {
                        ima_prvu = true;
                    }
                    if (k.ISBN == drugi_isbn)
                    {
                        ima_drugu = true;
                    }
                }

                if (ima_prvu && ima_drugu)
                {
                    rezultat.Add(p);
                }
            }

            return rezultat;
        }


    }
}
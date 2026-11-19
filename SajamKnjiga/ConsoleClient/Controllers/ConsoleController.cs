using System;
using System.Collections.Generic;
using Core.Models;
using Core.Services;

namespace ConsoleClient.Controllers
{
    public class ConsoleController
    {
        private SajamService sajamService;

        public ConsoleController()
        {
            sajamService = new SajamService();
        }

        public void Start()
        {
            bool radi = true;
            while (radi)
            {
                Console.WriteLine("\n=== SAJAM KNJIGA - GLAVNI MENI ===");
                Console.WriteLine("1. Prikaz svih posetilaca");
                Console.WriteLine("2. Dodavanje posetioca");
                Console.WriteLine("3. Izmena posetioca");
                Console.WriteLine("4. Brisanje posetioca");
                Console.WriteLine("5. Prikaz svih knjiga");
                Console.WriteLine("6. Dodavanje knjige");
                Console.WriteLine("7. Dodavanje knjige na listu zelja");
                Console.WriteLine("8. Kupovina knjige");
                Console.WriteLine("9. Prikaz svih autora");
                Console.WriteLine("10. Dodavanje autora");
                Console.WriteLine("11. Dodavanje knjige autoru");
                Console.WriteLine("12. Prikaz svih izdavaca");
                Console.WriteLine("13. Dodavanje izdavaca");
                Console.WriteLine("14. Postavljanje sefa izdavaca");
                Console.WriteLine("15. Posetioci koji su kupili knjigu od autora (#autor_posetioci)");
                Console.WriteLine("16. Posetioci sa dve izabrane knjige na listi zelja (#posetilac_uslov_knjiga)");
                Console.WriteLine("17. Izmena autora");
                Console.WriteLine("18. Izmena knjige");
                Console.WriteLine("19. Izmena izdavaca");
                Console.WriteLine("20. Izmena kupovine");
                Console.WriteLine("21. Brisanje knjige");
                Console.WriteLine("22. Brisanje autora");
                Console.WriteLine("23. Brisanje izdavaca");
                Console.WriteLine("24. Ponistavanje kupovine");
                Console.WriteLine("25. Prikaz liste zelja posetioca");
                Console.WriteLine("26. Prikaz knjiga izdavaca");
                Console.WriteLine("27. Prikaz autora izdavaca");
                Console.WriteLine("28. Prikaz kupljenih knjiga posetioca");
                Console.WriteLine("30. Prikaz knjiga autora");
                Console.WriteLine("31. Uklanjanje knjige sa autora");
                Console.WriteLine("0. Izlaz");
                Console.Write("Izaberite opciju: ");

                string izbor = Console.ReadLine();
                switch (izbor)
                {
                    case "1":
                        PrikaziPosetioce();
                        break;
                    case "2":
                        DodajPosetioca();
                        break;
                    case "3":
                        IzmeniPosetioca();
                        break;
                    case "4":
                        ObrisiPosetioca();
                        break;
                    case "5":
                        PrikaziKnjige();
                        break;
                    case "6":
                        DodajKnjigu();
                        break;
                    case "7":
                        DodajNaListuZelja();
                        break;
                    case "8":
                        KupiKnjigu();
                        break;
                    case "9":
                        PrikaziAutore();
                        break;
                    case "10":
                        DodajAutora();
                        break;
                    case "11":
                        DodajKnjiguAutoru();
                        break;
                    case "12":
                        PrikaziIzdavace();
                        break;
                    case "13":
                        DodajIzdavaca();
                        break;
                    case "14":
                        PostaviSefaIzdavaca();
                        break;
                    case "15":
                        PosetiociZaAutora();
                        break;
                    case "16":
                        PosetiociUslovKnjige();
                        break;
                    case "17": 
                        IzmeniAutora(); 
                        break;
                    case "18": 
                        IzmeniKnjigu(); 
                        break;
                    case "19": 
                        IzmeniIzdavaca(); 
                        break;
                    case "20": 
                        IzmeniKupovinu(); 
                        break;
                    case "21":
                        ObrisiKnjigu();
                        break;
                    case "22":
                        ObrisiAutora();
                        break;
                    case "23":
                        ObrisiIzdavaca();
                        break;
                    case "24":
                        ObrisiKupovinu();
                        break;
                    case "25":
                        PrikaziListuZelja();
                        break;
                    case "26":
                        PrikaziKnjigeIzdavaca();
                        break;
                    case "27":
                        PrikaziAutoreIzdavaca();
                        break;
                    case "28":
                        PrikaziKupljeneKnjigePosetioca();
                        break;
                    case "30":
                        PrikaziKnjigeAutora();
                        break;
                    case "31":
                        UkloniKnjiguAutoru();
                        break;
                    case "0":
                        radi = false;
                        break;
                    default:
                        Console.WriteLine("Nepoznata opcija.");
                        break;
                }
            }
        }

        private void UkloniKnjiguAutoru()
        {
            Console.Write("Broj licne karte autora: ");
            string lk = Console.ReadLine();

            Autor a = sajamService.AutorDAO.Pronadji_autora(lk);
            if (a == null)
            {
                Console.WriteLine("Autor nije pronadjen.");
                return;
            }

            Console.Write("ISBN knjige: ");
            string isbn = Console.ReadLine();

            bool imaKnjigu = false;
            if (a.Knjige != null)
            {
                foreach (Knjiga k in a.Knjige)
                {
                    if (k.ISBN == isbn)
                    {
                        imaKnjigu = true;
                        break;
                    }
                }
            }

            if (!imaKnjigu)
            {
                Console.WriteLine("Autor nema knjigu sa tim ISBN-om na spisku.");
                return;
            }

            sajamService.AutorDAO.Uklanjanje_knjige_sa_autora(lk, isbn);
            Console.WriteLine("Knjiga uspesno uklonjena sa autora.");
        }

        private void PrikaziKnjigeAutora()
        {
            Console.Write("Broj licne karte autora: ");
            string lk = Console.ReadLine();

            Autor a = sajamService.AutorDAO.Pronadji_autora(lk);
            if (a == null)
            {
                Console.WriteLine("Autor nije pronadjen.");
                return;
            }

            Console.WriteLine($"\n----- KNJIGE AUTORA: {a.Ime} {a.Prezime} ({a.Broj_licne_karte}) -----");
            if (a.Knjige == null || a.Knjige.Count == 0)
            {
                Console.WriteLine("(nema knjiga)");
                return;
            }

            foreach (Knjiga k in a.Knjige)
            {
                Console.WriteLine(k);
            }
        }


        // Provera da li je unos validan broj
        private int CitajBroj(int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                if(int.TryParse(Console.ReadLine(), out int rezultat) && rezultat >= min && rezultat <= max)
                {
                    return rezultat;
                }
                Console.WriteLine("Unos nije ispravan, pokusaj opet");
            }
        }


        private void PrikaziKupljeneKnjigePosetioca()
        {
            Console.Write("Broj clanske karte posetioca: ");
            string karta = Console.ReadLine();

            Posetilac p = sajamService.PosetilacDAO.Pronadji_posetioca(karta);
            if (p == null)
            {
                Console.WriteLine("Posetilac nije pronadjen.");
                return;
            }

            Console.WriteLine($"\n----- KUPLJENE KNJIGE POSETIOCA: {p.Ime} {p.Prezime} ({p.Broj_clanske_karte}) -----");
            if (p.Spisak_kupljenih_knjiga == null || p.Spisak_kupljenih_knjiga.Count == 0)
            {
                Console.WriteLine("(nema knjiga)");
                return;
            }

            foreach (Kupovina kup in p.Spisak_kupljenih_knjiga)
            {
                if (kup.Knjiga != null)
                {
                    Console.WriteLine($"{kup.Knjiga.Naziv} ({kup.Knjiga.ISBN}) | Datum: {kup.Datum_kupovine} | Ocena: {kup.Ocena} | Komentar: {kup.Komentar}");
                }
            }
        }

        private string CitajIzmenu(string trenutno)
        {
            string unos = Console.ReadLine();
            return string.IsNullOrWhiteSpace(unos) ? trenutno : unos.Trim();
        }

        private int CitajBrojIzmena(int trenutno, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                string unos = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(unos))
                {
                    return trenutno;
                }

                if (int.TryParse(unos, out int r) && r >= min && r <= max)
                {
                    return r;
                }
                Console.WriteLine("Unos nije ispravan.");
            }
        }

        //Promena adrese jer je napravljen kao manji deo ostalog 
        private Adresa IzmeniAdresu(Adresa stara) 
        {
            if (stara == null)
            {
                stara = new Adresa();
            }
            Console.WriteLine("Unesite ulicu: ");
            stara.Ulica = CitajIzmenu(stara.Ulica);
            Console.WriteLine("Unesite broj ulice: ");
            stara.Broj = CitajIzmenu(stara.Broj);
            Console.WriteLine("Unesite grad: ");
            stara.Grad = CitajIzmenu(stara.Grad);
            Console.WriteLine("Unesite drzavu: ");
            stara.Drzava = CitajIzmenu(stara.Drzava);

            return stara;
        }

        

        private Adresa UnesiAdresu()
        {
            Console.Write("Ulica: ");
            string ulica = Console.ReadLine();
            Console.Write("Broj: ");
            string broj = Console.ReadLine();
            Console.Write("Grad: ");
            string grad = Console.ReadLine();
            Console.Write("Drzava: ");
            string drzava = Console.ReadLine();

            return new Adresa(ulica, broj, grad, drzava);
        }

        private void PrikaziPosetioce()
        {
            foreach (Posetilac p in sajamService.PosetilacDAO.Prikaz_posetilaca())
            {
                Console.WriteLine(p);
            }
        }

        private void DodajPosetioca()
        {
            Console.Write("Broj clanske karte: ");
            string karta = Console.ReadLine();

            if(sajamService.PosetilacDAO.Pronadji_posetioca(karta) != null)
            {
                Console.WriteLine("Posetilac sa tom kartom vec postoji");
                return;
            }

            Console.Write("Ime: ");
            string ime = Console.ReadLine();
            Console.Write("Prezime: ");
            string prezime = Console.ReadLine();
            Console.Write("Datum rodjenja: ");
            string datum = Console.ReadLine();
            Adresa adresa = UnesiAdresu();
            Console.Write("Telefon: ");
            string tel = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Trenutna godina clanstva: ");
            int godina = CitajBroj(1);//int.Parse(Console.ReadLine());
            Console.Write("Status (R - redovan / V - VIP): ");
            Status_posetioca status = Console.ReadLine().ToUpper() == "V" ? Status_posetioca.VIP : Status_posetioca.Redovan;

            Posetilac novi = new Posetilac(ime, prezime, datum, adresa, tel, email, karta, godina, status, 0);
            sajamService.PosetilacDAO.Dodavanje_posetioca(novi);
            Console.WriteLine("Posetilac uspesno dodat.");
        }

        private void IzmeniPosetioca()
        {
            Console.Write("Unesite broj clanske karte posetioca za izmenu: ");
            string karta = Console.ReadLine();
            Posetilac postojeci = sajamService.PosetilacDAO.Pronadji_posetioca(karta);
            if (postojeci == null)
            {
                Console.WriteLine("Posetilac nije pronadjen.");
                return;
            }
            //broj clanske karte ostaje isti
            Console.Write("Novo ime: ");
            postojeci.Ime = CitajIzmenu(postojeci.Ime);
            Console.Write("Novo prezime: ");
            postojeci.Prezime = CitajIzmenu(postojeci.Prezime);
            Console.WriteLine("Unesite novi datum rodjena:");
            postojeci.Datum_rodjenja = CitajIzmenu(postojeci.Datum_rodjenja);
            Console.WriteLine("Unesite novu adresu:");
            postojeci.Adresa = IzmeniAdresu(postojeci.Adresa);
            Console.Write("Novi telefon: ");
            postojeci.Kontakt_telefon = CitajIzmenu(postojeci.Kontakt_telefon);
            Console.Write("Novi email: ");
            postojeci.Email = CitajIzmenu(postojeci.Email);
            Console.Write("Trenutna godina clanstva: ");
            postojeci.Trenutna_godina_clanstva = CitajBrojIzmena(postojeci.Trenutna_godina_clanstva, 1);
            Console.Write("Status (R - redovan / V - VIP, Enter ostavlja staro): ");
            string status = Console.ReadLine().Trim().ToUpper();
            if (status == "V")
            {
                postojeci.Status_Posetioca = Status_posetioca.VIP;
            }
            else if (status == "R")
            {
                postojeci.Status_Posetioca = Status_posetioca.Redovan;
            }
            
            //Prosecna ocena se ne menja jer zavisi od unetih ocena

            sajamService.PosetilacDAO.Izmena_posetioca(postojeci);
            Console.WriteLine("Posetilac izmenjen.");
        }

        private void ObrisiPosetioca()
        {
            Console.Write("Unesite broj clanske karte za brisanje: ");
            string karta = Console.ReadLine();

            if (sajamService.Obrisi_posetioca(karta))
            {
                Console.WriteLine("Posetilac obrisan.");
            }
            else
            {
                Console.WriteLine("Posetilac nije pronadjen.");
            }
        }

        //# prikaz_liste_zelja
        private void PrikaziListuZelja()
        {
            Console.Write("Broj clanske karte posetioca: ");
            string karta = Console.ReadLine();

            Posetilac p = sajamService.PosetilacDAO.Pronadji_posetioca(karta);
            if (p == null)
            {
                Console.WriteLine("Posetilac nije pronadjen.");
                return;
            }

            Console.WriteLine("\n----- LISTA ZELJA -----");
            Console.WriteLine(p.Ime + " " + p.Prezime + " (" + p.Broj_clanske_karte + ")");
            Console.WriteLine("--------------------------------------------");

            if (p.Lista_zelja == null || p.Lista_zelja.Count == 0)
            {
                Console.WriteLine("(lista zelja je prazna)");
                return;
            }

            foreach (Knjiga k in p.Lista_zelja)
            {
                string autori = "nepoznat autor";
                if (k.Autori != null && k.Autori.Count > 0)
                {
                    List<string> imena = new List<string>();
                    foreach (Autor a in k.Autori)
                    {
                        imena.Add(a.Ime + " " + a.Prezime);
                    }
                    autori = string.Join(", ", imena);
                }

                Console.WriteLine("   - [" + k.ISBN + "] " + k.Naziv + " (" + autori + ") - " + k.Cena);
            }
            Console.WriteLine("--------------------------------------------");
        }

        private void PrikaziKnjige()
        {
            foreach (Knjiga k in sajamService.KnjigaDAO.Prikaz_knjiga())
            {
                Console.WriteLine(k);
            }
        }

        private void DodajKnjigu()
        {
            Console.Write("ISBN: ");
            string isbn = Console.ReadLine();

            if (sajamService.KnjigaDAO.Pronadji_knjigu(isbn) != null)
            {
                Console.WriteLine("Knjiga sa tim ISBN vec postoji.");
                return;
            }

            Console.Write("Naziv: ");
            string naziv = Console.ReadLine();
            Console.Write("Zanr: ");
            string zanr = Console.ReadLine();
            Console.Write("Godina izdanja: ");
            string godina = Console.ReadLine();
            Console.Write("Cena: ");
            int cena = CitajBroj(0);
            Console.Write("Broj strana: ");
            int strana = CitajBroj(1);
            Console.Write("Sifra izdavaca: ");
            string sifraIzdavaca = Console.ReadLine();

            Izdavac izdavac = sajamService.IzdavacDAO.Pronadji_izdavaca(sifraIzdavaca);

            if (izdavac == null)
            {
                Console.WriteLine("Izdavac ne postoji. Prvo dodaj izdavaca.");
                return;
            }

            Knjiga nova = new Knjiga(
                isbn,
                naziv,
                zanr,
                godina,
                cena,
                strana,
                new List<Autor>(),
                izdavac,
                new List<Posetilac>(),
                new List<Posetilac>()
            );
            sajamService.KnjigaDAO.Dodavanje_knjige(nova);
            Console.WriteLine("Knjiga uspesno dodata.");
        }

        private void IzmeniKnjigu()
        {
            Console.Write("ISBN knjige za izmenu: ");
            string isbn = Console.ReadLine();
            Knjiga postojeci = sajamService.KnjigaDAO.Pronadji_knjigu(isbn);
            if (postojeci == null) 
            {
                Console.WriteLine("Knjiga nije pronadjena."); 
                return; 
            }

            Console.WriteLine("Nova sifra izdavaca (Enter ostavlja staru vrednost)");
            string sifra = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(sifra))
            {
                Izdavac izdavac = sajamService.IzdavacDAO.Pronadji_izdavaca(sifra.Trim());
                if (izdavac == null)
                {
                    Console.WriteLine("Izdavac ne postoji.");
                    return;
                }

                if (postojeci.Izdavac != null && postojeci.Izdavac != izdavac)
                {
                    postojeci.Izdavac.Spisak_knjiga.Remove(postojeci);
                }

                if (!izdavac.Spisak_knjiga.Contains(postojeci))
                {
                    izdavac.Spisak_knjiga.Add(postojeci);
                }

                postojeci.Izdavac = izdavac;
            }
            
            

            Console.Write("Novi naziv: ");
            postojeci.Naziv = CitajIzmenu(postojeci.Naziv);
            Console.Write("Nova zanra: ");
            postojeci.Zanra = CitajIzmenu(postojeci.Zanra);
            Console.WriteLine("Unesite godinu izdanja:");
            postojeci.Godina_izdanja = CitajIzmenu(postojeci.Godina_izdanja);
            Console.WriteLine("Nova cena:");
            postojeci.Cena = CitajBrojIzmena(postojeci.Cena);
            Console.Write("Novi broj strana: ");
            postojeci.Broj_strana = CitajBrojIzmena(postojeci.Broj_strana);
            
            

            sajamService.KnjigaDAO.Izmena_knjige(postojeci);
            Console.WriteLine("Knjiga izmenjena.");
        }

        private void ObrisiKnjigu()
        {
            Console.Write("ISBN knjige za brisanje: ");
            string isbn = Console.ReadLine();
            Console.WriteLine(sajamService.Obrisi_knjigu(isbn) ? "Knjiga obrisana." : "Knjiga nije pronadjena.");
        }

        private void DodajNaListuZelja()
        {
            Console.Write("Broj clanske karte posetioca: ");
            string karta = Console.ReadLine();
            Console.Write("ISBN knjige: ");
            string isbn = Console.ReadLine();

            Knjiga k = sajamService.KnjigaDAO.Pronadji_knjigu(isbn);
            if (k == null)
            {
                Console.WriteLine("Knjiga nije pronadjena.");
                return;
            }

            bool uspeh = sajamService.PosetilacDAO.Dodavanje_knjige_na_listu_zelja(karta, k);
            if (uspeh)
            {
                Console.WriteLine("Knjiga dodata na listu zelja.");
            }
            else
            {
                Console.WriteLine("Greska: Posetilac ne postoji ili je knjiga vec na njegovoj listi zelja.");
            }
        }

        private void KupiKnjigu()
        {
            Console.Write("Broj clanske karte posetioca: ");
            string karta = Console.ReadLine();
            Console.Write("ISBN knjige: ");
            string isbn = Console.ReadLine();
            Console.Write("Datum kupovine (npr. 16.11.2026): ");
            string datum = Console.ReadLine();
            Console.Write("Ocena (1-5): ");
            int ocena = CitajBroj(1, 5); //int.Parse(Console.ReadLine());
            Console.Write("Komentar: ");
            string komentar = Console.ReadLine();


            Kupovina nova = sajamService.Kupi_knjigu(karta, isbn, datum, ocena, komentar);

            Console.WriteLine(nova != null ? "Uspesna kupovina." : "Posetilac ili knjiga ne postoje.");
        }

        private void PrikaziAutore()
        {
            foreach (Autor a in sajamService.AutorDAO.Prikaz_autora())
            {
                Console.WriteLine(a);
            }
        }

        private void DodajAutora()
        {
            Console.Write("Broj licne karte: ");
            string lk = Console.ReadLine();

            if (sajamService.AutorDAO.Pronadji_autora(lk) != null)
            {
                Console.WriteLine("Autor sa tom kartom vec postoji");
                return;
            }

            Console.Write("Ime: ");
            string ime = Console.ReadLine();
            Console.Write("Prezime: ");
            string prezime = Console.ReadLine();
            Console.Write("Datum rodjenja: ");
            string datum = Console.ReadLine();
            Adresa adresa = UnesiAdresu();
            Console.Write("Telefon: ");
            string tel = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Godine iskustva: ");
            int god = CitajBroj(0); //int.Parse(Console.ReadLine());

            Autor novi = new Autor(ime, prezime, datum, adresa, tel, email, lk, god, new List<Knjiga>());
            sajamService.AutorDAO.Dodavanje_autora(novi);
            Console.WriteLine("Autor dodat.");
        }

        private void IzmeniAutora()
        {
            Console.Write("Unesite broj licne karte autora za izmenu: ");
            string karta = Console.ReadLine();
            Autor postojeci = sajamService.AutorDAO.Pronadji_autora(karta);
            if (postojeci == null) 
            { 
                Console.WriteLine("Autor nije pronadjen."); 
                return; 
            }
            
            Console.WriteLine("Godine sikustva (Enter ostavlja staru vrednost)");
            int god = CitajBrojIzmena(postojeci.Godine_iskustva, 0);

            // ako je autor sef nekog izdavaca, ne sme pasti ispod 5 godina
            foreach (Izdavac i in sajamService.IzdavacDAO.Prikaz_izdavaca())
            {
                if (i.Sef_izdavaca != null && i.Sef_izdavaca.Broj_licne_karte == postojeci.Broj_licne_karte && god < 5)
                {
                    Console.WriteLine("Autor je sef izdavaca " + i.Naziv + ", mora imati bar 5 godina iskustva.");
                    return;
                }
            }

            

            Console.Write("Novo ime: ");
            postojeci.Ime = CitajIzmenu(postojeci.Ime);
            Console.Write("Novo prezime: ");
            postojeci.Prezime = CitajIzmenu(postojeci.Prezime);
            Console.WriteLine("Unesite novi datum rodjena:");
            postojeci.Datum_rodjenja = CitajIzmenu(postojeci.Datum_rodjenja);
            postojeci.Adresa = IzmeniAdresu(postojeci.Adresa);
            Console.Write("Novi telefon: ");
            postojeci.Telefon = CitajIzmenu(postojeci.Telefon);
            Console.Write("Novi email: ");
            postojeci.Email = CitajIzmenu(postojeci.Email);

            postojeci.Godine_iskustva = god;



            sajamService.AutorDAO.Izmena_autora(postojeci);
            Console.WriteLine("Autor izmenjen.");
            
        }

        private void ObrisiAutora()
        {
            Console.Write("Broj licne karte autora za brisanje: ");
            string karta = Console.ReadLine();
            Console.WriteLine(sajamService.Obrisi_autora(karta) ? "Autor obrisan." : "Autor nije pronadjen.");
        }



        private void DodajKnjiguAutoru()
        {
            Console.Write("Broj licne karte autora: ");
            string lk = Console.ReadLine();
            Console.Write("ISBN knjige: ");
            string isbn = Console.ReadLine();

            Knjiga k = sajamService.KnjigaDAO.Pronadji_knjigu(isbn);
            if (k != null)
            {
                sajamService.AutorDAO.Dodavanje_knjige_autoru(lk, k);
                Console.WriteLine("Knjiga dodata autoru.");
            }
            else
            {
                Console.WriteLine("Knjiga nije pronadjena.");
            }
        }

        private void PrikaziIzdavace()
        {
            foreach (Izdavac i in sajamService.IzdavacDAO.Prikaz_izdavaca())
            {
                Console.WriteLine(i);
            }
        }

        private void DodajIzdavaca()
        {
            Console.Write("Sifra izdavaca: ");
            string sifra = Console.ReadLine();

            if (sajamService.IzdavacDAO.Pronadji_izdavaca(sifra) != null)
            {
                Console.WriteLine("Izdavac sa tom sifrom vec postoji");
                return;
            }

            Console.Write("Naziv: ");
            string naziv = Console.ReadLine();

            Izdavac novi = new Izdavac(sifra, naziv, null);
            sajamService.IzdavacDAO.Dodavanje_izdavaca(novi);
            Console.WriteLine("Izdavac dodat.");
        }

        private void IzmeniIzdavaca()
        {
            Console.Write("Sifra izdavaca za izmenu: ");
            string sifra = Console.ReadLine();
            Izdavac postojeci = sajamService.IzdavacDAO.Pronadji_izdavaca(sifra);
            if (postojeci == null) 
            { 
                Console.WriteLine("Izdavac nije pronadjen."); 
                return; 
            }

            Console.WriteLine("Novi naziv (Enter ostavlja staru vrednost)");
            postojeci.Naziv = CitajIzmenu(postojeci.Naziv);
            sajamService.IzdavacDAO.Izmena_izdavaca(postojeci);

            Console.Write("Broj licne karte novog sefa (Enter ostavlja starog): ");
            string lk = CitajIzmenu(postojeci.Sef_izdavaca != null ? postojeci.Sef_izdavaca.Broj_licne_karte : "");
            if (lk != "" && (postojeci.Sef_izdavaca == null || postojeci.Sef_izdavaca.Broj_licne_karte != lk))
            {
                Autor sef = sajamService.AutorDAO.Pronadji_autora(lk);
                if (sef == null || !sajamService.IzdavacDAO.Postavljanje_sefa_izdavaca(postojeci.Sifra, sef))
                {
                    Console.WriteLine("Sef nije promenjen (autor ne postoji ili ima manje od 5 godina iskustva).");
                }
                    
            }
            Console.WriteLine("Izdavac izmenjen.");
        }

        private void ObrisiIzdavaca()
        {
            Console.Write("Sifra izdavaca za brisanje: ");
            string sifra = Console.ReadLine();
            if (sajamService.Obrisi_izdavaca(sifra, out string poruka))
            {
                Console.WriteLine("Izdavac obrisan.");
            }
            else
            {
                Console.WriteLine(poruka);
            }
        }

        private void PrikaziKnjigeIzdavaca()
        {
            Console.Write("Sifra izdavaca: ");
            string sifra = Console.ReadLine();

            Izdavac i = sajamService.IzdavacDAO.Pronadji_izdavaca(sifra);
            if (i == null)
            {
                Console.WriteLine("Izdavac nije pronadjen.");
                return;
            }

            Console.WriteLine("\n----- KNJIGE IZDAVACA: " + i.Naziv + " (" + i.Sifra + ") ------");

            if (i.Spisak_knjiga == null || i.Spisak_knjiga.Count == 0)
            {
                Console.WriteLine("(nema knjiga)");
                return;
            }

            foreach (Knjiga k in i.Spisak_knjiga)
            {
                Console.WriteLine("   - [" + k.ISBN + "] " + k.Naziv + " (" + k.Godina_izdanja + ", " + k.Zanra + ") - " + k.Cena);
            }
        }

        private void PrikaziAutoreIzdavaca()
        {
            Console.Write("Sifra izdavaca: ");
            string sifra = Console.ReadLine();

            Izdavac i = sajamService.IzdavacDAO.Pronadji_izdavaca(sifra);
            if (i == null)
            {
                Console.WriteLine("Izdavac nije pronadjen.");
                return;
            }

            Console.WriteLine("\n----- AUTORI IZDAVACA: " + i.Naziv + " (" + i.Sifra + ") -----");

            // autori se izvode iz knjiga izdavaca, bez duplikata
            List<Autor> autori = new List<Autor>();
            if (i.Spisak_knjiga != null)
            {
                foreach (Knjiga k in i.Spisak_knjiga)
                {
                    if (k.Autori == null)
                    {
                        continue;
                    }
                    foreach (Autor a in k.Autori)
                    {
                        if (!autori.Contains(a))
                        {
                            autori.Add(a);
                        }
                    }
                }
            }

            if (autori.Count == 0)
            {
                Console.WriteLine("   (nema autora)");
                return;
            }

            foreach (Autor a in autori)
            {
                Console.WriteLine("   - " + a.Ime + " " + a.Prezime + ", " + a.Datum_rodjenja + ", " + a.Adresa + ", " + a.Telefon + ", " + a.Email);
            }
        }


        private void PostaviSefaIzdavaca()
        {
            Console.Write("Sifra izdavaca: ");
            string sifra = Console.ReadLine();
            Console.Write("Broj licne karte autora: ");
            string lk = Console.ReadLine();

            Autor a = sajamService.AutorDAO.Pronadji_autora(lk);
            if (a != null)
            {
                bool uspeh = sajamService.IzdavacDAO.Postavljanje_sefa_izdavaca(sifra, a);
                Console.WriteLine(uspeh ? "Sef izdavaca postavljen." : "Greska: Autor mora imati bar 5 godina iskustva ili izdavac ne postoji.");
            }
            else
            {
                Console.WriteLine("Autor nije pronadjen.");
            }
        }

        private void PosetiociZaAutora()
        {
            Console.Write("Broj licne karte autora: ");
            string lk = Console.ReadLine();
            List<Posetilac> lista = sajamService.Posetioci_za_autora(lk);
            foreach (Posetilac p in lista)
            {
                Console.WriteLine(p);
            }
        }

        private void PosetiociUslovKnjige()
        {
            Console.Write("ISBN prve knjige: ");
            string isbn1 = Console.ReadLine();
            Console.Write("ISBN druge knjige: ");
            string isbn2 = Console.ReadLine();
            List<Posetilac> lista = sajamService.Posetioci_uslov_knjige(isbn1, isbn2);
            foreach (Posetilac p in lista)
            {
                Console.WriteLine(p);
            }
        }

        private void IzmeniKupovinu()
        {
            Console.Write("Broj clanske karte: "); 
            string karta = Console.ReadLine();
            Console.Write("ISBN knjige: "); 
            string isbn = Console.ReadLine();
            Console.Write("Datum kupovine: "); 
            string datum = Console.ReadLine();

            Kupovina k = sajamService.KupovinaDAO.Pronadji_kupovinu(karta, isbn, datum);
            if (k == null) 
            { 
                Console.WriteLine("Kupovina nije pronadjena."); 
                return; 
            }

            int ocena = CitajBrojIzmena( (int)k.Ocena, 1, 5);
            string komentar = CitajIzmenu(k.Komentar);
            sajamService.Izmeni_kupovinu(karta, isbn, datum, ocena, komentar);
            Console.WriteLine("Kupovina izmenjena.");
        }

        private void ObrisiKupovinu()
        {
            Console.Write("Broj clanske karte: ");
            string karta = Console.ReadLine();
            Console.Write("ISBN knjige: ");
            string isbn = Console.ReadLine();
            Console.Write("Datum kupovine: ");
            string datum = Console.ReadLine();
            Console.WriteLine(sajamService.Obrisi_kupovinu(karta, isbn, datum) ? "Kupovina obrisana." : "Kupovina nije pronadjena.");
        }

      
          
    }
}
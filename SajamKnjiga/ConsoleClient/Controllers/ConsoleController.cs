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
                    case "0":
                        radi = false;
                        break;
                    default:
                        Console.WriteLine("Nepoznata opcija!");
                        break;
                }
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
            Console.WriteLine("Posetilac uspesno dodat!");
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

            Console.Write("Novo ime: ");
            postojeci.Ime = Console.ReadLine();
            Console.Write("Novo prezime: ");
            postojeci.Prezime = Console.ReadLine();
            Console.WriteLine("Unesite novu adresu:");
            postojeci.Adresa = UnesiAdresu();
            Console.Write("Novi telefon: ");
            postojeci.Kontakt_telefon = Console.ReadLine();
            Console.Write("Novi email: ");
            postojeci.Email = Console.ReadLine();

            sajamService.PosetilacDAO.Izmena_posetioca(postojeci);
            Console.WriteLine("Posetilac izmenjen!");
        }

        private void ObrisiPosetioca()
        {
            Console.Write("Unesite broj clanske karte za brisanje: ");
            string karta = Console.ReadLine();
            sajamService.PosetilacDAO.Brisanje_posetioca(karta);
            Console.WriteLine("Posetilac obrisan (ako je postojao).");
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
            Console.WriteLine("Knjiga uspesno dodata!");
        }

        private void DodajNaListuZelja()
        {
            Console.Write("Broj clanske karte posetioca: ");
            string karta = Console.ReadLine();
            Console.Write("ISBN knjige: ");
            string isbn = Console.ReadLine();

            Knjiga k = sajamService.KnjigaDAO.Pronadji_knjigu(isbn);
            if (k != null)
            {
                sajamService.PosetilacDAO.Dodavanje_knjige_na_listu_zelja(karta, k);
                Console.WriteLine("Knjiga dodata na listu zelja!");
            }
            else
            {
                Console.WriteLine("Knjiga nije pronadjena.");
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
                Console.WriteLine("Autpr sa tom kartom vec postoji");
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
            Console.WriteLine("Autor dodat!");
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
                Console.WriteLine("Knjiga dodata autoru!");
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
            Console.WriteLine("Izdavac dodat!");
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
                Console.WriteLine(uspeh ? "Sef izdavaca postavljen!" : "Greska: Autor mora imati bar 5 godina iskustva ili izdavac ne postoji.");
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
    }
}
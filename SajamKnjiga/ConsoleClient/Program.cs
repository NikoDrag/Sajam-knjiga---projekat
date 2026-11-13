using System;
using Core.Models;

namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Test Adresa
            Adresa adresa1 = new Adresa("Gogoljeva", "5", "Novi Sad", "Srbija");
            Adresa adresa2 = new Adresa("Knez Mihailova", "12", "Beograd", "Srbija");
            Console.WriteLine("--- ADRESA ---");
            Console.WriteLine(adresa1);

            // 2. Test Autor
            Autor autor1 = new Autor
            {
                Ime = "Milorad",
                Prezime = "Pavić",
                Datum_rodjenja = "15.10.1929.",
                Adresa = "Knez Mihailova 12, Beograd",
                Telefon = "011123456",
                Email = "pavic@gmail.com",
                Broj_licne_karte = "123456789",
                Godine_iskustva = 20
            };

            Autor mladiAutor = new Autor
            {
                Ime = "Petar",
                Prezime = "Petrović",
                Broj_licne_karte = "987654321",
                Godine_iskustva = 2
            };
            Console.WriteLine("\n--- AUTOR ---");
            Console.WriteLine(autor1);

            // 3. Test Izdavac
            Console.WriteLine("\n--- IZDAVAC ---");
            Izdavac izdavac = new Izdavac("IZD-1", "Laguna", autor1);
            izdavac.Spisak_autora.Add(autor1);
            izdavac.Spisak_autora.Add(mladiAutor);
            Console.WriteLine(izdavac);

            try
            {
                izdavac.Sef_izdavaca = mladiAutor;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Uspešno uhvaćena greška: " + ex.Message);
            }

            // 4. Test Posetilac, Knjiga i Kupovina
            Knjiga knjiga = new Knjiga();

            Posetilac posetilac = new Posetilac(
                "Marko",
                "Marković",
                "12.12.2002.",
                adresa1,
                "065543241",
                "marko12@gmail.com",
                "SK-1-2023",
                2026,
                Status_posetioca.Redovan,
                5.0
            );
            posetilac.Lista_zelja.Add(knjiga);

            Kupovina kupovina = new Kupovina(
                posetilac,
                knjiga,
                "13.11.2026.",
                5,
                "Odlična knjiga!"
            );
            posetilac.Spisak_kupljenih_knjiga.Add(kupovina);

            Console.WriteLine("\n--- POSETILAC ---");
            Console.WriteLine(posetilac);

            Console.WriteLine("\n--- KUPOVINA ---");
            Console.WriteLine(kupovina);
        }
    }
}
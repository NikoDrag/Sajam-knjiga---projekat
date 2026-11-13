using System;
using Core.Models;

namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Adresa adresa = new Adresa("Gogoljeva", "5", "Novi Sad", "Srbija");
            Console.WriteLine(adresa);
        }
    }
}
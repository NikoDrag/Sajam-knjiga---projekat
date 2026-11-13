using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Adresa
    {
        public string Ulica 
            { get; set; }
        public string Broj
        { get; set; }
        public string Grad
        { get; set; }
        public string Drzava
        { get; set; }

        public Adresa()
        {
            //prazan konstruktro
        }
        public Adresa(string ulica, string broj, string grad,string drzava)
        {
            //pun
            Ulica = ulica;
            Broj = broj;
            Grad = grad;
            Drzava = drzava;
        }

        public override string ToString()
        {
            return Ulica + " " + Broj + ", " + Grad + ", " + Drzava;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    
    public class Kupovina
    {
        public Posetilac Posetilac
            { get; set; }
        public Knjiga Knjiga
        { get; set; }
        public string Datum_kupovine
        { get; set; }
        public float Ocena
        { get; set; }
        public string Komentar
        { get; set; }

        public Kupovina()
        {

        }
        public Kupovina(Posetilac posetilac, Knjiga knjiga, string datum_kupovine,
            float ocena,string komentar)
        {
            Posetilac = posetilac;
            Knjiga = knjiga;
            Datum_kupovine = datum_kupovine;
            Ocena = ocena;
            Komentar = komentar;
        }

        public override string ToString()
        {
            return Posetilac + ", " + Knjiga + ", " + Datum_kupovine + ", " +
                   Ocena + ", " + Komentar;
        }
    }
}

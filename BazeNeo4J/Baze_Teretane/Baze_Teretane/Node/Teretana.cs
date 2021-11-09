using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baze_Teretane
{
    public class Teretana
    {
        public String id { get; set; }
        public String naziv { get; set; }
        public String lokacija { get; set; }
        public String opis { get; set; }
        public String cena { get; set; }
        public String ocena { get; set; }

        public List<Trener> treneri { get; set; }
        public List<Usluga> usluge { get; set; }

        public List<Korisnik> korisnik { get; set; }

    }
}

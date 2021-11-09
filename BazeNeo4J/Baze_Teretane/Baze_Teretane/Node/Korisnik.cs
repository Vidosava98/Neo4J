using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baze_Teretane
{
    public class Korisnik
    {
        public String id { get; set; }
        public String ime { get; set; }
        public String prezime { get; set; }
        public String nivo { get; set; }
        public String kilogram { get; set; }
        public String visina { get; set; }
        public String bolesti { get; set; }
        public String pol { get; set; }

        public Plan Plan { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Baze_Teretane.Pages
{
    public class DodajKorisnikaModel : PageModel
    {
        private readonly ILogger<DodajKorisnikaModel> _logger;
        BoltGraphClient client;

        [BindProperty]
        public Korisnik NoviKorisnik { get; set; }

        public DodajKorisnikaModel(ILogger<DodajKorisnikaModel> logger)
        {
            client = Manager.GetClient();
            _logger = logger;
        }
        public IActionResult OnGet()
        {

            return Page();
        }

        public IActionResult OnPostDodaj()
        {
            var queryMax = new Neo4jClient.Cypher.CypherQuery("match (n:Korisnik) return MAX(n.id)",
                                             new Dictionary<string, object>(), CypherResultMode.Set);
            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(queryMax).ToList().FirstOrDefault();
            //povecavamo za jedan i dodeljujemo novom korisniku 
            int pom = Int32.Parse(maxId);
            pom++;
            NoviKorisnik.id = pom.ToString();



            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (t:Korisnik{id:'" + NoviKorisnik.id + "', ime:'" + NoviKorisnik.ime + "', prezime:'" + NoviKorisnik.prezime + "', nivo:'" + NoviKorisnik.nivo + "',kilogram:'" + NoviKorisnik.kilogram + "', visina:'" + NoviKorisnik.visina + "',bolesti:'" + NoviKorisnik.bolesti + "',pol:'" + NoviKorisnik.pol + "'})return t", queryDict, CypherResultMode.Set);
            Korisnik korisnik = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query).FirstOrDefault();




            return RedirectToPage("./Admin");
        }
    }
}

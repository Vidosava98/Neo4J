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
    public class AdminModel : PageModel
    {
        private readonly ILogger<AdminModel> _logger;
        BoltGraphClient client;

        [BindProperty]
        public Teretana teretana { get; set; }

        [BindProperty]
        public Trener trener { get; set; }

        [BindProperty]
        public Korisnik korisnik { get; set; }
        [BindProperty]
        public Teretana teretanaIzmena { get; set; }

        [BindProperty]
        public Trener trenerIzmena { get; set; }

        [BindProperty]
        public Korisnik korisnikIzmena { get; set; }
        public AdminModel(ILogger<AdminModel> logger)
        {
            client = Manager.GetClient();
            _logger = logger;
        }
        public IActionResult OnGet()
        {
           
            return Page();
        }
        public IActionResult OnPostObrisiTeretanu()
        {
            Dictionary<string, object> queryDict3 = new Dictionary<string, object>();
            var query3 = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Teretana) where n.id='" + teretana.id + "' DETACH DELETE n", queryDict3, CypherResultMode.Set);
            teretana = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query3).FirstOrDefault();

            return Page();
        }
        public IActionResult OnPostIzmeniTeretanu()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("match (n:Teretana) where n.id='" + teretanaIzmena.id + "' set n.naziv='" + teretanaIzmena.naziv + "' return n", queryDict, CypherResultMode.Set);
            teretanaIzmena = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query).FirstOrDefault();

            return Page();
        }
        public IActionResult OnPostObrisiTrenera()
        {
            Dictionary<string, object> queryDict3 = new Dictionary<string, object>();
            var query3 = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Trener) where n.id='" + trener.id + "' DETACH DELETE n", queryDict3, CypherResultMode.Set);
            trener = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query3).FirstOrDefault();

            return Page();
        }
        public IActionResult OnPostIzmeniTrenera()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("match (n:Trener) where n.id='" + trenerIzmena.id + "' set n.ocena='" + trenerIzmena.ocena + "' return n", queryDict, CypherResultMode.Set);
            trenerIzmena = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query).FirstOrDefault();

            return Page();
        }
        public IActionResult OnPostObrisiKorisnika()
        {
            Dictionary<string, object> queryDict3 = new Dictionary<string, object>();
            var query3 = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Korisnik) where n.id='" + korisnik.id + "' DETACH DELETE n", queryDict3, CypherResultMode.Set);
            korisnik = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query3).FirstOrDefault();

            return Page();
        }
        public IActionResult OnPostIzmeniKorisnika()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("match (n:Korisnik) where n.id='" + korisnikIzmena.id + "' set n.kilogram='" + korisnikIzmena.kilogram + "' return n", queryDict, CypherResultMode.Set);
            korisnikIzmena = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query).FirstOrDefault();

            return Page();
        }
    }
}

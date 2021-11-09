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
    public class DodajTreneraModel : PageModel
    {
        private readonly ILogger<DodajTreneraModel> _logger;
        BoltGraphClient client;

        [BindProperty]
        public Trener NoviTrener { get; set; }
       
        [BindProperty]
        public Teretana teretana { get; set; }
        public DodajTreneraModel(ILogger<DodajTreneraModel> logger)
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
            var queryMax = new Neo4jClient.Cypher.CypherQuery("match (n:Trener) return MAX(n.id)",
                                             new Dictionary<string, object>(), CypherResultMode.Set);
            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(queryMax).ToList().FirstOrDefault();
            //povecavamo za jedan i dodeljujemo novom korisniku 
            int pom = Int32.Parse(maxId);
            pom++;
            NoviTrener.id = pom.ToString();



            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query3 = new Neo4jClient.Cypher.CypherQuery("CREATE (t:Trener{id:'" + NoviTrener.id + "', ime:'" + NoviTrener.ime + "', prezime:'" + NoviTrener.prezime + "', ocena:'" + NoviTrener.ocena + "'})return t", queryDict, CypherResultMode.Set);
            Trener trener = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query3).FirstOrDefault();

            Dictionary<string, object> queryDict3 = new Dictionary<string, object>();
            var query4 = new Neo4jClient.Cypher.CypherQuery("MATCH(a: Trener), (b: Teretana) WHERE b.id = '" + teretana.id + "' AND a.id = '" + NoviTrener.id + "' CREATE(a) -[r: Radi_U]->(b) RETURN b", queryDict3, CypherResultMode.Set);
            Teretana t = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query4).FirstOrDefault();


            return RedirectToPage("./Admin");
        }
    }
}

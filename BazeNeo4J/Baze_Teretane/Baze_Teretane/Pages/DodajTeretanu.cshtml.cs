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
    public class DodajTeretanuModel : PageModel
    {
        private readonly ILogger<DodajTeretanuModel> _logger;
        BoltGraphClient client;

        [BindProperty]
        public Teretana NovaTeretana { get; set; }
        public DodajTeretanuModel(ILogger<DodajTeretanuModel> logger)
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
            var queryMax = new Neo4jClient.Cypher.CypherQuery("match (n:Teretana) return MAX(n.id)",
                                             new Dictionary<string, object>(), CypherResultMode.Set);
            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(queryMax).ToList().FirstOrDefault();
            //povecavamo za jedan i dodeljujemo novom korisniku 
            int pom = Int32.Parse(maxId);
            pom++;
            NovaTeretana.id = pom.ToString();



            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (t:Teretana{id:'" + NovaTeretana.id + "', naziv:'" + NovaTeretana.naziv + "', lokacija:'" + NovaTeretana.lokacija + "', ocena:'" + NovaTeretana.ocena + "',opis:'" + NovaTeretana.opis + "',cena:'" + NovaTeretana.cena + "'})return t", queryDict, CypherResultMode.Set);
            Teretana teretana = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query).FirstOrDefault();




            return RedirectToPage("./Admin");
        }
    }
}

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
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        BoltGraphClient client;
        public List<Korisnik> korisniciTeretane { get; set; }


        public IndexModel(ILogger<IndexModel> logger)
        {
            client = Manager.GetClient();
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            string unetNazivTeretane = "Teretana1";

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("unetNazivTeretane", unetNazivTeretane);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Korisnik) RETURN n", queryDict, CypherResultMode.Set);

            korisniciTeretane = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query).ToList();

            return Page();
        }
    }
}

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
    public class PlanModel : PageModel
    {
        private readonly ILogger<PlanModel> _logger;
        BoltGraphClient client;
        [BindProperty]
        public String Preporuka { get; set; } 
        [BindProperty]
        public Trener trener { get;  set; }

        [BindProperty]
        public Korisnik korisnik {get;set;}
        public PlanModel(ILogger<PlanModel> logger)
        {
            client = Manager.GetClient();
            _logger = logger;
        }
        public void OnGet()
        {

        }
        public IActionResult OnPost(string id, string trenerid)
        {
            //ter.id = (id).ToString();
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Korisnik) where n.id='" + id + "' return n", queryDict, CypherResultMode.Set);
            korisnik = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query).FirstOrDefault();

            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Trener) where n.id='" + trenerid + "' return n", queryDict1, CypherResultMode.Set);
            trener = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query1).FirstOrDefault();


            return Page();
        }
        public IActionResult OnPostPosalji(string id, string trenerid)
        {
          
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH( t: Trener { id: '" + trenerid + "'}), (k:Korisnik {id: '" + id + "'}) WITH t, k CREATE(t)-[:PREPORUCUJE_PLAN {opisplana:'" + Preporuka + "'}]->(k) return t", queryDict, CypherResultMode.Set);
            Trener trener1 = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query).FirstOrDefault();
            //da se vrati na prethodnu stranicu
            return Page();
        }
    }
}
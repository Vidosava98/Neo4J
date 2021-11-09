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
    public class JesamClanModel : PageModel
    {
        private readonly ILogger<JesamClanModel> _logger;
        BoltGraphClient client;
        [BindProperty]
        public String KorisnikId { get; set; }
        [BindProperty]
        public Teretana ter { get; set; }
        [BindProperty]
        public bool ok { get; set; }
        [BindProperty]
        public Korisnik Clan { get; set; }
        public JesamClanModel(ILogger<JesamClanModel> logger)
        {
            client = Manager.GetClient();
            _logger = logger;
        }
        public void OnGet()
        {

        }
        public IActionResult OnPost(string terr)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Teretana) where n.id='" + terr + "' return n", queryDict, CypherResultMode.Set);
            ter = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query).FirstOrDefault();
     
            return Page();
        }
        public IActionResult OnPostPrikaziPlan(string teretana)
        {
            ok = true;
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Korisnik) where n.id='" + KorisnikId + "' return n", queryDict, CypherResultMode.Set);
            Clan = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query).FirstOrDefault();

            Dictionary<string, object> queryDict44 = new Dictionary<string, object>();
            var query44 = new Neo4jClient.Cypher.CypherQuery("MATCH(t: Trener )-[r] - (k: Korisnik { id: '"+KorisnikId+"' } ) WHERE EXISTS(r.opisplana) RETURN r", queryDict44, CypherResultMode.Set);
            Clan.Plan = ((IRawGraphClient)client).ExecuteGetCypherResults<Plan>(query44).FirstOrDefault();

            //MATCH(t: Trener { id: '1' })-[r] - (k: Korisnik { id: '1' } ) WHERE EXISTS(r.opisplana) RETURN r.opisplana
            //ter.id = (id).ToString();
            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Teretana) where n.id='" + teretana + "' return n", queryDict1, CypherResultMode.Set);
            ter = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query1).FirstOrDefault();

            // treba da ucita plan tog korisnika, samo sto plan kao node ne moze da bude property veze 
            //Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            //var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Korisnik) where n.id='" + id + "' return n", queryDict1, CypherResultMode.Set);
            //Clan = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query1).FirstOrDefault();

            return Page();
        }
    }
}
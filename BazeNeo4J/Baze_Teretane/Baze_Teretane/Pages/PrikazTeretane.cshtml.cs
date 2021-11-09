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
    public class PrikazTeretaneModel : PageModel
    {
        private readonly ILogger<PrikazTeretaneModel> _logger;
        BoltGraphClient client;
        [BindProperty]
        public Teretana ter { get; set; }

        [BindProperty]
        public String KorisnikId { get; set; }
        [BindProperty]
        public Trener trenerOcena { get; set; }
        [BindProperty]
        public Teretana teretanaOcena { get; set; }
        [BindProperty]
        public Trener trener { get; set; }
        
        [BindProperty]
        public List<Trener> SviTreneri { get; set; }
        [BindProperty]
        public List<Usluga> SveUsluge { get; set; }
        [BindProperty]
        public Korisnik Clan { get; set; }
        public PrikazTeretaneModel(ILogger<PrikazTeretaneModel> logger)
        {
            client = Manager.GetClient();
            _logger = logger;
        }
        public IActionResult OnPost(int id)
        {
            //ter.id = (id).ToString();
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Teretana) where n.id='" + id + "' return n", queryDict, CypherResultMode.Set);
            ter = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query).FirstOrDefault();

            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:RADI_U]->(m) where m.id= '" + ter.id + "' RETURN n", queryDict1, CypherResultMode.Set);
            SviTreneri = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query1).ToList();

            Dictionary<string, object> queryDict2 = new Dictionary<string, object>();
            var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:NUDI_USLUGU]->(m) where n.id= '" + ter.id + "' RETURN m", queryDict2, CypherResultMode.Set);
            SveUsluge = ((IRawGraphClient)client).ExecuteGetCypherResults<Usluga>(query2).ToList();

            return Page();
        }

        public IActionResult OnGet(int id)
        {
            //ter.id = (id).ToString();
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Teretana) where n.id='" + ter.id + "' return n", queryDict, CypherResultMode.Set);
            ter = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query).FirstOrDefault();

            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:RADI_U]->(m) where m.id= '" + ter.id + "' RETURN n", queryDict1, CypherResultMode.Set);
            SviTreneri = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query1).ToList();

            Dictionary<string, object> queryDict2 = new Dictionary<string, object>();
            var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:NUDI_USLUGU]->(m) where n.id= '" + ter.id + "' RETURN m", queryDict2, CypherResultMode.Set);
            SveUsluge = ((IRawGraphClient)client).ExecuteGetCypherResults<Usluga>(query2).ToList();

            return Page();
        }
        
        
        //public IActionResult OnPostPrikaziPlan(string teretana)
        //{
        //    ok = true;
        //    Dictionary<string, object> queryDict = new Dictionary<string, object>();
        //    var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Korisnik) where n.id='" + KorisnikId + "' return n", queryDict, CypherResultMode.Set);
        //    Clan = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query).FirstOrDefault();
        //    //ter.id = (id).ToString();
        //    Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
        //    var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Teretana) where n.id='" + teretana + "' return n", queryDict1, CypherResultMode.Set);
        //    ter = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query1).FirstOrDefault();

        //    // treba da ucita plan tog korisnika, samo sto plan kao node ne moze da bude property veze 
        //    //Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
        //    //var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Korisnik) where n.id='" + id + "' return n", queryDict1, CypherResultMode.Set);
        //    //Clan = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query1).FirstOrDefault();

        //    return Page();
        //}

        public IActionResult OnPostOceniTrenera(int id)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("match(n: Trener) where n.id = '" + id + "' return n", queryDict, CypherResultMode.Set);
            trener = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query).FirstOrDefault();

            Dictionary<string, object> queryDict2 = new Dictionary<string, object>();
            var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:RADI_U]->(m) where n.id= '" + id + "' RETURN m", queryDict2, CypherResultMode.Set);
            ter = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query2).FirstOrDefault();

            var queryMax = new Neo4jClient.Cypher.CypherQuery("match (n:Korisnik) return MAX(n.id)",
                                             new Dictionary<string, object>(), CypherResultMode.Set);
            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(queryMax).ToList().FirstOrDefault();

            int pom = Int32.Parse(maxId);

            trenerOcena.ocena = (Int32.Parse(trener.ocena + trenerOcena.ocena) / pom).ToString();

            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("match (n:Trener) where n.id='" + id + "' set n.ocena='" + trenerOcena.ocena + "' return n", queryDict1, CypherResultMode.Set);
            trenerOcena = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query1).FirstOrDefault();


            return RedirectToPage("./Korisnik");
        }

        public IActionResult OnPostOceniTeretanu(int id)
        {

            Dictionary<string, object> queryDict2 = new Dictionary<string, object>();
            var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:RADI_U]->(m) where n.id= '" + id + "' RETURN m", queryDict2, CypherResultMode.Set);
            ter = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query2).FirstOrDefault();

            var queryMax = new Neo4jClient.Cypher.CypherQuery("match (n:Korisnik) return MAX(n.id)",
                                             new Dictionary<string, object>(), CypherResultMode.Set);
            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(queryMax).ToList().FirstOrDefault();

            int pom = Int32.Parse(maxId);

            ter.ocena = (Int32.Parse(ter.ocena + teretanaOcena.ocena) / pom).ToString();

            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("match (n:Teretana) where n.id='" + ter.id + "' set n.ocena='" + teretanaOcena.ocena + "' return n", queryDict1, CypherResultMode.Set);
            teretanaOcena = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query1).FirstOrDefault();


            return RedirectToPage("./Korisnik");
        }
    }

        
    }


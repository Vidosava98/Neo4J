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
    public class UclaniSeModel : PageModel
    {
        private readonly ILogger<UclaniSeModel> _logger;
        BoltGraphClient client;
        [BindProperty]
        public int IdTeretane { get; set; }
        [BindProperty]
        public int IdNovogKorisnika { get; set; }
        [BindProperty]
        public Korisnik NoviKorisnik { get; set; }
        [BindProperty]
        public List<Usluga> SveUsluge { get; set; }
        [BindProperty]
        public List<Trener> SviTreneri { get; set; }
        [BindProperty]
        public int idTrenera { get; set; }
        [BindProperty]
        public int idUsluge { get; set; }
        [BindProperty]
        public bool uspesno { get; set; }
        public UclaniSeModel(ILogger<UclaniSeModel> logger)
        {
            client = Manager.GetClient();
            _logger = logger;
        }
        public IActionResult OnGet()
        {
            //nalazimo teretanu da bi napravili vezu od korisnika ka teretani 
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Teretana) where n.id='" + IdTeretane + "' return n", queryDict, CypherResultMode.Set);
            Teretana ter = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query).FirstOrDefault();
            //trazimo poslednji upisan id za ovu labelu
            var queryMax = new Neo4jClient.Cypher.CypherQuery("match (n:Korisnik) return MAX(n.id)",
                                              new Dictionary<string, object>(), CypherResultMode.Set);
            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(queryMax).ToList().FirstOrDefault();
            //povecavamo za jedan i dodeljujemo novom korisniku 
            int pom = Int32.Parse(maxId);
            pom++;
            NoviKorisnik.id = pom.ToString();
            //Pravimo vezu o korisnika ka teretani 
            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH(a: Korisnik), (b: Teretana) WHERE a.id = '" + NoviKorisnik.id + "' AND b.id = '" + IdTeretane + "' CREATE(a) -[r: CLAN]->(b) RETURN a", queryDict1, CypherResultMode.Set);
            Korisnik k = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query1).FirstOrDefault();

            //trazimo sve usluge ove teretane zbog select
            Dictionary<string, object> queryDict2 = new Dictionary<string, object>();
            var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:NUDI_USLUGU]->(m) where n.id= '" + ter.id + "' RETURN m", queryDict2, CypherResultMode.Set);
            SveUsluge = ((IRawGraphClient)client).ExecuteGetCypherResults<Usluga>(query2).ToList();

            //trazimo sve trenere zbog select
            Dictionary<string, object> queryDict3 = new Dictionary<string, object>();
            var query3 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:RADI_U]->(m) where m.id= '" + ter.id + "' RETURN n", queryDict3, CypherResultMode.Set);
            SviTreneri = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query3).ToList();
            return Page();
           
        }
        public IActionResult OnPost(int id)
        {
            IdTeretane = id;
          

            //trazimo sve usluge ove teretane zbog select
            Dictionary<string, object> queryDict2 = new Dictionary<string, object>();
            var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:NUDI_USLUGU]->(m) where n.id= '" + id + "' RETURN m", queryDict2, CypherResultMode.Set);
            SveUsluge = ((IRawGraphClient)client).ExecuteGetCypherResults<Usluga>(query2).ToList();

            //trazimo sve trenere zbog select
            Dictionary<string, object> queryDict3 = new Dictionary<string, object>();
            var query3 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:RADI_U]->(m) where m.id= '" + id + "' RETURN n", queryDict3, CypherResultMode.Set);
            SviTreneri = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query3).ToList();
            return Page();
        }
        public  IActionResult OnPostUclani(string id)
        {
            //trazimo poslednji upisan id za ovu labelu
            var queryMax = new Neo4jClient.Cypher.CypherQuery("match (n:Korisnik) return MAX(n.id)",
                                              new Dictionary<string, object>(), CypherResultMode.Set);
            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(queryMax).ToList().FirstOrDefault();
            //povecavamo za jedan i dodeljujemo novom korisniku 
            int pom = Int32.Parse(maxId);
            pom = pom+1;
            NoviKorisnik.id = pom.ToString();
            //prvo unosim podatke o korisniku u bazu 
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("CREATE(n: Korisnik { id: '" + NoviKorisnik.id + "' })" +
                " SET n.ime = '" + NoviKorisnik.ime + "', n.prezime = '" + NoviKorisnik.prezime + "', n.kilogram='" + NoviKorisnik.kilogram + "', n.nivo= '" + NoviKorisnik.nivo+ "', n.visina= '" + NoviKorisnik.visina + "', n.bolesti= '" + NoviKorisnik.bolesti + "', n.pol= '" + NoviKorisnik.pol + "'  " +
                "RETURN n", queryDict, CypherResultMode.Set);
            Korisnik korisnik = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query).FirstOrDefault();

            //Pravimo vezu od korisnika ka teretani 
            Dictionary<string, object> queryDict12 = new Dictionary<string, object>();
            var query12 = new Neo4jClient.Cypher.CypherQuery("MATCH(a: Korisnik), (b: Teretana) WHERE a.id = '" + NoviKorisnik.id + "' AND b.id = '" + id + "' CREATE(a) -[r: CLAN]->(b) RETURN a", queryDict12, CypherResultMode.Set);
            Korisnik k = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query12).FirstOrDefault();

            //izabrani trener veza sa korisnikom
            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH(k: Korisnik { id: '" + NoviKorisnik.id + "'}), (t: Trener { id: '" + idTrenera + "'}) WITH k, t CREATE(k) -[:VEZBA_SA]->(t) return k", queryDict1, CypherResultMode.Set);
            Korisnik Kor = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query1).FirstOrDefault();
            //veza usluga korisnik
            Dictionary<string, object> queryDict2 = new Dictionary<string, object>();
            var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH (k:Korisnik {id: '" + NoviKorisnik.id + "'}), (u:Usluga {id: '" + idUsluge + "'}) WITH k, u CREATE (k)-[:KORISTI]->(u) return k", queryDict2, CypherResultMode.Set);
            Korisnik a = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query2).FirstOrDefault();
            uspesno = true;

            return Page();
        }

    }
}
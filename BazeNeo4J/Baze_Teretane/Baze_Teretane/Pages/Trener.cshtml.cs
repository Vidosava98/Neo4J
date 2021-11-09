using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Neo4jClient;
using Neo4jClient.Cypher;


namespace Baze_Teretane.Pages
{
    public class TrenerModel : PageModel
    {
        private readonly ILogger<TrenerModel> _logger;
        BoltGraphClient client;
        [BindProperty]
        public Trener trener { get; set; }
        [BindProperty]
        public string IDTrenera { get; set; }
        [BindProperty]
        public String preporuka { get; set; }
        [BindProperty]
        public int idKorisnika { get; set; }
        [BindProperty]
        public List<Korisnik> RadiSaKorisnicima { get; set; }
        public List<Usluga> usluge { get; set; }
        public Teretana teretana { get; set; }
        [BindProperty]
        public Usluga novaUsluga { get; set; }
        [BindProperty]
        public string uslugaNova { get; set; }
        [BindProperty]
        public int IdobrisiUslugu { get; set; }
        public List<Korisnik> korisnici { get; set; }
        public Usluga uslugaObrisi { get; set; }
        [BindProperty]
        public Usluga updateUsluga { get; set; }
        [BindProperty]
        public Korisnik obrisiKorisnika { get; set; }
        [BindProperty]
        public string poruka { get; set; }
        [BindProperty]
        public bool uslugaDodata { get; set; }
        [BindProperty]
        public bool uslugaObrisana { get; set; }
        public TrenerModel(ILogger<TrenerModel> logger)
        {
            client = Manager.GetClient();
            _logger = logger;
        }
        public IActionResult OnGet()
        {
            uslugaDodata = false;
            uslugaObrisana = false;
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("match(n: Trener) where n.id = '" + IDTrenera + "' return n", queryDict, CypherResultMode.Set);
            trener = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query).FirstOrDefault();

            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:VEZBA_SA]->(m) where m.id= '" + IDTrenera + "' RETURN n", queryDict1, CypherResultMode.Set);
            RadiSaKorisnicima = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query1).ToList();

            return Page();
        }
        public IActionResult OnPost()  
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("match(n: Trener) where n.id = '"+ IDTrenera+"' return n", queryDict, CypherResultMode.Set);
            trener = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query).FirstOrDefault();

            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:VEZBA_SA]->(m) where m.id= '" + IDTrenera + "' RETURN n", queryDict1, CypherResultMode.Set);
            RadiSaKorisnicima = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query1).ToList();

            return Page();
        }
        
        public IActionResult OnPostObrisiPlan(string id, string idtrenera)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH p=(t:Trener{id:'"+idtrenera+"'})-[r:PREPORUCUJE_PLAN]->(k:Korisnik{id:'"+id+"'}) DELETE r", queryDict, CypherResultMode.Set);
            Trener trener1 = ((IRawGraphClient)client).ExecuteGetCypherResults<Trener>(query).FirstOrDefault();

            return Page();
        }
        public IActionResult OnPostSviKorisniciTeretane()
        {
            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:RADI_U]->(m) where n.id= '" + IDTrenera + "' RETURN m", queryDict1, CypherResultMode.Set);
            teretana = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query1).FirstOrDefault();

            Dictionary<string, object> queryDict2 = new Dictionary<string, object>();
            var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:CLAN]->(m) where m.id= '" + teretana.id + "' RETURN n", queryDict2, CypherResultMode.Set);
            korisnici = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query2).ToList();

            return Page();
        }
        public IActionResult OnPostSveUslugeTeretane()
        {
            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:RADI_U]->(m) where n.id= '" + IDTrenera + "' RETURN m", queryDict1, CypherResultMode.Set);
            teretana = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query1).FirstOrDefault();

            Dictionary<string, object> queryDict2 = new Dictionary<string, object>();
            var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:NUDI_USLUGU]->(m) where n.id= '" + teretana.id + "' RETURN m", queryDict2, CypherResultMode.Set);
            usluge = ((IRawGraphClient)client).ExecuteGetCypherResults<Usluga>(query2).ToList();

            return Page();
        }
        public IActionResult OnPostDodajUslugu()
        {
            uslugaDodata = true;
            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:RADI_U]->(m) where n.id= '" + IDTrenera + "' RETURN m", queryDict1, CypherResultMode.Set);
            teretana = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query1).FirstOrDefault();

            var queryMax = new Neo4jClient.Cypher.CypherQuery("match (n:Usluga) return MAX(n.id)",
                                             new Dictionary<string, object>(), CypherResultMode.Set);
            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(queryMax).ToList().FirstOrDefault();

            int pom = Int32.Parse(maxId);
            pom++;
            novaUsluga.id = pom.ToString();
            //novaUsluga.nazivusluge = naziv;
            Dictionary<string, object> queryDict2 = new Dictionary<string, object>();
            var query2 = new Neo4jClient.Cypher.CypherQuery("CREATE (u:Usluga{id:'" + novaUsluga.id + "',nazivusluge:'" + novaUsluga.nazivusluge + "'}) return u", queryDict2, CypherResultMode.Set);
            Usluga korisnik = ((IRawGraphClient)client).ExecuteGetCypherResults<Usluga>(query2).FirstOrDefault();

            Dictionary<string, object> queryDict3 = new Dictionary<string, object>();
            var query3 = new Neo4jClient.Cypher.CypherQuery("MATCH(a: Teretana), (b: Usluga) WHERE a.id = '" + teretana.id + "' AND b.id = '" + novaUsluga.id + "' CREATE(a) -[r: NUDI_USLUGU]->(b) RETURN b", queryDict3, CypherResultMode.Set);
            Usluga k = ((IRawGraphClient)client).ExecuteGetCypherResults<Usluga>(query3).FirstOrDefault();
            return Page();

        }
        public IActionResult OnPostObrisiUslugu()
        {
            uslugaObrisana = true;
            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:RADI_U]->(m) where n.id= '" + IDTrenera + "' RETURN m", queryDict1, CypherResultMode.Set);
            teretana = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query1).FirstOrDefault();

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("match(n: Usluga) where n.id = '" + IdobrisiUslugu + "' return n", queryDict, CypherResultMode.Set);
            uslugaObrisi = ((IRawGraphClient)client).ExecuteGetCypherResults<Usluga>(query).FirstOrDefault();

            if (uslugaObrisi != null)
            {
                Dictionary<string, object> queryDict2 = new Dictionary<string, object>();
                var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Teretana)-[r]->(m:Usluga) WHERE m.id = '" + uslugaObrisi.id + "' DELETE r", queryDict2, CypherResultMode.Set);
                Usluga korisnik = ((IRawGraphClient)client).ExecuteGetCypherResults<Usluga>(query2).FirstOrDefault();

                Dictionary<string, object> queryDict3 = new Dictionary<string, object>();
                var query3 = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Usluga) WHERE n.id = '" + uslugaObrisi.id + "' DELETE n", queryDict3, CypherResultMode.Set);
                Usluga k = ((IRawGraphClient)client).ExecuteGetCypherResults<Usluga>(query3).FirstOrDefault();

            }
            return Page();
        }
        public IActionResult OnPostIzmeniUslugu()
        {

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query = new Neo4jClient.Cypher.CypherQuery("match (n:Usluga) where n.id='" + updateUsluga.id + "' set n.nazivusluge='" + updateUsluga.nazivusluge + "' return n", queryDict, CypherResultMode.Set);
            uslugaObrisi = ((IRawGraphClient)client).ExecuteGetCypherResults<Usluga>(query).FirstOrDefault();

            return Page();
        }
        public IActionResult OnPostObrisiKorisnika()
        {
            Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH p=(n)-[r:VEZBA_SA]->(m) where n.id= '" + obrisiKorisnika.id + "' RETURN n", queryDict1, CypherResultMode.Set);
            Korisnik k = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query1).FirstOrDefault();

            if (k == null || IDTrenera == null)
            {
                poruka = "Izabrani id ne pripada Vasem korisniku, nije moguce izvrsiti brisanje!";
                return Page();
            }


            Dictionary<string, object> queryDict3 = new Dictionary<string, object>();
            var query3 = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Korisnik) where n.id='" + obrisiKorisnika.id + "' DETACH DELETE n", queryDict3, CypherResultMode.Set);
            korisnici = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query3).ToList();
            poruka = "Korisnik je obrisan!";

            return Page();
        }
    }
}

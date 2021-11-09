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
    public class KorisnikModel : PageModel
    {
        private readonly ILogger<KorisnikModel> _logger;
        BoltGraphClient client;
        public List<Teretana> SveTeretane { get; set; }
        public List<Teretana> ter { get; set; }

        public KorisnikModel(ILogger<KorisnikModel> logger)
        {
            client = Manager.GetClient();
            _logger = logger;
        }
        public IActionResult OnGet()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();


            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Teretana) RETURN n", queryDict, CypherResultMode.Set);

            SveTeretane = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query).ToList();
            return Page();
        }

        public IActionResult OnPost()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();


            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Teretana) RETURN n", queryDict, CypherResultMode.Set);

            SveTeretane = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query).ToList();
            return Page();
        }
        //public async Task<IActionResult> OnPostIzaberiAsync()
        //{
        //    int id = 2;
        //    Dictionary<string, object> queryDict = new Dictionary<string, object>();


        //    var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Teretana) where n.id='"+id+"' return n", queryDict, CypherResultMode.Set);

        //    ter = ((IRawGraphClient)client).ExecuteGetCypherResults<Teretana>(query).ToList();
        //    return RedirectToPage();
        //}
    }
}

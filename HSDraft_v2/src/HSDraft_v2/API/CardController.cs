using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using HSDraft_v2.Models;
using HSDraft_v2.BusinessLogic;
using System.IO;
using Microsoft.Framework.Runtime;

namespace HSDraft_v2.API
{
    [Route("api/[controller]")]
    public class CardController : Controller
    {
        private string cardBasePath;

        private readonly CardAppContext _dbcontext;

        public CardController(IApplicationEnvironment env, CardAppContext dbcontext)
        {
            cardBasePath = Path.Combine(env.ApplicationBasePath, @"Files\CardBase.json");
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public IEnumerable<Card> Get()
        {
            //return FileReader.GetCardBase(cardBasePath);
            return _dbcontext.Cards;
        }           
    }
}

using HSDraft_v2.Enums;
using HSDraft_v2.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HSDraft_v2.BusinessLogic
{
    public static  class FileReader
    {
        public static List<Card> GetCardBase(string filepath)
        {
            var cards = new List<Card>();
            using (FileStream fs = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader file = new StreamReader(fs))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                cards = serializer.Deserialize<List<Card>>(reader);
            }

            return cards.Where(x => x.Rarity != Rarity.None && AcceptedType(x)).ToList();
        }

        private static bool AcceptedType(Card card)
        {
            return card.Type == Cardtype.Minion || card.Type == Cardtype.Spell || card.Type == Cardtype.Weapon;
        }
    }
}

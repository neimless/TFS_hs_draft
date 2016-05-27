using HearthstoneDraft.Enums;
using HearthstoneDraft.Models.CardBase;
using HearthstoneDraft.Models.Draft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.Hosting;
using System.Linq;
using System.Collections.Generic;

namespace HearthstoneDraft.Logic
{
    public static class FileDatabase
    {
        private static string Cardbasefile = HostingEnvironment.MapPath(@"~\Files\CardBase.json");

        public static void RemoveCardFromCardbase(string id)
        {
            var cardbase = RetrieveCardbase();
            cardbase.Commons.Remove(cardbase.Commons.FirstOrDefault(x => x.Id == id));
            cardbase.Classcards.Remove(cardbase.Classcards.FirstOrDefault(x => x.Id == id));
            cardbase.Rares.Remove(cardbase.Rares.FirstOrDefault(x => x.Id == id));
            cardbase.Epics.Remove(cardbase.Epics.FirstOrDefault(x => x.Id == id));
            cardbase.Legendaries.Remove(cardbase.Legendaries.FirstOrDefault(x => x.Id == id));
            SaveCardbase(cardbase);
        }

        public static void AddCardToCardbase(Card card)
        {
            var cardbase = RetrieveCardbase();

            switch (card.Rarity)
            {
                case Rarity.Common:
                    cardbase.Commons.Add(card);
                    break;
                case Rarity.Rare:
                    cardbase.Rares.Add(card);
                    break;
                case Rarity.Epic:
                    cardbase.Epics.Add(card);
                    break;
                case Rarity.Legendary:
                    cardbase.Legendaries.Add(card);
                    break;
                default:
                    break;
            }

            SaveCardbase(cardbase);            
        }

        public static CardBase RetrieveCardbase()
        {
            WaitUntilFileIsReady(Cardbasefile);
            var cards = new List<Card>();
            using (FileStream fs = File.Open(Cardbasefile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader file = new StreamReader(fs))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                cards = serializer.Deserialize<List<Card>>(reader);
            }

            return SortCardsToCardBase(cards);
        }

        public static void SaveDraftFile(Draft draft)
        {
            var filename = HostingEnvironment.MapPath(@"~\Files\SavedDraft_" + draft.Id + ".json");
            using (FileStream fs = File.Open(filename, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, draft);
            }
        }

        public static List<string> GetSavedFilenames()
        {
            var result = new List<string>();
            var path = HostingEnvironment.MapPath(@"~\Files\");
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                if (file.Contains("SavedDraft_"))
                {
                    result.Add(file);
                }                
            }
            return result;
        }
         
        private static CardBase SortCardsToCardBase(List<Card> cards)
        {
            var result = new CardBase();
            result.Commons = cards.Where(x => AcceptedType(x) && (x.Rarity == Rarity.Common || x.Rarity == Rarity.Free) && x.PlayerClass == PlayerClass.None).OrderBy(x => x.Name).ToList();
            result.Classcards = cards.Where(x => AcceptedType(x) && (x.Rarity == Rarity.Common || x.Rarity == Rarity.Free) && x.PlayerClass != PlayerClass.None).OrderBy(x => x.Name).ToList();
            result.Rares = cards.Where(x => AcceptedType(x) && x.Rarity == Rarity.Rare).OrderBy(x => x.Name).ToList();
            result.Epics = cards.Where(x => AcceptedType(x) && x.Rarity == Rarity.Epic).OrderBy(x => x.Name).ToList();
            result.Legendaries = cards.Where(x => AcceptedType(x) && x.Rarity == Rarity.Legendary).OrderBy(x => x.Name).ToList();
            return result;
        }

        private static bool AcceptedType(Card card)
        {
            return card.Type == Cardtype.Minion || card.Type == Cardtype.Spell || card.Type == Cardtype.Weapon;
        }

        private static void SaveCardbase(CardBase cb)
        {
            WaitUntilFileIsReady(Cardbasefile);
            using (FileStream fs = File.Open(Cardbasefile, FileMode.Open, FileAccess.Write, FileShare.Read))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, cb);
            }
        }        

        private static void WaitUntilFileIsReady(string filename)
        {
            while(!IsFileReady(filename))
            { }
            return;
        }

        private static bool IsFileReady(string filename)
        {
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (inputStream.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
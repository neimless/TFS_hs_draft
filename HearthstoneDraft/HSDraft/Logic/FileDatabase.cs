using HSDraft.Enums;
using HSDraft.Models.CardBase;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Web.Hosting;
using System.Linq;
using System.Collections.Generic;
using HSDraft.Logic.IdentityDb;
using HSDraft.Models.History;

namespace HSDraft.Logic
{
    public static class FileDatabase
    {
        private static string Cardbasefile = HostingEnvironment.MapPath(@"~\Files\CardBase.json");
        private static string Userfile = HostingEnvironment.MapPath(@"~\Files\UserFile.json");
        private static string UserDbFile = HostingEnvironment.MapPath(@"~\Files\UserDb.json");
        private static string RoleDbFile = HostingEnvironment.MapPath(@"~\Files\RoleDb.json");
        private static string ClaimsDbFile = HostingEnvironment.MapPath(@"~\Files\ClaimsDb.json");
        private static string LoginDbFile = HostingEnvironment.MapPath(@"~\Files\UserLoginDb.json");
        private static string DraftHistoryFile = HostingEnvironment.MapPath(@"~\Files\DraftHistory.json");

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

        public static List<Card> GetAllCards()
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

            return cards.Where(x => AcceptedType(x)).ToList();
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

        public static History GetHistoryDbForPlayer(string name)
        {
            var result = new History();
            using (FileStream fs = File.Open(DraftHistoryFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader file = new StreamReader(fs))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                result = serializer.Deserialize<History>(reader);
            }

            var parsedResult = new History();
            parsedResult.Drafts = result.Drafts.Where(x => x.Players.Any(y => y.Name == name)).ToList();

            return parsedResult;
        }

        public static History GetHistoryDb()
        {
            var result = new History();
            using (FileStream fs = File.Open(DraftHistoryFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader file = new StreamReader(fs))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                result = serializer.Deserialize<History>(reader);
            }

            return result;
        }

        public static void UpdateDraftHistory(History history)
        {
            var existingHistory = GetHistoryDb();
            existingHistory.Drafts.AddRange(history.Drafts);
            existingHistory.DraftIds.AddRange(history.DraftIds);
            using (FileStream fs = File.Open(DraftHistoryFile, FileMode.Open, FileAccess.Write, FileShare.Read))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, existingHistory);
            }
        }

        public static void ResetDraftHistory()
        {
            var hist = new History();
            using (FileStream fs = File.Open(DraftHistoryFile, FileMode.Truncate, FileAccess.Write, FileShare.Read))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, hist);
            }
        }

        public static UserList GetUserfile()
        {
            var result = new UserList();
            using (FileStream fs = File.Open(Userfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader file = new StreamReader(fs))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                result = serializer.Deserialize<UserList>(reader);
            }

            return result;
        }

        public static UserFile GetUserDb()
        {
            var result = new UserFile();
            using (FileStream fs = File.Open(UserDbFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader file = new StreamReader(fs))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                result = serializer.Deserialize<UserFile>(reader);
            }

            return result;
        }

        public static UserLoginFile GetUserLoginDb()
        {
            var result = new UserLoginFile();
            using (FileStream fs = File.Open(LoginDbFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader file = new StreamReader(fs))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                result = serializer.Deserialize<UserLoginFile>(reader);
            }

            return result;
        }

        public static ClaimsFile GetClaimsDb()
        {
            var result = new ClaimsFile();
            using (FileStream fs = File.Open(ClaimsDbFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader file = new StreamReader(fs))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                result = serializer.Deserialize<ClaimsFile>(reader);
            }

            return result;
        }

        public static ClaimsFile UpdateClaimsDb(ClaimsFile file)
        {
            using (FileStream fs = File.Open(ClaimsDbFile, FileMode.Open, FileAccess.Write, FileShare.Read))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, file);
            }

            return GetClaimsDb();
        }

        public static UserLoginFile UpdateLoginDb(UserLoginFile file)
        {
            using (FileStream fs = File.Open(LoginDbFile, FileMode.Open, FileAccess.Write, FileShare.Read))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, file);
            }

            return GetUserLoginDb();
        }

        public static void UpdateUserfile(string name, string pass)
        {
            WaitUntilFileIsReady(Userfile);
            var file = GetUserfile();
            file.Users.Add(new UserInfo { Name = name, Password = pass });

            using (FileStream fs = File.Open(Userfile, FileMode.Open, FileAccess.Write, FileShare.Read))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, file);
            }
        }

        public static UserFile UpdateUserDb(UserFile file)
        {
            WaitUntilFileIsReady(UserDbFile);
            using (FileStream fs = File.Open(UserDbFile, FileMode.Open, FileAccess.Write, FileShare.Read))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, file);
            }

            return GetUserDb();
        }

        public static void ResetUserDb()
        {
            WaitUntilFileIsReady(UserDbFile);
            var db = GetUserDb();
            db.Users.RemoveAll(x => x.UserName != "neimless");
            using (FileStream fs = File.Open(UserDbFile, FileMode.Truncate, FileAccess.Write, FileShare.Read))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, db);
            }
        }

        public static RoleFile GetRoleDb()
        {
            var result = new RoleFile();
            using (FileStream fs = File.Open(RoleDbFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader file = new StreamReader(fs))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                result = serializer.Deserialize<RoleFile>(reader);
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
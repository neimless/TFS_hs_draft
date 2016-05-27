using HearthstoneDraft.Enums;
using HearthstoneDraft.Logic;
using HearthstoneDraft.Models.CardBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HearthstoneDraft.Models.Draft
{
    public class Draft
    {
        public Draft(int players)
        {
            this.Id = "#" + DateTime.Now.ToString("yyyyMMdd_hhmm");
            this.Players = new List<Player>();
            this.Packs = new List<Pack>();
            this.State = Draftstate.WaitingForPlayers;
            this.Maxplayers = players;
            this.Picknumber = 1;
            this.RandomizePacks();
            this.AllCards = new List<Card>();
            this.PopulateAllCards();
        }

        public string Id { get; set; }

        public Draftstate State { get; set; }

        public int Picknumber { get; set; }

        public int Maxplayers { get; set; }

        public List<Player> Players { get; set; }

        public List<Pack> Packs { get; set; }

        public List<Card> AllCards { get; set; }

        public void NextPack()
        {
            if (this.Packs.Take(this.Maxplayers).All(x => x.Cards.Count == 0))
            {
                for (int i = 0; i < this.Maxplayers; i++)
                {
                    this.Packs.RemoveAt(0);
                }
            }
        }

        public void NextPick()
        {
            if (this.Packs.Count == 0)
            {
                this.State = Draftstate.Finished;
            }

            this.Picknumber++;
            foreach (var item in this.Packs)
            {
                item.EnableForPick = true;
            }
        }

        public void AddPlayer(string name)
        {
            var player = new Player();
            player.Name = name;
            player.Position = this.Players.Count + 1;
            this.Players.Add(player);
            if (this.Players.Count == this.Maxplayers)
            {
                this.State = Draftstate.Started;
            }
        }

        private void PopulateAllCards()
        {
            foreach (var item in this.Packs)
            {
                this.AllCards.AddRange(item.Cards);
            }
        }

        private void RandomizePacks()
        {
            var cardbase = FileDatabase.RetrieveCardbase();
            var r = new Random();
            var packcount = this.Maxplayers * 3;
            for (int i = 0; i < packcount; i++)
            {
                var newPack = new Pack();
                for (int j = 0; j < 3; j++)
                {
                    if (R(r) < 2)
                    {
                        newPack.Cards.Add(this.RandomizeCard(cardbase, Rarity.Legendary, r));
                        continue;
                    }
                    if (R(r) < 12)
                    {
                        newPack.Cards.Add(this.RandomizeCard(cardbase, Rarity.Epic, r));
                        continue;
                    }
                    newPack.Cards.Add(this.RandomizeCard(cardbase, Rarity.Rare, r));
                }
                for (int j = 0; j < 4; j++)
                {
                    newPack.Cards.Add(this.RandomizeCard(cardbase, Rarity.Common, r));
                }
                for (int j = 0; j < 8; j++)
                {
                    if (R(r) < 50)
                    {
                        newPack.Cards.Add(this.RandomizeCard(cardbase, Rarity.Class, r));
                        continue;
                    }
                    newPack.Cards.Add(this.RandomizeCard(cardbase, Rarity.Common, r));
                }
                newPack.Cards = newPack.Cards.OrderBy(x => x.Rarity).ThenBy(x => x.Name).ToList();
                newPack.Packnumber = i + 1;
                this.Packs.Add(newPack);
            }
        }

        private int R(Random r)
        {
            return r.Next(101);
        }

        private Card RandomizeCard(HearthstoneDraft.Models.CardBase.CardBase cardbase, Rarity rarity, Random r)
        {            
            switch (rarity)
            {
                case Rarity.Common:
                    return cardbase.Commons.ElementAt(r.Next(cardbase.Commons.Count));
                case Rarity.Class:
                    return cardbase.Classcards.ElementAt(r.Next(cardbase.Classcards.Count));
                case Rarity.Rare:
                    return cardbase.Rares.ElementAt(r.Next(cardbase.Rares.Count));
                case Rarity.Epic:
                    return cardbase.Epics.ElementAt(r.Next(cardbase.Epics.Count));
                case Rarity.Legendary:
                    return cardbase.Legendaries.ElementAt(r.Next(cardbase.Legendaries.Count));
                default:
                    return null;
            }
        }
    }
}
using HearthstoneDraft.Enums;

namespace HearthstoneDraft.Models.CardBase
{
    public class Card
    {
        public Card()
        {
            this.AddedToDraft = 0;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public Rarity Rarity { get; set; }
        public Cardtype Type { get; set; }
        public PlayerClass PlayerClass { get; set; }
        public int Cost { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }
        public int Durability { get; set; }

        public int AddedToDraft { get; set; }
    }
}
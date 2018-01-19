using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Heartstone
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Card
    {
        [JsonProperty("cardId")]
        public string CardId { get; set; }

        [JsonProperty("dbfId")]
        public string DbfId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cardSet")]
        public string CardSet { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("faction")]
        public string Faction { get; set; }

        [JsonProperty("rarity")]
        public string Rarity { get; set; }

        [JsonProperty("attack")]
        public string Attack { get; set; }

        [JsonProperty("health")]
        public int? Health { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("race")]
        public string Race { get; set; }

        [JsonProperty("playerClass")]
        public string PlayerClass { get; set; }

        [JsonProperty("img")]
        public string Img { get; set; }

        [JsonProperty("imgGold")]
        public string ImgGold { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        public override string ToString()
        {
            return string.Format("[Card: CardId={0}, DbfId={1}, Name={2}, CardSet={3}, " +
                                 "Type={4}, Faction={5}, Rarity={6}, Attack={7}, Health={8}, " +
                                 "Text={9}, Race={10}, PlayerClass={11}, Img={12}, ImgGold={13}, " +
                                 "Locale={14}]", CardId, DbfId, Name, CardSet, Type, Faction, Rarity,
                                 Attack, Health.Value, Text, Race, PlayerClass, Img, ImgGold, Locale);
        }

    }

    [JsonArray]
    public class CardList : List<Card>
    {
        public CardList() { }

        public CardList(IEnumerable<Card> collection) : base(collection) { }
    }
}

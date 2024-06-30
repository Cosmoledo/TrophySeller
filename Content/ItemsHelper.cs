using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;

namespace TrophySeller
{
    public class ItemsHelper
    {
        public static List<Item> GetItems(int npcId)
        {
            var rules = Main.ItemDropsDB.GetRulesForNPCID(npcId, false);

            // get all items
            var itemDropInfo = new List<DropRateInfo>();
            foreach (var rule in rules)
            {
                rule.ReportDroprates(itemDropInfo, new DropRateInfoChainFeed(1f));
            }

            // to ids + remove bossbags
            List<int> ids = itemDropInfo.ConvertAll(info => info.itemId);
            ids.RemoveAll(id => ItemID.Sets.BossBag[id]);

            return IdsToItems(ids);
        }

        public static List<Item> FilterItemList(List<Item> items)
        {
            return IdsToItems(items.ConvertAll(item => item.netID));
        }

        private static List<Item> IdsToItems(List<int> ids)
        {
            var items = new List<Item>();

            foreach (var id in ids.ToHashSet())
            {
                var item = new Item(id);

                if (item.value == 0)
                {
                    item.value = 2000000;
                }

                items.Add(item);
            }

            items.Sort((itemA, itemB) => itemA.Name.CompareTo(itemB.Name));

            return items;
        }
    }
}

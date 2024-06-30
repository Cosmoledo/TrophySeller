using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace TrophySeller
{
    public class BossInfoHelper
    {
        public class BossInfo
        {
            public int bossId = -1;
            public int trophyItemId = -1;
            public List<Item> items = new();
            public string name = "";
            public string rawName = "";

            public override string ToString()
            {
                return string.Format(
                    "{0} ({1}), throphy: {2} ({3}), selling {4} items",
                    rawName,
                    bossId,
                    new Item(trophyItemId).Name,
                    trophyItemId,
                    items.Count
                );
            }
        }

        public static List<BossInfo> BossInfos = new();

        public static void LoadAllBosses()
        {
            var npc = new NPC();
            BossInfos = new();

            LogHelper.Log("Following IndexOutOfRangeException is expected. Don't worry.", false);
            for (int id = 0; id < 10_000; id++)
            {
                try
                {
                    npc.SetDefaults(id);
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }

                var items = ItemsHelper.GetItems(id);
                var trophyItemIndex = items.FindIndex(TrophyHelper.IsTrophyItem);

                var trophyId = -1;
                if (trophyItemIndex >= 0)
                {
                    trophyId = items[trophyItemIndex].netID;
                    items.RemoveAt(trophyItemIndex);
                }

                var isModded = npc.ModNPC != null;

                BossInfos.Add(
                    new BossInfo
                    {
                        bossId = id,
                        items = items,
                        name = npc.FullName,
                        rawName = isModded
                            ? (npc.ModNPC.Mod.Name + "/" + npc.ModNPC.Name)
                            : npc.FullName,
                        trophyItemId = trophyId,
                    }
                );
            }

            var removeIds = new int[] { NPCID.EaterofWorldsTail, NPCID.EaterofWorldsBody };

            // remove some bosses
            BossInfos.RemoveAll(info =>
                info.items.Count == 0 || info.trophyItemId == -1 || removeIds.Contains(info.bossId)
            );

            // combine duplicate trophies
            for (int i = BossInfos.Count - 1; i >= 0; i--)
            {
                var info = BossInfos[i];
                var hasDuplicates = false;

                var names = new HashSet<string> { info.name };
                var rawNames = new HashSet<string> { info.rawName };

                for (int k = i - 1; k >= 0; k--)
                {
                    var info2 = BossInfos[k];

                    if (info.trophyItemId != info2.trophyItemId)
                    {
                        continue;
                    }

                    info.items.AddRange(info2.items);
                    names.Add(info2.name);
                    rawNames.Add(info2.rawName);
                    hasDuplicates = true;

                    BossInfos.RemoveAt(k);
                }

                if (hasDuplicates)
                {
                    info.items = ItemsHelper.FilterItemList(info.items);
                    info.rawName = string.Join(" + ", rawNames);

                    if (names.Count > 1)
                    {
                        info.name = "\n-  " + string.Join("\n-  ", names);
                    }

                    i = BossInfos.Count - 1;
                }
            }

            // print
            BossInfos.ForEach(info => LogHelper.Log("Loaded: " + info, false));
        }
    }
}

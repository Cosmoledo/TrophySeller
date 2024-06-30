using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static TrophySeller.BossInfoHelper;

namespace TrophySeller.Content.NPCs
{
    [AutoloadHead]
    public class Seller : ModNPC
    {
        private static BossInfo bossInfo;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 23;

            NPCID.Sets.ExtraFramesCount[Type] = 9;
            NPCID.Sets.AttackFrameCount[Type] = 2;
            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 90;
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.HatOffsetY[Type] = 4;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Velocity = 1f };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Guide);
            AnimationType = NPCID.Guide;

            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
        }

        public override void ModifyActiveShop(string shopName, Item[] items)
        {
            if (bossInfo == null)
            {
                LogHelper.Log("ERROR: You should never have landed here!");
                items[0] = new Item(ItemID.YellowMarigold);
            }
            else
            {
                for (int i = 0; i < bossInfo.items.Count; i++)
                {
                    items[i] = bossInfo.items[i];
                }
            }

            base.ModifyActiveShop(shopName, items);
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return true;
        }

        public override bool CanChat()
        {
            return true;
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = "Shop";
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = bossInfo == null ? "" : Language.GetTextValue("LegacyInterface.28");
        }

        public override string GetChat()
        {
            bossInfo = null;

            var container = TrophyHelper.FindTrophy(NPC.position.ToTileCoordinates16());

            if (container.valid)
            {
                var curBossInfo = BossInfos.Find(bossInfo =>
                    bossInfo.trophyItemId == container.item.netID
                );

                if (curBossInfo == null)
                {
                    return Language.GetTextValue(
                        "Mods.TrophySeller.NPCs.Seller.Dialogue.InvalidTrophy",
                        container.item.Name
                    );
                }

                bossInfo = curBossInfo;

                return Language.GetTextValue(
                    "Mods.TrophySeller.NPCs.Seller.Dialogue.Trophy",
                    bossInfo.name
                );
            }

            return Language.GetTextValue("Mods.TrophySeller.NPCs.Seller.Dialogue.NoTrophy");
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(
                [
                    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                    new FlavorTextBestiaryInfoElement("Mods.TrophySeller.NPCs.Seller.Bestiary"),
                ]
            );
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.Shuriken;
            attackDelay = 10;
        }

        public override void TownNPCAttackProjSpeed(
            ref float multiplier,
            ref float gravityCorrection,
            ref float randomOffset
        )
        {
            multiplier = 12f;
            gravityCorrection = 2f;
            randomOffset = 1f;
        }
    }
}

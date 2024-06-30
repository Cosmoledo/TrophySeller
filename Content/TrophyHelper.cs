using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TrophySeller
{
    public class TrophyHelper
    {
        public class ItemContainer
        {
            public Item item = new();
            public bool valid = false;

            public override string ToString()
            {
                return string.Format("{0}: {1} ({2})", valid, item.Name, item.netID);
            }
        }

        public static ItemContainer FindTrophy(Point16 pos)
        {
            var container = new ItemContainer();

            if (pos.X == 0 && pos.Y == 0)
            {
                return container;
            }

            var tileAmount = new Dictionary<int, int>();
            var tiles = new Dictionary<int, Tile>();

            for (int x = -6; x <= 6; x++)
            {
                for (int y = -6; y <= 6; y++)
                {
                    var position = new Point16(pos.X + x, pos.Y + y);

                    var tile = Main.tile[position];

                    tileAmount[tile.TileType] = tileAmount.GetValueOrDefault(tile.TileType, 0) + 1;
                    tiles[tile.TileType] = tile;
                }
            }

            foreach (var found in tileAmount)
            {
                if (found.Value != 9)
                {
                    continue;
                }

                Item item;
                try
                {
                    var tile = tiles[found.Key];
                    item = new Item(TileToItem(tile));
                }
                catch (Exception)
                {
                    continue;
                }

                if (IsTrophyItem(item))
                {
                    container.item = item;
                    container.valid = true;
                    return container;
                }
            }

            return container;
        }

        public static int TileToItem(Tile tile)
        {
            var style = TileObjectData.GetTileStyle(tile);
            if (style == -1)
            {
                style = 0;
            }

            return TileLoader.GetItemDropFromTypeAndStyle(tile.TileType, style);
        }

        public static bool IsTrophyItem(Item item)
        {
            return item.consumable
                && (
                    item.createTile == TileID.Painting3X3 || item.Name.ToLower().Contains("trophy")
                );
        }
    }
}

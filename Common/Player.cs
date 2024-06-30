using Terraria.ModLoader;

namespace TrophySeller.Common
{
    public class Player : ModPlayer
    {
        public override void OnEnterWorld()
        {
            base.OnEnterWorld();

            BossInfoHelper.LoadAllBosses();
        }
    }
}

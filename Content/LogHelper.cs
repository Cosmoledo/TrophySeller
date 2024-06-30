using Terraria;
using Terraria.ModLoader;

namespace TrophySeller
{
    public class LogHelper
    {
        public static void Log(object obj, bool toChat = true, bool toFile = true)
        {
            if (toChat)
            {
                Main.NewText(obj.ToString());
            }

            if (toFile)
            {
                ModContent.GetInstance<TrophySeller>().Logger.Info(obj.ToString());
            }
        }

        public static void LogAll(params object[] objects)
        {
            foreach (var obj in objects)
            {
                Log(obj);
            }
        }
    }
}

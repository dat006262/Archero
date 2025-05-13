




using UnityEngine;

namespace com.F4A.MobileThird
{
    public static class SmartMapViewUtils
    {
        public const string KeyLife = "Life";
        public const string KeyGem = "Gem";

        public static int GetLife(int lifeDefault = 5)
        {
            return PlayerPrefs.GetInt(SmartMapViewUtils.KeyLife, lifeDefault);
        }

        public static void SetLife(int life)
        {
            PlayerPrefs.SetInt(SmartMapViewUtils.KeyLife, life);
        }

        public static int GetGem(int gemDefault = 10)
        {
            return PlayerPrefs.GetInt(SmartMapViewUtils.KeyGem, gemDefault);
        }

        public static void SetGem(int gem)
        {
            PlayerPrefs.SetInt(SmartMapViewUtils.KeyGem, gem);
        }

        public static int GetStarsLevel(int level)
        {
            return PlayerPrefs.GetInt(KeyLevelStars(level), 0);
        }

        public static void SetStarsLevel(int level, int stars)
        {
            Debug.Log(string.Format("<color=green>SetStarsLevel level: {0} - stars: {1}</color>", level, stars));
            PlayerPrefs.SetInt(KeyLevelStars(level), stars);
        }

        public static string KeyLevelStars(int number)
        {
            return string.Format("Level.{0:000}.StarsCount", number);
        }

        [System.Obsolete("This method is obsolete. Using method KeyLevelStars(int number)", true)]
        public static string FormatLevelStars(int number)
        {
            return string.Format("Level.{0:000}.StarsCount", number);
        }

        public static bool IsLevelLocked(int level)
        {
            return level > 1 && GetStarsLevel(level - 1) == 0;
        }

        /// <summary>
        /// SMV -> Smart View Map
        /// </summary>
        /// <returns></returns>
        public static int GetLastLevelUnlock()
        {
            return PlayerPrefs.GetInt("SMV.LastLevelUnlock", 1);
        }

        public static void SetLastLevelUnlock(int level)
        {
            PlayerPrefs.SetInt("SMV.LastLevelUnlock", level);
        }
    }
}

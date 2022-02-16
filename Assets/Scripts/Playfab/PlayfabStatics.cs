using PlayFab;
using UnityEngine;

namespace PlayFab.Utils
{
    public static class PlayFabLeaderboards
    {
        public static readonly string HIGHEST_SCORE = "HighestScore";
    }

    public static class PlayFabPlayerData
    {
        public static readonly string SCORE = "Score";
    }

    public static class PlayFabTitleData
    {
        public static readonly string MESSAGE1 = "TitleData_Message";
        public static readonly string ATTACK_RATE = "TitleData_AttackRate";
    }

    public static class PlayFabCurrencyData
    {
        public static readonly string GOLD = "GL";
        public static readonly string GEMS = "GM";
        public static readonly string ENERGY = "EN";
    }

    public static class PlayFabUtils
    {
        public static void OnError(PlayFabError err)
        {
            Debug.LogError($"PlayFabError\n:: {err.GenerateErrorReport()}");
        }
    }
}

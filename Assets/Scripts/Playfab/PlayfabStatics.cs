using System;
using PlayFab;
using UnityEngine;

namespace PlayFab.Utils
{
    [Serializable]
    public static class PlayFabLeaderboards
    {
        public static readonly string HIGHEST_SCORE = "HighestScore";
        public static readonly string DAILY_TIME = "DailyTime";

        public static string EnumToString(LeaderboardType leaderboardType)
        {
            switch (leaderboardType)
            {
                case LeaderboardType.HIGHEST_SCORE:
                    return HIGHEST_SCORE;
                case LeaderboardType.DAILY_TIME:
                    return DAILY_TIME;
                default:
                    return "ERROR";
            }
        }
    }

    [Serializable]
    public static class PlayFabPlayerData
    {
        public static readonly string HIGH_SCORE = "High_Score";
        public static string DAILY_BOARD_VER = "Daily_Board_Ver";
    }

    [Serializable]
    public static class PlayFabTitleData
    {
        public static readonly string MESSAGE1 = "TitleData_Message";
        public static readonly string ATTACK_RATE = "TitleData_AttackRate";
    }

    [Serializable]
    public static class PlayFabCurrencyData
    {
        public static readonly string GOLD = "GL";
        public static readonly string GEMS = "GM";
        public static readonly string ENERGY = "EN";
    }
    
    public static class PlayFabPlayerInfo
    {
        public static string PlayFabID;
    }
    
    public static class PlayFabUtils
    {
        public static void OnError(PlayFabError err)
        {
            Debug.LogError($"PlayFabError\n:: {err.GenerateErrorReport()}");
        }
    }
}

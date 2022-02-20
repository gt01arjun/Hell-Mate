using System;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using PlayFab.Utils;

public static class PlayFabLeaderboardController
{
    public static void SendLeaderboardStat(string leaderboardName, int val)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardName,
                    Value = val
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, PlayFabUtils.OnError);
    }
    private static  void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log($"Successfully updated leaderboard :: [{result.Request.ToJson().GetFromJSON("StatisticName")}]");
    }


    public static void GetLeaderboardAroundPlayer(string leaderboardName)
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = leaderboardName,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, PlayFabUtils.OnError);
    }
    private static void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        string statName = result.Request.ToJson().GetFromJSON("StatisticName");
        string str = $"Around Player *** {statName} ***\n";
        PrintLeaderboard(result.Leaderboard, str);
    }

    public static void GetLeaderboard(string leaderboardName, Action<GetLeaderboardResult> OnLeaderboardGet)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName,
            StartPosition = 0,
            MaxResultsCount = 10,
        };
        PlayFabClientAPI.GetLeaderboard(request,OnLeaderboardGet , PlayFabUtils.OnError);
    }
    
    public static void GetLeaderboardByVersion(string leaderboardName,int version, Action<GetLeaderboardResult> OnLeaderboardGet)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName,
            StartPosition = 0,
            MaxResultsCount = 10,
            Version = version,
        };
        PlayFabClientAPI.GetLeaderboard(request,OnLeaderboardGet , PlayFabUtils.OnError);
    }
    
    private static void PrintLeaderboard(List<PlayerLeaderboardEntry> entries, string statName)
    {
        string str = $"*** {statName} ***\n";
        foreach (var item in entries)
        {
            str += $"{item.Position}.  {item.DisplayName} :: {item.StatValue}\n";
        }
        Debug.Log(str);
    }
}

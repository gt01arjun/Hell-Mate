using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using PlayFab.Utils;

public class PlayFabLeaderboardController : MonoBehaviour
{
    private int _clicks;
    private bool _minTimeSent;


    private void Start()
    {
        _clicks = 0;
    }

    private void OnDisable()
    {
    }


    public void ButtonClick()
    {
        if (++_clicks >= 15 && _minTimeSent == false)
        {

            _minTimeSent = true;
        }
    }



    private void SendLeaderboardStat(string leaderboardName, int val)
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
    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log($"Successfully updated leaderboard :: [{result.Request.ToJson().GetFromJSON("StatisticName")}]");
    }


    private void GetLeaderboardAroundPlayer(string leaderboardName)
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = leaderboardName,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, PlayFabUtils.OnError);
    }
    private void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        string statName = result.Request.ToJson().GetFromJSON("StatisticName");
        string str = $"Around Player *** {statName} ***\n";
        PrintLeaderboard(result.Leaderboard, str);
    }

    private void GetLeaderboard(string leaderboardName)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName,
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, PlayFabUtils.OnError);
    }
    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        string statName = result.Request.ToJson().GetFromJSON("StatisticName");
        PrintLeaderboard(result.Leaderboard, statName);
    }

    private void PrintLeaderboard(List<PlayerLeaderboardEntry> entries, string statName)
    {
        string str = $"*** {statName} ***\n";
        foreach (var item in entries)
        {
            str += $"{item.Position}.  {item.DisplayName} :: {item.StatValue}\n";
        }
        Debug.Log(str);
    }
}

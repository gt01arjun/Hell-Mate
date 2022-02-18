using System;
using PlayFab.ClientModels;
using PlayFab.Utils;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class GetHighScoreLeaderboardData : MonoBehaviour
    {
        private TMP_Text _leaderboardText;

        private void OnEnable()
        {
            PlayFabLeaderboardController.GetHighScoreLeaderboardData += GetData;
            _leaderboardText = GetComponent<TMP_Text>();
            PlayFabLeaderboardController.GetLeaderboard(PlayFabLeaderboards.HIGHEST_SCORE);
        }

        private void OnDisable()
        {
            PlayFabLeaderboardController.GetHighScoreLeaderboardData -= GetData;
        }

        private void GetData(GetLeaderboardResult result,string statName)
        {
            string str = $"*** {statName} ***\n";
            foreach (var item in result.Leaderboard)
            {
                str += $"{item.Position}.  {item.DisplayName} :: {item.StatValue}\n";
            }

            _leaderboardText.text = str;
        }
    }
}
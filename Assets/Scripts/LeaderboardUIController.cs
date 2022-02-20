using PlayFab.ClientModels;
using PlayFab.Utils;
using TMPro;
using UnityEngine;

public class LeaderboardUIController : MonoBehaviour
{
    [SerializeField] private LeaderboardType _leaderboardType = LeaderboardType.HIGHEST_SCORE;
    private TMP_Text _leaderboardText;

    private void OnEnable()
    {
        GameManager.DisableMainMenuUI.AddListener(DisableLeaderBoard);
        _leaderboardText = GetComponent<TMP_Text>();
        PlayFabLeaderboardController.GetLeaderboard(PlayFabLeaderboards.EnumToString(_leaderboardType), GetData);
    }

    private void DisableLeaderBoard()
    {
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void GetData(GetLeaderboardResult result)
    {
        string statName = result.Request.ToJson().GetFromJSON("StatisticName");
        string str = $"*** {statName} ***\n";
        foreach (var item in result.Leaderboard)
        {
            str += $"{item.Position}.  {item.DisplayName} :: {item.StatValue}\n";
        }

        _leaderboardText.text = str;
    }
}
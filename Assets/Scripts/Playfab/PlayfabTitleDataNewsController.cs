using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Utils;

public class PlayfabTitleDataNewsController : MonoBehaviour
{
    private void OnEnable()
    {
        GetTitleData();
        GetTitleNews();
    }

    public void GetTitleData()
    {
        var request = new GetTitleDataRequest();
        PlayFabClientAPI.GetTitleData(request, OnTitleDataPulled, PlayFabUtils.OnError);
    }
    private void OnTitleDataPulled(GetTitleDataResult result)
    {
        if (result.Data == null)
        {
            Debug.LogWarning($"No Title Data Available");
            return;
        }
        if (result.Data.ContainsKey(PlayFabTitleData.MESSAGE1))
        {
            Debug.Log($"TitleData :: {result.Data[PlayFabTitleData.MESSAGE1]}");
        }
        if (result.Data.ContainsKey(PlayFabTitleData.ATTACK_RATE))
        {
            Debug.Log($"ATTACK_RATE :: {result.Data[PlayFabTitleData.ATTACK_RATE]}");
        }
    }

    public void GetTitleNews()
    {
        var request = new GetTitleNewsRequest();
        PlayFabClientAPI.GetTitleNews(request, OnTitleNewsPulled, PlayFabUtils.OnError);
    }
    private void OnTitleNewsPulled(GetTitleNewsResult result)
    {
        if (result.News == null || result.News.Count <= 0)
        {
            Debug.Log($"No News...");
            return;
        }

        string news = $"Got [{result.News.Count}] news\n";
        foreach (var item in result.News)
        {
            news += $"[{item.Timestamp.ToLocalTime()} :: {item.Title}] {item.Body}\n";
        }
        Debug.Log(news);
    }
}

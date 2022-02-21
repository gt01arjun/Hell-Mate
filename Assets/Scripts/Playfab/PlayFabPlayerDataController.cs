using System;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using PlayFab.Utils;

public static class PlayFabPlayerDataController
{
    public static void PushPlayerData(string playerData, string dataToPush,bool isPublisher = false)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {playerData,dataToPush},
            },
            
        };
        if(isPublisher) PlayFabClientAPI.UpdateUserPublisherData(request,OnUserDataPushed,PlayFabUtils.OnError);
        else PlayFabClientAPI.UpdateUserData(request, OnUserDataPushed, PlayFabUtils.OnError);
    }
    private static void OnUserDataPushed(UpdateUserDataResult obj)
    {
        Debug.Log($"Pushed user Data");
    }
    
    public static void PullPlayerData(Action<GetUserDataResult> OnUserDataPulled,bool isPublisher = false)
    {
        var request = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(request, OnUserDataPulled, PlayFabUtils.OnError);
    }

    public static void GetPlayerAccountInfo(Action<GetAccountInfoResult> OnUserDataPulled)
    {
        var request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request,OnUserDataPulled,PlayFabUtils.OnError);
    }
    // private static void OnUserDataPulled(GetUserDataResult result)
    // {
    //     if (result.Data == null) return;
    //     var data = result.Data;
    //     if (data.ContainsKey(PlayFabPlayerData.SCORE)) _scoreInput.text = data[PlayFabPlayerData.SCORE].Value;
    // }

}

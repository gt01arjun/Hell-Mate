using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System.Collections.Generic;
using PlayFab.Utils;

public class PlayfabPlayerDataController : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField _scoreInput;

    [SerializeField]
    private TMP_InputField _messageInput;

    [SerializeField]
    private TMP_InputField _colorInput;

    private void OnEnable()
    {
        PullPlayerData();
    }
    public void PushPlayerData()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {PlayFabPlayerData.SCORE, _scoreInput.text},
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnUserDataPushed, PlayFabUtils.OnError);
    }
    private void OnUserDataPushed(UpdateUserDataResult obj)
    {
        Debug.Log($"Pushed user Data");
    }

    public void PullPlayerData()
    {
        var request = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(request, OnUserDataPulled, PlayFabUtils.OnError);
    }
    private void OnUserDataPulled(GetUserDataResult result)
    {
        if (result.Data == null) return;
        var data = result.Data;
        if (data.ContainsKey(PlayFabPlayerData.SCORE)) _scoreInput.text = data[PlayFabPlayerData.SCORE].Value;
    }

}

using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Utils;

public class PlayfabLoginRegisterDeviceController : MonoBehaviour
{
    private void Start()
    {
        Login();
    }

    private void Login()
    {
        // Login or register using the Device's Unique ID
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, PlayFabUtils.OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log($"Login Success :: {result.ToJson()}");
    }
}

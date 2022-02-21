using System;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Utils;
using UnityEngine.Events;

public class PlayfabLoginRegisterController : MonoBehaviour
{
    [Header("Register")] [SerializeField] private GameObject _registerPanel;

    [SerializeField] private TMP_InputField _email_register;

    [SerializeField] private TMP_InputField _pass1_register;

    [SerializeField] private TMP_InputField _pass2_register;

    [Header("Login")] [SerializeField] private GameObject _loginPanel;

    [SerializeField] private TMP_InputField _email_login;

    [SerializeField] private TMP_InputField _pass1_login;


    [Header("Display Name")] [SerializeField]
    private GameObject _displayNamePanel;

    [SerializeField] private TMP_InputField _displayName;


    [Space] [SerializeField] private TMP_Text _messageText;

    [Space] [SerializeField] private UnityEvent _onLogin;

    private void OnEnable()
    {
        LoadLoginInfo();
    }

    public void RegisterButton()
    {
        var email = _email_register.text;
        var pass1 = _pass1_register.text;
        if (ValidateInputsRegister(email, pass1, _pass2_register.text) == false) return;

        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = pass1,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        _messageText.text = "Successfully Registered, please login";
        _messageText.color = Color.white;
        _email_login.text = _email_register.text;
        _registerPanel.gameObject.SetActive(false);
        _loginPanel.gameObject.SetActive(true);
    }

    public void LoginButton()
    {
        var email = _email_login.text;
        var pass1 = _pass1_login.text;
        if (ValidateInputsRegister(email, pass1) == false) return;

        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = pass1,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        PlayFabPlayerDataController.PullPlayerData((r) =>
        {
            if (r.Data.ContainsKey(PlayFabPlayerData.DISPLAY_NAME))
            {
                PlayFabPlayerInfo.DisplayName = r.Data[PlayFabPlayerData.DISPLAY_NAME].Value;
            }
            else
            {
                if (result.InfoResultPayload.PlayerProfile?.DisplayName == null || result.InfoResultPayload.PlayerProfile.DisplayName.Length < 3)
                {
                    ShowDisplayName();
                }
                else
                {
                    Debug.Log($"Successfully Logged-In [{result.InfoResultPayload.PlayerProfile.DisplayName}]");
                    SaveLoginInfo();
                    _messageText.color = Color.white;
                    _onLogin.Invoke();
                    gameObject.SetActive(false);
                    SavePlayerInfo(result);
                }
            }
        }, true);
}

private void SavePlayerInfo(LoginResult result)
{
    PlayFabPlayerInfo.PlayFabID = result.PlayFabId;
    Debug.Log($"PlayFabID : {result.PlayFabId}");
}

public void ResetPasswordButton()
{
    var email = _loginPanel.activeInHierarchy ? _email_login.text : _email_register.text;
    if (ValidateInputsRegister(email) == false) return;

    var request = new SendAccountRecoveryEmailRequest
    {
        Email = email,
        TitleId = PlayFabSettings.TitleId
    };
    PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
}

private void OnPasswordReset(SendAccountRecoveryEmailResult result)
{
    _messageText.text = "Password reset email Sent";
    _messageText.color = Color.white;
}

public void ShowDisplayName()
{
    _displayNamePanel.SetActive(true);
}

public void SetDisplayName()
{
    if (_displayName.text.Length < 3)
    {
        _messageText.text = "Invalid display name";
        _messageText.color = Color.red;
        return;
    }

    var request = new UpdateUserTitleDisplayNameRequest
    {
        DisplayName = _displayName.text
    };
    PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdated, OnError);
}

private void OnDisplayNameUpdated(UpdateUserTitleDisplayNameResult result)
{
    SaveLoginInfo();
    _onLogin.Invoke();
    _displayNamePanel.SetActive(false);
    gameObject.SetActive(false);
    PlayFabPlayerDataController.PushPlayerData(PlayFabPlayerData.DAILY_BOARD_VER, result.DisplayName, true);
}

private void OnError(PlayFabError err)
{
    var report = err.GenerateErrorReport();
    Debug.LogError($"PlayfabError\n::{report}");
    var errors = report.Split('\n');
    var errMsg = errors[errors.Length - 1];
    _messageText.text = errMsg;
    _messageText.color = Color.red;
}


private bool ValidateInputsRegister(string email, string pass1 = null, string pass2 = null)
{
    if (email.Length < 6 || email.Contains("@") == false || email.Contains(".") == false)
    {
        _messageText.text = $"Invalid Email, please enter a valid email";
        _messageText.color = Color.red;
        return false;
    }

    if (pass1 != null && pass1.Length < 6)
    {
        _messageText.text = $"Invalid password, must be at least 6 characters";
        _messageText.color = Color.red;
        return false;
    }

    if (pass2 != null && pass1 != pass2)
    {
        _messageText.text = $"Invalid password, both passwords must match";
        _messageText.color = Color.red;
        return false;
    }

    return true;
}


// _TODO: Please encrypt
private void SaveLoginInfo()
{
    PlayerPrefs.SetString("Credentials", $"{_email_login.text},{_pass1_login.text}");
}

private void LoadLoginInfo()
{
    var creds = PlayerPrefs.GetString("Credentials", ",").Split(',');
    _email_login.text = creds[0];
    _pass1_login.text = creds[1];
}
}
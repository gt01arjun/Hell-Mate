using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Utils;
using TMPro;
using System;

public class PlayfabCurrencyController : MonoBehaviour
{

    [SerializeField]
    private TMP_Text _goldValueText;

    [SerializeField]
    private TMP_Text _gemsValueText;

    [SerializeField]
    private TMP_Text _energyValueText;

    [SerializeField]
    private TMP_Text _energyTimeText;

    private float _secondsLeftToRecharge = 999f;
    private bool _canSendRequest = false;

    private void OnEnable()
    {
        GetVirtualCurrencies();
    }

    private void Update()
    {
        _secondsLeftToRecharge -= Time.deltaTime;
        _energyTimeText.text = $"({_secondsLeftToRecharge.ToString("0")})";
        if (_secondsLeftToRecharge < 0f && _canSendRequest)
        {
            _canSendRequest = false;
            GetVirtualCurrencies();
        }
    }

    public void SubtractGold5() => SubtractVirtualCurrency(PlayFabCurrencyData.GOLD, 5);
    public void SubtractGold10() => SubtractVirtualCurrency(PlayFabCurrencyData.GOLD, 10);
    public void SubtractGems5() => SubtractVirtualCurrency(PlayFabCurrencyData.GEMS, 5);
    public void SubtractGems10() => SubtractVirtualCurrency(PlayFabCurrencyData.GEMS, 10);
    public void SubtractEnergy5() => SubtractVirtualCurrency(PlayFabCurrencyData.ENERGY, 5);
    public void SubtractEnergy10() => SubtractVirtualCurrency(PlayFabCurrencyData.ENERGY, 10);

    public void AddGold() => AddVirtualCurrency(PlayFabCurrencyData.GOLD, 1);
    public void AddGems() => AddVirtualCurrency(PlayFabCurrencyData.GEMS, 1);
    public void AddEnergy() => AddVirtualCurrency(PlayFabCurrencyData.ENERGY, 1);


    public void GetVirtualCurrencies()
    {
        var request = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(request, OnInventoryPulled, PlayFabUtils.OnError);
    }
    private void OnInventoryPulled(GetUserInventoryResult result)
    {
        _goldValueText.text = result.VirtualCurrency[PlayFabCurrencyData.GOLD].ToString();
        _gemsValueText.text = result.VirtualCurrency[PlayFabCurrencyData.GEMS].ToString();
        _energyValueText.text = result.VirtualCurrency[PlayFabCurrencyData.ENERGY].ToString();
        _secondsLeftToRecharge = result.VirtualCurrencyRechargeTimes[PlayFabCurrencyData.ENERGY].SecondsToRecharge;
        _canSendRequest = true;
    }

    private void SubtractVirtualCurrency(string currencyCode, int amount)
    {
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = currencyCode,
            Amount = amount
        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request,
            res => GetVirtualCurrencies(),
            PlayFabUtils.OnError);
    }

    private void AddVirtualCurrency(string currencyCode, int amount)
    {
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = currencyCode,
            Amount = amount
        };
        PlayFabClientAPI.AddUserVirtualCurrency(request,
            res => GetVirtualCurrencies(),
            PlayFabUtils.OnError);
    }

}

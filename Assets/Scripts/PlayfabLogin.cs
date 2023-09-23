using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PlayfabLogin : MonoBehaviour, IDisposable
{
    [SerializeField] private Button _logInButton;
    [SerializeField] private TMP_Text _label;

    [SerializeField] private Color _colorSuccess;
    [SerializeField] private Color _colorError;

    [SerializeField] private String _logInSuccess = "Authorization complete!";
    [SerializeField] private String _logInError = "Authorization error!";

    private const string AuthGuidKey = "auth_guid_key"; 

    private void Start() 
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "74E99";
        }

        var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
        var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());

        var request = new LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = !needCreation
        };

        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            PlayerPrefs.SetString(AuthGuidKey, id);
            OnLoginSuccess(result);
        }, OnLoginError);

        //_logInButton.onClick.AddListener(OnLogInClick);
    }

    private void OnLogInClick()
    {
        _logInButton.interactable = false;

        var request = new LoginWithCustomIDRequest
        {
            CustomId = "Player 1",
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
    }

    private void OnLoginError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);
       // SetLabel(_logInError, _colorError);
        
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Complete login!!");
        //SetLabel(_logInSuccess, _colorSuccess);

        SetUserData(result.PlayFabId);
        //MakePurchase();
        GetInventory();
    }

    private void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            result => ShowInventory(result.Inventory), OnLoginError);
    }

    private void ShowInventory(List<ItemInstance> inventory)
    {
        var firstItem = inventory.First();
        Debug.Log($"{firstItem.ItemId}");
        ConsumePotion(firstItem.ItemInstanceId);
    }

    private void ConsumePotion(string itemInstanceId)
    {
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
        {
            ConsumeCount = 1,
            ItemInstanceId = itemInstanceId
        }, result =>
        {
            Debug.Log("Complate consume item");
        }, OnLoginError);
    }

    private void MakePurchase()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "main",
            ItemId = "health_potion",
            Price = 3,
            VirtualCurrency = "SC"
        }, result =>
        {
            Debug.Log("Complate purchase item");
        }, OnLoginError);
    }

    private void SetUserData(string playFabId)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"time_recieve_daily_revard", DateTime.UtcNow.ToString()}
            }
        }, result =>
        {
            Debug.Log("SetUserData");
            GetUserData(playFabId, "time_recieve_daily_revard");
        }, OnLoginError);
    }

    private void GetUserData(string playFabId, string keyData)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playFabId
        }, res =>
        {
            if (res.Data.ContainsKey(keyData))
            {
                Debug.Log($"{keyData}: {res.Data[keyData].Value}");
            }
        }, OnLoginError);
    }

    private void SetLabel(String text, Color color)
    {
        _label.text = text;
        _label.color = color;
    }

    public void Dispose()
    {
        //_logInButton.onClick?.RemoveAllListeners();
    }
}

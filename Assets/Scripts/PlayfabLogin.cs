using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine.UI;

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

        _logInButton.onClick.AddListener(OnLogInClick);
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
        SetLabel(_logInError, _colorError);
        
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Complete login!!");
        SetLabel(_logInSuccess, _colorSuccess);

    }

    private void SetLabel(String text, Color color)
    {
        _label.text = text;
        _label.color = color;
    }

    public void Dispose()
    {
        _logInButton.onClick?.RemoveAllListeners();
    }
}

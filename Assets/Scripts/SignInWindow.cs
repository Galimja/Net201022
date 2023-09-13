using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField] private Button _signInButton;
    
    protected override void SubscriptionsElementsUI()
    {
        base.SubscriptionsElementsUI();

        _signInButton.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        _loading.enabled = true;
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password,
        }, result =>
        {
            Debug.Log($"Success: {_username}");
            _loading.enabled = false;
            EnterInGameScene();
        }, error =>
        {
            Debug.LogError($"Fail: {error.ErrorMessage}");
            _loading.enabled = false;
        });
    }
}


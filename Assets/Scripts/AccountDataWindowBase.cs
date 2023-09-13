using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountDataWindowBase : MonoBehaviour
{
    [SerializeField] private InputField _usernameField;
    [SerializeField] private InputField _passwordField;
    [SerializeField] private Canvas _loadingField;

    protected string _username;
    protected string _password;
    protected Canvas _loading;

    private void Start()
    {
        _loading = _loadingField;
        SubscriptionsElementsUI();
    }

    protected virtual void SubscriptionsElementsUI()
    {
        _passwordField.onValueChanged.AddListener(UpdatePassword);
        _usernameField.onValueChanged.AddListener(UpdateUsername);
    }

    private void UpdatePassword(string pass)
    {
        _password = pass;
    }

    private void UpdateUsername(string username)
    {
        _username = username;
    }

    protected void EnterInGameScene()
    {
        SceneManager.LoadScene(1);
    }
}

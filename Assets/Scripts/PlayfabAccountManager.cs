using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayfabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
    }

    private void OnGetAccount(GetAccountInfoResult result)
    {
        _titleLabel.text = $"PlayFab id: {result.AccountInfo.PlayFabId}, " +
            $"Username: {result.AccountInfo.Username}, " +
            $"TitleInfo: {result.AccountInfo.TitleInfo}";
    }

    private void OnError(PlayFabError error)
    {
        var errorMassege = error.GenerateErrorReport();
        Debug.Log(errorMassege);
    }
}

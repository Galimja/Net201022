using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class PlayfabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;

    [SerializeField] private GameObject _newCharacterCreatePanel;

    [SerializeField] private Button _createCharecterButton;

    [SerializeField] private TMP_InputField _inputField;

    [SerializeField] private List<SlotCharacterWidget> _slots;

    private string _characterName;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnError);
        PlayFabServerAPI.GetRandomResultTables(new PlayFab.ServerModels.GetRandomResultTablesRequest(), 
            OnGetRandomResultTables, OnError);

        GetCharacters();

        foreach (var slot in _slots)
        {
            slot.SlotButton.onClick.AddListener(OpenCreateNewCharacter);
        }

        _inputField.onValueChanged.AddListener(OnNameChanged);
        _createCharecterButton.onClick.AddListener(CreaateCharacter);
    }

    private void CreaateCharacter()
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _characterName,
            ItemId = "character_token"
        }, result =>
        {
            UpdateCharacterStatistics(result.CharacterId);
        }, OnError);
    }

    private void UpdateCharacterStatistics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {"level", 1},
                {"gold", 0},
                {"exp", 0},
                {"hp", 100},
                {"dmg", 5}
            }
        }, result =>
        {
            Debug.Log("Complete!!!!!");
            CloseCreateNewCharacter();
            GetCharacters();
        }, OnError);
    }

    private void OnNameChanged(string changedName)
    {
        _characterName = changedName;
    }

    private void OpenCreateNewCharacter()
    {
        _newCharacterCreatePanel.SetActive(true);
    }

    private void CloseCreateNewCharacter()
    {
        _newCharacterCreatePanel.SetActive(false);
    }

    private void GetCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
            result =>
            {
                Debug.Log($"Character count: {result.Characters.Count}");
                ShowCharactersInSlot(result.Characters);
            }, OnError);
    }

    private void ShowCharactersInSlot(List<CharacterResult> characters)
    {
        if (characters.Count == 0)
        {
            foreach (var slot in _slots)
                slot.ShowemptySlot();
        }
        else if (characters.Count > 0 && characters.Count <= _slots.Count)
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest 
            { 
                CharacterId = characters.First().CharacterId
            }, result =>
            {
                var level = result.CharacterStatistics["level"].ToString();
                var gold = result.CharacterStatistics["gold"].ToString();
                var exp = result.CharacterStatistics["exp"].ToString();
                var hp = result.CharacterStatistics["hp"].ToString();
                var dmg = result.CharacterStatistics["dmg"].ToString();

                _slots.First().ShowInfoCharacterSlot(characters.First().CharacterName, 
                    level, gold, exp, hp, dmg);
            }, OnError);

        }
        else
        {
            Debug.LogError("Add slots for characters");
        }
    }

    private void OnGetRandomResultTables(PlayFab.ServerModels.GetRandomResultTablesResult result)
    {
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        Debug.Log("OnGetCatallogSuccess");

        ShowItems(result.Catalog);
    }

    private void ShowItems(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            Debug.Log($"{item.ItemId}");
        }
    }

    private void OnGetAccount(GetAccountInfoResult result)
    {
        _titleLabel.text = $"PlayFab id: {result.AccountInfo.PlayFabId}, " +
            $"Username: {result.AccountInfo.Username}";
    }

    private void OnError(PlayFabError error)
    {
        var errorMassege = error.GenerateErrorReport();
        Debug.Log(errorMassege);
    }
}

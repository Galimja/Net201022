using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotCharacterWidget : MonoBehaviour
{
    [SerializeField] private Button _button;

    [SerializeField] private GameObject _emptySlot;
    [SerializeField] private GameObject _infoCharacterSlot;

    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _levelLabel;
    [SerializeField] private TMP_Text _goldLabel;
    [SerializeField] private TMP_Text _expLabel;
    [SerializeField] private TMP_Text _hpLabel;
    [SerializeField] private TMP_Text _dmgLabel;


    public Button SlotButton => _button;

    public void ShowInfoCharacterSlot(string name, string level, 
        string gold, string exp, string hp, string dmg)
    {
        _nameLabel.text = $"Name: {name}";
        _levelLabel.text = $"Level: {level}";
        _goldLabel.text = $"Gold: {gold}";
        _expLabel.text = $"Exp: {exp}";
        _hpLabel.text = $"Hp: {hp}";
        _dmgLabel.text = $"Dmg: {dmg}";


        _infoCharacterSlot.SetActive(true);
        _emptySlot.SetActive(false);
    }

    public void ShowemptySlot()
    {
        _infoCharacterSlot.SetActive(false);
        _emptySlot.SetActive(true);
    }
}

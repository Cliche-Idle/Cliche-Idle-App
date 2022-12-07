using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UIViews;
using Cliche.UIElements;

public class CreateCharacterPopup : UIScript
{
    public CharacterHandler CharacterHandler;
    public SaveManager SaveManager;

    private string _playerName;

    private TextField _nameField;
    private Label _nameLabel;

    private Button _createCharacterConfirmButton;

    protected override void OnEnterFocus()
    {
        GetViewContainer().style.height = Length.Percent(100f);
        _nameField = GetViewContainer().Q<TextField>("CharacterNameField");
        // Display name if the player already entered one but backed out of the window
        if (_playerName != null)
        {
            _nameField.value = _playerName;
        }
        _nameField.RegisterValueChangedCallback((value) => {
            _nameLabel.text = value.newValue;
            _playerName = value.newValue;
            if (_playerName.Length != 0)
            {
                _createCharacterConfirmButton.SetEnabled(true);
            }
            else
            {
                _createCharacterConfirmButton.SetEnabled(false);
            }
        });

        // Character sheet
        _nameLabel = GetViewContainer().Q<Label>("Name");

        var raceLabel = GetViewContainer().Q<Label>("PlayerRace");
        raceLabel.text = CharacterHandler.CharacterSheet.Race.ToString();

        var characterDisplay = GetViewContainer().Q<CharacterDisplay>();
        characterDisplay.CharacterSheet = CharacterHandler.CharacterSheet;
        //

        _createCharacterConfirmButton = GetViewContainer().Q<Button>("CreateCharacterConfirmButton");
        _createCharacterConfirmButton.SetEnabled(false);
        _createCharacterConfirmButton.clicked += () => {
            CharacterHandler.CharacterSheet.Name = _playerName;
            SaveManager.SaveUserState();
            SceneManager.LoadScene("MainGameScreen");
        };
    }
}

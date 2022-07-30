using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    ActivityHandler activities;
    CharacterHandler character;
    CurrencyHandler currencies;
    ProgressionHandler progression;
    InventoryHandler inventory;

    private void Start() {
        activities = gameObject.GetComponent<ActivityHandler>();
        character = gameObject.GetComponent<CharacterHandler>();
        currencies = gameObject.GetComponent<CurrencyHandler>();
        progression = gameObject.GetComponent<ProgressionHandler>();
        inventory = gameObject.GetComponent<InventoryHandler>();
    }

    public void SaveUserState()
    {
        string activities_json = JsonUtility.ToJson(activities);
        File.WriteAllText(Application.persistentDataPath + "/activities.json", activities_json);

        string character_json = JsonUtility.ToJson(character);
        File.WriteAllText(Application.persistentDataPath + "/character.json", character_json);

        string currencies_json = JsonUtility.ToJson(currencies);
        File.WriteAllText(Application.persistentDataPath + "/currencies.json", currencies_json);

        string progression_json = JsonUtility.ToJson(progression);
        File.WriteAllText(Application.persistentDataPath + "/progression.json", progression_json);

        string inventory_json = JsonUtility.ToJson(inventory);
        File.WriteAllText(Application.persistentDataPath + "/inventory.json", inventory_json);
    }

    public void LoadUserState()
    {
        string activities_json = File.ReadAllText(Application.persistentDataPath + "/activities.json");
        JsonUtility.FromJsonOverwrite(activities_json, activities);

        string character_json = File.ReadAllText(Application.persistentDataPath + "/character.json");
        JsonUtility.FromJsonOverwrite(character_json, character);

        string currencies_json = File.ReadAllText(Application.persistentDataPath + "/currencies.json");
        JsonUtility.FromJsonOverwrite(currencies_json, currencies);

        string progression_json = File.ReadAllText(Application.persistentDataPath + "/progression.json");
        JsonUtility.FromJsonOverwrite(progression_json, progression);

        string inventory_json = File.ReadAllText(Application.persistentDataPath + "/inventory.json");
        JsonUtility.FromJsonOverwrite(inventory_json, inventory);
    }
}

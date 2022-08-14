using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

using Tooling.Firebase;

public class FirebaseAccountManagerWindow : EditorWindow
{
    public string FirebaseTestUsername;
    public string FirebaseTestPassword;

    private FirebaseAccountManager AccountManager;

    private readonly string TOOL_ASSET_PATH = "Assets/Tooling/UnityEditor/FirebaseAccountManager/Editor/UI";

    [MenuItem("Tools/Firebase Account Manager")]
    public static void Init()
    {
        FirebaseAccountManagerWindow window = GetWindow<FirebaseAccountManagerWindow>();
        window.titleContent = new GUIContent("Firebase Account Manager");
        Vector2 size = new Vector2(300, 200);
        window.minSize = size;
    }

    private VisualTreeAsset AccountItemRowTemplate;

    private FirebaseAccountsContainer AccountsList;

    public void CreateGUI()
    {
        AccountManager = new FirebaseAccountManager();

        var BaseWindowVisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{TOOL_ASSET_PATH}/Window.uxml");

        AccountItemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{TOOL_ASSET_PATH}/AccountItem.uxml");

        VisualElement BaseWindowLayout = BaseWindowVisualTree.Instantiate();

        rootVisualElement.Add(BaseWindowLayout);

        AccountsList = new FirebaseAccountsContainer();
        if (!File.Exists(Application.persistentDataPath + "/accounts.json"))
        {
            string accountsJson = JsonUtility.ToJson(AccountsList);
            File.WriteAllText(Application.persistentDataPath + "/accounts.json", accountsJson);
        }
        else
        {
            string accountsJson = File.ReadAllText(Application.persistentDataPath + "/accounts.json");
            AccountsList = JsonUtility.FromJson<FirebaseAccountsContainer>(accountsJson);
            if (AccountsList.Accounts == null)
            {
                // File exists but it's empty
                Debug.LogWarning($"<color=yellow>Editor-time automatic login file empty.</color>");
            }    
        }
        AccountListViewInit();
        DataEntryInit();
    }

    private TextField AccountNameField;
    private TextField AccountPasswordField;
    private Toggle ShowAccountPasswordToggle;
    private Button AddAccountButton;

    private void DataEntryInit()
    {
        AccountNameField = rootVisualElement.Q<TextField>("AccountEmailField");
        AccountPasswordField = rootVisualElement.Q<TextField>("AccountPasswordField");
        ShowAccountPasswordToggle = rootVisualElement.Q<Toggle>("ShowPasswordToggle");
        ShowAccountPasswordToggle.RegisterValueChangedCallback(toggled =>
        {
            AccountPasswordField.isPasswordField = !toggled.newValue;
        });
        AddAccountButton = rootVisualElement.Q<Button>("AddAccountButton");
        AddAccountButton.clicked += AddAccount;
        AddAccountButton.SetEnabled(false);
        AccountNameField.RegisterValueChangedCallback((ChangeEvent<string> evt) => {
            if (AccountsList.Accounts.Find(account => account.username == evt.newValue) != null)
            {
                AddAccountButton.text = "Edit";
            }
            else
            {
                AddAccountButton.text = "Add";
            }
            if (evt.newValue.Length == 0)
            {
                AddAccountButton.SetEnabled(false);
            }
            else if (AccountPasswordField.value.Length >= 6)
            {
                AddAccountButton.SetEnabled(true);
            }
        });
        AccountPasswordField.RegisterValueChangedCallback((ChangeEvent<string> evt) => {
            if (evt.newValue.Length < 6)
            {
                AddAccountButton.SetEnabled(false);
            }
            else if (AccountNameField.value.Length != 0)
            {
                AddAccountButton.SetEnabled(true);
            }
        });
    }

    private void UpdateDataEntryFields(FirebaseAccount account)
    {
        AccountNameField.value = account.username;
        AccountPasswordField.value = account.password;
    }

    private void AddAccount()
    {
        var account = AccountsList.Accounts.Find(account => account.username == AccountNameField.value);
        if (account != null)
        {
            account.password = AccountPasswordField.value;
            account.updatedDate = DateTime.UtcNow.ToString();
        }
        else
        {
            AccountsList.Accounts.Add(new FirebaseAccount()
            { 
                username = AccountNameField.value, 
                password =  AccountPasswordField.value, 
                updatedDate = DateTime.UtcNow.ToString(), 
                addedDate = DateTime.UtcNow.ToString()
            });
        }
        AccountManager.SaveToAccountsFile(AccountsList);
        AccountListView.Rebuild();
    }

    private ListView AccountListView;

    private FirebaseAccount SelectedAccount;

    private void AccountListViewInit()
    {
        AccountListView = rootVisualElement.Q<ListView>("AccountList");
        AccountListView.itemsSource = AccountsList.Accounts;
        //AccountListView.fixedItemHeight = 55;
        AccountListView.makeItem = () => AccountItemRowTemplate.CloneTree();
        AccountListView.bindItem = (element, index) =>
        {
            element.name = AccountsList.Accounts[index].username;
            element.Q<Label>("AccountName").text = AccountsList.Accounts[index].username;
            element.Q<Label>("AccountName").style.color = (AccountsList.Accounts[index].selected == true ? Color.green : Color.white);

            element.Q<Toggle>("ToggleAccountActive").SetValueWithoutNotify(AccountsList.Accounts[index].selected);
            element.Q<Toggle>("ToggleAccountActive").RegisterValueChangedCallback((ChangeEvent<bool> evt)=>{
                // Deselect the currently selected account
                AccountsList.Accounts.Find(account => account.selected == true).selected = false;
                // Select the newly selected account
                AccountsList.Accounts.Find(account => account.username == element.name).selected = evt.newValue;
                // Save and rebuilt list
                AccountManager.SaveToAccountsFile(AccountsList);
                AccountListView.Rebuild();

            });
            element.Q<Toggle>("ToggleAccountActive").name = $"{AccountsList.Accounts[index].username}";
            element.Q<Button>("DeleteAccount").clicked += (()=> {
                // Delete account from the list
                AccountsList.Accounts.RemoveAt(AccountsList.Accounts.FindIndex(account => account.username == element.name));
                // Save and rebuilt list
                AccountManager.SaveToAccountsFile(AccountsList);
                AccountListView.Rebuild();
            });
        };
        AccountListView.selectionType = SelectionType.Single;
        AccountListView.onSelectionChange += AccountListView_OnSelectionChange;
    }

    private void AccountListView_OnSelectionChange(IEnumerable<object> selectedItems)
    {
        var selectedItem = (FirebaseAccount)selectedItems.First();
        SelectedAccount = selectedItem;
        UpdateDataEntryFields(selectedItem);
    }
}


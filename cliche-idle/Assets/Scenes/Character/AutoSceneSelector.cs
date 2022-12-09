using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneSelector : MonoBehaviour
{
    public string GameSceneName;
    public SaveManager SaveManager;
    public bool RunInEditor = true;

    // Start is called before the first frame update
    void Start()
    {
        if (RunInEditor)
        {
            Debug.Log("<color=green>Character creator auto save load and scene switch is enabled.</color>");
            SaveManager.OnLoad += SwitchScene;
            SaveManager.LoadUserState();
        }
    }

    private void SwitchScene(bool loadSuccess)
    {
        if (loadSuccess)
        {
            Debug.Log("<color=green>Valid save found, skipping character creator.</color>");
            SceneManager.LoadScene("MainGameScreen");
        }
        else
        {
            Debug.Log("<color=yellow>No valid save found, staying in character creator.</color>");
        }
    }
}

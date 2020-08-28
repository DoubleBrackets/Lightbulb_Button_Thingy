using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MainMenuUIScript : MonoBehaviour
{

    private int playerUnlock = 1;

    public Color lockColor;
    private Color unlockColor;

    public GameObject main;
    public GameObject levelSelect;

    public Image[] levelSelects;

    void Start()
    {
        if (levelSelects[0] == null)
            return;
        if (PlayerPrefs.HasKey("LevelUnlocked"))
        {
            playerUnlock = PlayerPrefs.GetInt("LevelUnlocked");
        }
        else
        {
            PlayerPrefs.SetInt("LevelUnlocked", 1);
            PlayerPrefs.Save();
        }
        unlockColor = levelSelects[0].color;
        int l = levelSelects.Length;
        for(int x = playerUnlock;x < l;x++)
        {
            levelSelects[x].color = lockColor;
            levelSelects[x].GetComponent<Button>().interactable = false;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowLevelSelectWrapper()
    {
        main.SetActive(false);
        levelSelect.SetActive(true);
    }

    public void ShowMain()
    {
        main.SetActive(true);
        levelSelect.SetActive(false);
    }    


    public void ChangeScene(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName) != null)
            StartCoroutine(ChangeSceneCo(sceneName));
    }

    IEnumerator ChangeSceneCo(string sceneName)
    {
        FadeScript.fadeScript.FadeOut();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }

}

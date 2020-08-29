using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditorInternal;

public class UIScript : MonoBehaviour
{

    public static UIScript uiScript;

    public Image chargeBar;
    private float chargeBarWidth;

    public Image chargeIndicator;
    private float maxRatio = -1;

    public GameObject levelPassedUI;

    public GameObject pauseMenuUI;

    private bool paused = false;

    private bool passed = false;

    public int level;


    private void Awake()
    {
        uiScript = this;
        chargeBarWidth = chargeBar.rectTransform.rect.size.x;
        SetChargeBar(0,1);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !passed)
        {
            Pause();
        }
    }

    public void SetChargeBar(float value, float maxValue)
    {
        float ratio = Mathf.Min(1,value / maxValue);
        StartCoroutine(ChargeBarAnim(ratio));
    }

    IEnumerator ChargeBarAnim(float targetRatio)
    {
        float currentRatio = 0;
        for(int x = 0;x < 15;x++)
        {
            currentRatio = Mathf.Lerp(currentRatio, targetRatio,0.5f);
            ChangeBarToRatio(currentRatio);
            yield return new WaitForFixedUpdate();
        }
        ChangeBarToRatio(targetRatio);
        if (targetRatio == 1)
            yield break;
        yield return new WaitForSeconds(1f);
        for (int x = 0; x < 15; x++)
        {
            currentRatio = Mathf.Lerp(currentRatio, 0, 0.5f);
            ChangeBarToRatio(currentRatio);
            yield return new WaitForFixedUpdate();
        }
        ChangeBarToRatio(0);
    }

    private void ChangeBarToRatio(float ratio)
    {
        if(ratio > maxRatio)
        {
            maxRatio = ratio;
            chargeIndicator.rectTransform.anchoredPosition= new Vector2(chargeBarWidth * ratio - chargeBarWidth / 2f,chargeIndicator.rectTransform.anchoredPosition.y);
        }
        chargeBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, chargeBarWidth * ratio);
    }


    private void Pause()
    {
        if(paused)
        {
            //Unpause
            Time.timeScale = 1;
            pauseMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            //Pause
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        paused = !paused;

    }

    public void LevelFinished()
    {
        passed = true;
        Cursor.lockState = CursorLockMode.None;
        PlayerPrefs.SetInt("LevelUnlocked", level + 1);
        PlayerPrefs.Save();
        levelPassedUI.SetActive(true);
    }



    public void ChangeScene(string name)
    {
        if (SceneManager.GetSceneByName(name) != null)
        {
            StartCoroutine(ChangeSceneCo(name));
            Time.timeScale = 1;
        }
    }

    IEnumerator ChangeSceneCo(string name)
    {
        FadeScript.fadeScript.FadeOut();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
    }

    public void ReloadScene()
    {
        StartCoroutine(ReloadSceneCo());
        Time.timeScale = 1;
    }

    IEnumerator ReloadSceneCo()
    {
        FadeScript.fadeScript.FadeOut();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

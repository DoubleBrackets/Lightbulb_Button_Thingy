using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    public static UIScript uiScript;

    public Image chargeBar;
    private float chargeBarWidth;

    public Image chargeIndicator;
    private float maxRatio = -1;

    private void Awake()
    {
        uiScript = this;
        chargeBarWidth = chargeBar.rectTransform.rect.size.x;
        SetChargeBar(0,1);
    }

    public void SetChargeBar(float value, float maxValue)
    {
        float ratio = value / maxValue;
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

}

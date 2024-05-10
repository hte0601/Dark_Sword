using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUIBase : MonoBehaviour
{
    protected Text goldText;

    protected virtual void Awake()
    {
        goldText = transform.Find("GoldText").GetComponent<Text>();

        UpdateGoldText(GameSystem.CurrencyManager.GetGold());
        GameSystem.CurrencyManager.OnGoldValueChanged += UpdateGoldText;
    }

    protected virtual void OnDestroy()
    {
        GameSystem.CurrencyManager.OnGoldValueChanged -= UpdateGoldText;
    }

    protected virtual void UpdateGoldText(int gold)
    {
        goldText.text = gold.ToString();
    }
}

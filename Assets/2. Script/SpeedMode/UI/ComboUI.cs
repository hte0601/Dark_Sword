using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedMode
{
    public class ComboUI : MonoBehaviour
    {
        [SerializeField] private Text currentComboText;
        [SerializeField] private Text scoreMultiplierText;

        private void Start()
        {
            GameManager.instance.OnComboValueChanged += UpdateComboText;
        }

        private void UpdateComboText(int currentCombo, float scoreMultiplier)
        {
            currentComboText.text = currentCombo.ToString();
            scoreMultiplierText.text = string.Format("(x{0:0.0})", scoreMultiplier);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedMode
{
    public class SkillGaugeUI : MonoBehaviour
    {
        [SerializeField] private Slider skillGaugeSlider;
        [SerializeField] private Image sliderFillImage;

        private Color32 SKILL_GAUGE_COLOR = new(200, 195, 37, 255);
        private Color32 MAX_SKILL_GAUGE_COLOR = new(211, 0, 0, 255);

        private int maxSkillGauge;
        private bool isSkillGaugeMax = false;


        private void Start()
        {
            maxSkillGauge = GameMode.instance.modeRule.LoadSwordmanStatus().maxSkillGauge;
            skillGaugeSlider.maxValue = maxSkillGauge;

            Swordman swordman = Swordman.instance;

            UpdateSkillGaugeSlider(swordman.SkillGauge);
            swordman.OnSkillGaugeValueChanged += UpdateSkillGaugeSlider;
        }

        private void UpdateSkillGaugeSlider(int skillGauge)
        {
            skillGaugeSlider.value = skillGauge;

            if (skillGauge == maxSkillGauge && !isSkillGaugeMax)
            {
                isSkillGaugeMax = true;
                sliderFillImage.color = MAX_SKILL_GAUGE_COLOR;
            }
            else if (skillGauge != maxSkillGauge && isSkillGaugeMax)
            {
                isSkillGaugeMax = false;
                sliderFillImage.color = SKILL_GAUGE_COLOR;
            }
        }
    }
}

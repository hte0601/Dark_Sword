using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedMode
{
    public class TimeSliderUI : MonoBehaviour
    {
        [SerializeField] private Slider timeSlider;

        private void Start()
        {
            UpdateTimeSlider(GameManager.instance.Timer);

            GameManager.instance.OnTimerValueChanged += UpdateTimeSlider;
        }

        private void UpdateTimeSlider(float timer)
        {
            timeSlider.value = timer;
        }
    }
}

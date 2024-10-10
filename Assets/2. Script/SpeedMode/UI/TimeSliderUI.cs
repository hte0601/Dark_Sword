using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedMode
{
    public class TimeSliderUI : MonoBehaviour
    {
        [SerializeField] private Slider timeSlider;

        private void Awake()
        {
            timeSlider.maxValue = GameData.TimerData.MAX_TIME;
            timeSlider.value = GameData.TimerData.MAX_TIME;
        }

        private void Start()
        {
            GameManager.instance.OnTimerChanged += UpdateTimeSlider;
        }

        private void UpdateTimeSlider(float timer)
        {
            timeSlider.value = timer;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class SwordmanHealthUI : MonoBehaviour
    {
        [SerializeField] private List<GameObject> healthIconList;

        private void Start()
        {
            UpdateSwordmanHealthUI(Swordman.instance.CurrentHealth);
            Swordman.instance.OnCurrentHealthChanged += UpdateSwordmanHealthUI;
        }

        private void UpdateSwordmanHealthUI(int swordmanHealth)
        {
            for (int i = 0; i < healthIconList.Count; i++)
            {
                if (i < swordmanHealth)
                    healthIconList[i].SetActive(true);
                else
                    healthIconList[i].SetActive(false);
            }
        }
    }
}

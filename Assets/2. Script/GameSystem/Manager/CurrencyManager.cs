using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public static class CurrencyManager
    {
        private static CurrencyData currencyData;

        public static event Action<int> OnGoldValueChanged;

        static CurrencyManager()
        {
            currencyData = SaveDataManager.LoadData<CurrencyData>();
        }


        public static int GetGold()
        {
            return currencyData.gold;
        }

        public static void IncreaseGold(int amount)
        {
            currencyData.gold += amount;
            currencyData.Save();
            OnGoldValueChanged?.Invoke(currencyData.gold);
        }

        public static bool DecreaseGold(int amount)
        {
            if (currencyData.gold < amount)
            {
                return false;
            }
            else
            {
                currencyData.gold -= amount;
                currencyData.Save();
                OnGoldValueChanged?.Invoke(currencyData.gold);
                return true;
            }
        }
    }
}

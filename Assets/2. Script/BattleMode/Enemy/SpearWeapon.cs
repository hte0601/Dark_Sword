using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleMode
{
    public class SpearWeapon : MonoBehaviour
    {
        public SpearBoss boss;
        void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                if (gameObject.name == "GreenGoblin_Leg_L")
                    boss.GiveImpulse(other);
                boss.AttackTrigger(other);
                boss.SetWeaponGroundTouch(true);
            }
            else if (other.tag == "Ground")
            {
                boss.SetWeaponGroundTouch(true);
            }
        }
    }
}

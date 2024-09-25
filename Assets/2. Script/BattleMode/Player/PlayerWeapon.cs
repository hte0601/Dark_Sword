using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleMode
{
    public class PlayerWeapon : MonoBehaviour
    {
        void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.tag == "Boss")
            {
                Swordman.AttackTrigger(other);
            }
        }
    }
}

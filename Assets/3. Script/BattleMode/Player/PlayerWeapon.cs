using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Boss")
        {
            BattleSwordman.AttackTrigger(other);
        }
    }
}

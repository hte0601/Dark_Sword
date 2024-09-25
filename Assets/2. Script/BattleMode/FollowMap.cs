using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleMode
{
    public class FollowMap : MonoBehaviour
    {
        public float mapTypeValue;

        void FixedUpdate()
        {
            transform.Translate(Swordman.getVelocity() * Time.deltaTime * mapTypeValue, 0, 0);
        }
    }
}

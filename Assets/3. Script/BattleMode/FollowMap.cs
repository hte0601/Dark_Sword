using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMap : MonoBehaviour
{
    public float mapTypeValue;

    void FixedUpdate()
    {
        transform.Translate(BattleSwordman.getVelocity() * Time.deltaTime * mapTypeValue, 0, 0);
    }
}
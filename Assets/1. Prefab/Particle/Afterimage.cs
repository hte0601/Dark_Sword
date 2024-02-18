using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterimage : MonoBehaviour
{
    public ParticleSystemRenderer ps;
    GameObject player;
    void Start()
    {
        ps = GetComponent<ParticleSystemRenderer>();
        player = GameObject.FindWithTag("Player");
    }
    void Update()
    {
        if(ps != null)
        {
            if(player.transform.localScale.x == -1)
                ps.flip = new Vector3(0, 0, 0);
            else
                ps.flip = new Vector3(1, 0, 0);
        }
    }
}

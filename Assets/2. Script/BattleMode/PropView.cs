using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleMode
{
    public class PropView : MonoBehaviour
    {
        void Update()
        {
            if (-1 < transform.position.x && transform.position.x < 89)
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            else
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}

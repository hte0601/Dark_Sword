using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class PoolableObject : MonoBehaviour
    {
        [HideInInspector] public ObjectPool.ObjectType objectType;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [Header("Sound Effect")]
    [SerializeField] private bool hasSoundEffect = false;
    [SerializeField] private AudioSource soundEffectPlayer;


    protected void PlaySoundEffect()
    {
        if (hasSoundEffect)
            soundEffectPlayer.Play();
    }
}

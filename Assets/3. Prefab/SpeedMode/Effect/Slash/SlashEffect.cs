using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    public enum Color
    {
        Red,
        Green,
        Blue,
        Purple
    }

    [SerializeField] private Animator animator;

    public void Play(Color effectColor = Color.Red)
    {
        if (effectColor == Color.Red)
            animator.Play("Red_Slash");
        else if (effectColor == Color.Green)
            animator.Play("Green_Slash");
        else if (effectColor == Color.Blue)
            animator.Play("Blue_Slash");
        else if (effectColor == Color.Purple)
            animator.Play("Purple_Slash");
    }
}

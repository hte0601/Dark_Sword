using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimDemo_GM : MonoBehaviour {

	
    public int CurrentIndex = 1;
    public int MaxIndex = 5;

    //public Text CurrentStateText;
    public Sprite[] NumGroup;
    public Sprite[] StateTextImg;

    public Image StateImg;
    public Image NumImg;
    public void PreBtn()
    {
        CurrentIndex--;
        if (CurrentIndex <= 1)
        {
            CurrentIndex = 1;
        }
        ChangeAnim(CurrentIndex);
    }

    public void NextBtn()
    {
        CurrentIndex++;
        if (CurrentIndex >= MaxIndex)
        {
            CurrentIndex = MaxIndex;
        }
        ChangeAnim(CurrentIndex);
    }

   
    public Animator[] AnimArray;

    void ChangeAnim(int m_idx)
    {

        switch (m_idx)
        {
            case 1: //Idle
                for (int i = 0; i < AnimArray.Length; i++)
                {
                    AnimArray[i].Play("Idle");
                }

                StateImg.sprite = StateTextImg[0];
               // StateImg.SetNativeSize();
                NumImg.sprite = NumGroup[1];

               

                break;
            case 2: //Run
                for (int i = 0; i < AnimArray.Length; i++)
                {
                    AnimArray[i].Play("Run");
                }
                StateImg.sprite = StateTextImg[1];
               // StateImg.SetNativeSize();
                NumImg.sprite = NumGroup[2];
                break;
            case 3: //Attack
                AnimArray[0].Play("Attack_Sword");
                AnimArray[1].Play("Attack_Spear");
                AnimArray[2].Play("Attack_Axe");
                AnimArray[3].Play("Attack_Bow");
                AnimArray[4].Play("Attack_FireBottle");
                StateImg.sprite = StateTextImg[2];
              //  StateImg.SetNativeSize();
                NumImg.sprite = NumGroup[3];
                break;
            case 4: //Hit
                for (int i = 0; i < AnimArray.Length; i++)
                {
                    AnimArray[i].Play("Hit");
                }
                StateImg.sprite = StateTextImg[3];
                //StateImg.SetNativeSize();
                NumImg.sprite = NumGroup[4];
                break;
            case 5: //Die
                for (int i = 0; i < AnimArray.Length; i++)
                {
                    AnimArray[i].Play("Die");
                }
                StateImg.sprite = StateTextImg[4];
                //StateImg.SetNativeSize();
                NumImg.sprite = NumGroup[5];
                break;
        }



    }
}

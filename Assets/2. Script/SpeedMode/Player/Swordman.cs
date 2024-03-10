using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordman : MonoBehaviour
{   
    public static Swordman swordman;
    private Animator m_Anim;

    private void Start()
    {
        swordman = this;

        m_Anim = transform.Find("model").GetComponent<Animator>();
    }

    private void Update()
    {
        if (getPlayerState() == 4) return;

        if (Input.anyKeyDown)
        {
            if (Input.GetKey(KeyCode.Mouse0))
                return;
            SelectAnimation();
            SelectAttack();
        }
        else
            setPlayerState(0);
    }

    private void SelectAnimation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            setPlayerState(1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            setPlayerState(2);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            setPlayerState(3);
        }
    }

    //for moblie
    public void Attack()
    {
        if (getPlayerState() == 4) return;
        setPlayerState(1);
        SelectAttack();
    }

    public void Defense()
    {
        if (getPlayerState() == 4) return;
        setPlayerState(2);
        SelectAttack();
    }

    public void Pierce()
    {
        if (getPlayerState() == 4) return;
        setPlayerState(3);
        SelectAttack();
    }

    private void SelectAttack()
    {
        //적이 범위에 있을 경우
        if (Battle.IsEnemyInRange())
        {
            GameManager.isStart = true;
            //올바른 입력을 했을 경우
            if(Battle.getEnemyAction() == getPlayerState())
            {
                if(getPlayerState() == 1)
                {
                    ParticleManager.CreateHitParticle();
                }
                else if(getPlayerState() == 2)
                {
                    ParticleManager.CreateDefenseParticle();
                    SpeedSoundManager.PlayerSound("defense"); //막았을 때만 소리가 나기 위해 여기 존재
                }
                else if(getPlayerState() == 3)
                {
                    ParticleManager.CreateRedEnemyHitParticle();
                }
                Battle.EnemyDamaged(1);
                GameManager.TimeUp();
            }
            //잘못된 입력을 했을 경우
            else
            {
                GameManager.setTime(0);
                return;
            }
        }

        //공격 이펙트 및 사운드 출력
        switch (getPlayerState())
        {
            case 1:
                ParticleManager.CreateSlashParticle();
                SpeedSoundManager.PlayerSound("slash");
                break;
            case 3:
                ParticleManager.CreatePierceParticle();
                SpeedSoundManager.PlayerSound("pierce");
                break;
        }
    }

    private int getPlayerState()
    {
        return m_Anim.GetInteger("State");
    }

    public static void setPlayerState(int state)
    {
        swordman.m_Anim.SetInteger("State", state);
    }
}

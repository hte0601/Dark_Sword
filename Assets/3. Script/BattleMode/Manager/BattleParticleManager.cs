using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleParticleManager : MonoBehaviour
{
    public static BattleParticleManager pm;
    
    //Player particle
    public GameObject attackParticle;
    public GameObject pierceParticle;
    public GameObject sideCutParticle;
    public ParticleSystem defenseParticle;
    public ParticleSystem perfectDefenseParticle;
    public ParticleSystem brokenHeart;
    public ParticleSystem playerHit;
    public ParticleSystem playerGroggyHit;

    //enemy Particle
    public ParticleSystem enemyHit;
    public ParticleSystem enemyLanding;
    public ParticleSystem enemyThrowSpear;

    public ParticleSystem enemyPhaseChange;

    //AfterImage Particle
    public ParticleSystem[] playerAfterImage;
    public ParticleSystem[] enemyAfterImage;

    void Start()
    {
        pm = this;
    }

    //Player particle
    public static void CreateSlashParticle()
    {
        pm.attackParticle.SetActive(true);
    }

    public static void CreatePierceParticle()
    {
        pm.pierceParticle.SetActive(true);
    }

    public static void CreateSidecutParticle()
    {
        pm.sideCutParticle.SetActive(true);
    }

    public static void CreateDefenseParticle()
    {
        pm.defenseParticle.Play();
    }

    public static void CreatePerfectDefenseParticle()
    {
        pm.perfectDefenseParticle.Play();
    }

    public static void CreatePlayerHitParticle()
    {
        pm.playerHit.Play();
    }

    public static void CreatePlayerGroggyHitParticle()
    {
        pm.playerGroggyHit.Play();
    }

    public static void CreateAvoidParticle()
    {
        foreach(ParticleSystem index in pm.playerAfterImage)
        {
            index.Play();
        }
    }

    public static void StopAvoidParticle()
    {
        foreach(ParticleSystem index in pm.playerAfterImage)
        {
            index.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }


    //Enemy Particle
    public static void CreateEnemyHitParticle(Vector3 position)
    {
        pm.enemyHit.transform.position = position;
        pm.enemyHit.Play();
    }

    public static void CreateLandingParticle()
    {
        pm.enemyLanding.Play();
    }

    public static void CreateRushParticle()
    {
        foreach(ParticleSystem index in pm.enemyAfterImage)
        {
            index.Play();
        }
    }

    public static void CreateThrowSpearParticle(Vector3 position)
    {
        pm.enemyThrowSpear.transform.position = position;
        pm.enemyThrowSpear.Play();
    }

    public static void StopRushParticle()
    {
        foreach(ParticleSystem index in pm.enemyAfterImage)
        {
            index.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public static void CreateEnemyPhaseChangeParticle()
    {
        pm.enemyPhaseChange.Play();
    }

    //effect Particle
    public static void CreateBrokenHeartParticle()
    {
        pm.brokenHeart.Play();
    }
}

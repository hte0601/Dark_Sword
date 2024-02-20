using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpearBoss : Boss
{
    public static SpearBoss spearBoss;

    private float[,] PatternRate = new float[12, 3];
    private float PierceAttackRate;
    private float DownAttackRate;
    private float ThrowAndBackStepRate;
    private float PierceAndDownAttackRate;
    private float RushRate;
    private float PierceAttackThreeRate;
    private float KickRate;
    private float ThrowSpearRate;

    private int phase;
    private float bossDamage;
    private float bossSpeed;
    private bool weaponGroundTouched;

    private Vector3 SpearInitialPosition;
    private Quaternion SpearInitialRotation;
    private Coroutine spearCoroutine;

    public GameObject Spear;
    public GameObject SpearHitPoint;
    public Transform SpearParent;

    public SpriteRenderer headRenderer;
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer legLeftRenderer;
    public SpriteRenderer legRightRenderer;

    private static class State 
    {
        public const int Groggy = -2;
        public const int Die = -1;
        public const int Idle = 0;
        public const int Run = 1;
        public const int PierceAttack = 2;
        public const int DownAttack = 3;
        public const int ThrowAndBackStep = 4;
        public const int PierceAndDownAttack = 5;
        public const int Rush = 6;
        public const int PierceAttackThree = 7;
        public const int Kick = 8;
        public const int ThrowSpear = 9;
    }

    private void Awake()
    {
        spearBoss = this;
    }

    protected override void Start()
    {
        bossDamage = 30 + BattleGameManager.DebuffBossDamageUp;
        bossSpeed = 1 + BattleGameManager.DebuffBossSpeedUp;
        phase = BattleGameManager.DebuffBossStartPhase;

        SetStatus(1000, 5 * bossSpeed, bossDamage);
        base.Start();
        hpBar.value = health / maxHealth * 100;
        SpearInitialPosition = Spear.transform.localPosition;
        SpearInitialRotation = Spear.transform.localRotation;
        IniPatternRate();

        animator.speed = bossSpeed;
        if (phase == 1)
            StartCoroutine(PhaseCheck());
        else if (phase == 2)
            Start2Phase();
        else
            Debug.Log("phase ini err");

        StartCoroutine(SelectAttackPattern());
    }

    protected override void Update()
    {
        base.Update();
        
        if(DistanceFromPlayer() > 3 && (getState() == State.Run || getState() == State.Idle))
        {
            MoveToPlayer();
            setState(State.Run);
            BattleSoundManager.EnemyFootSound();
        }
        else if(getState() == State.Run)
        {
            setState(State.Idle);
            BattleSoundManager.EnemyWalkingSoundStop();
        }
    }

    private void IniPatternRate()
    {
        PatternRate[State.PierceAttack, 1] = 0.5f;
        PatternRate[State.PierceAttack, 2] = 0.3f;
        PatternRate[State.DownAttack, 1] = 0.5f;
        PatternRate[State.DownAttack, 2] = 0.5f;
        PatternRate[State.ThrowAndBackStep, 1] = 0.3f;
        PatternRate[State.ThrowAndBackStep, 2] = 0.3f;
        PatternRate[State.PierceAndDownAttack, 1] = 0f;
        PatternRate[State.PierceAndDownAttack, 2] = 0.3f;
        PatternRate[State.Rush, 1] = 0f;
        PatternRate[State.Rush, 2] = 0.5f;
        PatternRate[State.PierceAttackThree, 1] = 0f;
        PatternRate[State.PierceAttackThree, 2] = 0.5f;
        PatternRate[State.Kick, 1] = 0f; //temp
        PatternRate[State.Kick, 2] = 0f; //temp
        PatternRate[State.ThrowSpear, 1] = 2f;
        PatternRate[State.ThrowSpear, 2] = 2f;
    }

    private void SetPatternRate()
    {
        PierceAttackRate = PatternRate[State.PierceAttack, phase];
        DownAttackRate = PatternRate[State.DownAttack, phase];
        ThrowAndBackStepRate = PatternRate[State.ThrowAndBackStep, phase];
        PierceAndDownAttackRate = PatternRate[State.PierceAndDownAttack, phase];
        RushRate = PatternRate[State.Rush, phase];
        PierceAttackThreeRate = PatternRate[State.PierceAttackThree, phase];
        KickRate = PatternRate[State.Kick, phase];
        ThrowSpearRate = PatternRate[State.ThrowSpear, phase];
    }

    IEnumerator SelectAttackPattern()
    {
        int pattern;
        while (true)
        {
            do
            {
                yield return new WaitForSeconds(0.5f / bossSpeed);

                SetPatternRate();                

                //너무 가까우면 패턴 확률 높이기
                if (DistanceFromPlayer() < 2)
                {
                    ThrowAndBackStepRate += 0.5f;
                    KickRate += 2f;
                }

                //가까우면 러쉬, 창 던지기 패턴 발동 안 함
                if (DistanceFromPlayer() < 5)
                {
                    RushRate = 0f;
                    ThrowSpearRate = 0f;
                }
                
                pattern = RandomPattern();

                //범위 밖에서도 가능한 패턴일 경우 break
                if (pattern == State.Rush || pattern == State.ThrowSpear)
                    break;
            }
            while (DistanceFromPlayer() > 3);

            //범위 내에 플레이어가 들어왔으니 애니메이션 실행
            PlayAttackAnimation(pattern);

            while(NowAttack())
                yield return null;

            //발차기 공격이 성공적으로 들어갔을 경우 창던지기 패턴을 연계함
            if(pattern == State.Kick && DistanceFromPlayer() > 8)
            {
                pattern = State.ThrowSpear;
                PlayAttackAnimation(pattern);

                while (NowAttack())
                    yield return null;
            }

            //패턴 반복속도 랜덤 부여
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 1.5f) / bossSpeed);
        }
    }

    int RandomPattern()
    {
        float sumRate = PierceAttackRate + DownAttackRate + ThrowAndBackStepRate + PierceAndDownAttackRate + RushRate
            + PierceAttackThreeRate + KickRate + ThrowSpearRate;
        float rand = UnityEngine.Random.Range(0f, sumRate);

        if (rand < PierceAttackRate)
            return State.PierceAttack;
        rand -= PierceAttackRate;

        if (rand < DownAttackRate)
            return State.DownAttack;
        rand -= DownAttackRate;

        if (rand < ThrowAndBackStepRate)
            return State.ThrowAndBackStep;
        rand -= ThrowAndBackStepRate;

        if (rand < PierceAndDownAttackRate)
            return State.PierceAndDownAttack;
        rand -= PierceAndDownAttackRate;

        if (rand < RushRate)
            return State.Rush;
        rand -= RushRate;

        if (rand < PierceAttackThreeRate)
            return State.PierceAttackThree;
        rand -= PierceAttackThreeRate;

        if (rand < KickRate)
            return State.Kick;
        rand -= KickRate;

        if (rand < ThrowSpearRate)
            return State.ThrowSpear;
        rand -= ThrowSpearRate;

        return State.PierceAttack;
    }

    bool NowAttack()
    {
        return getState() != State.Idle && getState() != State.Run && getState() != State.Groggy && getState() != State.Die;
    }

    void PlayAttackAnimation(int pattern)
    {
        if(nowCoroutine != null)
        {
            nowCoroutine = null;
            return;
        }
            
        BattleSoundManager.EnemyWalkingSoundStop();

        
        if (pattern == State.PierceAttack)
        {
            setState(State.PierceAttack);
            nowCoroutine = StartCoroutine(PierceAttackAnimation());
        }
        else if (pattern == State.DownAttack)
        {
            setState(State.DownAttack);
            nowCoroutine = StartCoroutine(DownAttackAnimation());
        }
        else if (pattern == State.ThrowAndBackStep)
        {
            setState(State.ThrowAndBackStep);
            nowCoroutine = StartCoroutine(ThrowAndBackStepAnimation());
        }
        else if (pattern == State.PierceAndDownAttack)
        {
            setState(State.PierceAndDownAttack);
            nowCoroutine = StartCoroutine(PierceAndDownAttackAnimation());
        }
        else if (pattern == State.Rush)
        {
            setState(State.Rush);
            nowCoroutine = StartCoroutine(RushAnimation());
        }
        else if (pattern == State.PierceAttackThree)
        {
            setState(State.PierceAttackThree);
            nowCoroutine = StartCoroutine(PierceAttackThreeAnimation());
        }
        else if (pattern == State.Kick)
        {
            setState(State.Kick);
            nowCoroutine = StartCoroutine(KickAnimation());
        }
        else if (pattern == State.ThrowSpear)
        {
            setState(State.ThrowSpear);
            nowCoroutine = StartCoroutine(ThrowSpearAnimation());
        }
    }

    IEnumerator PierceAttackAnimation()
    {
        while (!isAnimation("Attack_Spear"))
            yield return null;

        while (getNormalizedTime() < 0.34f)
            yield return null;

        isAlreadyDamage = false;
        BattleSoundManager.EnemyPierceAttackSound();

        while (getNormalizedTime() < 0.46f)
            yield return null;
        
        isAlreadyDamage = true;

        while (getNormalizedTime() < 0.95f)
            yield return null;

        setState(State.Idle);
        AttackCoroutineEnd();
    }

    IEnumerator DownAttackAnimation()
    {
        while (!isAnimation("DownAttack_Spear"))
            yield return null;

        while (getNormalizedTime() < 0.34f)
            yield return null;

        isAlreadyDamage = false;
        BattleSoundManager.EnemyDownAttackSound();

        while (getNormalizedTime() < 0.46f)
            yield return null;

        isAlreadyDamage = true;

        while (getNormalizedTime() < 0.95f)
            yield return null;

        setState(State.Idle);
        AttackCoroutineEnd();
    }

    IEnumerator ThrowAndBackStepAnimation()
    {
        while (!isAnimation("ThrowAndBackStep_Spear"))
            yield return null;

        while (getNormalizedTime() < 0.23f)
            yield return null;

        Jump();

        while (getNormalizedTime() < 0.35f)
            yield return null;

        StartCoroutine(ThrowSpear());

        while (getNormalizedTime() < 0.74f)
            yield return null;

        BattleParticleManager.CreateLandingParticle();
        BattleSoundManager.EnemyLandingSound();

        while (getNormalizedTime() < 0.95f)
            yield return null;

        setState(State.Idle);
        AttackCoroutineEnd();
    }

    IEnumerator PierceAndDownAttackAnimation()
    {
        while (!isAnimation("PierceAndDownAttack_Spear"))
            yield return null;

        while (getNormalizedTime() < 0.19f)
            yield return null;

        isAlreadyDamage = false;

        while (getNormalizedTime() < 0.28f)
            yield return null;

        isAlreadyDamage = true;

        while (getNormalizedTime() < 0.6f)
            yield return null;

        isAlreadyDamage = false;

        while (getNormalizedTime() < 0.72f)
            yield return null;

        isAlreadyDamage = true;

        while (getNormalizedTime() < 0.95f)
            yield return null;

        setState(State.Idle);
        AttackCoroutineEnd();
    }

    IEnumerator RushAnimation()
    {
        while (!isAnimation("SpearRush"))
            yield return null;

        isAlreadyDamage = false;
        isGroggyAttack = true;
        BattleParticleManager.CreateRushParticle();

        SetSpeed(9);
        while (Vector3.Distance(targetTransform.position, Spear.transform.position) > 1.5f)
        {
            MoveToPlayer();
            yield return null;
        }
        BattleParticleManager.StopRushParticle();
        SetSpeed(5);
        isAlreadyDamage = true;
        isGroggyAttack = false;
        stamina -= 30f;
        staminaBar.value = stamina;
        setState(State.Idle);
        AttackCoroutineEnd();
    }

    IEnumerator PierceAttackThreeAnimation()
    {
        while (!isAnimation("Pierce_SpearThreetime"))
            yield return null;

        while (getNormalizedTime() < 0.27f)
            yield return null;

        isAlreadyDamage = false;

        while (getNormalizedTime() < 0.32f)
            yield return null;

        isAlreadyDamage = true;

        while (getNormalizedTime() < 0.45f)
            yield return null;

        isAlreadyDamage = false;

        while (getNormalizedTime() < 0.50f)
            yield return null;

        isAlreadyDamage = true;

        while (getNormalizedTime() < 0.63f)
            yield return null;

        isAlreadyDamage = false;

        while (getNormalizedTime() < 0.68f)
            yield return null;

        isAlreadyDamage = true;

        while (getNormalizedTime() < 0.95f)
            yield return null;

        setState(State.Idle);
        AttackCoroutineEnd();
    }

    IEnumerator KickAnimation()
    {
        while (!isAnimation("Kick"))
            yield return null;

        while (getNormalizedTime() < 0.5f)
            yield return null;

        isAlreadyDamage = false;
        isAlreadyPush = false;

        while (getNormalizedTime() < 0.67f)
            yield return null;

        isAlreadyDamage = true;
        isAlreadyPush = true;

        while (getNormalizedTime() < 0.95f)
            yield return null;

        setState(State.Idle);
        AttackCoroutineEnd();
    }

    IEnumerator ThrowSpearAnimation()
    {
        while (!isAnimation("ThrowSpear"))
            yield return null;

        while (getNormalizedTime() < 0.5f)
            yield return null;

        StartCoroutine(ThrowSpear());

        while (getNormalizedTime() < 0.95f)
            yield return null;

        setState(State.Idle);
        AttackCoroutineEnd();
    }

    private void AttackCoroutineEnd()
    {
        nowCoroutine = null;
    }
    
    private void Jump()
    {
        Vector2 jumpVector = new Vector2(transform.localScale.x / 4 * rigidbody.mass * 20, rigidbody.mass * 10);
        rigidbody.AddForce(jumpVector, ForceMode2D.Impulse);
    }

    IEnumerator ThrowSpear()
    {
        if(spearCoroutine != null)
        {
            spearCoroutine = null;
            yield break;
        }
        isAlreadyDamage = false;
        
        Spear.transform.SetParent(null);
        SetWeaponGroundTouch(false);
        
        //창을 플레이어 방향으로 회전
        Quaternion direction = Quaternion.LookRotation(targetTransform.position - Spear.transform.position);
        Spear.transform.rotation = direction * Quaternion.Euler(0, 90, 90);

        BattleSoundManager.EnemyThrowSpearSound();

        while(!getWeaponGroundTouch())
        {
            Spear.transform.Translate(Spear.transform.up * Time.deltaTime * 30f, Space.World);
            yield return null;
        }

        BattleParticleManager.CreateThrowSpearParticle(SpearHitPoint.transform.position);
        BattleSoundManager.EnemyWeaponSound();
        
        isAlreadyDamage = true;
        Spear.transform.SetParent(SpearParent);
        Spear.transform.localPosition = SpearInitialPosition;
        Spear.transform.localRotation = SpearInitialRotation;
        Spear.transform.localScale = new Vector3(0.6f, 0.6f, 1);
        spearCoroutine = null;
    }

    IEnumerator PhaseCheck()
    {
        while(maxHealth / 2 < health)
            yield return null;

        Start2Phase();
    }

    void Start2Phase()
    {
        phase = 2;

        var redHead = Resources.Load<Sprite>("RedGoblin_Head");
        var redBody = Resources.Load<Sprite>("RedGoblin_Body");
        var redLeg = Resources.Load<Sprite>("RedGoblin_Leg");

        headRenderer.sprite = redHead;
        bodyRenderer.sprite = redBody;
        legLeftRenderer.sprite = redLeg;
        legRightRenderer.sprite = redLeg;

        BattleParticleManager.CreateEnemyPhaseChangeParticle();
        BattleSoundManager.EnemyPhaseChangeSound();
    }

    public void SetWeaponGroundTouch(bool value)
    {
        weaponGroundTouched = value;
    }

    private bool getWeaponGroundTouch()
    {
        return weaponGroundTouched;
    }

    private bool WithinRange()
    {
        return Vector3.Distance(targetTransform.position, transform.position) < 3;
    }

    private float DistanceFromPlayer()
    {
        return Vector3.Distance(targetTransform.position, transform.position);
    }

    public static Vector3 getPosition()
    {
        return spearBoss.transform.position;
    }
}

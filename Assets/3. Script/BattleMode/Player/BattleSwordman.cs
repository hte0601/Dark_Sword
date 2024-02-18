using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class BattleSwordman : LivingEntity
{
    public static BattleSwordman battleSwordman;
    public GameObject EnemyHitPoint;
    private Rigidbody2D rigidBody;
    private Vector3 vector;
    private float xVector;
    private float defenseTiming;
    private float staminaRegen;
    private float perfectDefenseTime;
    private const float stiffnessPoint = 20f;

    private bool isDefense;
    private bool isGroggy;
    private bool playerAttachLeftWall;
    private bool playerAttachRightWall;

    private Coroutine nowCoroutine;
    public Slider hpBar;
    public Slider staminaBar;
    public CapsuleCollider2D PlayerCollider;
    public CapsuleCollider2D playerTrigger;

    private static class State 
    {
        public const int Groggy = -2;
        public const int Die = -1;
        public const int Idle = 0;
        public const int Attack = 2;
        public const int Defense = 3;
        public const int Avoid = 4;
    }

    void Awake()
    {
        battleSwordman = this;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = transform.Find("model").GetComponent<Animator>();

        vector = new Vector3(0, 0, 0);
        isDefense = false;
        isGroggy = false;
        playerAttachLeftWall = false;
        playerAttachRightWall = false;

        SetStatus(500, 5, 200);
        hpBar.value = health / maxHealth * 100;
        staminaBar.value = stamina;
        perfectDefenseTime = 0.3f - BattleGameManager.DebuffPerfectDefenseTimeDown;
        staminaRegen = 30f - BattleGameManager.DebuffStaminaRegenDown;

        StartCoroutine(StaminaRegen());
        StartCoroutine(StaminaClock());
    }

    void FixedUpdate()
    {
        UpdateXVector();

        if(Mathf.Abs(rigidBody.velocity.y) < 0.2f)
        {
            vector.x = xVector;
            rigidBody.velocity = vector * speed;
        }

        if (vector.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (vector.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            Avoid();

        if (Input.GetKey(KeyCode.A))
            Attack();

        if (Input.GetKey(KeyCode.S))
            DefenseUp();

        if (Input.GetKeyUp(KeyCode.S))
            DefenseDown();
    }

    private void UpdateXVector()
    {
        if (getState() == State.Groggy || getState() == State.Die || getState() == State.Attack || CheckPlayerAttachWall())
        {
            xVector = 0;
            animator.SetFloat("vector", xVector);
            return;
        }
        else if (getState() == State.Avoid)
            return;

        xVector = xVectorInput();
        animator.SetFloat("vector", xVector);
        

        if (xVector != 0)
            BattleSoundManager.PlayerFootSound();
        else
            BattleSoundManager.playerSoundStop();
    }

    private bool CheckPlayerAttachWall()
    {
        return (playerAttachLeftWall && xVectorInput() < 0) ||
            (playerAttachRightWall && xVectorInput() > 0);
    }


    public void Attack()
    {
        if (stamina <= 0f)
            return;

        if (getState() == State.Attack)
        {
            if (0.6f < getNormalizedTime() && getNormalizedTime() < 0.92f)
            {
                if (isAnimation("Attack") && getCombo() == 1)
                {
                    setCombo(2);
                    nowCoroutine = StartCoroutine(PierceAnimation());
                }
                else if (isAnimation("Pierce") && getCombo() == 2)
                {
                    setCombo(3);
                    nowCoroutine = StartCoroutine(SidecutAnimation());
                }
            }
        }
        else if (getState() == State.Idle)
        {
            setState(State.Attack);
            nowCoroutine = StartCoroutine(AttackAnimation());
        }
    }

    public void DefenseUp()
    {
        if (stamina <= 0f)
            return;

        if (getState() == State.Idle)
        {
            defenseTiming = 0f;
            setState(State.Defense);
            nowCoroutine = StartCoroutine(DefenseAnimation());
        }
    }

    public void DefenseDown()
    {
        isDefense = false;
    }

    public void Avoid()
    {
        if (stamina <= 0f || isAnimation("Avoid"))
            return;

        if (getState() == State.Idle || (getState() == State.Attack && getNormalizedTime() > 0.55f))
        {
            setState(State.Avoid);
            nowCoroutine = StartCoroutine(AvoidAnimation());
        }
    }

    protected override void Die()
    {
        StopAllCoroutines();
        setState(State.Die);
        BattleParticleManager.CreateBrokenHeartParticle();
        isDead = true;

        //Victory UI load
        StartCoroutine(GameOver());
        BattleSoundManager.PlayGameOverSound();
    }

    public override void OnDamage(float dmg)
    {
        if(isDefense)
        {
            if(defenseTiming < perfectDefenseTime)
            {
                GameObject.FindWithTag("Boss").GetComponent<Boss>().OnStiffness(stiffnessPoint);
                BattleParticleManager.CreatePerfectDefenseParticle();
                BattleSoundManager.PlayerPerfectDefenseSound();
            }
            else if(defenseTiming < 1f) // 0.3second : 1second = 0dmg : 50dmg
            {
                stamina -= 50 * (defenseTiming - perfectDefenseTime) / (1 - perfectDefenseTime);
                BattleParticleManager.CreateDefenseParticle();
                BattleSoundManager.PlayerDefenseSound();
            }
            else
            {
                stamina -= 50f;
                BattleParticleManager.CreateDefenseParticle();
                BattleSoundManager.PlayerDefenseSound();
            }

            if(stamina < 0)
            {
                OnGroggy();
            }

            return;
        }
        
        BattleSoundManager.PlayerHitSound();
        BattleParticleManager.CreatePlayerHitParticle();
        base.OnDamage(dmg);
        hpBar.value = health / maxHealth * 100;
    }

    public void OnGroggy()
    {
        if (defenseTiming < 0.3f)
            return;
        isGroggy = true;
    }

    
    IEnumerator AttackAnimation()
    {
        while (!isAnimation("Attack"))
            yield return null;

        stamina -= 15f;
        BattleParticleManager.CreateSlashParticle();

        while (getNormalizedTime() < 0.35f)
            yield return null;

        isAlreadyDamage = false;
        BattleSoundManager.PlayerSlashSound();

        while (getNormalizedTime() < 0.95f)
        {
            if (getCombo() != 1)
            {
                isAlreadyDamage = true;
                yield break;
            }
            if (getState() == State.Avoid)
            {
                isAlreadyDamage = true;
                yield break;
            }

            yield return null;
        }
            
        isAlreadyDamage = true;
        setCombo(1);
        setState(State.Idle);
    }

    IEnumerator PierceAnimation()
    {
        while (!isAnimation("Pierce"))
            yield return null;

        stamina -= 15f;
        BattleParticleManager.CreatePierceParticle();

        while (getNormalizedTime() < 0.35f)
            yield return null;

        isAlreadyDamage = false;
        BattleSoundManager.PlayerPierceSound();

        while (getNormalizedTime() < 0.95f)
        {
            if (getCombo() != 2)
            {
                isAlreadyDamage = true;
                yield break;
            }
            if (getState() == State.Avoid)
            {
                isAlreadyDamage = true;
                yield break;
            }

            yield return null;
        }

        isAlreadyDamage = true;
        setCombo(1);
        setState(State.Idle);
    }

    IEnumerator SidecutAnimation()
    {
        while (!isAnimation("Sidecut"))
            yield return null;

        stamina -= 15f;
        BattleParticleManager.CreateSidecutParticle();

        while (getNormalizedTime() < 0.35f)
            yield return null;

        isAlreadyDamage = false;            
        BattleSoundManager.PlayerSidecutSound();

        isGroggyAttack = true;
        while (getNormalizedTime() < 0.95f)
        {
            if (getState() == State.Avoid)
            {
                isAlreadyDamage = true;
                yield break;
            }

            yield return null;
        }

        isGroggyAttack = false;
        isAlreadyDamage = true;
        setCombo(1);
        setState(State.Idle);
    }

    IEnumerator DefenseAnimation()
    {
        isDefense = true;
        rigidBody.mass *= 2;

        while (!isAnimation("Defense"))
        {   
            defenseTiming += 1f * Time.deltaTime;
            yield return null;
        }
        
        animator.SetLayerWeight(1, 1);

        while (isDefense)
        {
            defenseTiming += 1f * Time.deltaTime;
            yield return null;
        }

        rigidBody.mass /= 2;
        animator.SetLayerWeight(1, 0);

        setState(State.Idle);
    }


    IEnumerator AvoidAnimation()
    {
        PlayerCollider.enabled = false;
        playerTrigger.enabled = false;

        if (xVectorInput() == 0)
            xVector = transform.localScale.x * -1;
        else
            xVector = xVectorInput();

        stamina -= 20f;
        SetSpeed(10f);
        BattleParticleManager.CreateAvoidParticle();
        BattleSoundManager.PlayerAvoidSound();

        while (!isAnimation("Avoid"))
            yield return null;

        while (getNormalizedTime() < 0.95f)
            yield return null;

        while (Math.Abs(SpearBoss.getPosition().x - transform.position.x) < 1.35)
            yield return null;

        BattleParticleManager.StopAvoidParticle();
        xVector = 0f;

        PlayerCollider.enabled = true;
        playerTrigger.enabled = true;

        SetSpeed(5f);
        setCombo(1);
        setState(State.Idle);
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        BattleGameManager.GameOver();
        BattleGameManager.DebuffReset();
        BattleSoundManager.GameOverUIPlay();
    }

    IEnumerator StaminaRegen()
    {
        while (true)
        {
            if (getState() == State.Idle)
                stamina += staminaRegen * Time.deltaTime;
            
            if (stamina > 100f)
                stamina = 100f;

            staminaBar.value = stamina;
            yield return null;
        }
    }

    IEnumerator StaminaClock()
    {
        while (true)
        {
            if (isGroggy)
            {
                setState(State.Groggy);
                BattleParticleManager.CreatePlayerGroggyHitParticle();

                try
                {
                    StopCoroutine(nowCoroutine);
                    nowCoroutine = null;
                }
                catch (NullReferenceException nre)
                {
                    Debug.Log(nre);
                }

                yield return new WaitForSeconds(2f);
                //value reset
                Initiate();
            }

            yield return null;
        }
    }

    void Initiate()
    {
        setState(State.Idle);
        setCombo(1);
        SetSpeed(5f);
        staminaBar.value = stamina;
        isAlreadyDamage = true;
        isGroggy = false;
        isDefense = false;
        PlayerCollider.enabled = true;
        playerTrigger.enabled = true;
        BattleParticleManager.StopAvoidParticle();
        animator.SetLayerWeight(1, 0);
    }


    private int getCombo()
    {
        return animator.GetInteger("Combo");
    }

    private void setCombo(int combo)
    {
        animator.SetInteger("Combo", combo);
    }

    public static float getVelocity()
    {
        return battleSwordman.rigidBody.velocity.x;
    }


    private float xVectorInput()
    {
        if (BattleGameManager.DeviceType == "PC")
        {
            return Input.GetAxisRaw("Horizontal");
        }
        else if (BattleGameManager.DeviceType == "Mobile")
        {
            return PlayerJoystick.JoystickX;
        }
        else
        {
            return 0;
        }
    }

    public static void AttackTrigger(Collider2D other)
    {
        if(battleSwordman.getState() == State.Attack && !battleSwordman.isAlreadyDamage)
        {
            battleSwordman.isAlreadyDamage = true;
            other.gameObject.GetComponent<Boss>().OnDamage(battleSwordman.damage);
            BattleParticleManager.CreateEnemyHitParticle(battleSwordman.EnemyHitPoint.transform.position);
            if(battleSwordman.isGroggyAttack)
            {
                battleSwordman.isGroggyAttack = false;
                other.gameObject.GetComponent<Boss>().OnStiffness(stiffnessPoint);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "LeftWall")
        {
            playerAttachLeftWall = true;
        }
        else if(collision.gameObject.tag == "RightWall")
        {
            playerAttachRightWall = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "LeftWall")
        {
            playerAttachLeftWall = false;
        }
        else if(collision.gameObject.tag == "RightWall")
        {
            playerAttachRightWall = false;
        }
    }
}

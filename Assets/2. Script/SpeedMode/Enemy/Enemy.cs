using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolableObject
{
    public Animator animator;

    protected int state;
    protected int order;

    protected int maxHealth;
    public int currentHealth;
    protected float moveSpeed;

    protected Transform frontEnemyTransform;

    protected virtual void Awake()
    {
        moveSpeed = SpeedMode.balance.ENEMY_MOVE_SPEED;
    }

    protected virtual void OnEnable()
    {
        animator.SetInteger("Action", state);
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        //자기 자신이 가장 앞에 있는 enemy일 경우 battlePos로 이동
        if (order == 0)
            transform.position = Vector3.MoveTowards(transform.position, GameManager.battlePos, moveSpeed * Time.deltaTime);
        //그렇지 않을 경우 자기 앞에 있는 enemy의 위치에 x좌표 +2한 위치로 이동
        else
            transform.position = Vector3.MoveTowards(transform.position, Battle.EnemyList[order - 1].gameObject.transform.position + SpeedMode.balance.ENEMY_ENEMY_GAP, moveSpeed * Time.deltaTime);
    }

    public void HideEnemy()
    {
        transform.SetParent(ObjectPool.objectPool.transform);
        gameObject.SetActive(false);
    }

    public void EmergeEnemy()
    {
        transform.SetParent(null);
        gameObject.SetActive(true);
    }

    public void SetState(int state)
    {
        this.state = state;
    }

    public void setOrder(int order)
    {
        this.order = order;
    }

    public void onDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        SpeedSoundManager.EnemySound();
        SetState(0);
    }
}

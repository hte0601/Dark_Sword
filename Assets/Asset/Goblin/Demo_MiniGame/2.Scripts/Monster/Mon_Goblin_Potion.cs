using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lofle;


public class Mon_Goblin_Potion : Mon_Bass
{

    protected StateMachine<Mon_Goblin_Potion> _stateMachine = null;

    [Header("[Option_Setting]")]
    public GameObject WeaponSocket;

    //public PhotonView m_Photonview;
    public Transform RotateSocket;

    public override void Init()
    {
 


   
            _stateMachine = new StateMachine<Mon_Goblin_Potion>(this);
            StateCo =  StartCoroutine(_stateMachine.Coroutine<RunState>());
          
       

        

    }
    public float StuneTime;
    public override void HittedFuc(float stunTime)
    {
        StuneTime = stunTime;
        StopCoroutine(StateCo);
        StateCo = StartCoroutine(_stateMachine.Coroutine<HitState>());
    }


    public GameObject FireBottle;
   protected void CreatePotion()
    {
   
            //float dir = this.transform.position.x - Current_Tartget.transform.position.x ;  // 적이 오른쪽이면 -  적이 왼쪽이면 +
            
            GameObject tmpobj = Instantiate(FireBottle, WeaponSocket.transform.position, WeaponSocket.transform.localRotation);

            if (bLeft)
            {
                tmpobj.transform.right = -RotateSocket.transform.right;
            }
            else
            {
                tmpobj.transform.right = RotateSocket.transform.right;
            }



            Vector3 pos1 = Current_Tartget.transform.position - this.transform.position;
       
            float tmpangle = Vector3.Angle(this.transform.up, pos1);

           
            tmpobj.GetComponent<PotionScript>().Fire(Current_Tartget.transform.position,tmpangle,m_Damage);
        

        // tmpobj.transform.right = dir;

    }
    //void Update()
    //{
   
    //}


 


    public override void Damaged(float DamageValue, Vector2 dir, float stunTime)
    {

        if (stunTime > 0)
            HittedFuc(stunTime);

        m_rigidbody2D.velocity = new Vector2(0, 0);
        m_rigidbody2D.AddForce(dir, ForceMode2D.Impulse);



        float PreHP = m_HP;
        m_HP -= DamageValue;

        if (DamageValue > 0)
            SetCreateBloodEffect(DamageValue);



        SyncHp(m_HP);





        if (m_HP <= 0)
        {

            StopCoroutine(StateCo);
            StateCo = StartCoroutine(_stateMachine.Coroutine<DieState>());

            // InGameManager.Instance.Del_mini_Monster(this.gameObject);


        }

    }



    public override void DefaulAttack_Collider(GameObject obj)
    {
  
       

        if (obj.CompareTag("Player") )
        {


      
                PlayerController tmp_Player = obj.transform.root.GetComponent<PlayerController>();

                Vector2 dir = new Vector2(0, 0);
                tmp_Player.Damaged(m_Damage, dir);

            


        }
    }
    public override void Skill_1Attack_Collider(GameObject obj)
    {

        if (!obj.CompareTag("Mon_WeaponCollider"))
            return;


    }
    public override void Skill_2Attack_Collider(GameObject obj)
    {

    

    }
    public override void Skill_3Attack_Collider(GameObject obj)
    {

  


    }
    public override void Skill_4Attack_Collider(GameObject obj)
    {

    }

    public bool b_DefaultAttack_Anim = false;

    public override void DefaultAttack_Anim_1_Enter()
    {
        // Is_OnceAttack = true;

      //Debug.Log("Attack1공격");
        b_DefaultAttack_Anim = true;
        CreatePotion();
        RotateSocketFuc(RotateSocket.transform.position, Current_Tartget.transform.position, 45);
    }

    public void RotateSocketFuc(Vector3 currentPos, Vector3 targetPos, float initialAngle)
    {
        if (RotateSocket == null)
            return;

        Vector3 v = targetPos - currentPos;

        float Angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;




        if (float.IsNaN(Angle))
        {

        }
        else
        {


            if (bLeft)
            {
                RotateSocket.transform.localRotation = Quaternion.AngleAxis(Angle - 180, Vector3.forward);
            }
            else
            {
                RotateSocket.transform.localRotation = Quaternion.AngleAxis(-Angle, Vector3.forward);
            }



        }


    }



    public override void DefaultAttack_Anim_1_Exit()
    {
        b_DefaultAttack_Anim = false;
    }

    private class IdleState : State<Mon_Goblin_Potion>
    {
        protected override void Begin()
        {
            Owner.SetAnim("Demo_Idle");
            RandomTime = 0.5f;
        }

        private float RandomTime = 1;
        private float TimeTic = 0;
        protected override void Update()
        {
          
            TimeTic += Time.deltaTime;
            if (TimeTic > RandomTime)
            {
                TimeTic = 0;

                if (Owner.Current_Tartget == null)
                {
                    Invoke<IdleState>();
                    return;
                }
                else
                {

                    Invoke<AttackState>();

                }


            
            }

        }

        protected override void End()
        {

        }

    }
    private class RunState : State<Mon_Goblin_Potion>
    {
        protected override void Begin()
        {

            Owner.SetAnim("Demo_Run");
        }
        private float updateTimeTic = 0;
        private float updateTime = 0.5f;


        protected override void Update()
        {


            if (Owner.Current_Tartget == null)
            {
                Invoke<IdleState>();
                return;
            }



            updateTimeTic += Time.deltaTime;

            Owner.Move();
            if (updateTimeTic > updateTime)
            {
                updateTimeTic = 0;


             

                float CurrentEnermyDis = Vector2.Distance(Owner.Current_Tartget.transform.position, Owner.transform.position);



                if (CurrentEnermyDis <= Owner.AttackDis)
                {
                    Invoke<AttackState>();
                }

            }

        }

        protected override void End()
        {

        }
    }
    private class AttackState : State<Mon_Goblin_Potion>
    {
        protected override void Begin()
        {
            Owner.SetAnim("Demo_Attack_Bow");
           
        }
     


        protected override void Update()
        {

            if (Owner.Current_Tartget == null)
            {
                Invoke<IdleState>();
                return;
            }

      
               

                float CurrentEnermyDis = Vector2.Distance(Owner.Current_Tartget.transform.position, Owner.transform.position);


                if (!Owner.b_DefaultAttack_Anim)
                {
                Owner.Filp();
                // Debug.Log("애니메이션 끝::");

                if (CurrentEnermyDis > Owner.AttackDis)
                    {
                        Invoke<RunState>();
                    }
                }

          

            
        }

        protected override void End()
        {
        }
    }




    private class HitState : State<Mon_Goblin_Potion>
    {
        protected override void Begin()
        {
            Owner.SetAnim("Demo_Hit");
            TimeTic = 0;
        }

       
        private float TimeTic = 0;
        protected override void Update()
        {
            

            TimeTic += Time.deltaTime;
            if (TimeTic >Owner. StuneTime)
            {
                TimeTic = 0;
                Invoke<IdleState>();
            }

        }

        protected override void End()
        {

        }

    }
    private class DieState : State<Mon_Goblin_Potion>
    {
        protected override void Begin()
        {
            Owner.SetAnim("Demo_Die");
            DieTime = 5;
        }
        private float DieTime = 5;
        private float TimeTic = 0;
        protected override void Update()
        {
         
            TimeTic += Time.deltaTime;
            if (TimeTic > DieTime)
            {
                TimeTic = 0;
                Destroy(Owner.gameObject);
            }

        }

        protected override void End()
        {

        }

    }


}

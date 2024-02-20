using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionScript : MonoBehaviour
{

    public float movingSpeed = 4f;

    public float destroyTime = 3f;

    public float Damage = 0;


    private Vector2 selfPos;
    private Quaternion selfRo;


    private Rigidbody2D m_rigidbody;

    bool m_bUpdateCheck = true;
    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();


    }

    public void  Fire(Vector3 targetPos,float angle, float damage)
    {
      //  Debug.Log("ㅇㅇㅇㅇㅇ::"+angle);

        if (angle>50)
            SetFire( targetPos, damage);
      else
            SetFire_2(targetPos, damage);
    }


    private void SetFire(Vector3 targetPos, float damage)
    {

        Damage = damage;
        //  Debug.Log("targetPos:" + targetPos);
        // Debug.Log("50이상");
        Vector3 velocity = GetVelocity(transform.position, targetPos, 45f);
        SetVelocity(velocity * 1.8f);  //중력값 3기준. 1.8f 
    }

    
    private void SetFire_2(Vector3 targetPos, float damage)
    {
        Damage = damage;
        // Debug.Log("50미만");
        Vector3 tmpPos = new Vector2(transform.position.x, transform.position.y - 2);
        Vector2 dir = targetPos - tmpPos;
        m_rigidbody.AddForce(dir * 130);
    }

    void SetVelocity(Vector3 velocity)
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    public GameObject FirePrefab;
    IEnumerator destroyObj()
    {
    //    Debug.Log("dkdkdjfkja111");
        yield return new WaitForSeconds(0);
     

        GameObject tmpobj = Instantiate(FirePrefab, new Vector3( this.transform.position.x, this.transform.position.y+0.2f, this.transform.position.z), Quaternion.identity);
        tmpobj.GetComponent<PotionFireScript>().Setdamage(Damage);
        Destroy(this.gameObject);
      //  Debug.Log("22222222");
    }


    Vector3 v1;
    void Update()
    {
        if (m_bUpdateCheck == false) return;

        this.transform.right = m_rigidbody.velocity;

    }



    void smoothMove()
    {

        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 8);
        transform.rotation = Quaternion.Lerp(transform.rotation, selfRo, Time.deltaTime * 8);


    }

    Vector3 GetVelocity(Vector3 currentPos, Vector3 targetPos, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 planarPosition = new Vector3(currentPos.x, 0, currentPos.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = currentPos.y - targetPos.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (targetPos.x > currentPos.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }



    Collider2D[] colliderpoint = new Collider2D[1]; 
    void OnTriggerEnter2D(Collider2D other)
    {


        if (!m_bUpdateCheck)
            return;

  

            if (other.CompareTag("Player") )
            {

                m_bUpdateCheck = false;
                m_rigidbody.isKinematic = true;
                m_rigidbody.velocity = new Vector2(0, 0);
                this.transform.SetParent(other.transform);

      
                    PlayerController tmp_Player = other.transform.root.GetComponent<PlayerController>();
                    Vector2 dir = new Vector2(0, 0);
                    tmp_Player.Damaged(1, dir);
                

                StartCoroutine("destroyObj");
            }
            else if (other.CompareTag("Ground"))
            {
                m_bUpdateCheck = false;
                m_rigidbody.isKinematic = true;
                m_rigidbody.velocity = new Vector2(0, 0);
                this.transform.SetParent(other.transform);
                StartCoroutine("destroyObj");
             
            }

           
        

    }



}

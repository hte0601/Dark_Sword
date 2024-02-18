using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionFireScript : MonoBehaviour
{

    public float destroyTime = 10f;
    public float Damage;
    bool m_bUpdateCheck = true;
    void Awake()
    {
        StartCoroutine(destroyObj());
    }


 
    void SetVelocity(Vector3 velocity)
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    IEnumerator destroyObj()
    {
        yield return new WaitForSeconds(destroyTime);

       Destroy(gameObject);




    }

    public void Setdamage( float damage)
    {
        //  Debug.Log("ㅇㅇㅇㅇㅇ::"+angle);

        Damage = damage;

    }


 



    public List<GameObject> HittedObjectList = new List<GameObject>();


    void OnTriggerEnter2D(Collider2D other)
    {
       

        if (!HittedObjectList.Contains(other.gameObject))
        {
            HittedObjectList.Add(other.gameObject);
        }
        else
        {
            return;
        }




        if (other.CompareTag("Player") )
        {

         
                PlayerController tmp_Player = other.transform.root.GetComponent<PlayerController>();
             
                tmp_Player.FireDamage(Damage, 2);

      
        }


    
    }

    void OnTriggerExit2D(Collider2D other)
    {
       


        HittedObjectList.Remove(other.gameObject);

    }


 
   
}

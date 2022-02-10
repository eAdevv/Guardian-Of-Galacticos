using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillSystem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

       /* if (this.gameObject.tag.Equals("Laser"))
        {
            if (collision.gameObject.tag.Equals("Enemy") || collision.gameObject.tag.Equals("EnemyKamikaze"))
            {
                GetComponentInParent<Shooting>().totalScore += collision.gameObject.GetComponent<EnemySystem>().score;
                GameObject Go = Instantiate(collision.gameObject.GetComponent<EnemySystem>().explosion_effect, collision.gameObject.transform.position, Quaternion.identity) as GameObject;
                Destroy(Go.gameObject, 1.75f);
                Destroy(collision.gameObject);
            }
        }
       */
        
       
       
    }
}

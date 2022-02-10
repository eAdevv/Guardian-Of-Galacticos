using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySystem : MonoBehaviour
{
    [Header ("Enemy Attributes")]
    public int score;
    public float enemyHealth;
    public GameObject explosion_effect;
    
    [Header("Enemy Shooting System")]
    public GameObject bulletEnemy;
    public float bulletEnemyForce;
    public Transform enemyFirePos;
    public float Range;

    [Header ("AI System")]
    public bool detected;
    private NavMeshAgent agent;

    Vector2 direction;
    Transform target;
    Shooting shooting;
    AudioSource audio_Source;
    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        audio_Source = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        shooting = player.GetComponent<Shooting>();
        target = player.transform;
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    void Update()
    {
        if (player.activeSelf == true)
        {
            agent.SetDestination(target.position);

            #region FACE TARGET

            Vector2 targetPos = target.position;

            direction = targetPos - (Vector2)transform.position;

            RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, direction, Range);

            if (rayInfo)
            {
                if (rayInfo.collider.gameObject.tag == "Player")
                {
                    if (detected == false)
                        detected = true;
                }
                else
                    detected = true;
            }

            if (detected)
                gameObject.transform.up = direction;

            #endregion
        }
        else
        {
            Destroy(gameObject);
        }
        

        if (enemyHealth <= 0)
        {
            shooting.totalScore += score;
            GameObject go = Instantiate(explosion_effect, gameObject.transform.position, Quaternion.identity) as GameObject;
            audio_Source.PlayOneShot(shooting.enemyExplosion,0.75f);
            Destroy(go.gameObject, 1.75f);
            Destroy(gameObject);      
        }        
    }

    private void Awake()
    {
        if(gameObject.tag.Equals("Enemy"))
            StartCoroutine(enemyShooting());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Bullet"))
        {
            enemyHealth -= shooting.bulletDamage;
            audio_Source.PlayOneShot(shooting.impactSound,0.05f);
            Destroy(collision.gameObject);
        }    
    }

    IEnumerator enemyShooting()
    {
        GameObject enemyBullet = Instantiate(bulletEnemy, enemyFirePos.position, enemyFirePos.rotation) as GameObject;
        Rigidbody2D rb = enemyBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * bulletEnemyForce, ForceMode2D.Impulse);
        Destroy(enemyBullet, 1.3f);
        yield return new WaitForSeconds(2f);
        StartCoroutine(enemyShooting());    
    }
}

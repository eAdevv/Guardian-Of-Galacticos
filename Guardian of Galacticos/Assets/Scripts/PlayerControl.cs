using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    [Header("Attributes")]
    public float speed = 5f;
    public int playerHealth = 100; 
    
    [Header("Skill System")]
    public GameObject shieldObj;
    public GameObject laserObj;
    public TMP_Text skillTextOne;
    public TMP_Text skillTextThree;
    public TMP_Text aidKitText;
    public float coolDownTime_one;
    public float coolDownTime_Three;
    public float coolDownTime_aidKit;
    public bool isSkillOneActivated;
    public bool isSkillThreeActivated;
    public bool skillOne_Used = false;
    public bool skillThree_Used = false;
    public bool aidKit_Used = false;


    [Header("Others")]
    public GameObject playerExplosion;
    public GameObject deadPanel;
    public Rigidbody2D rb;
    public Camera cam;
    public HealthBarScript healthBar;
    public AudioClip bladeSound, laserActive;
    Vector2 movement;
    Vector2 mousePosition;
    Shooting shooting;
    buttonKeyboard ButtonKeyboard;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isSkillOneActivated = false;
        shooting = gameObject.GetComponent<Shooting>();
        healthBar.SetMaxHealth(playerHealth);

        GameObject.Find("Player Explosion Voice").GetComponent<AudioSource>().Stop();
    }
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        if (playerHealth <= 0)
        {
            deadSystem();
        }
        
        if(Input.GetKeyDown(KeyCode.H) && aidKit_Used == false && playerHealth < 100)
        {
            playerHealth += 50;
            aidKit_Used = true;        
        }
      
        if(aidKit_Used)
            coolDownSystem(ref coolDownTime_aidKit,60f, ref aidKit_Used, aidKitText);

        if (skillOne_Used)
            coolDownSystem(ref coolDownTime_one, 16f, ref skillOne_Used,skillTextOne);  
        
        if (skillThree_Used)
            coolDownSystem(ref coolDownTime_Three, 25f, ref skillThree_Used, skillTextThree);        
       
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        Vector2 lookDirection = mousePosition - rb.position;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if(laserObj.activeSelf == true)
        {
            if(collision.gameObject.tag.Equals("Enemy") || collision.gameObject.tag.Equals("EnemyKamikaze"))
            {
                effect_sound_Control(collision,bladeSound);
                GetComponent<Shooting>().totalScore += collision.gameObject.GetComponent<EnemySystem>().score;
            }
            else if (collision.gameObject.tag.Equals("bulletEnemy"))
            {
                Destroy(collision.gameObject);
            }
        }
        else if(shieldObj.activeSelf == true)
        {
            if(collision.gameObject.tag.Equals("EnemyKamikaze"))
            {
                effect_sound_Control(collision, GetComponent<Shooting>().enemyExplosion);
            }
            else if (collision.gameObject.tag.Equals("bulletEnemy"))
            {
                Destroy(collision.gameObject);
            }
        }
        else
        {
            if (collision.gameObject.tag.Equals("EnemyKamikaze"))
            {
                effect_sound_Control(collision, GetComponent<Shooting>().enemyExplosion);
                playerHealth -= 50;
            }
            else if(collision.gameObject.tag.Equals("bulletEnemy"))
            {
                playerHealth -= 20;
                Destroy(collision.gameObject);
            }            
            healthBar.SetHealth(playerHealth);
        }

    }

    void deadSystem()
    {
        gameObject.SetActive(false);
        GameObject playerExp = Instantiate(playerExplosion, gameObject.transform.position, Quaternion.identity) as GameObject;
        Destroy(playerExp.gameObject, 1.75f);
        GameObject.Find("Background Music").GetComponent<AudioSource>().Stop();
        GameObject.Find("Player Explosion Voice").GetComponent<AudioSource>().Play();
        deadPanel.SetActive(true);
    }

    public void shieldOpen() // Button 1 access this Function.
    {       
        skillOpen(ref skillOne_Used, shieldObj,ref isSkillOneActivated,6f);           
    }

    public void laserOpen() // Button 3 access this Function.
    {
        skillOpen(ref skillThree_Used, laserObj,ref isSkillThreeActivated,10f);
    }

    IEnumerator skillClose(GameObject obj,float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
        isSkillOneActivated = false;
        isSkillThreeActivated = false;
        speed = 5f;
        shooting.ammo = 12f;
    }

    void skillOpen(ref bool isUsed,GameObject obj,ref bool isActivated,float skillTime)
    {
        if (isSkillThreeActivated == false && isSkillOneActivated == false && shooting.isSkillTwoActivated == false && isUsed == false)
        {
            isUsed = true;
            obj.SetActive(true);
            isActivated = true;
            StartCoroutine(skillClose(obj, skillTime));
        }
        if(isSkillOneActivated == true)
        {
            speed = 7.5f;
            shooting.ammo = 24f;
        }
    }

    public void coolDownSystem(ref float coolDownTime , float temp , ref bool isUsed,TMP_Text coolDownText)
    {
        coolDownTime -= Time.deltaTime;
        coolDownText.text = (int)coolDownTime+"";
        if (coolDownTime <= 0f)
        {
            coolDownText.text = null;
            coolDownTime = temp;
            isUsed = false;
        }
    }

    void effect_sound_Control(Collider2D collision,AudioClip Voice)
    {
        GameObject Go = Instantiate(collision.gameObject.GetComponent<EnemySystem>().explosion_effect, collision.gameObject.transform.position, Quaternion.identity) as GameObject;
        GetComponent<AudioSource>().PlayOneShot(Voice, 0.5f);
        Destroy(Go.gameObject, 1.75f);
        Destroy(collision.gameObject);

    }

    private void OnParticleCollision(GameObject other)
    {
        playerHealth = 0;
    }
}

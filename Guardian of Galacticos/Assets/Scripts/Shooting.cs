using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Shooting : MonoBehaviour
{
    [Header("Shooting System")]
    public GameObject bullet;
    public Transform firePoint;
    public float ammo;
    public float reloadTime = 3f;
    public float bulletForce = 10f;
    public float bulletDamage = 50f;
    public TMP_Text ammoText;

    [Header("Auto Gun (Skill Two)")]
    public float coolDownTime_Two;
    public bool skillTwo_Used = false;
    public bool isSkillTwoActivated;
    public TMP_Text skillTextTwo;

    [Header("Game Sounds")]
    public AudioClip fireSound;
    public AudioClip impactSound;
    public AudioClip enemyExplosion;

    [Space]
    public int totalScore;
    public TMP_Text scoreText;

    PlayerControl playerControl;
    private void Start()
    {
        isSkillTwoActivated = false;
        playerControl = gameObject.GetComponent<PlayerControl>();
        ammoText.text = ammo + "";
        totalScore = 0;
        scoreText.text = totalScore + "";

    }
    void Update()
    {
        ammoText.text = ammo + "";
        scoreText.text = totalScore + "";
        if (ammo > 0 || isSkillTwoActivated)
        {
            if(isSkillTwoActivated) // Skill Two (Auto Gun) System
            {
                bulletDamage = 7f; // For Balance
                if (Input.GetButton("Fire1")) 
                {                   
                    Shoot(0.03F);                   
                }
            }
            else if (Input.GetButtonDown("Fire1") && playerControl.isSkillThreeActivated == false) // Default Gun System
            {
                Shoot(0.3F);
                ammo -= 1f;
            }
        }
        else // Reloading for Default Gun System
        {
            reloadTime -= Time.deltaTime;
            if(reloadTime <= 0 )
            {
                ammo = 16f;
                reloadTime = 3f;
            }
        }

        if (skillTwo_Used)      
            playerControl.coolDownSystem(ref coolDownTime_Two, 16f, ref skillTwo_Used,skillTextTwo); 
                

    }

    void Shoot(float soundVolume)
    {
         GameObject bulletGameObj = Instantiate(bullet, firePoint.position, firePoint.rotation) as GameObject;
         Rigidbody2D rb = bulletGameObj.GetComponent<Rigidbody2D>();
         rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
         GetComponent<AudioSource>().PlayOneShot(fireSound, soundVolume);
         Destroy(bulletGameObj.gameObject, 2f);
    }

    public void autoGunOpen() // Button 2 access this Function.
    {
        if(isSkillTwoActivated == false && playerControl.isSkillOneActivated == false && playerControl.isSkillThreeActivated == false && skillTwo_Used == false)
        {
            skillTwo_Used = true;
            isSkillTwoActivated = true;
            ammo = 12f;
            StartCoroutine(autoGunClose());
        }
    }

    IEnumerator autoGunClose()
    {
        yield return new WaitForSeconds(8f);
        isSkillTwoActivated = false;
        bulletDamage = 50f;
    }
    



}

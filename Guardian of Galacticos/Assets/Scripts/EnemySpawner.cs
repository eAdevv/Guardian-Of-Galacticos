using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    private float randomXposition;
    private float randomYposition;
    private float randomSpawner;
    public float randomLaserCombinationSpawner;
    public float laserTime = 45f;
    public float laserCoolDown = 45f;
    private Vector3 spawnPos;

    public GameObject [] enemies;
    public GameObject [] laserBoundaries;
    public float timer = 1.5f;

    GameObject player;
    Shooting shooting;
    void Start()
    {
        player = GameObject.Find("Player");
        StartCoroutine(enemySpawner());
        shooting = player.GetComponent<Shooting>();
        randomLaserCombinationSpawner = Random.Range(0, 8);
    }

    
    private void Update()
    {
        if (player.activeSelf == true && shooting.totalScore >= 2500) // LASER BOUNDARIES CONTROL
        {

            timer = 0.85f;
            switch (randomLaserCombinationSpawner)
            {
                case 0:
                    laserBoundaries[0].SetActive(true);
                    laserBoundaries[1].SetActive(true);     // COMBINATION 1
                    break;
                case 1:
                    laserBoundaries[2].SetActive(true);     // COMBINATION 2
                    laserBoundaries[3].SetActive(true);
                    break;
                case 2:
                    laserBoundaries[0].SetActive(true);
                    laserBoundaries[1].SetActive(true);     // COMBINATION 3
                    laserBoundaries[2].SetActive(true);
                    laserBoundaries[3].SetActive(true);
                    break;
                case 3:
                    laserBoundaries[0].SetActive(true);
                    laserBoundaries[2].SetActive(true);     // COMBINATION 4
                    break;
                case 4:
                    laserBoundaries[0].SetActive(true);     
                    laserBoundaries[3].SetActive(true);     // COMBINATION 5
                    break;
                case 5:
                    laserBoundaries[1].SetActive(true);
                    laserBoundaries[2].SetActive(true);     // COMBINATION 6
                    break;
                case 6:
                    laserBoundaries[1].SetActive(true);
                    laserBoundaries[2].SetActive(true);
                    laserBoundaries[3].SetActive(true);     // COMBINATION 7
                    break;
                case 7:
                    laserBoundaries[0].SetActive(true);
                    laserBoundaries[2].SetActive(true);
                    laserBoundaries[3].SetActive(true);     // COMBINATION 8
                    break;
            }

            laserTime -= Time.deltaTime;

            if (laserTime <= 0)
            {
                laserCoolDown -= Time.deltaTime;

                for (int i = 0; i < laserBoundaries.Length; i++)
                {
                    if (laserBoundaries[i].activeSelf == true)
                        laserBoundaries[i].SetActive(false);
                }

                if (laserCoolDown <= 0)
                {
                    laserTime = 45f;
                    laserCoolDown = 45f;
                    randomLaserCombinationSpawner = Random.Range(0, 8); // RANDOM COMBINATION
                }

            }
        }
        else if (player.activeSelf == false)
        {
            for (int i = 0; i < laserBoundaries.Length; i++)
                Destroy(laserBoundaries[i]);
        }
    }

    IEnumerator enemySpawner()
    {
        if (player.activeSelf == true)
        {
            randomSpawner = Random.Range(0, 4);

            switch (randomSpawner)
            {
                case 0:
                    randomXposition = Random.Range(-9f, 10f);   /// TOP AREA
                    randomYposition = Random.Range(5.5f, 8f);
                    break;
                case 1:
                    randomXposition = Random.Range(-9f, 10f);   /// BOTTOM AREA
                    randomYposition = Random.Range(-5.5f, -8f);
                    break;
                case 2:
                    randomXposition = Random.Range(9.5f, 12f);   /// RIGHT AREA
                    randomYposition = Random.Range(-5f, 6f);
                    break;
                case 3:
                    randomXposition = Random.Range(-9.5f, -12f);   /// RIGHT AREA
                    randomYposition = Random.Range(-5f, 6f);
                    break;

            }

            spawnPos = new Vector3(randomXposition, randomYposition, 0f);
            Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(timer);
            StartCoroutine(enemySpawner());
        }
      
    }

}

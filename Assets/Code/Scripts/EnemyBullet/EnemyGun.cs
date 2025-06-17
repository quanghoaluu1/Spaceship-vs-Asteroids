using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{

    public GameObject EnemyBullet;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("FireEnemyBullet", 1f, 2f); // Bắn lần đầu sau 1s, rồi cứ 2s bắn 1 viên
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    void FireEnemyBullet()
    {
        GameObject playerShip = GameObject.Find("Player");

        if (playerShip != null)
        {
            GameObject bullet = (GameObject)Instantiate(EnemyBullet);

            bullet.transform.position = transform.position;

            Vector2 direction = playerShip.transform.position - bullet.transform.position;

            bullet.GetComponent<EnemyBullet>().SetDirection(direction);
        }
    }
}
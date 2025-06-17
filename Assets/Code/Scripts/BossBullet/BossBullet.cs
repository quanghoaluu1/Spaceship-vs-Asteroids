using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Vector2 direction;
    float speed;
    Vector2 _direction;
    bool isReady;
    public AudioClip getHitSound;
    private AudioSource audioSource;

    void Awake()
    {
        speed = 5f;
        isReady = false;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;

        isReady = true;
    }

    void Update()
    {
        if (isReady)
        {
            Vector2 position = transform.position;
            position += _direction * speed * Time.deltaTime;
            transform.position = position;

            // Huỷ viên đạn nếu ra ngoài màn hình
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            if ((transform.position.x < min.x) || (transform.position.x > max.x) ||
                (transform.position.y < min.y) || (transform.position.y > max.y))
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController == null) return;


            if (playerController.IsInvincible())
            {
                Debug.Log("Player đang bất tử, không mất máu. Còn " + playerController.IsInvincibleTime() + " giây.");
                Destroy(this.gameObject);
                return;
            }

            //PlaySoundAtPosition(getHitSound, transform.position, 5f);

            //Nếu đến đây là chắc chắn chưa bất tử → xử lý mất máu và kích hoạt khiên
            playerController.ActivateShield(); // Bật trạng thái bất tử + khiên + nhấp nháy

            playerController.LoseLife();
            Destroy(this.gameObject);
        }
    }
    private void PlaySoundAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.spatialBlend = 0f; // 0 = 2D sound
        aSource.Play();

        Destroy(tempGO, clip.length);
    }
}
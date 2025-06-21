using System.Collections;
using UnityEngine;
using static CustomizablePowerUp;

public class TakeablePowerUp : MonoBehaviour
{
    CustomizablePowerUp customPowerUp;

    void Start()
    {
        customPowerUp = GetComponent<CustomizablePowerUp>();
        //this.audio.clip = customPowerUp.pickUpSound;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            PowerUpManager.Instance.Add(customPowerUp);
            if (customPowerUp.pickUpSound != null)
            {
                PlaySoundAtPosition(customPowerUp.pickUpSound, transform.position, 1.5f);
            }

            ApplyPowerUpEffect(collider.gameObject);

            Destroy(gameObject);
        }
    }

    void PlaySoundAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.spatialBlend = 0f;
        aSource.Play();

        Destroy(tempGO, clip.length);
    }

    void ApplyPowerUpEffect(GameObject player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        switch (customPowerUp.powerUpType)
        {
            case CustomizablePowerUp.PowerUpType.Health:
                controller.Heal(customPowerUp.healthAmount);
                break;

            case CustomizablePowerUp.PowerUpType.Shield:
                controller.ActivateShieldNoBlink();
                break;

            case CustomizablePowerUp.PowerUpType.Speed:
                controller.ApplySpeedBoost(duration: 5f, multiplier: 1.5f);
                break;

            case CustomizablePowerUp.PowerUpType.Bomb:
                controller.TriggerBomb();
                controller.ApplyEffect();
                break;
        }
    }
}

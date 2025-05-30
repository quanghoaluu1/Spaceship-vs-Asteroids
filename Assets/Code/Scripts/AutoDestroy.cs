using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 0.5f;
    public AudioClip explosionClip;
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = explosionClip;
        audio.Play();
        Destroy(gameObject, lifetime);
    }

}

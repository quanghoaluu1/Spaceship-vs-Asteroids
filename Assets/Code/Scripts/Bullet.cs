using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    private float fireCooldown = 0.5f;
    private float lastFireTime = 0f;

    public AudioClip laserSound;
    private AudioSource audioSource;
    private PlayerInputActions inputActions;

    private static int maxBullets = 10;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Fire.performed += ctx => Fire();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Fire()
    {
        if (GameObject.FindGameObjectsWithTag("LaserShot").Length >= maxBullets)
        {
            Debug.LogWarning("Max bullet limit reached");
            return;
        }

        if (Time.time - lastFireTime < fireCooldown) return;
        lastFireTime = Time.time;

        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Prefab or firePoint not assigned");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = firePoint.up * bulletSpeed;
        audioSource.PlayOneShot(laserSound);
        Destroy(bullet, 1.75f);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject firePoint;  // Đổi thành GameObject
    public float bulletSpeed = 10f;

    private float fireCooldown = 0.5f;
    private float lastFireTime = 0f;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Fire.performed += ctx => Fire();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Fire()
    {
        if (Time.time - lastFireTime < fireCooldown) return;
        lastFireTime = Time.time;

        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Prefab or firePoint not assigned");
            return;
        }

        // Lấy vị trí từ transform của firePoint GameObject
        GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.right * bulletSpeed;

        Destroy(bullet, 1.75f);
    }
}

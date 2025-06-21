using UnityEngine;

public class LazerShooter : MonoBehaviour
{
    public GameObject laserBeamPrefab;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    private Transform player;

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("SpawnPop");
        }
    }

    public void SetTarget(Transform playerTransform)
    {
        player = playerTransform;

        // Xoay trục X ngay khi spawn
        Vector3 direction = (player.position - transform.position).normalized;
        transform.right = direction;
    }

    public void FireLaserImmediately()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 shootFrom = transform.position;
        Vector3 targetPoint = transform.position + direction * 20f;

        GameObject laser = Instantiate(laserBeamPrefab, Vector3.zero, Quaternion.identity, transform);
        laser.GetComponent<LaserBeam>().Init(shootFrom, targetPoint);
    }
}

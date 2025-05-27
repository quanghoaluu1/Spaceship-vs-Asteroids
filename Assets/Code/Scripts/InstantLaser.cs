using UnityEngine;
using UnityEngine.InputSystem;

public class InstantLaser : MonoBehaviour
{
    [Header("Thiết lập laser")]
    public Transform firePoint;
    public float maxDistance = 100f;
    public LineRenderer lineRenderer;

    [Header("Layer để raycast trúng")]
    public LayerMask hitLayers;

    [Header("Hiệu ứng laser")]
    public float laserDuration = 0.1f;

    private bool isLaserActive = false;
    private float laserTimer = 0f;

    private void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        // Gán material đơn giản
        Material laserMat = new Material(Shader.Find("Unlit/Color"));
        laserMat.color = Color.cyan;
        lineRenderer.material = laserMat;

        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        // Nhấn để bắn
        if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            isLaserActive = true;
            laserTimer = laserDuration;
            lineRenderer.enabled = true;
        }

        // Nếu đang hiển thị laser thì cập nhật tia
        if (isLaserActive)
        {
            UpdateLaser();

            laserTimer -= Time.deltaTime;
            if (laserTimer <= 0f)
            {
                isLaserActive = false;
                lineRenderer.enabled = false;
            }
        }
    }

    private void UpdateLaser()
    {
        RaycastHit hit;
        Vector3 startPos = firePoint.position;
        Vector3 direction = firePoint.up;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);

        if (Physics.Raycast(startPos, direction, out hit, maxDistance, hitLayers))
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, startPos + direction * maxDistance);
        }
    }
}

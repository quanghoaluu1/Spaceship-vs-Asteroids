using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(EdgeCollider2D))]
public class LaserBeam : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float laserDuration = 0.8f;
    public float fadeDuration = 0.1f;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private float elapsedTime;

    private EdgeCollider2D edgeCollider;
    private bool hasHitPlayer = false;

    public void Init(Vector3 from, Vector3 to)
    {
        startPoint = from;
        endPoint = to;
        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.isTrigger = true; // Dùng trigger để đơn giản hóa
        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, startPoint); // Start với độ dài 0

        elapsedTime = 0f;
        while (elapsedTime < laserDuration)
        {
            float t = elapsedTime / laserDuration;
            Vector3 currentPoint = Vector3.Lerp(startPoint, endPoint, t);
            lineRenderer.SetPosition(1, currentPoint);

            // Cập nhật EdgeCollider theo LineRenderer
            List<Vector2> points = new List<Vector2>
            {
                transform.InverseTransformPoint(startPoint),
                transform.InverseTransformPoint(currentPoint)
            };
            edgeCollider.SetPoints(points);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        transform.parent = null;

        // Fade out
        float fadeTime = 0f;
        Gradient grad = lineRenderer.colorGradient;
        GradientColorKey[] colorKey = grad.colorKeys;

        while (fadeTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, fadeTime / fadeDuration);
            lineRenderer.colorGradient = new Gradient
            {
                colorKeys = colorKey,
                alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey(alpha, 0f),
                    new GradientAlphaKey(0f, 1f)
                }
            };
            fadeTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitPlayer) return;

        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController == null) return;

            if (!playerController.IsInvincible())
            {
                playerController.ActivateShield();
            }

            playerController.TakeDamage(30);

            hasHitPlayer = true;
            // Nếu muốn tia biến mất ngay khi va chạm:
            //Destroy(gameObject);
        }
    }
}

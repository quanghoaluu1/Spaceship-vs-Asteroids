using UnityEngine;

public class PowerUpMover : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        // Di chuyển sang trái
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Nếu vượt khỏi rìa trái của màn hình thì huỷ object
        float leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

        if (transform.position.x < leftEdge - 1f)
        {
            Destroy(gameObject);
        }
    }
}

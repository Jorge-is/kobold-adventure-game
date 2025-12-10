using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [Tooltip("Point A (start)")]
    public Transform pointA;
    [Tooltip("Point B (end)")]
    public Transform pointB;

    public float speed = 2f;
    public float waitTimeAtPoint = 0.2f; // pausa al llegar al punto
    public bool startAtA = true;

    Rigidbody2D rb;
    float waitTimer = 0f;
    Transform currentTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Validación
        if (pointA == null || pointB == null)
        {
            Debug.LogError("¡Debes asignar PointA y PointB en el inspector!");
            enabled = false;
            return;
        }

        currentTarget = startAtA ? pointB : pointA; // si empezamos en A, ir a B primero
    }

    void FixedUpdate()
    {
        if (waitTimer > 0f)
        {
            waitTimer -= Time.fixedDeltaTime;
            return;
        }

        Vector2 target = currentTarget.position;
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        // si llegó al target (con tolerancia)
        if (Vector2.Distance(rb.position, target) < 0.01f)
        {
            // intercambiar objetivo
            currentTarget = currentTarget == pointA ? pointB : pointA;
            waitTimer = waitTimeAtPoint;
        }
    }
}

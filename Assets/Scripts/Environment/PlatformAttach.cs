using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    [Tooltip("Tag del jugador")]
    public string playerTag = "Player";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
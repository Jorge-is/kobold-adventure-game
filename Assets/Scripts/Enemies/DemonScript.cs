using UnityEngine;

public class DemonScript : MonoBehaviour
{
    [SerializeField] private int Vida = 3;
    private Animator animator;
    private bool estaMuerto = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si lo golpea una bala del jugador
        if (collision.CompareTag("Bala") && !estaMuerto)
        {
            TomarDano(1);
            Destroy(collision.gameObject); // destruir bala al impactar
        }

        // Si toca al jugador
        if (collision.CompareTag("Player") && !estaMuerto)
        {
            collision.GetComponent<Kobold>().Golpear();
        }
    }

    public void TomarDano(int dano)
    {
        Vida -= dano;
        if (Vida <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        estaMuerto = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false; // desactiva físicas
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false; // evita nuevas colisiones

        if (animator != null)
        {
            animator.SetTrigger("death");
        }

        Destroy(gameObject, 1.2f); // espera a que acabe la animación
    }
}
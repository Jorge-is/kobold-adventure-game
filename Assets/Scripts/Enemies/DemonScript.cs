using UnityEngine;

public class DemonScript : MonoBehaviour
{
    [SerializeField] private int Vida = 4;
    private Animator animator;
    private bool estaMuerto = false;
    private Rigidbody2D rb;
    private Collider2D col;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (estaMuerto) return;

        // Si lo golpea una bala del jugador
        if (collision.CompareTag("Bala"))
        {
            TomarDano(1);
            Destroy(collision.gameObject); // destruir bala al impactar
        }

        // Si toca al jugador
        if (collision.CompareTag("Player"))
        {
            var kobold = collision.GetComponent<Kobold>();
            if (kobold != null) kobold.Golpear();
        }
    }

    public void TomarDano(int dano)
    {
        if (estaMuerto) return;

        Vida -= dano;

        // Reproducir animación de daño
        if (animator != null)
        {
            animator.SetTrigger("hurt");
        }

        // Si ya no tiene vida ejecuta la animación de muerte
        if (Vida <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        if (estaMuerto) return;
        estaMuerto = true;

        // Desactivar físicas y colisiones para que no interfieran mientras muere
        if (rb != null) rb.simulated = false; 
        if (col != null) col.enabled = false;

        // Lanzar el trigger en el Animator
        if (animator != null)
        {
            // Asegurarse de que la animación de daño no interfiera con la de muerte
            animator.ResetTrigger("hurt");
            animator.ResetTrigger("death");

            // Reproduce directamente el estado "Death" sin esperar transiciones
            animator.Play("DeathDemonAnimation", 0, 0f);
        }
    }

    // Método para ser llamado por un Animation Event al final del clip Death
    public void OnDeathAnimComplete()
    {
        Destroy(gameObject);
    }
}
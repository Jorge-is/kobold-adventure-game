using UnityEngine;

public class BalaScript : MonoBehaviour
{
    public AudioClip Sonido;
    public float Velocidad;

    private Rigidbody2D Rigidbody2D;
    private Vector2 Direccion;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Sonido);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.linearVelocity = Direccion * Velocidad;
    }

    public void SetDireccion(Vector2 direccion)
    {
        Direccion = direccion;

        // Ajustar orientación del sprite de la bala
        if (direccion.x != 0)
        {
            // Voltea el sprite según la dirección en el eje x
            transform.localScale = new Vector3(Mathf.Sign(direccion.x), 1, 1);
        }
    }

    public void DestruirBala()
    {
        Destroy(gameObject);
    }
}

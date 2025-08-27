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
    }

    public void DestruirBala()
    {
        Destroy(gameObject);
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Kobold kobold = collision.GetComponent<Kobold>();
        //GruntScript grunt = collision.GetComponent<GruntScript>();
        if (kobold != null)
        {
            kobold.Golpear();
        }
        if (grunt != null)
        {
            grunt.Golpear();
        }
        DestruirBala();
    }*/
}

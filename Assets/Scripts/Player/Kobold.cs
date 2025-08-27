using UnityEngine;

public class Kobold : MonoBehaviour
{
    public GameObject BalaPrefab;
    [SerializeField] private Transform PuntoDisparo;
    //[SerializeField] private float rango;
    //[SerializeField] private GameObject EfectoImpacto;
    public float Velocidad;
    public float FuerzaSalto;

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float EntradaHorizontal;
    private bool ConectadoTierra;
    private float UltimoDisparo;
    private int vida = 5;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        EntradaHorizontal = Input.GetAxisRaw("Horizontal");

        if (EntradaHorizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (EntradaHorizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Animator.SetBool("running", EntradaHorizontal != 0.0f /*&& ConectadoTierra*/);

        if (Physics2D.Raycast(transform.position, Vector3.down, 1f))
        {
            ConectadoTierra = true;
        }
        else ConectadoTierra = false;
        
        if (Input.GetKeyDown(KeyCode.W) && ConectadoTierra)
        {
            Saltar();
        }
        
        if (Input.GetKey(KeyCode.Space) && Time.time > UltimoDisparo + 0.25f)
        {
            Disparar();
            UltimoDisparo = Time.time;
        }
    }
    
    private void Saltar()
    {
        Rigidbody2D.AddForce(Vector2.up * FuerzaSalto);
    }
    
    private void Disparar()
    {
        Vector3 direccion;
        if (transform.localScale.x == 1.0f) direccion = Vector3.right;
        else direccion = Vector3.left;

        //GameObject bala = Instantiate(BalaPrefab, transform.position + direccion * 0.1f, Quaternion.identity);
        GameObject bala = Instantiate(BalaPrefab, PuntoDisparo.position, Quaternion.identity);

        //Ignorar la colisión entre el jugador y la bala
        Collider2D KoboldCollider = GetComponent<Collider2D>();
        Collider2D BalaCollider = bala.GetComponent<Collider2D>();
        if (KoboldCollider != null && BalaCollider != null)
        {
            Physics2D.IgnoreCollision(KoboldCollider, BalaCollider);
        }

        bala.GetComponent<BalaScript>().SetDireccion(direccion);
    }

    /*
    private void Disparar()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(PuntoDisparo.position, PuntoDisparo.right, rango);

        if (raycastHit2D)
        {
            if (raycastHit2D.transform.CompareTag("Enemigo"))
            {
                raycastHit2D.transform.GetComponent<Enemigo>().TomarDano(20);
                Instantiate(EfectoImpacto, raycastHit2D.point, Quaternion.identity);
            }
        }
    }*/

    public void Golpear()
    {
        vida--;
        if (vida == 0) Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.linearVelocity = new Vector2(EntradaHorizontal * Velocidad, Rigidbody2D.linearVelocity.y);
    }
}

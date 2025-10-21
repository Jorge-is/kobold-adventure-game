using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kobold : MonoBehaviour
{
    public GameObject BalaPrefab;
    [SerializeField] private Transform PuntoDisparo;

    private float mover;

    public float Velocidad = 5;
    public float FuerzaSalto = 4;

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;

    private bool ConectadoTierra;
    public Transform ControlSuelo;
    public float RadioSuelo = 0.1f;
    public LayerMask CapaSuelo;

    private int coins;
    public TMP_Text textCoins;

    private float UltimoDisparo;
    private int vida = 5;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        mover = Input.GetAxisRaw("Horizontal");
        Rigidbody2D.linearVelocity = new Vector2(mover * Velocidad, Rigidbody2D.linearVelocity.y);

        // Jugador mira en la dirección del movimiento
        if (mover != 0) transform.localScale = new Vector3(Mathf.Sign(mover), 1, 1);

        // Animaciones
        // Animator.SetBool("running", mover != 0.0f);
        Animator.SetFloat("speed", Mathf.Abs(mover));
        //Animator.SetFloat("VerticalSpeed", Rigidbody2D.linearVelocity.y);
        //Animator.SetBool("grounded", ConectadoTierra);

        // Saltar
        if (Input.GetKeyDown(KeyCode.W /* "Jump" */) && ConectadoTierra)
        {
            Saltar();
        }

        // Disparar
        if (Input.GetKey(KeyCode.Space) && Time.time > UltimoDisparo + 0.25f)
        {
            Disparar();
            UltimoDisparo = Time.time;
        }
    }
    
    private void Saltar()
    {
        Rigidbody2D.linearVelocity = new Vector2(Rigidbody2D.linearVelocity.x, FuerzaSalto);
        //Rigidbody2D.AddForce(Vector2.up * FuerzaSalto);
    }

    private void FixedUpdate()
    {
        ConectadoTierra = Physics2D.OverlapCircle(ControlSuelo.position, RadioSuelo, CapaSuelo);
        //Animator.SetBool("jumping", !ConectadoTierra); // Aún no tengo animación de salto
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coins ++;
            textCoins.text = coins.ToString();
        }

        if (collision.transform.CompareTag("Defeat"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.transform.CompareTag("Barrel"))
        {
            Vector2 golpearBarril = (Rigidbody2D.position - (Vector2)collision.transform.position).normalized;
            Rigidbody2D.linearVelocity = Vector2.zero;
            Rigidbody2D.AddForce(golpearBarril * 3, ForceMode2D.Impulse);

            BoxCollider2D[] colliders = collision.gameObject.GetComponents<BoxCollider2D>();

            foreach (BoxCollider2D collider in colliders)
            {
                collider.enabled = false;
            }

            collision.GetComponent<Animator>().enabled=true;
            Destroy(collision.gameObject, 0.5f);
        }
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


}

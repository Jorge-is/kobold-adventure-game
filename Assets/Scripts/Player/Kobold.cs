using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kobold : MonoBehaviour
{
    public GameObject BalaPrefab;
    [SerializeField] private Transform PuntoDisparo;
    [SerializeField] private int vida = 5;
    //[SerializeField] private GameObject EfectoDanio;

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

    public AudioSource audioSource;
    public AudioClip coinClip;
    public AudioClip barrelClip;

    private float UltimoDisparo;

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
            audioSource.PlayOneShot(coinClip);
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
            audioSource.PlayOneShot(barrelClip);
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
        // Determinar dirección según orientación del jugador
        Vector2 direccion = transform.localScale.x == 1.0f ? Vector2.right : Vector2.left;

        // Instanciar la bala en el punto de disparo 
        GameObject bala = Instantiate(BalaPrefab, PuntoDisparo.position, Quaternion.identity);

        //Ignorar la colisión entre el jugador y la bala
        Collider2D KoboldCollider = GetComponent<Collider2D>();
        Collider2D BalaCollider = bala.GetComponent<Collider2D>();
        if (KoboldCollider != null && BalaCollider != null)
        {
            Physics2D.IgnoreCollision(KoboldCollider, BalaCollider);
        }

        // Configurar la dirección de la bala
        bala.GetComponent<BalaScript>().SetDireccion(direccion);
    }

    public void Golpear()
    {
        vida--;

        /*if (EfectoDanio != null)
        {
            Instantiate(EfectoDanio, transform.position, Quaternion.identity);
        }*/

        if (vida <= 0)
        {
            // Reinicia la escena si muere
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }
}

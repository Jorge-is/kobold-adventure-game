using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Kobold : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject BalaPrefab;
    [SerializeField] private Transform PuntoDisparo;
    private float ultimoDisparo;

    //[SerializeField] private GameObject EfectoDanio;

    [Header("Movimiento")]
    public float Velocidad = 5f;
    public float FuerzaSalto = 4f;
    public float FuerzaGolpe = 6f;

    [Header("Vida")]
    [SerializeField] private int vida = 5;

    [Header("Detección de suelo")]
    public Transform ControlSuelo;
    public float RadioSuelo = 0.1f;
    public LayerMask CapaSuelo;

    [Header("Vida UI")]
    public Image[] corazones;
    public Sprite corazonLleno;
    public Sprite corazonVacio;

    [Header("Monedas y Sonidos")]
    private int coins;
    public TMP_Text textCoins;
    public AudioSource audioSource;
    public AudioClip coinClip;
    public AudioClip barrelClip;

    // Componentes
    private Rigidbody2D rb;
    private Animator animator;

    // Estados
    private bool conectadoTierra;
    private bool recibeDanio;

    // --------- UI MÓVIL ----------
    [Header("Controles Mobile")]
    public Joystick joystickMovimiento; // arrastra aquí el FixedJoystick izquierdo
    public Button jumpButton; // asigna el botón Jump
    public Button fireButton; // asigna el botón Fire

    // Zona muerta para evitar movimientos no deseados
    private float joystickDeadzone = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        textCoins.text = coins.ToString();

        // Inicializar corazones de vida al comienzo
        ActualizarCorazones();

        // Suscribir botones si están asignados
        if (jumpButton != null)
            jumpButton.onClick.AddListener(OnJumpButtonPressed);
        if (fireButton != null)
            fireButton.onClick.AddListener(OnFireButtonPressed);
    }

    void OnDestroy()
    {
        // Liberar eventos
        if (jumpButton != null)
            jumpButton.onClick.RemoveListener(OnJumpButtonPressed);
        if (fireButton != null)
            fireButton.onClick.RemoveListener(OnFireButtonPressed);
    }

    void Update()
    {
        // Bloquear movimiento mientras recibe daño
        if (recibeDanio) return;

        float mover = 0f;

        // CONTROL DE MOVIMIENTO
        if (joystickMovimiento != null)
        {
            // Si el joystick está asignado, usar su valor
            mover = Mathf.Abs(joystickMovimiento.Horizontal) > joystickDeadzone
                ? joystickMovimiento.Horizontal
                : 0f;
        }
        else
        {
            // Si no hay joystick, usar teclado
            mover = Input.GetAxisRaw("Horizontal");
        }

        // Aplicar movimiento horizontal
        rb.linearVelocity = new Vector2(mover * Velocidad, rb.linearVelocity.y);

        // Orientación del jugador, mira en la dirección del movimiento
        if (mover != 0) transform.localScale = new Vector3(Mathf.Sign(mover), 1, 1);

        // Animaciones
        animator.SetFloat("speed", Mathf.Abs(mover));
        //Animator.SetBool("grounded", ConectadoTierra);
        animator.SetBool("recibeDanio", recibeDanio);

        // SALTO CON TECLADO 
        if (Input.GetKeyDown(KeyCode.W) && conectadoTierra)
        {
            Saltar();
        }

        // DISPARO CON TECLADO
        if (Input.GetKey(KeyCode.Space) && Time.time > ultimoDisparo + 0.25f)
        {
            Disparar();
            ultimoDisparo = Time.time;
        }
    }

    private void FixedUpdate()
    {
        conectadoTierra = Physics2D.OverlapCircle(ControlSuelo.position, RadioSuelo, CapaSuelo);
        //Animator.SetBool("jumping", !ConectadoTierra); // Aún no tengo animación de salto
    }

    private void Saltar()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, FuerzaSalto);
        //Rigidbody2D.AddForce(Vector2.up * FuerzaSalto);
    }

    private void OnJumpButtonPressed()
    {
        if (conectadoTierra && !recibeDanio)
            Saltar();
    }

    private void OnFireButtonPressed()
    {
        if (Time.time > ultimoDisparo + 0.25f && !recibeDanio)
        {
            Disparar();
            ultimoDisparo = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            audioSource.PlayOneShot(coinClip);
            Destroy(collision.gameObject);
            coins ++;
            textCoins.text = coins.ToString();
        }

        // Reiniciar nivel al caer en zona de derrota
        if (collision.CompareTag("Defeat"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.CompareTag("Barrel"))
        {
            audioSource.PlayOneShot(barrelClip);
            Vector2 golpearBarril = (rb.position - (Vector2)collision.transform.position).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(golpearBarril * 3, ForceMode2D.Impulse);

            foreach (BoxCollider2D col in collision.GetComponents<BoxCollider2D>())
                col.enabled = false;

            Animator anim = collision.GetComponent<Animator>();
            if (anim != null) anim.enabled = true;

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
        Collider2D koboldCollider = GetComponent<Collider2D>();
        Collider2D balaCollider = bala.GetComponent<Collider2D>();

        if (koboldCollider != null && balaCollider != null)
        {
            Physics2D.IgnoreCollision(koboldCollider, balaCollider);
        }


        // Configurar la dirección de la bala
        bala.GetComponent<BalaScript>().SetDireccion(direccion);
    }

    public void RecibeDanio(Vector2 origen, int cantDanio)
    {
        if (recibeDanio) return;

        recibeDanio = true;
        vida -= cantDanio;
        if (vida < 0) vida = 0;

        ActualizarCorazones(); // Actualizar interfaz de vida

        animator.SetBool("recibeDanio", true);

        // Rebote visual al recibir daño
        Vector2 rebote = ((Vector2)transform.position - origen).normalized + Vector2.up * 0.5f;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(rebote * FuerzaGolpe, ForceMode2D.Impulse);

        if (vida <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Invoke(nameof(DesactivaDanio), 0.5f); // Tiempo de recuperación
        }
    }

    public void DesactivaDanio()
    {
        recibeDanio = false;
        rb.linearVelocity = Vector2.zero;
    }

    private void ActualizarCorazones()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            if (i < vida)
                corazones[i].sprite = corazonLleno;
            else
                corazones[i].sprite = corazonVacio;
        }
    }
}

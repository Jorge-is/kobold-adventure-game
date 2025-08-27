using UnityEngine;

public class DemonScript : MonoBehaviour
{
    [SerializeField] private float Vida;
    [SerializeField] private GameObject EfectoMuerte;

    public void TomarDano(float dano)
    {
        Vida -= dano;
        if (Vida <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        Instantiate(EfectoMuerte, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    /*public GameObject Kobold;
    public GameObject BalaPrefab;

    private float UltimoDisparo;
    private int vida = 3;

    private void Update()
    {
        if (Kobold == null) return;

        Vector3 direccion = Kobold.transform.position - transform.position;
        if (direccion.x >= 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

        float distancia = Mathf.Abs(Kobold.transform.position.x - transform.position.x);

        if (distancia < 1.0f && Time.time > UltimoDisparo + 0.25f)
        {
            Disparar();
            UltimoDisparo = Time.time;
        }
    }

    private void Disparar()
    {
        Vector3 direccion;
        if (transform.localScale.x == 1.0f) direccion = Vector3.right;
        else direccion = Vector3.left;

        GameObject bala = Instantiate(BalaPrefab, transform.position + direccion * 0.1f, Quaternion.identity);
        bala.GetComponent<BalaScript>().SetDireccion(direccion);
    }

    public void Golpear()
    {
        vida = vida - 1;
        if (vida == 0) Destroy(gameObject);
    }*/
}

using UnityEngine;

public class CamaraScript : MonoBehaviour
{
    public GameObject Kobold;

    void Update()
    {

        if (Kobold != null)
        {
            Vector3 posicion = transform.position;
            posicion.x = Kobold.transform.position.x;
            transform.position = posicion;
        }
    }
}

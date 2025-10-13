using UnityEngine;

public class CamaraScript : MonoBehaviour
{
    public GameObject Kobold;

    void LateUpdate()
    {
        if (Kobold != null)
        {
            Vector3 posicion = transform.position;
            posicion.x = Kobold.transform.position.x;
            posicion.y = Kobold.transform.position.y;
            transform.position = posicion;
        }
    }
}

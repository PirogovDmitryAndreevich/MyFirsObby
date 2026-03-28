using UnityEngine;

[RequireComponent(typeof (Rigidbody), typeof(Collider))]
public class ConveerResource : MonoBehaviour
{
    public void SetModel(GameObject model)
    {
        Instantiate(model, transform.position, Quaternion.identity, transform);
    }
}

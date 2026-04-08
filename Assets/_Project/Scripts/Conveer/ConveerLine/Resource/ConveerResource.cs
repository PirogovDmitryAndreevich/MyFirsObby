using UnityEngine;

public class ConveerResource : MonoBehaviour, IConveerResource
{
    public int Reward { get; private set; }

    public void Collect()
    {
        Destroy(gameObject);
    }

    public void SetModel(GameObject view, int reward)
    {
        Reward = reward;
        Instantiate(view, transform);
    }
}
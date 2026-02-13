using UnityEngine;

public class AutoDestroyFX : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2f);
    }
}

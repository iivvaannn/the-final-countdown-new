using UnityEngine;

public class IdleBreathing : MonoBehaviour
{
    public float amount = 0.01f;   // how high it moves
    public float speed = 1f;       // how fast

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * speed) * amount;
        transform.localPosition = startPos + new Vector3(0, y, 0);
    }
}
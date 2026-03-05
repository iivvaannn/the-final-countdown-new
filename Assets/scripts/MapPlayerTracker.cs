using UnityEngine;

public class MapPlayerMarker : MonoBehaviour
{
    public Transform player;
    public RectTransform mapRect;

    public Vector2 worldMin;
    public Vector2 worldMax;

    RectTransform marker;

    void Start()
    {
        marker = GetComponent<RectTransform>();
    }

    void Update()
    {
        Debug.Log(player.position);
        Debug.Log("Updating marker");
        Vector3 pos = player.position;

        float percentX =
            Mathf.InverseLerp(worldMin.x, worldMax.x, pos.x);

        float percentY =
            Mathf.InverseLerp(worldMin.y, worldMax.y, pos.z);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        float mapX = (percentX - 0.5f) * mapRect.rect.width;
        float mapY = (percentY - 0.5f) * mapRect.rect.height;

        marker.anchoredPosition = new Vector2(mapX, mapY);
    }
}
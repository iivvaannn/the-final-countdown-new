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
        Vector3 pos = player.position;

        float normalizedX =
            Mathf.InverseLerp(
                worldMin.x,
                worldMax.x,
                pos.x
            );

        float normalizedZ =
            Mathf.InverseLerp(
                worldMin.y,
                worldMax.y,
                pos.z
            );

        float mapX =
            -((normalizedX * mapRect.rect.width)
            - (mapRect.rect.width / 2f));

        float mapY =
            (normalizedZ * mapRect.rect.height)
            - (mapRect.rect.height / 2f);

        mapY = -mapY;
        marker.anchoredPosition =
            new Vector2(mapX * 8f, mapY * 8f);
    }
}
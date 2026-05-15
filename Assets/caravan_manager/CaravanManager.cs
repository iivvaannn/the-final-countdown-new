using UnityEngine;
using System.Collections;

public class CaravanManager : MonoBehaviour
{
    [Header("Caravan")]
    public GameObject caravanPrefab;
    public Transform spawnPoint;

    [Header("UI Image Alert")]
    public GameObject alertImage; // din Canva image

    GameObject currentCaravan;

    void OnEnable()
    {
        LightingManager.OnDayStart += OnNewDay;
    }

    void OnDisable()
    {
        LightingManager.OnDayStart -= OnNewDay;
    }

    void OnNewDay()
    {
        int day = LightingManager.Instance.CurrentDay;

        // Spawnar var 3:e dag
        if (day % 3 == 0)
        {
            SpawnCaravan();
            StartCoroutine(ShowImage());
        }
    }

    void SpawnCaravan()
    {
        if (currentCaravan != null)
            Destroy(currentCaravan);

        currentCaravan = Instantiate(caravanPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Spawning caravan...");

        // OPTIONAL: ta bort efter 60 sek
        // Destroy(currentCaravan, 60f);
    }

    IEnumerator ShowImage()
    {
        if (alertImage == null) yield break;

        alertImage.SetActive(true);

        CanvasGroup cg = alertImage.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = alertImage.AddComponent<CanvasGroup>();

        cg.alpha = 0f;

        float t = 0f;

        // Fade in
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            cg.alpha = t;
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        // Fade out
        while (t > 0f)
        {
            t -= Time.deltaTime * 2f;
            cg.alpha = t;
            yield return null;
        }

        alertImage.SetActive(false);
    }
}
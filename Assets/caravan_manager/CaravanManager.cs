using UnityEngine;
using System.Collections;

public class CaravanManager : MonoBehaviour
{
    public GameObject caravanPrefab;
    public Transform spawnPoint;

    public GameObject alertImage;

    GameObject currentCaravan;

    void Start()
    {
        CheckDay();
    }

    void OnEnable()
    {
        LightingManager.OnDayStart += CheckDay;
        LightingManager.OnNightStart += RemoveCaravan;
    }
    void OnDisable()
    {
        LightingManager.OnDayStart -= CheckDay;
        LightingManager.OnNightStart -= RemoveCaravan;
    }

    void CheckDay()
    {
        int day = LightingManager.Instance.CurrentDay;

        Debug.Log("Checking day: " + day);

        bool caravanDay =
            day == 1 ||
            day == 3 ||
            day == 6 ||
            day == 9;

        if (caravanDay)
        {
            Debug.Log("CARAVAN ARRIVED!");

            SpawnCaravan();

            StartCoroutine(ShowImage());
        }
    }

    void SpawnCaravan()
    {
        // prevents duplicates
        if (currentCaravan != null)
            return;

        currentCaravan =
            Instantiate(
                caravanPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );
    }

    void RemoveCaravan()
    {
        if (currentCaravan != null)
        {
            Destroy(currentCaravan);

            currentCaravan = null;

            Debug.Log("Caravan left for the night");
        }
    }

    IEnumerator ShowImage()
    {
        if (alertImage == null)
        {
            Debug.LogError("alertImage är NULL");
            yield break;
        }

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
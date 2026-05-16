using UnityEngine;
using System.Collections;

public class CaravanManager : MonoBehaviour
{
    public GameObject caravanPrefab;
    public Transform spawnPoint;

    public GameObject alertImage; // din UI image

    void OnEnable()
    {
        LightingManager.OnDayStart += CheckDay;
    }

    void OnDisable()
    {
        LightingManager.OnDayStart -= CheckDay;
    }

    void CheckDay()
    {
        int day = LightingManager.Instance.CurrentDay;

        Debug.Log("Checking day: " + day);

        if (day % 3 == 0)
        {
            Debug.Log("CARAVAN ARRIVED!");
            SpawnCaravan();
            StartCoroutine(ShowImage()); // ?? DETTA SAKNADES
        }
    }

    void SpawnCaravan()
    {
        Instantiate(caravanPrefab, spawnPoint.position, spawnPoint.rotation);
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
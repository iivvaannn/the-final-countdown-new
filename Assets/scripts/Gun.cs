using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    [Header("Audio")]
    public AudioClip shootSound;      // Dra in ljudfilen i Inspector
    private AudioSource audioSource;

    void Start()
    {
        // Hämta AudioSource från samma GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Om ingen AudioSource finns, lägg till en automatiskt
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // Spela ljudsegment (exempel: mellan 3 och 5 sekunder)
            PlaySoundSegment(3f, 5f);

            // Raycast / damage-logik
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Enemyhealthscript enemyHit = hit.transform.GetComponent<Enemyhealthscript>();
            if (enemyHit != null)
            {
                enemyHit.takeDamage(damage);
            }
        }
    }

    void PlaySoundSegment(float startTime, float endTime)
    {
        if (shootSound == null || audioSource == null) return;

        audioSource.clip = shootSound;
        audioSource.time = startTime;    // Starttid i sekunder
        audioSource.Play();
        StartCoroutine(StopAudioAfter(endTime - startTime));
    }

    IEnumerator StopAudioAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }
}

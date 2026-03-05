using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    public AudioSource source;

    [Header("Sounds")]
    public AudioClip[] idleSounds;
    public AudioClip aggroSound;
    public AudioClip attackSound;
    public AudioClip deathSound;

    float nextIdleTime;

    void Start()
    {
        ScheduleNextIdle();
    }

    void Update()
    {
        if (Time.time >= nextIdleTime)
        {
            PlayIdle();
            ScheduleNextIdle();
        }
    }

    void ScheduleNextIdle()
    {
        nextIdleTime = Time.time + Random.Range(6f, 14f);
    }

    void PlayIdle()
    {
        if (idleSounds.Length == 0) return;

        source.pitch = Random.Range(0.9f, 1.1f);
        source.PlayOneShot(
            idleSounds[Random.Range(0, idleSounds.Length)]
        );
    }

    public void PlayAggro()
    {
        source.pitch = Random.Range(0.95f, 1.05f);
        source.PlayOneShot(aggroSound);
    }

    public void PlayAttack()
    {
        source.pitch = Random.Range(0.95f, 1.05f);
        source.PlayOneShot(attackSound);
    }

    public void PlayDeath()
    {
        source.pitch = 1f;
        source.PlayOneShot(deathSound);
    }
}
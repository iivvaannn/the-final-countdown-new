using UnityEngine;

public class RandomAmbient : MonoBehaviour
{
    public AudioClip[] sounds;
    public float minDelay = 10f;
    public float maxDelay = 30f;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        Invoke(nameof(PlayRandom), Random.Range(minDelay, maxDelay));
    }

    void PlayRandom()
    {
        source.clip = sounds[Random.Range(0, sounds.Length)];
        source.Play();
        Invoke(nameof(PlayRandom), Random.Range(minDelay, maxDelay));
    }
}
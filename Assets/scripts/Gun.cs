using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public float nextTimeToFire = 0f;

    [Header("Audio")]
    public AudioClip shootSound;
    private AudioSource audioSource;

    [Header("Ammo")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        // auto reload when the amo hits 0
        //if (currentAmmo <= 0)
        //{
        //    StartCoroutine(Reload());
        //    return;
        //}

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            PlaySoundSegment(3f, 5f);
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo = Mathf.Max(currentAmmo - 1, 0);

        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Enemyhealthscript enemyHit = hit.transform.GetComponent<Enemyhealthscript>();
            if (enemyHit != null)
            {
                enemyHit.takeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactOG = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactOG, 2f);
        }
    }

    void PlaySoundSegment(float startTime, float endTime)
    {
        if (shootSound == null || audioSource == null) return;

        audioSource.clip = shootSound;
        audioSource.time = startTime;
        audioSource.Play();
        StartCoroutine(StopAudioAfter(endTime - startTime));
    }

    IEnumerator StopAudioAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}

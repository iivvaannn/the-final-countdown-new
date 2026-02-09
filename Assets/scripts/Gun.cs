using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public WeaponType weaponType;

    [Header("Stats")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    [Header("References")]
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;

    [Header("Audio")]
    public AudioClip shootSound;
    [Range(0f, 1f)] public float shootVolume = 0.35f;   // ? added
    private AudioSource audioSource;

    [Header("Ammo")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    private float nextTimeToFire = 0f;

    void Start()
    {
        fpsCam = Camera.main;

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

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;

        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (shootSound != null)
            audioSource.PlayOneShot(shootSound, shootVolume);   // ? changed

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
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}

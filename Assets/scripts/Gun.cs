using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public WeaponType weaponType;

    [Header("Stats")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 12f;
    public float impactForce = 30f;
    public GameObject bloodPrefab;

    [Header("References")]
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public Animator weaponAnimator;

    [Header("Shoot Audio")]
    public AudioClip shootSound;
    public AudioClip emptyClickSound;
    [Range(0f, 1f)] public float shootVolume = 0.4f;

    [Header("Reload Audio")]
    public AudioClip reloadSound;
    [Range(0f, 1f)] public float reloadVolume = 0.6f;

    private AudioSource audioSource;

    [Header("Ammo")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 1.6f;

    private bool isReloading = false;
    private float nextTimeToFire = 0f;

    void Start()
    {
        if (!fpsCam) fpsCam = Camera.main;

        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            TryShoot();
        }
    }

    void TryShoot()
    {
        if (Time.time < nextTimeToFire) return;

        if (currentAmmo <= 0)
        {
            if (emptyClickSound)
                audioSource.PlayOneShot(emptyClickSound, 0.6f);
            return;
        }

        nextTimeToFire = Time.time + 1f / fireRate;
        Shoot();
    }

    void Shoot()
    {
        currentAmmo--;

        if (muzzleFlash)
            muzzleFlash.Play();

        if (weaponAnimator)
            weaponAnimator.SetTrigger("Fire");

        if (shootSound)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(shootSound, shootVolume);
        }

        if (fpsCam)
        {
            fpsCam.transform.localRotation *= Quaternion.Euler(
                Random.Range(-1.2f, -0.6f),
                Random.Range(-0.3f, 0.3f),
                0f
            );
        }

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            // FIX: works even if collider is on child object
            Enemyhealthscript enemyHit = hit.transform.GetComponentInParent<Enemyhealthscript>();

            if (enemyHit)
            {
                enemyHit.takeDamage(damage);

                // ADDED: blood spawn
                if (bloodPrefab)
                {
                    Instantiate(
                        bloodPrefab,
                        hit.point + hit.normal * 0.02f,
                        Quaternion.LookRotation(hit.normal)
                    );
                }
            }

            if (hit.rigidbody && enemyHit == null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }

        // AmmoUI.Instance.Update(currentAmmo, maxAmmo, weaponType);
    }

    void TryReload()
    {
        if (currentAmmo == maxAmmo) return;
        if (isReloading) return;

        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        isReloading = true;

        if (weaponAnimator)
            weaponAnimator.SetTrigger("Reload");

        if (reloadSound)
            audioSource.PlayOneShot(reloadSound, reloadVolume);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}

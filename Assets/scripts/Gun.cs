using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public WeaponType weaponType;
    [Header("UI")]
    public Sprite weaponIcon;
    [Header("Recoil")]
    public float recoilUp = 2f;
    public float recoilSide = 1f;
    public float recoilKick = 0.05f;

    [Header("Stats")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 5f;
    public float impactForce = 30f;
    public GameObject bloodPrefab;

    [Header("Shotgun")]
    public bool isShotgun = false;
    public int pelletsPerShot = 8;
    public float spread = 4f;

    [Header("References")]
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public Animator weaponAnimator;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip emptyClickSound;
    public AudioClip reloadSound;

    [Range(0f, 1f)] public float shootVolume = 0.4f;
    [Range(0f, 1f)] public float reloadVolume = 0.6f;

    [Header("Ammo")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 1.6f;

    private AudioSource audioSource;

    private bool isReloading = false;
    private Coroutine reloadRoutine;
    private float nextTimeToFire = 0f;

    private Vector3 originalCamLocalPos;

    // ------------------------------------------------

    void Start()
    {
        if (!fpsCam)
            fpsCam = Camera.main;

        audioSource = GetComponent<AudioSource>();

        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        currentAmmo = maxAmmo;

        if (fpsCam)
            originalCamLocalPos = fpsCam.transform.localPosition;
    }

    // ------------------------------------------------

    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        HandleInput();
        RecoverCameraPosition();
    }

    // ------------------------------------------------
    // INPUT
    // ------------------------------------------------

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
            TryReload();

        if (Input.GetMouseButton(0))
            TryShoot();
    }

    // ------------------------------------------------
    // SHOOT
    // ------------------------------------------------

    void TryShoot()
    {
        if (isReloading)
            return;

        if (Time.time < nextTimeToFire)
            return;

        int ammoCost = isShotgun ? 2 : 1;

        if (currentAmmo < ammoCost)
        {
            if (emptyClickSound)
                audioSource.PlayOneShot(emptyClickSound, 0.6f);
            return;
        }

        nextTimeToFire = Time.time + 1f / fireRate;

        Shoot(ammoCost);
    }

    void Shoot(int ammoCost)
    {
        currentAmmo -= ammoCost;

        if (muzzleFlash)
            muzzleFlash.Play();

        if (weaponAnimator)
            weaponAnimator.SetTrigger("Fire");

        if (shootSound)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(shootSound, shootVolume);
        }

        ApplyRecoil();

        if (isShotgun)
        {
            for (int i = 0; i < pelletsPerShot; i++)
                ShootRay();
        }
        else
        {
            ShootRay();
        }
    }

    // ------------------------------------------------
    // RECOIL (FIXED)
    // ------------------------------------------------

    void ApplyRecoil()
    {
        if (!fpsCam) return;

        float vertical = Random.Range(recoilUp * 0.8f, recoilUp);
        float horizontal = Random.Range(-recoilSide, recoilSide);

        fpsCam.transform.localRotation *= Quaternion.Euler(
            -vertical,
            horizontal,
            0f
        );

        // small kickback
        fpsCam.transform.localPosition -= Vector3.forward * recoilKick;
    }

    void RecoverCameraPosition()
    {
        if (!fpsCam) return;

        fpsCam.transform.localPosition =
            Vector3.Lerp(
                fpsCam.transform.localPosition,
                originalCamLocalPos,
                Time.deltaTime * 8f
            );
    }

    // ------------------------------------------------
    // RAYCAST
    // ------------------------------------------------

    void ShootRay()
    {
        Vector3 direction = fpsCam.transform.forward;

        direction += fpsCam.transform.right *
                     Random.Range(-spread, spread) * 0.01f;

        direction += fpsCam.transform.up *
                     Random.Range(-spread, spread) * 0.01f;

        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position,
                            direction,
                            out hit,
                            range))
        {
            Enemyhealthscript enemy =
                hit.transform.GetComponentInParent<Enemyhealthscript>();

            if (enemy)
            {
                enemy.takeDamage(damage);

                if (bloodPrefab)
                {
                    Instantiate(
                        bloodPrefab,
                        hit.point + hit.normal * 0.02f,
                        Quaternion.LookRotation(hit.normal)
                    );
                }
            }

            if (hit.rigidbody && enemy == null)
                hit.rigidbody.AddForce(-hit.normal * impactForce);
        }
    }

    // ------------------------------------------------
    // RELOAD (FULLY FIXED)
    // ------------------------------------------------

    void TryReload()
    {
        if (isReloading)
            return;

        if (currentAmmo >= maxAmmo)
            return;

        if (reloadRoutine != null)
            StopCoroutine(reloadRoutine);

        reloadRoutine = StartCoroutine(Reload());
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
        reloadRoutine = null;
    }
}
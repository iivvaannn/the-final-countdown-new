using UnityEngine;
using UnityEngine.Audio;
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

    [Header("Recoil Recovery")]
    public float recoilRecoverySpeed = 8f;

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

    // ADD THIS
    public AudioMixerGroup mixerGroup;

    [Header("Ammo")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public int reserveAmmo = 90;
    public float reloadTime = 1.6f;

    // SNIPER ZOOM
    [Header("Sniper Zoom")]
    public bool isSniper = false;
    public float sniperZoomFOV = 20f;
    public float sniperZoomSpeed = 10f;

    // ADDED
    [Header("Sniper Aim Assist")]
    public float sniperHitRadius = 0.5f;

    private AudioSource audioSource;

    private bool isReloading = false;
    private Coroutine reloadRoutine;
    private float nextTimeToFire = 0f;

    private Vector3 originalCamLocalPos;
    private float defaultFOV;

    // ADDED
    private float targetFOV;

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

        // ADD THIS
        if (mixerGroup != null)
            audioSource.outputAudioMixerGroup = mixerGroup;

        currentAmmo = maxAmmo;

        if (fpsCam)
        {
            originalCamLocalPos = fpsCam.transform.localPosition;

            defaultFOV = fpsCam.fieldOfView;
            targetFOV = defaultFOV;
        }
    }

    // ------------------------------------------------

    void Update()
    {
        Debug.Log(gameObject.name + " update running");

        if (!gameObject.activeInHierarchy)
            return;

        HandleSniperZoom();
        HandleInput();
        RecoverCameraPosition();
    }

    // ------------------------------------------------
    // SNIPER ZOOM
    // ------------------------------------------------

    void HandleSniperZoom()
    {
        if (!isSniper || fpsCam == null)
            return;

        if (Input.GetMouseButton(1))
        {
            targetFOV = sniperZoomFOV;
        }
        else
        {
            targetFOV = defaultFOV;
        }

        fpsCam.fieldOfView = Mathf.Lerp(
            fpsCam.fieldOfView,
            targetFOV,
            sniperZoomSpeed * Time.deltaTime
        );
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

        // NO AMMO
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
        // EXTRA SAFETY
        if (currentAmmo < ammoCost)
            return;

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

        // ONLY RECOIL IF ACTUALLY SHOOTING
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
    // RECOIL
    // ------------------------------------------------

    void ApplyRecoil()
    {
        if (!fpsCam) return;

        // DISABLE RECOIL WHILE SCOPED
        if (isSniper && Input.GetMouseButton(1))
            return;

        float vertical = Random.Range(recoilUp * 0.8f, recoilUp);
        float horizontal = Random.Range(-recoilSide, recoilSide);

        fpsCam.transform.localRotation *= Quaternion.Euler(
            -vertical,
            horizontal,
            0f
        );

        fpsCam.transform.localPosition -= Vector3.forward * recoilKick;
    }

    void RecoverCameraPosition()
    {
        if (!fpsCam) return;

        fpsCam.transform.localPosition =
            Vector3.Lerp(
                fpsCam.transform.localPosition,
                originalCamLocalPos,
                Time.deltaTime * recoilRecoverySpeed
            );
    }

    // ------------------------------------------------
    // RAYCAST
    // ------------------------------------------------

    void ShootRay()
    {
        Ray ray = fpsCam.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        Vector3 direction = ray.direction;

        // NORMAL WEAPONS USE SPREAD
        if (!isSniper || !Input.GetMouseButton(1))
        {
            direction += fpsCam.transform.right *
                         Random.Range(-spread, spread) * 0.01f;

            direction += fpsCam.transform.up *
                         Random.Range(-spread, spread) * 0.01f;
        }

        RaycastHit hit;

        // SNIPER AIM ASSIST
        float hitRadius = 0.08f;

        if (isSniper && Input.GetMouseButton(1))
            hitRadius = sniperHitRadius;

        if (Physics.SphereCast(
            ray.origin,
            hitRadius,
            direction,
            out hit,
            range))
        {
            Debug.Log("Hit: " + hit.transform.name);

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
    // RELOAD
    // ------------------------------------------------

    void TryReload()
    {
        if (isReloading)
            return;

        if (currentAmmo >= maxAmmo)
            return;

        // NO RESERVE AMMO
        if (reserveAmmo <= 0)
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

        int ammoNeeded = maxAmmo - currentAmmo;

        int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToLoad;

        reserveAmmo -= ammoToLoad;

        isReloading = false;
        reloadRoutine = null;
    }
}
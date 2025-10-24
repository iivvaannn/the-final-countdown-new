
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
    }


    void shoot()
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
}

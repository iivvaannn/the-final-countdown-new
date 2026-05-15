using UnityEngine;

public class KnifeAttack : MonoBehaviour
{
    public float damage = 7f;
    public float range = 2f;
    public Camera cam;

    float nextAttackTime = 0f;
    public float cooldown = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + cooldown;
            Attack();
        }
    }

    void Attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Enemyhealthscript enemy = hit.transform.GetComponentInParent<Enemyhealthscript>();

            if (enemy != null)
            {
                enemy.takeDamage(damage);
                Debug.Log("Knife hit");
            }
        }
    }
}
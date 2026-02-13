using UnityEngine;

public class RandomBloodMaterial : MonoBehaviour
{
    public Material[] bloodMaterials;

    void Awake()
    {
        ParticleSystemRenderer r = GetComponent<ParticleSystemRenderer>();

        if (r != null && bloodMaterials.Length > 0)
        {
            r.material = bloodMaterials[Random.Range(0, bloodMaterials.Length)];
        }
    }
}

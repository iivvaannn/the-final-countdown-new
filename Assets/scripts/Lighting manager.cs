using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    // Scene References
    [SerializeField] private Light Sun;
    [SerializeField] private LightingPreset Preset;

    // Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime * 0.05f;
            TimeOfDay %= 24f;
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (Sun != null)
        {
            Sun.color = Preset.DirectionalColor.Evaluate(timePercent);
            Sun.transform.localRotation =
                Quaternion.Euler((timePercent * 360f) - 90f, 170f, 0f);
        }
    }

    private void OnValidate()
    {
        if (Sun != null)
            return;

        if (RenderSettings.sun != null)
        {
            Sun = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();

            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    Sun = light;
                    return;
                }
            }
        }
    }
}

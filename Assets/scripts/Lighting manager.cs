using UnityEngine;
using System;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    public static LightingManager Instance;

    public static event Action OnDayStart;
    public static event Action OnNightStart;

    [Header("Scene")]
    [SerializeField] private Light Sun;
    [SerializeField] private LightingPreset Preset;

    [Header("Time")]
    [Range(0, 24)]
    public float TimeOfDay = 8f; // start morning

    [Header("Game Time")]
    public int CurrentDay = 1;

    public float dayStart = 6f;
    public float nightStart = 20f;

    public enum TimeState { Day, Night }
    public TimeState CurrentState;

    bool nightTriggered;
    bool dayTriggered;
    bool initialized;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeState();
    }

    void Update()
    {
        if (Preset == null) return;

        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime * 0.02f;
            TimeOfDay %= 24f;

            CheckTimeState();
        }

        UpdateLighting(TimeOfDay / 24f);
    }

    // =========================
    // INITIALIZATION FIX
    // =========================
    void InitializeState()
    {
        initialized = true;

        if (TimeOfDay >= nightStart || TimeOfDay < dayStart)
        {
            CurrentState = TimeState.Night;
            nightTriggered = true;
            dayTriggered = false;
        }
        else
        {
            CurrentState = TimeState.Day;
            dayTriggered = true;
            nightTriggered = false;
        }

        Debug.Log("Game Started, Day " + CurrentDay);
    }

    // =========================
    // DAY / NIGHT LOGIC
    // =========================
    void CheckTimeState()
    {
        if (!initialized) return;

        // NIGHT START
        if (TimeOfDay >= nightStart && !nightTriggered)
        {
            CurrentState = TimeState.Night;

            nightTriggered = true;
            dayTriggered = false;

            Debug.Log("Night Started");

            OnNightStart?.Invoke();
        }

        // DAY START (SUNRISE)
        if (TimeOfDay >= dayStart && TimeOfDay < nightStart && !dayTriggered)
        {
            CurrentState = TimeState.Day;

            dayTriggered = true;
            nightTriggered = false;

            // Day increases ONLY after surviving night
            CurrentDay++;

            Debug.Log("Day " + CurrentDay);

            OnDayStart?.Invoke();
        }
    }

    // =========================
    // LIGHTING
    // =========================
    void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight =
            Preset.AmbientColor.Evaluate(timePercent);

        RenderSettings.fogColor =
            Preset.FogColor.Evaluate(timePercent);

        if (Sun != null)
        {
            Sun.color =
                Preset.DirectionalColor.Evaluate(timePercent);

            Sun.transform.localRotation =
                Quaternion.Euler((timePercent * 360f) - 90f, 170f, 0f);
        }
    }

    // =========================
    // AUTO SUN FINDER
    // =========================
    void OnValidate()
    {
        if (Sun != null) return;

        if (RenderSettings.sun != null)
            Sun = RenderSettings.sun;
    }
}
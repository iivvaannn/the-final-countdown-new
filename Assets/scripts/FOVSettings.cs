using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FOVSettings : MonoBehaviour
{
    public Camera playerCamera;

    public Slider fovSlider;

    public TMP_Text fovText;

    // makes the text move with the slider handle
    public RectTransform textTransform;

    public Vector3 textOffset = new Vector3(0f, 30f, 0f);

    void Start()
    {
        float savedFOV =
            PlayerPrefs.GetFloat("PlayerFOV", 90f);

        playerCamera.fieldOfView = savedFOV;

        if (fovSlider != null)
            fovSlider.value = savedFOV;

        UpdateFOVText(savedFOV);
    }

    void Update()
    {
        FollowSlider();
    }

    public void SetFOV(float fov)
    {
        playerCamera.fieldOfView = fov;

        PlayerPrefs.SetFloat("PlayerFOV", fov);

        UpdateFOVText(fov);
    }

    void UpdateFOVText(float fov)
    {
        if (fovText != null)
        {
            fovText.text =
                Mathf.RoundToInt(fov).ToString();
        }
    }

    void FollowSlider()
    {
        if (fovSlider == null || textTransform == null)
            return;

        RectTransform handle =
            fovSlider.handleRect;

        if (handle != null)
        {
            textTransform.position =
                handle.position + textOffset;
        }
    }
}
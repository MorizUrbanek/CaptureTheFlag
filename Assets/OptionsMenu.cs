using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public Slider sensitivitySlider,volumeSlider;
    public TextManager sensitivityValue, volumeValue;
    public TMP_Dropdown resolutionsDropdown;
    public PlayerCameraController camController;
    static private float sensitivity = 1f, volume = 50f;
    Resolution[] resolutions;

    private void Start()
    {
        sensitivitySlider.value = sensitivity;
        sensitivityValue.ChangeText(sensitivity.ToString("0.00"));

        volumeSlider.value = volume;
        volumeValue.ChangeText(volume.ToString("0.0"));

        resolutions = Screen.resolutions;

        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }

    public void Setsensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
        sensitivityValue.ChangeText(sensitivity.ToString("0.00"));
        camController.SetmouseSensetivity(newSensitivity);
    }
    
    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        volumeValue.ChangeText(volume.ToString("0.00"));
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    static public float GetSensitivity()
    {
        return sensitivity;
    }
    static public float GetVolume()
    {
        return volume;
    }
}

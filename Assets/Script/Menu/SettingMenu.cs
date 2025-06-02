using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer; // Reference to the AudioMixer for controlling volume
    public GameObject mainMenu;   // Reference to the Main Menu UI object
    public GameObject settingMenu; // Reference to the Settings Menu UI object

    public void SetVolume(float volume)
    {
        // Set the master volume using the AudioMixer
        audioMixer.SetFloat("Volume", volume);
    }

    public void ToggleVerticalSync(bool isOn)
    {
        if (isOn)
        {
            QualitySettings.vSyncCount = 1; // Enable vertical sync
        }
        else
        {
            QualitySettings.vSyncCount = 0; // Disable vertical sync
        }

        // Optional: Save the VSync setting using PlayerPrefs
        PlayerPrefs.SetInt("VSyncEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Restore saved VSync setting when initializing (can be called in Start)
    public void ApplySavedVSyncSetting(UnityEngine.UI.Toggle toggle)
    {
        int saved = PlayerPrefs.GetInt("VSyncEnabled", 1); // Default is enabled
        QualitySettings.vSyncCount = saved;
        toggle.isOn = saved == 1;
    }

    public void backToMainMenu()
    {
        // Show main menu and hide settings menu
        mainMenu.SetActive(true);
        settingMenu.SetActive(false);
    }
}

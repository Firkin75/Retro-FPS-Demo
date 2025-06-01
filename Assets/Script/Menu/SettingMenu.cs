using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject mainMenu;
    public GameObject settingMenu;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        
    }
    public void ToggleVerticalSync(bool isOn)
    {
        if (isOn)
        {
            QualitySettings.vSyncCount = 1; // 开启垂直同步
        }
        else
        {
            QualitySettings.vSyncCount = 0; // 关闭垂直同步
        }

        // 可选：保存设置
        PlayerPrefs.SetInt("VSyncEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 初始设置时恢复保存的状态（可在Start里调用）
    public void ApplySavedVSyncSetting(UnityEngine.UI.Toggle toggle)
    {
        int saved = PlayerPrefs.GetInt("VSyncEnabled", 1); // 默认是开
        QualitySettings.vSyncCount = saved;
        toggle.isOn = saved == 1;
    }

    public void backToMainMenu()
    { 
    
       mainMenu.SetActive(true);
       settingMenu.SetActive(false);
        
    
    }
}

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
            QualitySettings.vSyncCount = 1; // ������ֱͬ��
        }
        else
        {
            QualitySettings.vSyncCount = 0; // �رմ�ֱͬ��
        }

        // ��ѡ����������
        PlayerPrefs.SetInt("VSyncEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // ��ʼ����ʱ�ָ������״̬������Start����ã�
    public void ApplySavedVSyncSetting(UnityEngine.UI.Toggle toggle)
    {
        int saved = PlayerPrefs.GetInt("VSyncEnabled", 1); // Ĭ���ǿ�
        QualitySettings.vSyncCount = saved;
        toggle.isOn = saved == 1;
    }

    public void backToMainMenu()
    { 
    
       mainMenu.SetActive(true);
       settingMenu.SetActive(false);
        
    
    }
}

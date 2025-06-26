using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButtons : MonoBehaviour {
    // VIDEO
    [SerializeField] private TMP_Dropdown fullscreenDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    
    // CONTROLS
    [SerializeField] private Slider sensSlider;
    
    // AUDIO
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    
    public void SetValues() {
        // VIDEO
        fullscreenDropdown.value = Screen.fullScreenMode switch {
            FullScreenMode.ExclusiveFullScreen => 0,
            FullScreenMode.Windowed => 1,
            FullScreenMode.MaximizedWindow => 2,
            _ => fullscreenDropdown.value
        };

        for (int i = 0; i < resolutionDropdown.options.Count; i++) {
            string x = resolutionDropdown.options[i].text.Split('x')[0];
            if (int.Parse(x) == Screen.width) {
                resolutionDropdown.value = i;
                break;
            }
        }

        // CONTROLS
        sensSlider.value = PlayerPrefs.GetFloat("sens", .5f);
        
        //AUDIO
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public void SetFullscreenMode() {
        Screen.fullScreenMode = fullscreenDropdown.value switch {
            0 => FullScreenMode.ExclusiveFullScreen,
            1 => FullScreenMode.Windowed,
            2 => FullScreenMode.MaximizedWindow,
            _ => Screen.fullScreenMode
        };
    }
    
    public void ChangeResolution() {
        string[] values = resolutionDropdown.options[resolutionDropdown.value].text.Split('x');

        if (values.Length == 2 && int.TryParse(values[0], out int x) && int.TryParse(values[1], out int y)) {
            Screen.SetResolution(x, y, Screen.fullScreenMode);
        }
    }

    public void ChangeSensitivity() {
        PlayerPrefs.SetFloat("sens", sensSlider.value);
    }

    public void ChangeSFXVolume() {
        RuntimeManager.GetBus("SFX").setVolume(sfxSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }
    
    public void ChangeMasterVolume() {
        RuntimeManager.GetBus("Master").setVolume(masterSlider.value);
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
    }
    
    public void ChangeMusicVolume() {
        RuntimeManager.GetBus("Music").setVolume(musicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }
}

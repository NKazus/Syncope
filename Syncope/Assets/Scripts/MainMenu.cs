using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button []levels;

    [Header("Graphics")]
    [SerializeField] private Dropdown gDropDown;
    [SerializeField] private Dropdown rDropDown;
    [SerializeField] private Toggle wToggle;

    [Header("Sounds")]
    [SerializeField] private AudioMixerGroup mixer;
    [SerializeField] private Toggle mToggle;
    [SerializeField] private Slider volume;
    [SerializeField] private Slider effects;

    private int _levelComplete;
    private Resolution[] _res;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        CheckAvailableLevels();
        CheckGraphicSettings();
        CheckSoundSettings();
    }


    private void CheckAvailableLevels()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].interactable = false;
        }
        _levelComplete = PlayerPrefs.GetInt("LevelComplete");
        for (int i = 0; i < _levelComplete; i++)
        {
            levels[i].interactable = true;
        }
    }

    private void CheckGraphicSettings()
    {
        //full-screen
        if (PlayerPrefs.HasKey("FullScreen"))
        {
            if (PlayerPrefs.GetInt("FullScreen") == 1)
                Screen.fullScreen = true;
            else
                Screen.fullScreen = false;
        }
        else
        {
            Screen.fullScreen = true;
        }
        wToggle.isOn = !Screen.fullScreen;

        //quality
        gDropDown.AddOptions(QualitySettings.names.ToList());
        if (PlayerPrefs.HasKey("Quality"))
        {
            gDropDown.value = PlayerPrefs.GetInt("Quality");
            QualitySettings.SetQualityLevel(gDropDown.value);
        }
        else
            gDropDown.value = QualitySettings.GetQualityLevel();

        //resolution
        Resolution[] resolution = Screen.resolutions;
        _res = resolution.ToArray();
        _res.Distinct();
        string[] resStr = new string[_res.Length];
        for (int i = 0; i < _res.Length; i++)
        {
            resStr[i] = _res[i].width.ToString() + "x" + _res[i].height.ToString();
        }
        rDropDown.AddOptions(resStr.ToList());
        if (PlayerPrefs.HasKey("Resolution"))
        {
            rDropDown.value = PlayerPrefs.GetInt("Resolution");
            Screen.SetResolution(_res[rDropDown.value].width, _res[rDropDown.value].height, Screen.fullScreen);
        }
        else
        {
            rDropDown.value = _res.Length - 1;//max by default
            Screen.SetResolution(_res[_res.Length - 1].width, _res[_res.Length - 1].height, Screen.fullScreen);
        }
    }

    private void CheckSoundSettings()
    {
        //master
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            volume.value = PlayerPrefs.GetFloat("MasterVolume");

        }
        else
        {
            volume.value = 1.0f;// max by default
        }
        mixer.audioMixer.SetFloat("masterVolume", Mathf.Lerp(-80, 0, volume.value));

        //effects
        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            effects.value = PlayerPrefs.GetFloat("EffectsVolume");

        }
        else
        {
            effects.value = 1.0f;// max by default
        }
        mixer.audioMixer.SetFloat("effectsVolume", Mathf.Lerp(-80, 0, volume.value));

        //music enabled
        if (PlayerPrefs.HasKey("EnableMusic"))
        {
            if (PlayerPrefs.GetInt("EnableMusic") == 1)
                mToggle.isOn = true;
            else
                mToggle.isOn = false;
            if (mToggle.isOn)
                mixer.audioMixer.SetFloat("musicVolume", 0);
            else
                mixer.audioMixer.SetFloat("musicVolume", -80);
        }
        else
        {
            mToggle.isOn = true;
            mixer.audioMixer.SetFloat("musicVolume", 0);
        }
    }

    public void ChangeGraphicQuality()
    {
        QualitySettings.SetQualityLevel(gDropDown.value);
        PlayerPrefs.SetInt("Quality",gDropDown.value);
    }
    
    public void ChangeResolution()
    {
        Screen.SetResolution(_res[rDropDown.value].width, _res[rDropDown.value].height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution",rDropDown.value);
    }

    public void ChangeScreenMode()
    {
        Screen.fullScreen = !wToggle.isOn;
        if (Screen.fullScreen)
        {
            PlayerPrefs.SetInt("FullScreen", 1);
        }
        else
            PlayerPrefs.SetInt("FullScreen", 0);
    }

    public void TurnMusic()
    {
        if(mToggle.isOn)
            mixer.audioMixer.SetFloat("musicVolume", 0);
        else
            mixer.audioMixer.SetFloat("musicVolume", -80);
        PlayerPrefs.SetInt("EnableMusic", mToggle.isOn ? 1 : 0);
    }

    public void ChangeVolume()
    {
        mixer.audioMixer.SetFloat("masterVolume", Mathf.Lerp(-80, 0, volume.value));
        PlayerPrefs.SetFloat("MasterVolume", volume.value);
    }

    public void ChangeEffectsVolume()
    {
        mixer.audioMixer.SetFloat("effectsVolume", Mathf.Lerp(-80, 0, effects.value));
        PlayerPrefs.SetFloat("EffectsVolume", effects.value);
    }
}

using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button []_levels;

    [Header("Graphics")]
    [SerializeField] private Dropdown _gDropDown;
    [SerializeField] private Dropdown _rDropDown;
    [SerializeField] private Toggle _wToggle;

    [Header("Sounds")]
    [SerializeField] private AudioMixerGroup _mixer;
    [SerializeField] private Toggle _mToggle;
    [SerializeField] private Slider _volume;
    [SerializeField] private Slider _effects;

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
        for (int i = 0; i < _levels.Length; i++)
        {
            _levels[i].interactable = false;
        }
        //PlayerPrefs.DeleteAll();
        _levelComplete = PlayerPrefs.GetInt("LevelComplete");
        for (int i = 0; i < _levelComplete; i++)
        {
            _levels[i].interactable = true;
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
        _wToggle.isOn = !Screen.fullScreen;

        //quality
        _gDropDown.AddOptions(QualitySettings.names.ToList());
        if (PlayerPrefs.HasKey("Quality"))
        {
            _gDropDown.value = PlayerPrefs.GetInt("Quality");
            QualitySettings.SetQualityLevel(_gDropDown.value);
        }
        else
            _gDropDown.value = QualitySettings.GetQualityLevel();

        //resolution
        Resolution[] resolution = Screen.resolutions;
        _res = resolution.ToArray();
        _res.Distinct();
        string[] resStr = new string[_res.Length];
        for (int i = 0; i < _res.Length; i++)
        {
            resStr[i] = _res[i].width.ToString() + "x" + _res[i].height.ToString();
        }
        _rDropDown.AddOptions(resStr.ToList());
        if (PlayerPrefs.HasKey("Resolution"))
        {
            _rDropDown.value = PlayerPrefs.GetInt("Resolution");
            Screen.SetResolution(_res[_rDropDown.value].width, _res[_rDropDown.value].height, Screen.fullScreen);
        }
        else
        {
            _rDropDown.value = _res.Length - 1;//max by default
            Screen.SetResolution(_res[_res.Length - 1].width, _res[_res.Length - 1].height, Screen.fullScreen);
        }
    }

    private void CheckSoundSettings()
    {
        //master
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            _volume.value = PlayerPrefs.GetFloat("MasterVolume");

        }
        else
        {
            _volume.value = 1.0f;// max by default
        }
        _mixer.audioMixer.SetFloat("masterVolume", Mathf.Lerp(-80, 0, _volume.value));

        //effects
        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            _effects.value = PlayerPrefs.GetFloat("EffectsVolume");

        }
        else
        {
            _effects.value = 1.0f;// max by default
        }
        _mixer.audioMixer.SetFloat("effectsVolume", Mathf.Lerp(-80, 0, _volume.value));

        //music enabled
        if (PlayerPrefs.HasKey("EnableMusic"))
        {
            if (PlayerPrefs.GetInt("EnableMusic") == 1)
                _mToggle.isOn = true;
            else
                _mToggle.isOn = false;
            if (_mToggle.isOn)
                _mixer.audioMixer.SetFloat("musicVolume", 0);
            else
                _mixer.audioMixer.SetFloat("musicVolume", -80);
        }
        else
        {
            _mToggle.isOn = true;
            _mixer.audioMixer.SetFloat("musicVolume", 0);
        }
    }

    public void ChangeGraphicQuality()
    {
        QualitySettings.SetQualityLevel(_gDropDown.value);
        PlayerPrefs.SetInt("Quality",_gDropDown.value);
    }
    
    public void ChangeResolution()
    {
        Screen.SetResolution(_res[_rDropDown.value].width, _res[_rDropDown.value].height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution",_rDropDown.value);
    }

    public void ChangeScreenMode()
    {
        Screen.fullScreen = !_wToggle.isOn;
        if (Screen.fullScreen)
        {
            PlayerPrefs.SetInt("FullScreen", 1);
        }
        else
            PlayerPrefs.SetInt("FullScreen", 0);
    }

    public void TurnMusic()
    {
        if(_mToggle.isOn)
            _mixer.audioMixer.SetFloat("musicVolume", 0);
        else
            _mixer.audioMixer.SetFloat("musicVolume", -80);
        PlayerPrefs.SetInt("EnableMusic", _mToggle.isOn ? 1 : 0);
    }

    public void ChangeVolume()
    {
        _mixer.audioMixer.SetFloat("masterVolume", Mathf.Lerp(-80, 0, _volume.value));
        PlayerPrefs.SetFloat("MasterVolume", _volume.value);
    }

    public void ChangeEffectsVolume()
    {
        _mixer.audioMixer.SetFloat("effectsVolume", Mathf.Lerp(-80, 0, _effects.value));
        PlayerPrefs.SetFloat("EffectsVolume", _effects.value);
    }
}

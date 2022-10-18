using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button []levels;
    public GameObject loadingScreen;
    public GameObject loadingText;
    public Dropdown gDropDown;
    public Dropdown rDropDown;
    public Toggle wToggle;
    public AudioMixerGroup mixer;
    public Toggle mToggle;
    public Slider volume;
    public Slider effects;

    private int levelComplete;
    Resolution[] res;
    private bool loaded = false;
    private AudioSource buttonSound;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].interactable = false;
        }
        //PlayerPrefs.DeleteAll();
        levelComplete = PlayerPrefs.GetInt("LevelComplete");
        for(int i = 0; i < levelComplete; i++)
        {
            levels[i].interactable = true;
        }

        if (PlayerPrefs.HasKey("FullScreen"))
        {
            if (PlayerPrefs.GetInt("FullScreen") == 1)
                Screen.fullScreen = true;
            else
                Screen.fullScreen = false;
            wToggle.isOn = !Screen.fullScreen;
        }
        else
        {
            Screen.fullScreen = true;
            wToggle.isOn = !Screen.fullScreen;
        }
            

        gDropDown.AddOptions(QualitySettings.names.ToList());
        if (PlayerPrefs.HasKey("Quality"))
        {
            gDropDown.value = PlayerPrefs.GetInt("Quality");
            QualitySettings.SetQualityLevel(gDropDown.value);
        }
        else
            gDropDown.value = QualitySettings.GetQualityLevel();

        Resolution[] resolution = Screen.resolutions;
        res = resolution.ToArray(); 
        res.Distinct();
        string[] resStr = new string[res.Length];
        for(int i = 0; i < res.Length; i++)
        {
            resStr[i] = res[i].width.ToString() + "x" + res[i].height.ToString();
        }
        rDropDown.AddOptions(resStr.ToList());
        if (PlayerPrefs.HasKey("Resolution"))
        {
            rDropDown.value = PlayerPrefs.GetInt("Resolution");
            Screen.SetResolution(res[rDropDown.value].width, res[rDropDown.value].height, Screen.fullScreen);
        }
        else
        {
            rDropDown.value = res.Length - 1;//по умолчанию ставим максимально доступное
            Screen.SetResolution(res[res.Length - 1].width, res[res.Length - 1].height, Screen.fullScreen);//при старте ставится максимально доступное + фул скрин
        }

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            volume.value = PlayerPrefs.GetFloat("MasterVolume");//берем значение громкости
            
        }
        else
        {
            volume.value = 1.0f;// по умолчанию максимальная
        }
        mixer.audioMixer.SetFloat("masterVolume", Mathf.Lerp(-80, 0, volume.value));

        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            effects.value = PlayerPrefs.GetFloat("EffectsVolume");//берем значение громкости эффектов

        }
        else
        {
            effects.value = 1.0f;// по умолчанию максимальная
        }
        mixer.audioMixer.SetFloat("effectsVolume", Mathf.Lerp(-80, 0, volume.value));

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

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadLevel(int index)
    {
        loadingScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (!loaded)
        {
            loaded = !loaded;
            StartCoroutine(LoadAsync(index));
        }
    }

    IEnumerator LoadAsync(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex: index);
        print("asyncload");
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            if(asyncLoad.progress>=0.9f && !asyncLoad.allowSceneActivation)
            {
                loadingText.SetActive(true);
                if (Input.anyKeyDown)
                    asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void ChangeGraphicQuality()
    {
        QualitySettings.SetQualityLevel(gDropDown.value);
        PlayerPrefs.SetInt("Quality",gDropDown.value);
    }
    
    public void ChangeResolution()
    {
        Screen.SetResolution(res[rDropDown.value].width, res[rDropDown.value].height, Screen.fullScreen);
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

    public void ButtonPressedSound()
    {
        buttonSound = GetComponent<AudioSource>();
        buttonSound.pitch = Random.Range(0.9f, 1.1f);
        buttonSound.Play();
    }
}

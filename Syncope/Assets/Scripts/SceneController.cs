using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private GameObject _loadingText;

    private int _levelComplete;
    private int _sceneIndex;
    private AudioSource _buttonSound;
    private bool _loaded = false;

    public void Awake()
    {
        GlobalEventManager.RestartSceneEvent.AddListener(RestartLevel);
    }

    private void Start()
    {
        _levelComplete = PlayerPrefs.GetInt("LevelComplete");
        _sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public int GetSceneIndex()
    {
        return _sceneIndex;
    }

    public void LoadLevel(int index)
    {
        _loadingScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (!_loaded)
        {
            _loaded = !_loaded;
            StartCoroutine(LoadAsync(index));
        }
    }


    public void RestartLevel()
    {
        SceneManager.LoadScene(_sceneIndex);
    }

    public void StartNextLevel()
    {
        if(_sceneIndex > _levelComplete && _sceneIndex < 5)
        {
            PlayerPrefs.SetInt("LevelComplete", _sceneIndex);
        }
        _loadingScreen.SetActive(true);
        if (_sceneIndex < 5)
            StartCoroutine(LoadAsync(_sceneIndex + 1));
        else
            GoToMainMenu();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }

    IEnumerator LoadAsync(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex: index);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f && !asyncLoad.allowSceneActivation)
            {
                _loadingText.SetActive(true);
                if (Input.anyKeyDown)
                    asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ButtonPressedSound()
    {
        _buttonSound = GetComponent<AudioSource>();
        _buttonSound.pitch = Random.Range(0.9f, 1.1f);
        _buttonSound.Play();
    }
}

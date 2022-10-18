using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject loadingText;

    private int levelComplete;
    private int sceneIndex;
    private AudioSource buttonSound;

    private void Start()
    {
        levelComplete = PlayerPrefs.GetInt("LevelComplete");
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void StartNextLevel()
    {
        if(sceneIndex > levelComplete && sceneIndex < 5)
        {
            PlayerPrefs.SetInt("LevelComplete", sceneIndex);
        }
        loadingScreen.SetActive(true);
        if (sceneIndex < 5)
            StartCoroutine(LoadAsync(sceneIndex + 1));
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
                loadingText.SetActive(true);
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
        buttonSound = GetComponent<AudioSource>();
        buttonSound.pitch = Random.Range(0.9f, 1.1f);
        buttonSound.Play();
    }
}

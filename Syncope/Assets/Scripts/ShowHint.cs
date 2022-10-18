using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowHint : MonoBehaviour
{
    public GameObject hintPanel;

    private bool wasShown;
    private string sceneHintKey;

    private void Start()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (sceneIndex)
        {
            case 1: sceneHintKey = "ShowHint1"; break;
            case 2: sceneHintKey = "ShowHint2"; break;
            case 3: sceneHintKey = "ShowHint3"; break;
            case 4: sceneHintKey = "ShowHint4"; break;
            case 5: sceneHintKey = "ShowHint5"; break;
        }

        if (PlayerPrefs.HasKey(sceneHintKey) && PlayerPrefs.GetInt(sceneHintKey) == 1)
        {
                wasShown = true;
        }
        else
            wasShown = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character") && !wasShown)
        {
            hintPanel.SetActive(true);
            Time.timeScale = 0f;
            wasShown = true;
            PlayerPrefs.SetInt(sceneHintKey, 1);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void HideHint()
    {
        hintPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

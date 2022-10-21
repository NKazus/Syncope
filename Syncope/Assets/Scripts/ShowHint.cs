using UnityEngine;

public class ShowHint : MonoBehaviour
{
    [SerializeField] private GameObject _hintPanel;
    [SerializeField] private SceneController _sceneController;

    private bool _wasShown;
    private string _sceneHintKey;

    private void Start()
    {
        int sceneIndex = _sceneController.GetSceneIndex();
        switch (sceneIndex)
        {
            case 1: _sceneHintKey = "ShowHint1"; break;
            case 2: _sceneHintKey = "ShowHint2"; break;
            case 3: _sceneHintKey = "ShowHint3"; break;
            case 4: _sceneHintKey = "ShowHint4"; break;
            case 5: _sceneHintKey = "ShowHint5"; break;
        }

        if (PlayerPrefs.HasKey(_sceneHintKey) && PlayerPrefs.GetInt(_sceneHintKey) == 1)
        {
            _wasShown = true;
        }
        else
        {
            _wasShown = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character") && !_wasShown)
        {
            _hintPanel.SetActive(true);
            Time.timeScale = 0f;
            _wasShown = true;
            PlayerPrefs.SetInt(_sceneHintKey, 1);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void HideHint()
    {
        _hintPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
